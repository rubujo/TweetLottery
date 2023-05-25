using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Web;
using TweetLottery.Client.Codes.Extensions;
using TweetLottery.Client.Codes.Sets;
using TweetLottery.Codes.Models;

namespace TweetLottery.Codes.Utils;

/// <summary>
/// Twitter API 工具
/// </summary>
public static partial class TwitterApiUtil
{
	/// <summary>
	/// 取得 API 網址
	/// </summary>
	/// <param name="queryString">字串，查詢字串</param>
	/// <returns>字串</returns>
	public static string GetApiUrl(string queryString)
	{
		string encodedQueryString = HttpUtility.UrlEncode(queryString);

		// 目前最多為 20。
		int Count = 20;

		// 參考來源：https://zhuanlan.zhihu.com/p/422958616

		return $"{StringSet.ApiAdaptiveBaseUrl}?include_profile_interstitial_type=1" +
			"&include_blocking=1" +
			"&include_blocked_by=1" +
			"&include_followed_by=1" +
			"&include_want_retweets=1" +
			"&include_mute_edge=1" +
			"&include_can_dm=1" +
			"&include_can_media_tag=1" +
			"&include_ext_has_nft_avatar=1" +
			"&include_ext_is_blue_verified=1 " +
			"&include_ext_verified_type=1 " +
			"&include_ext_profile_image_shape=1" +
			"&skip_status=1" +
			"&cards_platform=Web-12" +
			"&include_cards=1" +
			"&include_ext_alt_text=true" +
			"&include_ext_limited_action_results=false" +
			"&include_quote_count=true" +
			"&include_reply_count=1" +
			"&tweet_mode=extended" +
			"&include_ext_views=true" +
			"&include_entities=true" +
			"&include_user_entities=true" +
			"&include_ext_media_color=true" +
			"&include_ext_media_availability=true" +
			"&include_ext_sensitive_media_warning=true" +
			"&include_ext_trusted_friends_metadata=true" +
			"&send_error_codes=true" +
			"&simple_quoted_tweet=true" +
			$"&q={encodedQueryString}" +
			"&query_source=hashtag_click" +
			$"&count={Count}" +
			"&requestContext=launch" +
			"&pc=1" +
			"&spelling_corrections=1" +
			"&include_ext_edit_control=true" +
			"&ext=mediaStats%2ChighlightedLabel%2ChasNftAvatar%2CvoiceInfo%2CbirdwatchPivot%2Cenrichments%2CsuperFollowMetadata%2CunmentionInfo%2CeditControl%2Cvibe";
	}

	/// <summary>
	/// 取得 adaptive.json 的資料
	/// </summary>
	/// <param name="httpClientFactory">IHttpClientFactory</param>
	/// <param name="apiUrl">字串，API 網址</param>
	/// <param name="authToken">字串，Cookie 參數 auth_token</param>
	/// <param name="csrfToken">字串，Cookie 參數 ct0</param>
	/// <param name="cursor">字串，查詢字串 cursor</param>
	/// <returns>Task&lt;JsonElement&gt;</returns>
	public static async Task<JsonElement?> GetAdaptiveJsonData(
		IHttpClientFactory httpClientFactory,
		string apiUrl,
		string authToken,
		string csrfToken,
		string cursor = "")
	{
		using HttpClient httpClient = HttpClientUtil
			.GetHttpClient(httpClientFactory, StringSet.UserAgent);

		// 設定 Twitter 網站需要的連線請求標頭。
		HttpClientUtil.SetTwitterRequestHeader(httpClient, authToken, csrfToken);

		// 設定 Client Hints。
		ClientHintsUtil.SetClientHints(httpClient);

		string targetApiUrl = apiUrl;

		// 判斷網址是否需要增加查詢字串 cursor。
		if (!string.IsNullOrEmpty(cursor))
		{
			targetApiUrl = targetApiUrl += $"&cursor={cursor}";
		}

		HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(targetApiUrl);

		string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();

		JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web)
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			WriteIndented = true,
		};

		return JsonSerializer.Deserialize<JsonElement>(jsonContent, jsonSerializerOptions);
	}

	/// <summary>
	/// 取得推文資料
	/// </summary>
	/// <param name="jsonElement">JsonElement</param>
	/// <returns>List&lt;TweetData&gt;</returns>
	public static List<TweetData> GetTweets(JsonElement? jsonElement)
	{
		List<TweetData> dataSet = new();

		JsonElement? jeTweets = jsonElement?.Get("globalObjects")?.Get("tweets");

		JsonProperty[]? jpTweets = jeTweets?.EnumerateObject().ToArray();

		if (jpTweets != null)
		{
			foreach (JsonProperty jsonProperty in jpTweets)
			{
				JsonElement jeTweet = jsonProperty.Value;

				dataSet.Add(new TweetData()
				{
					CreatedAt = DateTime.TryParseExact(
						jeTweet.Get("created_at")?.GetString(),
						"ddd MMM d HH:mm:ss zzz yyyy",
						CultureInfo.InvariantCulture,
						DateTimeStyles.None,
						out DateTime result) ? result : null,
					ID = jeTweet.Get("id")?.GetInt64(),
					IDStr = jeTweet.Get("id_str")?.GetString(),
					FullText = jeTweet.Get("full_text")?.GetString(),
					UserID = jeTweet.Get("user_id")?.GetInt64(),
					UserIDStr = jeTweet.Get("user_id_str")?.GetString()
				});
			}
		}

		return dataSet;
	}

	/// <summary>
	/// 取得使用者資料
	/// </summary>
	/// <param name="jsonElement">JsonElement</param>
	/// <returns>List&lt;UserData&gt;</returns>
	public static List<UserData> GetUsers(JsonElement? jsonElement)
	{
		List<UserData> dataSet = new();

		JsonElement? jeUsers = jsonElement?.Get("globalObjects")?.Get("users");

		JsonProperty[]? jpUsers = jeUsers?.EnumerateObject().ToArray();

		if (jpUsers != null)
		{
			foreach (JsonProperty jsonProperty in jpUsers)
			{
				JsonElement jeUser = jsonProperty.Value;

				dataSet.Add(new UserData()
				{
					ID = jeUser.Get("id")?.GetInt64(),
					IDStr = jeUser.Get("id_str")?.GetString(),
					Name = jeUser.Get("name")?.GetString(),
					ScreenName = jeUser.Get("screen_name")?.GetString(),
					ProfileImageUrlHttps = jeUser.Get("profile_image_url_https")?.GetString()
				});
			}
		}

		return dataSet;
	}

	/// <summary>
	/// 取得 cursor 字串
	/// </summary>
	/// <param name="jsonElement">JsonElement</param>
	/// <returns>字串</returns>
	public static string? GetCursorString(JsonElement? jsonElement)
	{
		string? cursor = string.Empty;

		JsonElement? jeInstructions = jsonElement?.Get("timeline")?.Get("instructions");

		if (jeInstructions?.ValueKind == JsonValueKind.Array)
		{
			// 備註：
			// 在第一次取得時 cursor 會包含在 addEntries 內的最後一個。
			// 在之後的取得，cursor 會在 replaceEntry 內。(最後一個)

			// 要取得第一個 addEntries。
			JsonElement? jeAddEntries = jeInstructions?.EnumerateArray().FirstOrDefault().Get("addEntries");

			if (jeAddEntries != null)
			{
				JsonElement? jeEntries = jeAddEntries?.Get("entries");

				if (jeEntries?.ValueKind == JsonValueKind.Array)
				{
					cursor = jeEntries?.EnumerateArray().LastOrDefault().Get("content")?.Get("operation")?.Get("cursor")?.Get("value")?.GetString();
				}
			}

			// 要取得最後一個 replaceEntry。
			JsonElement? jeReplaceEntry = jeInstructions?.EnumerateArray().LastOrDefault().Get("replaceEntry");

			if (jeReplaceEntry != null)
			{
				cursor = jeReplaceEntry?.Get("entry")?.Get("content")?.Get("operation")?.Get("cursor")?.Get("value")?.GetString();

				// 當 jeAddEntries 為 null 時，則表示沒有資料可再取得。
				if (jeAddEntries == null)
				{
					cursor = string.Empty;
				}
			}
		}

		return cursor;
	}
}