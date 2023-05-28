namespace TweetLottery
{
    partial class FMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            BtnFetchTweets = new Button();
            LVFetchedTweets = new ListView();
            TBLog = new TextBox();
            TBAuthToken = new TextBox();
            TBCsrfToken = new TextBox();
            TBQueryString = new TextBox();
            BtnCancel = new Button();
            LAuthToken = new Label();
            LCsrfToken = new Label();
            LQueryString = new Label();
            BtnReset = new Button();
            LNumber = new Label();
            NUPDrawAmount = new NumericUpDown();
            BtnDrawTweets = new Button();
            LVersion = new Label();
            CBExcludeSameUser = new CheckBox();
            BtnExportTweets = new Button();
            CBNotEmulateManualSurf = new CheckBox();
            CBNotDownloadProfileImage = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)NUPDrawAmount).BeginInit();
            SuspendLayout();
            // 
            // BtnFetchTweets
            // 
            BtnFetchTweets.Location = new Point(713, 12);
            BtnFetchTweets.Name = "BtnFetchTweets";
            BtnFetchTweets.Size = new Size(75, 23);
            BtnFetchTweets.TabIndex = 6;
            BtnFetchTweets.Text = "獲取推文";
            BtnFetchTweets.UseVisualStyleBackColor = true;
            BtnFetchTweets.Click += BtnFetchTweets_Click;
            // 
            // LVFetchedTweets
            // 
            LVFetchedTweets.FullRowSelect = true;
            LVFetchedTweets.GridLines = true;
            LVFetchedTweets.Location = new Point(12, 147);
            LVFetchedTweets.Name = "LVFetchedTweets";
            LVFetchedTweets.Size = new Size(776, 227);
            LVFetchedTweets.TabIndex = 16;
            LVFetchedTweets.UseCompatibleStateImageBehavior = false;
            LVFetchedTweets.View = View.Details;
            LVFetchedTweets.MouseClick += LVFetchedTweets_MouseClick;
            LVFetchedTweets.MouseDoubleClick += LVFetchedTweets_MouseDoubleClick;
            // 
            // TBLog
            // 
            TBLog.Location = new Point(12, 380);
            TBLog.Multiline = true;
            TBLog.Name = "TBLog";
            TBLog.ReadOnly = true;
            TBLog.ScrollBars = ScrollBars.Both;
            TBLog.Size = new Size(776, 100);
            TBLog.TabIndex = 17;
            // 
            // TBAuthToken
            // 
            TBAuthToken.Location = new Point(93, 6);
            TBAuthToken.Name = "TBAuthToken";
            TBAuthToken.Size = new Size(614, 23);
            TBAuthToken.TabIndex = 1;
            // 
            // TBCsrfToken
            // 
            TBCsrfToken.Location = new Point(92, 35);
            TBCsrfToken.Name = "TBCsrfToken";
            TBCsrfToken.Size = new Size(614, 23);
            TBCsrfToken.TabIndex = 3;
            // 
            // TBQueryString
            // 
            TBQueryString.Location = new Point(92, 64);
            TBQueryString.Name = "TBQueryString";
            TBQueryString.Size = new Size(614, 23);
            TBQueryString.TabIndex = 5;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(713, 41);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 7;
            BtnCancel.Text = "取消";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // LAuthToken
            // 
            LAuthToken.AutoSize = true;
            LAuthToken.Location = new Point(12, 9);
            LAuthToken.Name = "LAuthToken";
            LAuthToken.Size = new Size(71, 15);
            LAuthToken.TabIndex = 0;
            LAuthToken.Text = "Auth Token";
            // 
            // LCsrfToken
            // 
            LCsrfToken.AutoSize = true;
            LCsrfToken.Location = new Point(12, 38);
            LCsrfToken.Name = "LCsrfToken";
            LCsrfToken.Size = new Size(74, 15);
            LCsrfToken.TabIndex = 2;
            LCsrfToken.Text = "CSRF Token";
            // 
            // LQueryString
            // 
            LQueryString.AutoSize = true;
            LQueryString.Location = new Point(12, 67);
            LQueryString.Name = "LQueryString";
            LQueryString.Size = new Size(55, 15);
            LQueryString.TabIndex = 4;
            LQueryString.Text = "查詢字串";
            // 
            // BtnReset
            // 
            BtnReset.Location = new Point(713, 70);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(75, 23);
            BtnReset.TabIndex = 8;
            BtnReset.Text = "重設";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // LNumber
            // 
            LNumber.AutoSize = true;
            LNumber.Location = new Point(12, 123);
            LNumber.Name = "LNumber";
            LNumber.Size = new Size(55, 15);
            LNumber.TabIndex = 12;
            LNumber.Text = "抽取數量";
            // 
            // NUPDrawAmount
            // 
            NUPDrawAmount.Location = new Point(93, 118);
            NUPDrawAmount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            NUPDrawAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            NUPDrawAmount.Name = "NUPDrawAmount";
            NUPDrawAmount.Size = new Size(120, 23);
            NUPDrawAmount.TabIndex = 13;
            NUPDrawAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // BtnDrawTweets
            // 
            BtnDrawTweets.Location = new Point(335, 119);
            BtnDrawTweets.Name = "BtnDrawTweets";
            BtnDrawTweets.Size = new Size(75, 23);
            BtnDrawTweets.TabIndex = 15;
            BtnDrawTweets.Text = "抽取推文";
            BtnDrawTweets.UseVisualStyleBackColor = true;
            BtnDrawTweets.Click += BtnDrawTweets_Click;
            // 
            // LVersion
            // 
            LVersion.AutoSize = true;
            LVersion.Location = new Point(12, 483);
            LVersion.Name = "LVersion";
            LVersion.Size = new Size(55, 15);
            LVersion.TabIndex = 18;
            LVersion.Text = "版本號：";
            // 
            // CBExcludeSameUser
            // 
            CBExcludeSameUser.AutoSize = true;
            CBExcludeSameUser.Location = new Point(219, 122);
            CBExcludeSameUser.Name = "CBExcludeSameUser";
            CBExcludeSameUser.Size = new Size(110, 19);
            CBExcludeSameUser.TabIndex = 14;
            CBExcludeSameUser.Text = "排除相同使用者";
            CBExcludeSameUser.UseVisualStyleBackColor = true;
            // 
            // BtnExportTweets
            // 
            BtnExportTweets.Location = new Point(632, 93);
            BtnExportTweets.Name = "BtnExportTweets";
            BtnExportTweets.Size = new Size(75, 23);
            BtnExportTweets.TabIndex = 11;
            BtnExportTweets.Text = "匯出推文";
            BtnExportTweets.UseVisualStyleBackColor = true;
            BtnExportTweets.Click += BtnExportTweets_Click;
            // 
            // CBNotEmulateManualSurf
            // 
            CBNotEmulateManualSurf.AutoSize = true;
            CBNotEmulateManualSurf.Location = new Point(93, 93);
            CBNotEmulateManualSurf.Name = "CBNotEmulateManualSurf";
            CBNotEmulateManualSurf.Size = new Size(110, 19);
            CBNotEmulateManualSurf.TabIndex = 9;
            CBNotEmulateManualSurf.Text = "不模擬人工瀏覽";
            CBNotEmulateManualSurf.UseVisualStyleBackColor = true;
            // 
            // CBNotDownloadProfileImage
            // 
            CBNotDownloadProfileImage.AutoSize = true;
            CBNotDownloadProfileImage.Location = new Point(209, 93);
            CBNotDownloadProfileImage.Name = "CBNotDownloadProfileImage";
            CBNotDownloadProfileImage.Size = new Size(134, 19);
            CBNotDownloadProfileImage.TabIndex = 10;
            CBNotDownloadProfileImage.Text = "不下載個人檔案圖檔";
            CBNotDownloadProfileImage.UseVisualStyleBackColor = true;
            // 
            // FMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 507);
            Controls.Add(CBNotDownloadProfileImage);
            Controls.Add(CBNotEmulateManualSurf);
            Controls.Add(BtnExportTweets);
            Controls.Add(CBExcludeSameUser);
            Controls.Add(LVersion);
            Controls.Add(BtnDrawTweets);
            Controls.Add(NUPDrawAmount);
            Controls.Add(LNumber);
            Controls.Add(BtnReset);
            Controls.Add(LQueryString);
            Controls.Add(LCsrfToken);
            Controls.Add(LAuthToken);
            Controls.Add(BtnCancel);
            Controls.Add(TBQueryString);
            Controls.Add(TBCsrfToken);
            Controls.Add(TBAuthToken);
            Controls.Add(TBLog);
            Controls.Add(LVFetchedTweets);
            Controls.Add(BtnFetchTweets);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FMain";
            Text = "推文抽獎";
            Load += MForm_Load;
            ((System.ComponentModel.ISupportInitialize)NUPDrawAmount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnFetchTweets;
        private ListView LVFetchedTweets;
        private TextBox TBLog;
        private TextBox TBAuthToken;
        private TextBox TBCsrfToken;
        private TextBox TBQueryString;
        private Button BtnCancel;
        private Label LAuthToken;
        private Label LCsrfToken;
        private Label LQueryString;
        private Button BtnReset;
        private Label LNumber;
        private NumericUpDown NUPDrawAmount;
        private Button BtnDrawTweets;
        private Label LVersion;
        private CheckBox CBExcludeSameUser;
        private Button BtnExportTweets;
        private CheckBox CBNotEmulateManualSurf;
        private CheckBox CBNotDownloadProfileImage;
    }
}