namespace TweetLottery.Codes.Models;

/// <summary>
/// 推文資料
/// </summary>
public class TweetData
{
    /// <summary>
    /// 建立於
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// ID 值
    /// </summary>
    public long? ID { get; set; }

    /// <summary>
    /// ID 值字串
    /// </summary>
    public string? IDStr { get; set; }

    /// <summary>
    /// 推文全文
    /// </summary>
    public string? FullText { get; set; }

    /// <summary>
    /// 使用者 ID 值
    /// </summary>
    public long? UserID { get; set; }

    /// <summary>
    /// 使用者 ID 值字串
    /// </summary>
    public string? UserIDStr { get; set; }

    /// <summary>
    /// 使用者資料
    /// </summary>
    public UserData? UserData { get; set; }
}