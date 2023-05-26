﻿namespace TweetLottery.Codes.Extensions;

/// <summary>
/// ListView 的擴充方法
/// <para>來源：https://stackoverflow.com/a/40205173 </para>
/// </summary>
public static class ListViewExtension
{
	/// <summary>
	/// 取得選擇的 ListViewItem
	/// </summary>
	/// <param name="listView">ListView</param>
	/// <returns>IEnumerable&lt;ListViewItem&gt;</returns>
	public static IEnumerable<ListViewItem> GetSelectedListViewItems(this ListView listView)
	{
		return listView.SelectedItems.OfType<ListViewItem>();
	}

	/// <summary>
	///  取得 ListViewItem
	/// </summary>
	/// <param name="listView">ListView</param>
	/// <returns>IEnumerable&lt;ListViewItem&gt;</returns>
	public static IEnumerable<ListViewItem> GetListViewItems(this ListView listView)
	{
		return listView.Items.OfType<ListViewItem>();
	}
}