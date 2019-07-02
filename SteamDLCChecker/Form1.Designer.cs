namespace SteamDLCChecker
{
	partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			this.userInfo = new System.Windows.Forms.TextBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.button1 = new System.Windows.Forms.Button();
			this.appList = new System.Windows.Forms.ListBox();
			this.dlcList = new System.Windows.Forms.ListBox();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// userInfo
			// 
			this.userInfo.Location = new System.Drawing.Point(46, 64);
			this.userInfo.MaxLength = 0;
			this.userInfo.Multiline = true;
			this.userInfo.Name = "userInfo";
			this.userInfo.Size = new System.Drawing.Size(696, 25);
			this.userInfo.TabIndex = 0;
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(0, 52);
			this.linkLabel1.Location = new System.Drawing.Point(43, 33);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(508, 22);
			this.linkLabel1.TabIndex = 1;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "在网页上登陆steam账户后打开这个链接，将文本复制到下面这个对话框中";
			this.linkLabel1.UseCompatibleTextRendering = true;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(43, 188);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(696, 42);
			this.progressBar.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(298, 117);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(169, 33);
			this.button1.TabIndex = 3;
			this.button1.Text = "开始";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1_Click);
			// 
			// appList
			// 
			this.appList.FormattingEnabled = true;
			this.appList.ItemHeight = 15;
			this.appList.Location = new System.Drawing.Point(30, 275);
			this.appList.Name = "appList";
			this.appList.Size = new System.Drawing.Size(304, 184);
			this.appList.TabIndex = 4;
			this.appList.SelectedIndexChanged += new System.EventHandler(this.AppList_SelectedIndexChanged);
			// 
			// dlcList
			// 
			this.dlcList.FormattingEnabled = true;
			this.dlcList.ItemHeight = 15;
			this.dlcList.Location = new System.Drawing.Point(363, 275);
			this.dlcList.Name = "dlcList";
			this.dlcList.Size = new System.Drawing.Size(376, 184);
			this.dlcList.TabIndex = 5;
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Location = new System.Drawing.Point(654, 488);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(97, 15);
			this.linkLabel2.TabIndex = 6;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "获取最新地址";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel2_LinkClicked);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 525);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.dlcList);
			this.Controls.Add(this.appList);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.userInfo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Steam DLC 检漏 2019.7.2.1 by Chill";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox userInfo;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox appList;
		private System.Windows.Forms.ListBox dlcList;
		private System.Windows.Forms.LinkLabel linkLabel2;
	}
}

