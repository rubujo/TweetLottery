using GetCachable;
using System.Reflection;
using System.Text.Json;
using TweetLottery.Codes;
using TweetLottery.Codes.Extensions;
using TweetLottery.Codes.Models;
using TweetLottery.Codes.Utils;

namespace TweetLottery;

// 阻擋設計工具。
partial class DesignerBlocker { }

/// <summary>
/// FMain 的方法
/// </summary>
public partial class FMain
{
	/// <summary>
	/// 自定義初始化
	/// </summary>
	private void CustomInit()
	{
		try
		{
			LVersion.InvokeIfRequired(() =>
			{
				Version? version = Assembly.GetExecutingAssembly().GetName().Version;

				string verText = version != null ? $"v{version}" : "無";

				// 設定版本號顯示。
				LVersion.Text = $"版本號：{verText}";
			});

			InitListView(LVFetchedTweets);
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	/// <summary>
	/// 將資料加入至加入至 ListView
	/// </summary>
	/// <param name="listview">ListView</param>
	/// <param name="listTweetData">List&lt;TweetData&gt;</param>
	public void AddDataToListView(ListView listview, List<TweetData> listTweetData)
	{
		List<ListViewItem> dataSet = new();

		foreach (TweetData tweetData in listTweetData.OrderBy(n => n.CreatedAt))
		{
			WriteLog(this, $"正在處理推文（{tweetData.ID}）……");

			string? screenName = tweetData.UserData?.ScreenName;

			if (screenName == null)
			{
				continue;
			}

			ListViewItem newListViewItem = new(screenName)
			{
				UseItemStyleForSubItems = false
			};

			string[] subItems = new string[5];

			subItems[0] = tweetData.UserData?.Name ?? string.Empty;
			subItems[1] = tweetData.UserData?.IDStr ?? string.Empty;
			subItems[2] = tweetData.IDStr ?? string.Empty;
			subItems[3] = tweetData.FullText ?? string.Empty;
			subItems[4] = tweetData.CreatedAt?.ToString() ?? string.Empty;

			newListViewItem.SubItems.AddRange(subItems);

			// 使用 screenName 當作 imgKey。
			string? imgKey = tweetData.UserData?.ScreenName,
				profileImageUrl = tweetData.UserData?.ProfileImageUrlHttps;

			if (!string.IsNullOrEmpty(imgKey))
			{
				listview.InvokeIfRequired(() =>
				{
					if (listview.SmallImageList != null &&
						!listview.SmallImageList.Images.ContainsKey(imgKey))
					{
						// 以 imgKey 為鍵值，將 Image 暫存 10 分鐘。
						Image? image = BetterCacheManager.GetCachableData(imgKey, () =>
						{
							try
							{
								WriteLog(this, $"正在下載使用者螢幕名稱（{imgKey}）的個人設定圖檔……");

								// 取得 HttpClient。
								using HttpClient httpClient = HttpClientUtil.GetHttpClient(_httpClientFactory);

								byte[] bytes = httpClient.GetByteArrayAsync(profileImageUrl).Result;

								using MemoryStream memoryStream = new(bytes);

								return Image.FromStream(memoryStream);
							}
							catch (Exception ex)
							{
								WriteLog(this, $"使用者螢幕名稱（{imgKey}）的個人設定圖檔下載失敗。");
								WriteLog(this, ex.ToString());

								Bitmap bitmap = new(64, 64);

								using (Graphics graphics = Graphics.FromImage(bitmap))
								{
									graphics.Clear(Color.FromKnownColor(KnownColor.White));
								}

								return bitmap;
							}
						}, 10);

						listview.SmallImageList.Images.Add(imgKey, image);

						image.Dispose();
						image = null;

						WriteLog(this, $"使用者（{imgKey}）的個人設定圖檔下載完成。");
					}
					else
					{
						WriteLog(this, $"使用者（{imgKey}）的個人設定圖檔已存在，不需要再下載一次。");
					}

					newListViewItem.ImageKey = imgKey;
				});
			}

			dataSet.Add(newListViewItem);
		}

		listview.InvokeIfRequired(() =>
		{
			listview.BeginUpdate();
			listview.Items.Clear();
			listview.Items.AddRange(dataSet.ToArray());
			listview.EndUpdate();
		});
	}

	/// <summary>
	/// 複製至剪貼簿
	/// </summary>
	/// <param name="listView">ListView</param>
	public void CopyToClipboard(ListView listView)
	{
		listView.InvokeIfRequired(() =>
		{
			ListView.SelectedListViewItemCollection selectedItems = listView.SelectedItems;

			string copiedContent = string.Empty;

			foreach (ListViewItem listViewItem in selectedItems)
			{
				string tempContent = string.Empty;

				int count = 0;

				foreach (ListViewItem.ListViewSubItem listViewSubItem in listViewItem.SubItems)
				{
					string currentContent = listViewSubItem.Text;

					tempContent += currentContent;

					if (count != listViewItem.SubItems.Count - 1)
					{
						if (!string.IsNullOrEmpty(currentContent))
						{
							tempContent += "	";
						}
					}

					count++;
				}

				copiedContent += $"{tempContent}{Environment.NewLine}";
			}

			Clipboard.SetText(copiedContent);

			WriteLog(this, "已將選取的推文複製至剪貼簿。");
		});
	}

	/// <summary>
	/// 初始化 ListView
	/// </summary>
	/// <param name="listview">ListView</param>
	public static void InitListView(ListView listview)
	{
		ColumnHeader[] columnHeaders =
		{
			new ColumnHeader()
			{
				Name = "ScreenName",
				Text = "使用者螢幕名稱",
				TextAlign = HorizontalAlignment.Left,
				Width = 120,
				DisplayIndex = 0
			},
			new ColumnHeader()
			{
				Name = "Name",
				Text = "使用者名稱",
				TextAlign = HorizontalAlignment.Left,
				Width = 120,
				DisplayIndex = 1
			},
			new ColumnHeader()
			{
				Name = "UserIDStr",
				Text = "使用者 ID 值",
				TextAlign = HorizontalAlignment.Left,
				Width = 0,
				DisplayIndex = 2
			},
			new ColumnHeader()
			{
				Name = "IDStr",
				Text = "推文 ID 值",
				TextAlign = HorizontalAlignment.Left,
				Width = 80,
				DisplayIndex = 3
			},
			new ColumnHeader()
			{
				Name = "FullText",
				Text = "推文全文",
				TextAlign = HorizontalAlignment.Left,
				Width = 300,
				DisplayIndex = 4
			},
			new ColumnHeader()
			{
				Name = "CreateAt",
				Text = "建立於",
				TextAlign = HorizontalAlignment.Left,
				Width = 120,
				DisplayIndex = 5
			}
		};

		listview.Columns.AddRange(columnHeaders);

		ImageList imageList = new()
		{
			ImageSize = new Size(32, 32),
			ColorDepth = ColorDepth.Depth32Bit
		};

		listview.SmallImageList = imageList;
	}

	/// <summary>
	/// 開啟推文網址
	/// </summary>
	/// <param name="listView">ListView</param>
	/// <param name="e">MouseEventArgs</param>
	public static void OpenTweetUrl(ListView listView, MouseEventArgs e)
	{
		listView.InvokeIfRequired(() =>
		{
			ListViewItem? focusedItem = listView.FocusedItem;

			if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
			{
				if (focusedItem.SubItems.Count >= 6)
				{
					string idStr = focusedItem.SubItems[3].Text;

					CustomFunction.OpenBrowser($"https://twitter.com/i/web/status/{idStr}");
				}
			}
		});
	}

	/// <summary>
	/// 開啟使用者 Twitter 網址
	/// </summary>
	/// <param name="listView">ListView</param>
	/// <param name="e">MouseEventArgs</param>
	public static void OpenUserTwitterUrl(ListView listView, MouseEventArgs e)
	{
		listView.InvokeIfRequired(() =>
		{
			ListViewItem? focusedItem = listView.FocusedItem;

			if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
			{
				if (focusedItem.SubItems.Count >= 6)
				{
					string screenName = focusedItem.Text;

					CustomFunction.OpenBrowser($"https://twitter.com/{screenName}");
				}
			}
		});
	}

	/// <summary>
	/// 獲取推文資料
	/// </summary>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>Task</returns>
	public async Task FetchTweets(CancellationToken cancellationToken = default)
	{
		try
		{
			cancellationToken.ThrowIfCancellationRequested();

			int roundIndex = 1;

			string? authToken = TBAuthToken.Text,
				ct0 = TBCsrfToken.Text,
				queryString = TBQueryString.Text,
				apiUrl = TwitterApiUtil.GetApiUrl(queryString),
				cursorString = string.Empty;

			if (string.IsNullOrEmpty(authToken))
			{
				ShowWarnMsg(this, $"請輸入 Auth Token。");

				return;
			}

			if (string.IsNullOrEmpty(ct0))
			{
				ShowWarnMsg(this, $"請輸入 CSRF Token。");

				return;
			}

			if (string.IsNullOrEmpty(queryString))
			{
				ShowWarnMsg(this, $"請輸入查詢字串。");

				return;
			}

			WriteLog(this, $"使用的查詢字串：{queryString}");
			WriteLog(this, $"正在獲取推文…… （第 {roundIndex} 頁）");

			JsonElement? jsonData = await TwitterApiUtil
				.GetAdaptiveJsonData(_httpClientFactory, apiUrl, authToken, ct0);

			cursorString = TwitterApiUtil.GetCursorString(jsonData);

			FetchedTweets.AddRange(TwitterApiUtil.GetTweets(jsonData).Where(n => !FetchedTweets.Contains(n)));
			FetchedUsers.AddRange(TwitterApiUtil.GetUsers(jsonData).Where(n => !FetchedUsers.Contains(n)));

			roundIndex++;

			while (!string.IsNullOrEmpty(cursorString))
			{
				cancellationToken.ThrowIfCancellationRequested();

				jsonData = await TwitterApiUtil
					.GetAdaptiveJsonData(_httpClientFactory, apiUrl, authToken, ct0, cursorString);

				cursorString = TwitterApiUtil.GetCursorString(jsonData);

				FetchedTweets.AddRange(TwitterApiUtil.GetTweets(jsonData).Where(n => !FetchedTweets.Contains(n)));
				FetchedUsers.AddRange(TwitterApiUtil.GetUsers(jsonData).Where(n => !FetchedUsers.Contains(n)));

				WriteLog(this, $"正在獲取推文…… （第 {roundIndex} 頁）");

				roundIndex++;
			}

			WriteLog(this, $"推文獲取完成，共獲取到 {FetchedTweets.Count} 則推文。");

			// 繫結 TweetData 跟 UserData。
			foreach (TweetData tweetData in FetchedTweets)
			{
				UserData? userData = FetchedUsers.FirstOrDefault(n => n.ID == tweetData.UserID);

				if (userData != null)
				{
					tweetData.UserData = userData;
				}
			}
		}
		catch (OperationCanceledException)
		{
			try
			{
				WriteLog(this, $"已取消作業，共獲取到 {FetchedTweets.Count} 則推文。");

				// 繫結 TweetData 跟 UserData。
				foreach (TweetData tweetData in FetchedTweets)
				{
					UserData? userData = FetchedUsers.FirstOrDefault(n => n.ID == tweetData.UserID);

					if (userData != null)
					{
						tweetData.UserData = userData;
					}
				}

				AddDataToListView(LVFetchedTweets, FetchedTweets);
			}
			catch (Exception ex)
			{
				ShowErrMsg(this, ex.ToString());
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	/// <summary>
	/// 顯示錯誤訊息
	/// </summary>
	public static readonly Action<Form, string> ShowMsg =
		new((Form form, string message) =>
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			MessageBox.Show(
				message,
				form.Text,
				MessageBoxButtons.OK);
		});

	/// <summary>
	/// 顯示錯誤訊息
	/// </summary>
	public static readonly Action<Form, string> ShowErrMsg =
		new((Form form, string message) =>
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			MessageBox.Show(
				message,
				form.Text,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		});

	/// <summary>
	/// 顯示警告訊息
	/// </summary>
	public static readonly Action<Form, string> ShowWarnMsg =
		new((Form form, string message) =>
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			MessageBox.Show(
				message,
				form.Text,
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning);
		});

	/// <summary>
	/// 取得 TBLog(
	/// </summary>
	/// <returns>TextBox</returns>
	public TextBox GetTBLog()
	{
		return TBLog;
	}

	/// <summary>
	/// 寫紀錄
	/// </summary>
	/// <param name="form">FMain</param>
	/// <param name="message">字串，訊息內容</param>
	public static void WriteLog(FMain form, string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			return;
		}

		try
		{
			TextBox textBox = form.GetTBLog();

			textBox.InvokeIfRequired(() =>
			{
				textBox.Text += $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] " +
					$"{message}{Environment.NewLine}";
				textBox.SelectionStart = textBox.TextLength;
				textBox.ScrollToCaret();
			});
		}
		catch (Exception ex)
		{
			ShowErrMsg(form, ex.ToString());
		}
	}
}