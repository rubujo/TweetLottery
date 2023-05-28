# 推文抽獎

簡易的 Twitter 推文隨機抽取應用程式，透過搜尋指定的`查詢字串`以獲取相關推文，配合自行設定的`抽取數量`，會在獲取到的推文中隨機抽取出指定數量的推文。

## 一、使用方式

1. 先從網頁瀏覽器瀏覽 Twitter 網站並登入您的帳號。
2. 開啟網頁瀏覽器的 `DevTools` （或類似的工具），點選`網路`分頁，在該分頁的`篩選`欄位輸入 `.json` 後，在其下方的`名稱`處手動篩選在`標頭`分頁中的`要求標頭`（Request Header）區塊有 `cookie` 值的項目。
3. 承 "2."，從該項目的 `cookie` 值中，取得 `auth_token` 跟 `ct0` （或取得 `x-csrf-token`）的值。
4. 開啟本應用程式程式，在對應欄位輸入對應值。
   - 在 `Auth Token` 欄位中輸入 `auth_token`。
   - 在 `CSRF Token` 欄位中輸入 `ct0` 或 `x-csrf-token`。
     - ※理論上 `ct0` 以及 `x-csrf-token` 的值會是一樣的。
   - 在`查詢字串`欄位中輸入要使用的`查詢字串`。
     - e.g. 如要查詢指定的 `Hashtag`，可以輸入如：`#測試用`。
5. 點選`獲取推文`按鈕，已開始獲取推文。
   - ※本應用程式是透過 Twitter 網站的 API（adaptive.json）來搜尋推文。
6. 在推文獲取完成後，設定`抽取數量`，最後再點選`抽取推文`按鈕即可。
   - 想要限制一個使用者只能抽取一則推文的話，可以勾選`排除相同使用者`選項即可。
   - ※`抽取數量`的上下限值為 1~1000。
7. 可以點選 `匯出推文` 或 `匯出抽取結果` 按鈕，以將對應的資料匯出成 Microsoft Excel 格式的檔案。

### (1). 額外功能

1. 對推文雙擊左鍵，會開啟推文的網址。
2. 對推文雙擊右鍵，會開啟發布該推文的使用者的 Twitter 網址。
3. 對推文單擊右鍵，會將該推文複製至剪貼簿。
4. 勾選`不模擬人工瀏覽`選項，將停用模擬人工瀏覽機制。
5. 勾選`不下載個人檔案圖檔`選項，將不會下載使用者個人檔案圖檔。
 
### (2). 模擬人工瀏覽機制

- 獲取推文，預設會隨機延遲 2~5 秒。
- 下載使用者個人檔案圖檔，預設會隨機延遲 3~7 秒。

## 二、注意事項以及免責聲明

1. 本應用程式的設計是在獲取全部可獲取的推文後，才開始處理獲取到的推文，所以應用程式會有段時間無回應是正常情況。
   - 每次獲取推文，一次最多只會回傳 20 則，需要以分頁的方式來獲取推文。
   - 有模擬人工瀏覽的機制。（隨機秒數暫停機制）
   - 包含下載使用者個人檔案圖檔。
2. 本應用程式是使用`偽亂數生成器`來產生隨機值，故無法保證其隨機性。
3. `使用本應用程式，無法保證您不會違反 Twitter 服務的服務條款以及您的 Twitter 帳號不會被禁用，請自行承擔相關的風險以及責任。`

## 三、參考來源

- [Twitter推特高级搜索接口分析及爬虫编写](https://zhuanlan.zhihu.com/p/422958616)