namespace TweetLottery
{
	partial class FDrawResult
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			LVDrawResult = new ListView();
			BtnExportDrawResult = new Button();
			SuspendLayout();
			// 
			// LVDrawResult
			// 
			LVDrawResult.FullRowSelect = true;
			LVDrawResult.GridLines = true;
			LVDrawResult.Location = new Point(12, 41);
			LVDrawResult.Name = "LVDrawResult";
			LVDrawResult.Size = new Size(776, 394);
			LVDrawResult.TabIndex = 1;
			LVDrawResult.UseCompatibleStateImageBehavior = false;
			LVDrawResult.View = View.Details;
			LVDrawResult.MouseClick += LVDrawResult_MouseClick;
			LVDrawResult.MouseDoubleClick += LVDrawResult_MouseDoubleClick;
			// 
			// BtnExportDrawResult
			// 
			BtnExportDrawResult.Location = new Point(698, 12);
			BtnExportDrawResult.Name = "BtnExportDrawResult";
			BtnExportDrawResult.Size = new Size(90, 23);
			BtnExportDrawResult.TabIndex = 0;
			BtnExportDrawResult.Text = "匯出抽取結果";
			BtnExportDrawResult.UseVisualStyleBackColor = true;
			BtnExportDrawResult.Click += BtnExportDrawResult_Click;
			// 
			// FDrawResult
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 447);
			Controls.Add(BtnExportDrawResult);
			Controls.Add(LVDrawResult);
			FormBorderStyle = FormBorderStyle.Fixed3D;
			MaximizeBox = false;
			Name = "FDrawResult";
			Text = "抽取結果";
			FormClosing += FDrawResult_FormClosing;
			Load += FDrawResult_Load;
			ResumeLayout(false);
		}

		#endregion

		private ListView LVDrawResult;
		private Button BtnExportDrawResult;
	}
}