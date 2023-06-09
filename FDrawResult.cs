﻿using TweetLottery.Codes.Extensions;

namespace TweetLottery;

/// <summary>
/// FDrawResult
/// </summary>
public partial class FDrawResult : Form
{
	/// <summary>
	/// 建構函式
	/// </summary>
	/// <param name="fMain">FMain</param>
	/// <param name="dataSet">List&lt;ListViewItem&gt;</param>
	public FDrawResult(FMain fMain, List<ListViewItem>? dataSet)
	{
		InitializeComponent();

		Icon = Properties.Resources.app_icon;
		Text = $"抽取結果 - {fMain.Text}";

		_FMain = fMain;
		_DataSet = dataSet;

		_TBAuthToken = fMain.Controls
			.OfType<TextBox>()
			.FirstOrDefault(n => n.Name == "TBAuthToken")!;
		_TBCsrfToken = fMain.Controls
			.OfType<TextBox>()
			.FirstOrDefault(n => n.Name == "TBCsrfToken")!;
		_TBQueryString = fMain.Controls
			.OfType<TextBox>()
			.FirstOrDefault(n => n.Name == "TBQueryString")!;
		_BtnFetchTweets = fMain.Controls
			.OfType<Button>()
			.FirstOrDefault(n => n.Name == "BtnFetchTweets")!;
		_BtnCancel = fMain.Controls
			.OfType<Button>()
			.FirstOrDefault(n => n.Name == "BtnCancel")!;
		_BtnReset = fMain.Controls
			.OfType<Button>()
			.FirstOrDefault(n => n.Name == "BtnReset")!;
		_CBNotEmulateManualSurf = fMain.Controls
			.OfType<CheckBox>()
			.FirstOrDefault(n => n.Name == "CBNotEmulateManualSurf")!;
		_CBNotDownloadProfileImage = fMain.Controls
			.OfType<CheckBox>()
			.FirstOrDefault(n => n.Name == "CBNotDownloadProfileImage")!;
		_CBHideTokenField = fMain.Controls
			.OfType<CheckBox>()
			.FirstOrDefault(n => n.Name == "CBHideTokenField")!;
		_NUPDrawAmount = fMain.Controls
			.OfType<NumericUpDown>()
			.FirstOrDefault(n => n.Name == "NUPDrawAmount")!;
		_CBExcludeSameUser = fMain.Controls
			.OfType<CheckBox>()
			.FirstOrDefault(n => n.Name == "CBExcludeSameUser")!;
		_BtnDrawTweets = fMain.Controls
			.OfType<Button>()
			.FirstOrDefault(n => n.Name == "BtnDrawTweets")!;
		_LVFetchedTweets = fMain.Controls
			.OfType<ListView>()
			.FirstOrDefault(n => n.Name == "LVFetchedTweets")!;
	}

	private void FDrawResult_Load(object sender, EventArgs e)
	{
		try
		{
			SetFMainControls(false);

			FMain.InitListView(LVDrawResult);

			LVDrawResult.InvokeIfRequired(() =>
			{
				LVDrawResult.SmallImageList = _LVFetchedTweets.SmallImageList;

				LVDrawResult.BeginUpdate();
				LVDrawResult.Items.Clear();
				LVDrawResult.Items.AddRange(_DataSet?.ToArray());
				LVDrawResult.EndUpdate();
			});
		}
		catch (Exception ex)
		{
			FMain.ShowErrMsg(this, ex.ToString());
		}
	}

	private void FDrawResult_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			SetFMainControls(true);
		}
		catch (Exception ex)
		{
			FMain.ShowErrMsg(this, ex.ToString());
		}
	}

	private void LVDrawResult_MouseClick(object sender, MouseEventArgs e)
	{
		switch (e.Button)
		{
			case MouseButtons.Left:
				break;
			case MouseButtons.Right:
				FMain.CopyToClipboard(_FMain, LVDrawResult);
				break;
			default:
				break;
		}
	}

	private void LVDrawResult_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		switch (e.Button)
		{
			case MouseButtons.Left:
				FMain.OpenTweetUrl(LVDrawResult, e);
				break;
			case MouseButtons.Right:
				FMain.OpenUserTwitterUrl(LVDrawResult, e);
				break;
			default:
				break;
		}
	}

	private void BtnRedrawWithSameRule_Click(object sender, EventArgs e)
	{
		Control[] ctrlSet1 =
		{
			BtnRedrawWithSameRule,
			BtnExportDrawResult
		};

		try
		{
			ctrlSet1.SetEnabled(false);

			Hide();

			_FMain.BtnDrawTweets_Click(sender, e);
		}
		catch (Exception ex)
		{
			FMain.ShowErrMsg(this, ex.ToString());
		}
		finally
		{
			ctrlSet1.SetEnabled(true);
		}
	}

	private async void BtnExportDrawResult_Click(object sender, EventArgs e)
	{
		Control[] ctrlSet1 =
		{
			BtnRedrawWithSameRule,
			BtnExportDrawResult
		};

		try
		{
			ctrlSet1.SetEnabled(false);

			await FMain.DoExportTask(this, LVDrawResult, CancellationToken.None);
		}
		catch (Exception ex)
		{
			FMain.ShowErrMsg(this, ex.ToString());
		}
		finally
		{
			ctrlSet1.SetEnabled(true);
		}
	}

	/// <summary>
	/// 設定 FMain 的控制項
	/// </summary>
	/// <param name="enabled">布林值，是否啟用，預設值為 true</param>
	private void SetFMainControls(bool enabled = true)
	{
		_TBAuthToken.InvokeIfRequired(() =>
		{
			_TBAuthToken.Enabled = enabled;
		});

		_TBCsrfToken.InvokeIfRequired(() =>
		{
			_TBCsrfToken.Enabled = enabled;
		});

		_TBQueryString.InvokeIfRequired(() =>
		{
			_TBQueryString.Enabled = enabled;
		});

		_BtnFetchTweets.InvokeIfRequired(() =>
		{
			_BtnFetchTweets.Enabled = enabled;
		});

		_BtnCancel.InvokeIfRequired(() =>
		{
			_BtnCancel.Enabled = enabled;
		});

		_BtnReset.InvokeIfRequired(() =>
		{
			_BtnReset.Enabled = enabled;
		});

		_CBNotEmulateManualSurf.InvokeIfRequired(() =>
		{
			_CBNotEmulateManualSurf.Enabled = enabled;
		});

		_CBNotDownloadProfileImage.InvokeIfRequired(() =>
		{
			_CBNotDownloadProfileImage.Enabled = enabled;
		});

		_CBHideTokenField.InvokeIfRequired(() =>
		{
			_CBHideTokenField.Enabled = enabled;
		});

		_NUPDrawAmount.InvokeIfRequired(() =>
		{
			_NUPDrawAmount.Enabled = enabled;
		});

		_CBExcludeSameUser.InvokeIfRequired(() =>
		{
			_CBExcludeSameUser.Enabled = enabled;
		});

		_BtnDrawTweets.InvokeIfRequired(() =>
		{
			_BtnDrawTweets.Enabled = enabled;
		});
	}
}