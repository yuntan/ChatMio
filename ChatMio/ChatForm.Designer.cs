namespace ChatMio
{
	partial class ChatForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent ()
		{
			this.components = new System.ComponentModel.Container();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.chatBox = new System.Windows.Forms.RichTextBox();
			this.postBox = new System.Windows.Forms.TextBox();
			this.menuButton = new System.Windows.Forms.Button();
			this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.connectMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.userListMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.modifyMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.removeUserMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.logoutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.exitMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.postButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.statusStrip.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 426);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 23, 0);
			this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.statusStrip.Size = new System.Drawing.Size(361, 22);
			this.statusStrip.TabIndex = 0;
			// 
			// statusLabel
			// 
			this.statusLabel.Font = new System.Drawing.Font("Meiryo UI", 10F);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// chatBox
			// 
			this.chatBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chatBox.BackColor = System.Drawing.Color.Pink;
			this.chatBox.ForeColor = System.Drawing.Color.Red;
			this.chatBox.Location = new System.Drawing.Point(12, 12);
			this.chatBox.Name = "chatBox";
			this.chatBox.ReadOnly = true;
			this.chatBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.chatBox.Size = new System.Drawing.Size(337, 302);
			this.chatBox.TabIndex = 0;
			this.chatBox.Text = "";
			// 
			// postBox
			// 
			this.postBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.postBox.BackColor = System.Drawing.Color.Pink;
			this.postBox.ForeColor = System.Drawing.Color.Red;
			this.postBox.Location = new System.Drawing.Point(6, 20);
			this.postBox.Multiline = true;
			this.postBox.Name = "postBox";
			this.postBox.Size = new System.Drawing.Size(256, 67);
			this.postBox.TabIndex = 0;
			// 
			// menuButton
			// 
			this.menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.menuButton.ContextMenuStrip = this.menuStrip;
			this.menuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.menuButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.menuButton.Location = new System.Drawing.Point(268, 19);
			this.menuButton.Name = "menuButton";
			this.menuButton.Size = new System.Drawing.Size(63, 31);
			this.menuButton.TabIndex = 2;
			this.menuButton.Text = "Menu";
			this.menuButton.UseVisualStyleBackColor = true;
			this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
			// 
			// menuStrip
			// 
			this.menuStrip.BackColor = System.Drawing.Color.Pink;
			this.menuStrip.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold);
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectMenu,
            this.userListMenu,
            this.modifyMenu,
            this.removeUserMenu,
            this.logoutMenu,
            this.exitMenu});
			this.menuStrip.Name = "contextMenuStrip1";
			this.menuStrip.Size = new System.Drawing.Size(172, 136);
			// 
			// connectMenu
			// 
			this.connectMenu.ForeColor = System.Drawing.Color.Red;
			this.connectMenu.Name = "connectMenu";
			this.connectMenu.Size = new System.Drawing.Size(171, 22);
			this.connectMenu.Text = "接続";
			this.connectMenu.Click += new System.EventHandler(this.connectMenu_Click);
			// 
			// userListMenu
			// 
			this.userListMenu.ForeColor = System.Drawing.Color.Red;
			this.userListMenu.Name = "userListMenu";
			this.userListMenu.Size = new System.Drawing.Size(171, 22);
			this.userListMenu.Text = "ユーザー情報一覧";
			this.userListMenu.Click += new System.EventHandler(this.userListMenu_Click);
			// 
			// modifyMenu
			// 
			this.modifyMenu.ForeColor = System.Drawing.Color.Red;
			this.modifyMenu.Name = "modifyMenu";
			this.modifyMenu.Size = new System.Drawing.Size(171, 22);
			this.modifyMenu.Text = "ユーザー情報変更";
			this.modifyMenu.Click += new System.EventHandler(this.modifyMenu_Click);
			// 
			// removeUserMenu
			// 
			this.removeUserMenu.ForeColor = System.Drawing.Color.Red;
			this.removeUserMenu.Name = "removeUserMenu";
			this.removeUserMenu.Size = new System.Drawing.Size(171, 22);
			this.removeUserMenu.Text = "ユーザー情報削除";
			this.removeUserMenu.Click += new System.EventHandler(this.removeUserMenu_Click);
			// 
			// logoutMenu
			// 
			this.logoutMenu.ForeColor = System.Drawing.Color.Red;
			this.logoutMenu.Name = "logoutMenu";
			this.logoutMenu.Size = new System.Drawing.Size(171, 22);
			this.logoutMenu.Text = "ログアウト";
			this.logoutMenu.Click += new System.EventHandler(this.logoutMenu_Click);
			// 
			// exitMenu
			// 
			this.exitMenu.ForeColor = System.Drawing.Color.Red;
			this.exitMenu.Name = "exitMenu";
			this.exitMenu.Size = new System.Drawing.Size(171, 22);
			this.exitMenu.Text = "終了";
			this.exitMenu.Click += new System.EventHandler(this.exitMenu_Click);
			// 
			// postButton
			// 
			this.postButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.postButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.postButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.postButton.Location = new System.Drawing.Point(268, 56);
			this.postButton.Name = "postButton";
			this.postButton.Size = new System.Drawing.Size(63, 31);
			this.postButton.TabIndex = 1;
			this.postButton.Text = "投稿";
			this.postButton.UseVisualStyleBackColor = true;
			this.postButton.Click += new System.EventHandler(this.postButton_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.postBox);
			this.groupBox1.Controls.Add(this.postButton);
			this.groupBox1.Controls.Add(this.menuButton);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.groupBox1.Location = new System.Drawing.Point(12, 320);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(337, 93);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// ChatForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Crimson;
			this.ClientSize = new System.Drawing.Size(361, 448);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.chatBox);
			this.Controls.Add(this.statusStrip);
			this.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "ChatForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ChatMio";
			this.Load += new System.EventHandler(this.ChatForm_Load);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.RichTextBox chatBox;
		private System.Windows.Forms.TextBox postBox;
		private System.Windows.Forms.Button menuButton;
		private System.Windows.Forms.ContextMenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem connectMenu;
		private System.Windows.Forms.Button postButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ToolStripMenuItem userListMenu;
		private System.Windows.Forms.ToolStripMenuItem modifyMenu;
		private System.Windows.Forms.ToolStripMenuItem removeUserMenu;
		private System.Windows.Forms.ToolStripMenuItem logoutMenu;
		private System.Windows.Forms.ToolStripMenuItem exitMenu;
	}
}

