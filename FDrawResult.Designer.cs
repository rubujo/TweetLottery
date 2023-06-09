﻿namespace TweetLottery
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
			BtnRedrawWithSameRule = new Button();
			SuspendLayout();
			// 
			// LVDrawResult
			// 
			LVDrawResult.FullRowSelect = true;
			LVDrawResult.GridLines = true;
			LVDrawResult.Location = new Point(12, 12);
			LVDrawResult.Name = "LVDrawResult";
			LVDrawResult.Size = new Size(776, 394);
			LVDrawResult.TabIndex = 0;
			LVDrawResult.UseCompatibleStateImageBehavior = false;
			LVDrawResult.View = View.Details;
			LVDrawResult.MouseClick += LVDrawResult_MouseClick;
			LVDrawResult.MouseDoubleClick += LVDrawResult_MouseDoubleClick;
			// 
			// BtnExportDrawResult
			// 
			BtnExportDrawResult.Location = new Point(698, 412);
			BtnExportDrawResult.Name = "BtnExportDrawResult";
			BtnExportDrawResult.Size = new Size(90, 23);
			BtnExportDrawResult.TabIndex = 2;
			BtnExportDrawResult.Text = "匯出抽取結果";
			BtnExportDrawResult.UseVisualStyleBackColor = true;
			BtnExportDrawResult.Click += BtnExportDrawResult_Click;
			// 
			// BtnRedrawWithSameRule
			// 
			BtnRedrawWithSameRule.Location = new Point(592, 412);
			BtnRedrawWithSameRule.Name = "BtnRedrawWithSameRule";
			BtnRedrawWithSameRule.Size = new Size(100, 23);
			BtnRedrawWithSameRule.TabIndex = 1;
			BtnRedrawWithSameRule.Text = "以同樣條件重抽";
			BtnRedrawWithSameRule.UseVisualStyleBackColor = true;
			BtnRedrawWithSameRule.Click += BtnRedrawWithSameRule_Click;
			// 
			// FDrawResult
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 447);
			Controls.Add(BtnRedrawWithSameRule);
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
		private Button BtnRedrawWithSameRule;
	}
}