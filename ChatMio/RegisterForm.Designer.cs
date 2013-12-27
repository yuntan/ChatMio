namespace ChatMio
{
	partial class RegisterForm
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
			if (disposing && ( components != null )) {
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
			this.components = new System.ComponentModel.Container();
			this.registerButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.nameLabel = new System.Windows.Forms.Label();
			this.nameBox = new System.Windows.Forms.TextBox();
			this.passLabel = new System.Windows.Forms.Label();
			this.passBox = new System.Windows.Forms.TextBox();
			this.textColorLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.sizePreviewBox = new System.Windows.Forms.TextBox();
			this.sizeBox = new System.Windows.Forms.NumericUpDown();
			this.fontSizeLabel = new System.Windows.Forms.Label();
			this.colorBox = new System.Windows.Forms.ComboBox();
			this.fromLabel = new System.Windows.Forms.Label();
			this.prefectureBox = new System.Windows.Forms.ComboBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sizeBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// registerButton
			// 
			this.registerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.registerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.registerButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.registerButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.registerButton.Location = new System.Drawing.Point(33, 286);
			this.registerButton.Margin = new System.Windows.Forms.Padding(24, 24, 24, 12);
			this.registerButton.Name = "registerButton";
			this.registerButton.Size = new System.Drawing.Size(165, 42);
			this.registerButton.TabIndex = 1;
			this.registerButton.Text = "登録";
			this.registerButton.UseVisualStyleBackColor = true;
			this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.clearButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.clearButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.clearButton.Location = new System.Drawing.Point(250, 286);
			this.clearButton.Margin = new System.Windows.Forms.Padding(24, 24, 24, 12);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(166, 42);
			this.clearButton.TabIndex = 2;
			this.clearButton.Text = "クリア";
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// nameLabel
			// 
			this.nameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.nameLabel.AutoSize = true;
			this.nameLabel.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nameLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.nameLabel.Location = new System.Drawing.Point(31, 10);
			this.nameLabel.Margin = new System.Windows.Forms.Padding(0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(107, 28);
			this.nameLabel.TabIndex = 5;
			this.nameLabel.Text = "ユーザー名";
			// 
			// nameBox
			// 
			this.nameBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.nameBox.BackColor = System.Drawing.Color.Pink;
			this.nameBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nameBox.ForeColor = System.Drawing.Color.Red;
			this.nameBox.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.nameBox.Location = new System.Drawing.Point(185, 8);
			this.nameBox.Margin = new System.Windows.Forms.Padding(0);
			this.nameBox.MaxLength = 18;
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(200, 31);
			this.nameBox.TabIndex = 0;
			this.nameBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.nameBox.WordWrap = false;
			// 
			// passLabel
			// 
			this.passLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.passLabel.AutoSize = true;
			this.passLabel.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.passLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.passLabel.Location = new System.Drawing.Point(31, 202);
			this.passLabel.Margin = new System.Windows.Forms.Padding(0);
			this.passLabel.Name = "passLabel";
			this.passLabel.Size = new System.Drawing.Size(107, 28);
			this.passLabel.TabIndex = 6;
			this.passLabel.Text = "パスワード";
			// 
			// passBox
			// 
			this.passBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.passBox.BackColor = System.Drawing.Color.Pink;
			this.passBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.passBox.ForeColor = System.Drawing.Color.Red;
			this.passBox.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.passBox.Location = new System.Drawing.Point(185, 200);
			this.passBox.Margin = new System.Windows.Forms.Padding(0);
			this.passBox.MaxLength = 10;
			this.passBox.Name = "passBox";
			this.passBox.Size = new System.Drawing.Size(200, 31);
			this.passBox.TabIndex = 4;
			this.passBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.passBox.UseSystemPasswordChar = true;
			this.passBox.WordWrap = false;
			// 
			// textColorLabel
			// 
			this.textColorLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.textColorLabel.AutoSize = true;
			this.textColorLabel.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textColorLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.textColorLabel.Location = new System.Drawing.Point(41, 106);
			this.textColorLabel.Margin = new System.Windows.Forms.Padding(0);
			this.textColorLabel.Name = "textColorLabel";
			this.textColorLabel.Size = new System.Drawing.Size(88, 28);
			this.textColorLabel.TabIndex = 7;
			this.textColorLabel.Text = "文字の色";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
			this.tableLayoutPanel1.Controls.Add(this.colorBox, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.fromLabel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.nameLabel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.nameBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.textColorLabel, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.prefectureBox, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.fontSizeLabel, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.passLabel, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.passBox, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(27, 27);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(18);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 240);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.sizePreviewBox, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.sizeBox, 0, 0);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(170, 144);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(230, 48);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// sizePreviewBox
			// 
			this.sizePreviewBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.sizePreviewBox.BackColor = System.Drawing.Color.Pink;
			this.sizePreviewBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.sizePreviewBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.sizePreviewBox.ForeColor = System.Drawing.Color.Red;
			this.sizePreviewBox.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.sizePreviewBox.Location = new System.Drawing.Point(148, 8);
			this.sizePreviewBox.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
			this.sizePreviewBox.MaxLength = 2;
			this.sizePreviewBox.Name = "sizePreviewBox";
			this.sizePreviewBox.ReadOnly = true;
			this.sizePreviewBox.Size = new System.Drawing.Size(66, 31);
			this.sizePreviewBox.TabIndex = 1;
			this.sizePreviewBox.Text = "Aa";
			this.sizePreviewBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.sizePreviewBox.WordWrap = false;
			// 
			// sizeBox
			// 
			this.sizeBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.sizeBox.BackColor = System.Drawing.Color.Pink;
			this.sizeBox.DecimalPlaces = 1;
			this.sizeBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.sizeBox.ForeColor = System.Drawing.Color.Red;
			this.sizeBox.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.sizeBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
			this.sizeBox.Location = new System.Drawing.Point(16, 8);
			this.sizeBox.Margin = new System.Windows.Forms.Padding(16, 3, 3, 3);
			this.sizeBox.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.sizeBox.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.sizeBox.Name = "sizeBox";
			this.sizeBox.Size = new System.Drawing.Size(100, 31);
			this.sizeBox.TabIndex = 0;
			this.sizeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.sizeBox.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.sizeBox.ValueChanged += new System.EventHandler(this.sizeBox_ValueChanged);
			// 
			// fontSizeLabel
			// 
			this.fontSizeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.fontSizeLabel.AutoSize = true;
			this.fontSizeLabel.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.fontSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.fontSizeLabel.Location = new System.Drawing.Point(31, 154);
			this.fontSizeLabel.Margin = new System.Windows.Forms.Padding(0);
			this.fontSizeLabel.Name = "fontSizeLabel";
			this.fontSizeLabel.Size = new System.Drawing.Size(107, 28);
			this.fontSizeLabel.TabIndex = 9;
			this.fontSizeLabel.Text = "文字サイズ";
			// 
			// colorBox
			// 
			this.colorBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.colorBox.BackColor = System.Drawing.Color.Pink;
			this.colorBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.colorBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.colorBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.colorBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.colorBox.ForeColor = System.Drawing.Color.Red;
			this.colorBox.FormattingEnabled = true;
			this.colorBox.Location = new System.Drawing.Point(185, 104);
			this.colorBox.Margin = new System.Windows.Forms.Padding(0);
			this.colorBox.Name = "colorBox";
			this.colorBox.Size = new System.Drawing.Size(200, 32);
			this.colorBox.TabIndex = 2;
			this.colorBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.colorBox_DrawItem);
			// 
			// fromLabel
			// 
			this.fromLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.fromLabel.AutoSize = true;
			this.fromLabel.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.fromLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.fromLabel.Location = new System.Drawing.Point(50, 58);
			this.fromLabel.Margin = new System.Windows.Forms.Padding(0);
			this.fromLabel.Name = "fromLabel";
			this.fromLabel.Size = new System.Drawing.Size(69, 28);
			this.fromLabel.TabIndex = 8;
			this.fromLabel.Text = "出身地";
			// 
			// prefectureBox
			// 
			this.prefectureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.prefectureBox.BackColor = System.Drawing.Color.Pink;
			this.prefectureBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.prefectureBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.prefectureBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.prefectureBox.ForeColor = System.Drawing.Color.Red;
			this.prefectureBox.FormattingEnabled = true;
			this.prefectureBox.Location = new System.Drawing.Point(185, 56);
			this.prefectureBox.Margin = new System.Windows.Forms.Padding(0);
			this.prefectureBox.Name = "prefectureBox";
			this.prefectureBox.Size = new System.Drawing.Size(200, 32);
			this.prefectureBox.TabIndex = 1;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// RegisterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Crimson;
			this.ClientSize = new System.Drawing.Size(449, 349);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.registerButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RegisterForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ChatMio ユーザー登録";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sizeBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button registerButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.Label passLabel;
		private System.Windows.Forms.TextBox passBox;
		private System.Windows.Forms.Label textColorLabel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ComboBox colorBox;
		private System.Windows.Forms.Label fromLabel;
		private System.Windows.Forms.ComboBox prefectureBox;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Label fontSizeLabel;
		private System.Windows.Forms.NumericUpDown sizeBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TextBox sizePreviewBox;

	}
}