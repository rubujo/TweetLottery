using GetCachable;
using OfficeOpenXml;
using OfficeOpenXml.Style.XmlAccess;
using OfficeOpenXml.Style;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using TweetLottery.Codes;
using TweetLottery.Codes.Extensions;
using TweetLottery.Codes.Models;
using TweetLottery.Codes.Utils;
using OfficeOpenXml.Drawing;
using System.Drawing.Imaging;

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
	/// 取得 TBLog
	/// </summary>
	/// <returns>TextBox</returns>
	public TextBox GetTBLog()
	{
		return TBLog;
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
								WriteLog(this, $"正在下載使用者（{imgKey}）的個人設定圖檔……");

								// 取得 HttpClient。
								using HttpClient httpClient = HttpClientUtil.GetHttpClient(_httpClientFactory);

								byte[] bytes = httpClient.GetByteArrayAsync(profileImageUrl).Result;

								using MemoryStream memoryStream = new(bytes);

								return Image.FromStream(memoryStream);
							}
							catch (Exception ex)
							{
								WriteLog(this, $"使用者（{imgKey}）的個人設定圖檔下載失敗。");
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
	/// 抽取推文
	/// </summary>
	/// <param name="drawAmount">數值，抽取數量</param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>Task</returns>
	public async Task DrawTweets(decimal drawAmount, CancellationToken cancellationToken = default)
	{
		try
		{
			cancellationToken.ThrowIfCancellationRequested();

			bool excludeSameUser = CBExcludeSameUser.Checked;

			List<ListViewItem> drawResult = new();

			int maxValue = LVFetchedTweets.Items.Count - 1;

			for (int roundIndex = 0; roundIndex < drawAmount; roundIndex++)
			{
				cancellationToken.ThrowIfCancellationRequested();

				int itemIndex = RandomNumberGenerator.GetInt32(0, maxValue);

				// 使用深複製。
				ListViewItem listViewItem = (ListViewItem)LVFetchedTweets.Items[itemIndex].Clone();

				string idStr = listViewItem.SubItems[3].Text,
					screenName = listViewItem.SubItems[0].Text;

				// 重新抽取次數。
				int reTryCount = 0;

				if (excludeSameUser)
				{
					// 排除重複的推文以及相同的使用者。
					while (drawResult.Any(n => n.SubItems[3].Text == idStr) ||
						drawResult.Any(n => n.SubItems[0].Text == screenName))
					{
						cancellationToken.ThrowIfCancellationRequested();

						// 重試達 5 次後，結束 while 迴圈。
						if (reTryCount == 5)
						{
							break;
						}

						WriteLog(this, $"抽取到已抽取過的推文（{idStr}）或使用者（{screenName}）相同，正在重新抽選……");

						itemIndex = RandomNumberGenerator.GetInt32(0, maxValue);

						listViewItem = (ListViewItem)LVFetchedTweets.Items[itemIndex].Clone();

						idStr = listViewItem.SubItems[3].Text;
						screenName = listViewItem.SubItems[0].Text;

						reTryCount++;
					}
				}
				else
				{
					// 排除重複的推文。
					while (drawResult.Any(n => n.SubItems[3].Text == idStr))
					{
						cancellationToken.ThrowIfCancellationRequested();

						// 重試達 5 次後，結束 while 迴圈。
						if (reTryCount == 5)
						{
							break;
						}

						WriteLog(this, $"抽取到已抽取過的推文（{idStr}），正在重新抽選……");

						itemIndex = RandomNumberGenerator.GetInt32(0, maxValue);

						listViewItem = (ListViewItem)LVFetchedTweets.Items[itemIndex].Clone();

						idStr = listViewItem.SubItems[3].Text;
						screenName = listViewItem.SubItems[0].Text;
					}
				}

				// 重試達 5 次後，還抽取不到時，則放棄此次的抽取。
				if (reTryCount == 5)
				{
					WriteLog(this, $"重新抽取達 5 次，放棄此次（{roundIndex}）的抽取。");

					continue;
				}

				WriteLog(this, $"抽取推文（{idStr}）。");

				drawResult.Add(listViewItem);
			}

			WriteLog(this, "推文抽取完成。");

			if (drawResult.Any())
			{
				FDrawResult fDrawResult = new(this, drawResult);

				fDrawResult.ShowDialog();
			}

			await Task.CompletedTask;
		}
		catch (OperationCanceledException)
		{
			WriteLog(this, $"已取消抽取。");
		}
		catch (Exception ex)
		{
			ShowErrMsg(this, ex.ToString());
		}
	}

	/// <summary>
	/// 複製至剪貼簿
	/// </summary>
	/// <param name="listView">ListView</param>
	public static void CopyToClipboard(FMain fMain, ListView listView)
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

			WriteLog(fMain, "已將選取的推文複製至剪貼簿。");
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
	/// 執行匯出任務
	/// </summary>
	/// <param name="form">Form</param>
	/// <param name="listView">ListView</param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>Task</returns>
	public static async Task DoExportTask(
		Form form,
		ListView listView,
		CancellationToken cancellationToken = default)
	{
		try
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (listView.Items.Count <= 0)
			{
				if (listView.Name == "LVFetchedTweets")
				{
					ShowWarnMsg(form, "請先獲取推文。");
				}
				else if (listView.Name == "LVDrawResult")
				{
					ShowWarnMsg(form, "請先抽取推文。");
				}

				return;
			}

			string fileName = listView.Name switch
			{
				"LVFetchedTweets" => "匯出推文",
				"LVDrawResult" => "匯出抽選貼文",
				_ => string.Empty
			};

			SaveFileDialog saveFileDialog = new()
			{
				Filter = "Excel 活頁簿|*.xlsx",
				Title = "儲存檔案",
				FileName = $"{fileName}_{DateTime.Now:yyyyMMdd}"
			};

			DialogResult dialogResult = saveFileDialog.ShowDialog();

			if (dialogResult != DialogResult.OK)
			{
				return;
			}

			await Task.Run(() =>
			{
				try
				{
					using FileStream fileStream = (FileStream)saveFileDialog.OpenFile();

					ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

					using ExcelPackage package = new();

					double[] widthSet = { 5.0, 30.0, 30.0, 30.0, 30.0, 80.0, 30.0 };

					ExcelWorkbook workbook = package.Workbook;

					string workSheetName = listView.Name switch
					{
						"LVFetchedTweets" => "推文",
						"LVDrawResult" => "抽選貼文",
						_ => string.Empty
					};

					ExcelWorksheet worksheet1 = workbook.Worksheets.Add(workSheetName);

					worksheet1.DefaultRowHeight = 28;

					// 欄位寬度設定。
					for (int i = 0; i < widthSet.Length; i++)
					{
						worksheet1.Column(i + 1).Width = widthSet[i];
					}

					#region 建置風格

					ExcelNamedStyleXml headerStyle = workbook.Styles.CreateNamedStyle("HeaderStyle");

					headerStyle.Style.Border.Top.Style = ExcelBorderStyle.Thin;
					headerStyle.Style.Border.Left.Style = ExcelBorderStyle.Thin;
					headerStyle.Style.Border.Right.Style = ExcelBorderStyle.Thin;
					headerStyle.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					headerStyle.Style.Font.Name = "微軟正黑體";
					headerStyle.Style.Font.Bold = true;
					headerStyle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					headerStyle.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					headerStyle.Style.WrapText = false;

					ExcelNamedStyleXml contentStyle = workbook.Styles.CreateNamedStyle("ContentStyle");

					contentStyle.Style.Border.Top.Style = ExcelBorderStyle.Thin;
					contentStyle.Style.Border.Left.Style = ExcelBorderStyle.Thin;
					contentStyle.Style.Border.Right.Style = ExcelBorderStyle.Thin;
					contentStyle.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					contentStyle.Style.Font.Name = "微軟正黑體";
					contentStyle.Style.Font.Bold = false;
					contentStyle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
					contentStyle.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					contentStyle.Style.WrapText = false;

					#endregion

					#region 建置標題

					ExcelRange headerFirstRange = worksheet1.Cells[1, 1];

					headerFirstRange.StyleName = "HeaderStyle";
					headerFirstRange.Value = "頭像";
					headerFirstRange.Style.Fill.SetBackground(Color.BlanchedAlmond);

					for (int i = 0; i < listView.Columns.Count; i++)
					{
						ColumnHeader header = listView.Columns[i];

						ExcelRange range = worksheet1.Cells[1, i + 2];

						range.StyleName = "HeaderStyle";
						range.Style.Fill.SetBackground(Color.BlanchedAlmond);
						range.Value = header.Text;
					}

					#endregion

					int startIdx1 = 2;

					listView.InvokeIfRequired(() =>
					{
						IEnumerable<ListViewItem> dataSet = listView.GetListViewItems();

						foreach (ListViewItem listViewItem in dataSet)
						{
							ExcelRange firstRange = worksheet1.Cells[startIdx1, 1];

							firstRange.StyleName = "ContentStyle";
							firstRange.Value = string.Empty;
							firstRange.Style.Fill.SetBackground(listViewItem.BackColor);

							Image? image = listView.SmallImageList?.Images[listViewItem.ImageKey];

							if (image != null)
							{
								// 將 image 轉換成 stream。
								Stream? imageStream = image.ToStream(ImageFormat.Png);

								if (imageStream != null)
								{
									// 2021-04-01
									// 對名稱加料，以免造成例外。
									// 還沒找到方法可以重複利用 ExcelPicture。
									ExcelPicture picture = worksheet1.Drawings
										.AddPicture($"row{startIdx1}_{listViewItem.ImageKey}",
										imageStream);

									int zeroBasedRow = startIdx1 - 1;

									picture.SetPosition(zeroBasedRow, 0, 0, 0);

									imageStream.Close();
									imageStream.Dispose();
									imageStream = null;
								}
							}

							for (int j = 0; j < listViewItem.SubItems.Count; j++)
							{
								ListViewItem.ListViewSubItem listViewSubItem = listViewItem.SubItems[j];

								ExcelRange excelRange = worksheet1.Cells[startIdx1, j + 2];

								excelRange.StyleName = "ContentStyle";
								excelRange.Value = listViewSubItem.Text;
								excelRange.Style.Font.Color.SetColor(listViewItem.SubItems[j].ForeColor);
								excelRange.Style.Fill.SetBackground(listViewItem.BackColor);
								excelRange.Style.WrapText = true;
							}

							startIdx1++;
						}
					});

					workbook.Properties.Title = fileName;
					workbook.Properties.Subject = workSheetName;
					workbook.Properties.Category = string.Empty;
					workbook.Properties.Keywords = string.Empty;
					workbook.Properties.Author = "推文抽獎";
					workbook.Properties.Comments = "由推文抽獎產生。";
					workbook.Properties.Company = string.Empty;

					package.SaveAs(fileStream);
				}
				catch (Exception ex)
				{
					ShowErrMsg(form, ex.ToString());
				}
			}, cancellationToken);
		}
		catch (OperationCanceledException)
		{
			ShowMsg(form, $"已取消匯出推文。");
		}
		catch (Exception ex)
		{
			ShowErrMsg(form, ex.ToString());
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