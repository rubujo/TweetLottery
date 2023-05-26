using TweetLottery.Codes.Models;

namespace TweetLottery;

// 阻擋設計工具。
partial class DesignerBlocker { }

/// <summary>
/// FMain 的變數
/// </summary>
public partial class FMain
{
	/// <summary>
	/// 共用的 CancellationTokenSource
	/// </summary>
	private CancellationTokenSource? SharedCancellationTokenSource = null;

	/// <summary>
	/// 共用的 CancellationToken
	/// </summary>
	private CancellationToken? SharedCancellationToken = null;

	/// <summary>
	/// IHttpClientFactory
	/// </summary>
	private readonly IHttpClientFactory _httpClientFactory;

	/// <summary>
	/// 獲取到的推文資料
	/// </summary>
	private readonly List<TweetData> FetchedTweets = new();

	/// <summary>
	/// 獲取到的使用者資料
	/// </summary>
	private readonly List<UserData> FetchedUsers = new();
}