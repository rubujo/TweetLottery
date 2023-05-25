using TweetLottery.Client.Codes.Sets;

namespace TweetLottery.Codes.Utils;

/// <summary>
/// HttpClient 工具
/// </summary>
public class HttpClientUtil
{
	/// <summary>
	/// 取得 HttpClient
	/// </summary>
	/// <param name="httpClientFactory">IHttpClientFactory</param>
	/// <param name="userAgent">字串，使用者代理字串，預設值為空白</param>
	/// <returns>HttpClient</returns>
	public static HttpClient GetHttpClient(IHttpClientFactory httpClientFactory, string userAgent = "")
	{
		HttpClient httpClient = httpClientFactory.CreateClient();

		// 設定使用者代理字串。
		SetUserAgent(httpClient, userAgent);

		return httpClient;
	}

	/// <summary>
	/// 設定 Twitter 網站需要的連線請求標頭
	/// </summary>
	/// <param name="httpClient">HttpClient</param>
	/// <param name="authToken">字串，Cookie 參數 auth_token</param>
	/// <param name="csrfToken">字串，Cookie 參數 ct0</param>
	public static void SetTwitterRequestHeader(HttpClient httpClient, string authToken, string csrfToken)
	{
		httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {StringSet.PublicBearerToken}");
		httpClient.DefaultRequestHeaders.Add("Cookie", $"auth_token={authToken};ct0={csrfToken};");
		httpClient.DefaultRequestHeaders.Add("Referer", "https://twitter.com/");
		httpClient.DefaultRequestHeaders.Add("x-csrf-token", csrfToken);
		httpClient.DefaultRequestHeaders.Add("x-twitter-active-user", "yes");
		httpClient.DefaultRequestHeaders.Add("x-twitter-client-language", "zh-tw");
	}

	/// <summary>
	/// 為 HttpClient 設定指定的使用者代理字串
	/// </summary>
	/// <param name="httpClient">HttpClient</param>
	/// <param name="userAgent">字串，使用者代理字串</param>
	/// <returns>布林值</returns>
	public static bool SetUserAgent(HttpClient httpClient, string userAgent)
	{
		if (string.IsNullOrEmpty(userAgent))
		{
			return false;
		}

		bool canAdd = CanAddUserAgent(httpClient, userAgent);

		if (canAdd)
		{
			httpClient.DefaultRequestHeaders.UserAgent.Clear();
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
		}

		return canAdd;
	}

	/// <summary>
	/// 檢查使用者代理字串是否可以被新增
	/// </summary>
	/// <param name="httpClient">HttpClient</param>
	/// <param name="userAgent">字串，使用者代理字串</param>
	/// <returns>布林值</returns>
	public static bool CanAddUserAgent(HttpClient httpClient, string userAgent)
	{
		return httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);
	}
}