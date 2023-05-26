namespace TweetLottery;

// 阻擋設計工具。
partial class DesignerBlocker { };

/// <summary>
/// FDrawResult 變數
/// </summary>
public partial class FDrawResult
{
	/// <summary>
	/// 抽選到的項目資料
	/// </summary>
	private readonly List<ListViewItem>? _DataSet;

	/// <summary>
	/// FMain
	/// </summary>
	private readonly FMain _FMain;

	/// <summary>
	/// FMain 的 BtnFetchTweets
	/// </summary>
	private readonly Button _BtnFetchTweets;

	/// <summary>
	/// FMain 的 BtnReset
	/// </summary>
	private readonly Button _BtnReset;

	/// <summary>
	/// FMain 的 NUPDrawAmount
	/// </summary>
	private readonly NumericUpDown _NUPDrawAmount;

	/// <summary>
	/// FMain 的 BtnDrawTweets
	/// </summary>
	private readonly Button _BtnDrawTweets;

	/// <summary>
	/// FMain 的 BtnDrawTweets
	/// </summary>
	private readonly ListView _LVFetchedTweets;
}