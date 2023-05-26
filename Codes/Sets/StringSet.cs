namespace TweetLottery.Client.Codes.Sets;

/// <summary>
/// 字串組
/// </summary>
public class StringSet
{
	/// <summary>
	/// 使用者代理字串
	/// </summary>
	public static readonly string UserAgent = Properties.Settings.Default.UserAgent;

	/// <summary>
	/// Twitter.com 公開的 BearerToken
	/// </summary>
	public static readonly string PublicBearerToken = Properties.Settings.Default.PublicBearerToken;

	/// <summary>
	/// adaptive.json 的基礎網址
	/// </summary>
	public static readonly string ApiAdaptiveBaseUrl = "https://twitter.com/i/api/2/search/adaptive.json";
}