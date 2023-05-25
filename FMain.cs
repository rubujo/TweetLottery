using GetCachable;
using System.Data;
using System.Security.Cryptography;
using TweetLottery.Codes.Extensions;
using TweetLottery.Codes.Models;
using TweetLottery.Codes.Utils;
using static System.Net.Mime.MediaTypeNames;

namespace TweetLottery;

/// <summary>
/// FMain
/// </summary>
public partial class FMain : Form
{
	/// <summary>
	/// 建構函式
	/// </summary>
	/// <param name="httpClientFactory">IHttpClientFactory</param>
	public FMain(IHttpClientFactory httpClientFactory)
	{
		InitializeComponent();

		_httpClientFactory = httpClientFactory;
	}

	private void MForm_Load(object sender, EventArgs e)
	{
		try
		{
			CustomInit();
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	private async void BtnFetchTweets_Click(object sender, EventArgs e)
	{
		Control[] ctrlSet1 =
		{
			TBAuthToken,
			TBCsrfToken,
			TBQueryString,
			BtnFetchTweets,
			BtnReset,
			BtnDrawTweet,
			NUPDrawNumber
		};

		try
		{
			ctrlSet1.SetEnabled(false);

			FetchedTweets.Clear();
			FetchedUsers.Clear();

			SharedCancellationTokenSource = new();
			SharedCancellationToken = SharedCancellationTokenSource.Token;

			await FetchTweets(SharedCancellationToken.Value).ContinueWith(task =>
			{
				try
				{
					AddDataToListView(LVFetchedTweets, FetchedTweets);
				}
				catch (Exception ex)
				{
					ShowErrMsg(this, ex.ToString());
				}
			});
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
		finally
		{
			ctrlSet1.SetEnabled(true);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			if (SharedCancellationTokenSource?.IsCancellationRequested == false)
			{
				SharedCancellationTokenSource?.Cancel();
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	private void BtnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FetchedTweets.Clear();
			FetchedUsers.Clear();

			LVFetchedTweets.InvokeIfRequired(() =>
			{
				LVFetchedTweets.BeginUpdate();
				LVFetchedTweets.Items.Clear();
				LVFetchedTweets.EndUpdate();
			});

			TBLog.InvokeIfRequired(() =>
			{
				TBLog.Clear();
			});
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	private void BtnDrawTweet_Click(object sender, EventArgs e)
	{
		Control[] ctrlSet1 =
		{
			TBAuthToken,
			TBCsrfToken,
			TBQueryString,
			BtnFetchTweets,
			BtnCancel,
			BtnReset,
			BtnDrawTweet,
			NUPDrawNumber
		};

		try
		{
			ctrlSet1.SetEnabled(false);

			int itemCount = LVFetchedTweets.Items.Count;

			decimal number = NUPDrawNumber.Value;

			if (number <= 0)
			{
				ShowWarnMsg(this, "抽取數量不可小於 1。");

				return;
			}

			if (itemCount <= 0)
			{
				ShowWarnMsg(this, "請先獲取貼文。");

				return;
			}

			if (number > itemCount)
			{
				ShowWarnMsg(this, "抽取數量不可大於獲取的貼文數量。");

				return;
			}

			List<ListViewItem> drawResult = new();

			for (int i = 0; i < number; i++)
			{
				int index = RandomNumberGenerator.GetInt32(0, LVFetchedTweets.Items.Count);

				ListViewItem listViewItem = LVFetchedTweets.Items[index];

				drawResult.Add(listViewItem);
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
		finally
		{
			ctrlSet1.SetEnabled(true);
		}
	}

	private void LVFetchedTweets_MouseClick(object sender, MouseEventArgs e)
	{
		switch (e.Button)
		{
			case MouseButtons.Left:
				break;
			case MouseButtons.Right:
				OpenTweetUrl(LVFetchedTweets, e);
				break;
			default:
				break;
		}
	}

	private void LVFetchedTweets_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		switch (e.Button)
		{
			case MouseButtons.Left:
				CopyToClipboard(LVFetchedTweets);
				break;
			case MouseButtons.Right:
				OpenUserTwitterUrl(LVFetchedTweets, e);
				break;
			default:
				break;
		}
	}
}