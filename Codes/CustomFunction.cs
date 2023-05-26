using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace TweetLottery.Codes;

/// <summary>
/// 自定義函式
/// </summary>
public class CustomFunction
{
    /// <summary>
    /// 開啟網頁瀏覽器
    /// <para>參考 1：https://github.com/dotnet/runtime/issues/17938#issuecomment-235502080 </para>
    /// <para>參考 2：https://github.com/dotnet/runtime/issues/17938#issuecomment-249383422 </para>
    /// </summary>
    /// <param name="url">字串，網址</param>
    public static void OpenBrowser(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
            url = url.Replace("&", "^&");

            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
        else
        {
            Debug.WriteLine("不支援的作業系統。");
        }
    }

    /// <summary>
    /// 取得隨機間隔值（毫秒）
    /// </summary>
	/// <param name="start">數值，起始數值，預設值為 3</param>
	/// <param name="end">數值，結束數值，預設值為 10</param>
	/// <returns>數值</returns>
    public static int GetRandomInterval(int start = 3, int end = 10)
    {
        return RandomNumberGenerator.GetInt32(start, end) * 1000;
    }
}