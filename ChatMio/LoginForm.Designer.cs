namespace ChatMio
{
	partial class LoginForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && ( components != null )) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.nameLabel = new System.Windows.Forms.Label();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.passLabel = new System.Windows.Forms.Label();
			this.passBox = new System.Windows.Forms.TextBox();
			this.loginButton = new System.Windows.Forms.Button();
			this.registerButton = new System.Windows.Forms.Button();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nameLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.nameLabel.Location = new System.Drawing.Point(45, 45);
			this.nameLabel.Margin = new System.Windows.Forms.Padding(36, 36, 0, 0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(107, 28);
			this.nameLabel.TabIndex = 4;
			this.nameLabel.Text = "ユーザー名";
			// 
			// nameBox
			// 
			this.nameBox.BackColor = System.Drawing.Color.Pink;
			this.nameBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nameBox.ForeColor = System.Drawing.Color.Red;
			this.nameBox.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.nameBox.Location = new System.Drawing.Point(167, 45);
			this.nameBox.Margin = new System.Windows.Forms.Padding(36);
			this.nameBox.MaxLength = 18;
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(200, 31);
			this.nameBox.TabIndex = 5;
			this.nameBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.nameBox.WordWrap = false;
			this.nameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameBox_KeyDown);
			// 
			// passLabel
			// 
			this.passLabel.AutoSize = true;
			this.passLabel.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.passLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.passLabel.Location = new System.Drawing.Point(45, 109);
			this.passLabel.Margin = new System.Windows.Forms.Padding(36, 36, 0, 0);
			this.passLabel.Name = "passLabel";
			this.passLabel.Size = new System.Drawing.Size(107, 28);
			this.passLabel.TabIndex = 6;
			this.passLabel.Text = "パスワード";
			// 
			// passBox
			// 
			this.passBox.BackColor = System.Drawing.Color.Pink;
			this.passBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.passBox.ForeColor = System.Drawing.Color.Red;
			this.passBox.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.passBox.Location = new System.Drawing.Point(167, 109);
			this.passBox.Margin = new System.Windows.Forms.Padding(36);
			this.passBox.MaxLength = 10;
			this.passBox.Name = "passBox";
			this.passBox.Size = new System.Drawing.Size(200, 31);
			this.passBox.TabIndex = 7;
			this.passBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.passBox.UseSystemPasswordChar = true;
			this.passBox.WordWrap = false;
			this.passBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passBox_KeyDown);
			// 
			// loginButton
			// 
			this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.loginButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.loginButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.loginButton.Location = new System.Drawing.Point(45, 173);
			this.loginButton.Margin = new System.Windows.Forms.Padding(36, 36, 0, 0);
			this.loginButton.Name = "loginButton";
			this.loginButton.Size = new System.Drawing.Size(145, 42);
			this.loginButton.TabIndex = 8;
			this.loginButton.Text = "ログイン";
			this.loginButton.UseVisualStyleBackColor = true;
			this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
			// 
			// registerButton
			// 
			this.registerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.registerButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.registerButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.registerButton.Location = new System.Drawing.Point(222, 173);
			this.registerButton.Margin = new System.Windows.Forms.Padding(36, 36, 0, 0);
			this.registerButton.Name = "registerButton";
			this.registerButton.Size = new System.Drawing.Size(145, 42);
			this.registerButton.TabIndex = 9;
			this.registerButton.Text = "登録";
			this.registerButton.UseVisualStyleBackColor = true;
			this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// LoginForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Crimson;
			this.ClientSize = new System.Drawing.Size(419, 251);
			this.Controls.Add(this.registerButton);
			this.Controls.Add(this.loginButton);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.nameBox);
			this.Controls.Add(this.passLabel);
			this.Controls.Add(this.passBox);
			this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(435, 290);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(435, 290);
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ChatMio ログイン";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoginForm_FormClosed);
			this.Load += new System.EventHandler(this.LoginForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label passLabel;
		private System.Windows.Forms.TextBox passBox;
		private System.Windows.Forms.Button loginButton;
		private System.Windows.Forms.Button registerButton;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}

