using System.Data;
using System.Security.Cryptography;
using TweetLottery.Codes.Extensions;

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
			BtnDrawTweets,
			NUPDrawAmount
		};

		try
		{
			ctrlSet1.SetEnabled(false);

			// 先執行一次重設。
			BtnReset_Click(this, e);

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

			TBLog.InvokeIfRequired(TBLog.Clear);
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	private void BtnDrawTweets_Click(object sender, EventArgs e)
	{
		Control[] ctrlSet1 =
		{
			TBAuthToken,
			TBCsrfToken,
			TBQueryString,
			BtnFetchTweets,
			BtnCancel,
			BtnReset,
			BtnDrawTweets,
			NUPDrawAmount
		};

		try
		{
			ctrlSet1.SetEnabled(false);

			int itemCount = LVFetchedTweets.Items.Count;

			decimal number = NUPDrawAmount.Value;

			if (number <= 0)
			{
				ShowWarnMsg(this, "抽取數量不可小於 1。");

				return;
			}

			if (itemCount <= 0)
			{
				ShowWarnMsg(this, "請先獲取推文。");

				return;
			}

			if (number > itemCount)
			{
				ShowWarnMsg(this, "抽取數量不可大於獲取的推文數量。");

				return;
			}

			// 抽取前先清除舊的紀錄。
			TBLog.InvokeIfRequired(TBLog.Clear);

			List<ListViewItem> drawResult = new();

			for (int i = 0; i < number; i++)
			{
				int index = RandomNumberGenerator.GetInt32(0, LVFetchedTweets.Items.Count);

				ListViewItem listViewItem = LVFetchedTweets.Items[index];

				// 排除重複的推文以及重複的使用者。
				while (drawResult.Contains(listViewItem) &&
					!drawResult.Any(n => n.SubItems[0].Text == listViewItem.SubItems[0].Text))
				{
					index = RandomNumberGenerator.GetInt32(0, LVFetchedTweets.Items.Count);

					listViewItem = LVFetchedTweets.Items[index];
				}

				drawResult.Add(listViewItem);
			}

			if (drawResult.Any())
			{
				string drawResultMessage = string.Empty;

				drawResultMessage += $"抽選中的推文：{Environment.NewLine}";

				int orderIndex = 1;

				foreach (ListViewItem listViewItem in drawResult)
				{
					string screenName = listViewItem.SubItems[0].Text,
						name = listViewItem.SubItems[1].Text,
						userIDStr = listViewItem.SubItems[2].Text,
						idStr = listViewItem.SubItems[3].Text,
						fullText = listViewItem.SubItems[4].Text,
						createdAt = listViewItem.SubItems[5].Text;

					drawResultMessage += $"[排序：{orderIndex}] [使用者：{name}] " +
						$"[全文：{fullText}] " +
						$"[推文網址：https://twitter.com/i/web/status/{idStr}]{Environment.NewLine}";

					orderIndex++;
				}

				WriteLog(this, drawResultMessage);
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
				CopyToClipboard(LVFetchedTweets);
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
				OpenTweetUrl(LVFetchedTweets, e);
				break;
			case MouseButtons.Right:
				OpenUserTwitterUrl(LVFetchedTweets, e);
				break;
			default:
				break;
		}
	}
}