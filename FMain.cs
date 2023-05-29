using TweetLottery.Codes.Extensions;
using TweetLottery.Codes.Models;

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
			CBNotEmulateManualSurf,
			CBNotDownloadProfileImage,
			BtnExportTweets,
			NUPDrawAmount,
			CBExcludeSameUser,
			BtnDrawTweets
		};

		try
		{
			// 先執行一次重設。
			BtnReset_Click(this, e);

			ctrlSet1.SetEnabled(false);

			SharedCancellationTokenSource = new();
			SharedCancellationToken = SharedCancellationTokenSource.Token;

			bool notDownloadProfileImage = CBNotDownloadProfileImage.Checked,
				notEmulateManualSurf = CBNotEmulateManualSurf.Checked;

			await FetchTweets(notEmulateManualSurf, SharedCancellationToken.Value).ContinueWith(task =>
			{
				// 當有獲取到推文時才進一步處理。
				if (FetchedTweets.Any())
				{
					try
					{
						// 繫結 TweetData 跟 UserData。
						foreach (TweetData tweetData in FetchedTweets)
						{
							UserData? userData = FetchedUsers.FirstOrDefault(n => n.ID == tweetData.UserID);

							if (userData != null)
							{
								tweetData.UserData = userData;
							}
						}

						AddDataToListView(
							LVFetchedTweets,
							FetchedTweets,
							notDownloadProfileImage,
							notEmulateManualSurf);
					}
					catch (Exception ex)
					{
						ShowErrMsg(this, ex.ToString());
					}
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
		Control[] ctrlSet1 =
		{
			TBAuthToken,
			TBCsrfToken,
			TBQueryString,
			BtnFetchTweets,
			BtnCancel,
			BtnReset,
			CBNotEmulateManualSurf,
			CBNotDownloadProfileImage,
			BtnExportTweets,
			NUPDrawAmount,
			CBExcludeSameUser,
			BtnDrawTweets
		};

		try
		{
			ctrlSet1.SetEnabled(false);

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
		finally
		{
			ctrlSet1.SetEnabled(true);
		}
	}

	private void CBHideTokenField_CheckedChanged(object sender, EventArgs e)
	{
		CheckBox control = (CheckBox)sender;

		if (control.Checked)
		{
			TBAuthToken.Visible = false;
			TBCsrfToken.Visible = false;
		}
		else
		{
			TBAuthToken.Visible = true;
			TBCsrfToken.Visible = true;
		}
	}

	public async void BtnDrawTweets_Click(object sender, EventArgs e)
	{
		Control[] ctrlSet1 =
		{
			TBAuthToken,
			TBCsrfToken,
			TBQueryString,
			BtnFetchTweets,
			BtnReset,
			CBNotEmulateManualSurf,
			CBNotDownloadProfileImage,
			BtnExportTweets,
			NUPDrawAmount,
			CBExcludeSameUser,
			BtnDrawTweets
		};

		try
		{
			ctrlSet1.SetEnabled(false);

			int itemCount = LVFetchedTweets.Items.Count;

			decimal drawAmount = NUPDrawAmount.Value;

			if (drawAmount <= 0)
			{
				ShowWarnMsg(this, "抽取數量不可小於 1。");

				return;
			}

			if (itemCount <= 0)
			{
				ShowWarnMsg(this, "請先獲取推文。");

				return;
			}

			if (drawAmount > itemCount)
			{
				ShowWarnMsg(this, $"抽取數量（{drawAmount}）不可大於獲取的推文數量（{itemCount}）。");

				return;
			}

			SharedCancellationTokenSource = new();
			SharedCancellationToken = SharedCancellationTokenSource.Token;

			await DrawTweets(drawAmount, SharedCancellationToken.Value);
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

	private void LVFetchedTweets_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Delete)
			{
				ListView control = (ListView)sender;

				control.InvokeIfRequired(() =>
				{
					foreach (ListViewItem listViewItem in control.SelectedItems)
					{
						listViewItem.Remove();
					}
				});
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	private void LVFetchedTweets_MouseClick(object sender, MouseEventArgs e)
	{
		try
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
					break;
				case MouseButtons.Right:
					CopyToClipboard(this, LVFetchedTweets);
					break;
				default:
					break;
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	private void LVFetchedTweets_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		try
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
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	private async void BtnExportTweets_Click(object sender, EventArgs e)
	{
		Control[] ctrlSet1 =
		{
			TBAuthToken,
			TBCsrfToken,
			TBQueryString,
			BtnFetchTweets,
			BtnReset,
			NUPDrawAmount,
			CBExcludeSameUser,
			BtnDrawTweets,
			BtnExportTweets
		};

		try
		{
			ctrlSet1.SetEnabled(false);

			SharedCancellationTokenSource = new();
			SharedCancellationToken = SharedCancellationTokenSource.Token;

			await DoExportTask(this, LVFetchedTweets, SharedCancellationToken.Value);
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
}