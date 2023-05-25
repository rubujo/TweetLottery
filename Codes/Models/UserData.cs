namespace TweetLottery.Codes.Models;

/// <summary>
/// 使用者資料
/// </summary>
public class UserData
{
    /// <summary>
    /// ID 值
    /// </summary>
    public long? ID { get; set; }

    /// <summary>
    /// ID 值字串
    /// </summary>
    public string? IDStr { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 螢幕名稱
    /// </summary>
    public string? ScreenName { get; set; }

    /// <summary>
    /// 個人檔案圖檔 HTTPS 網址
    /// </summary>
    public string? ProfileImageUrlHttps { get; set; }
}