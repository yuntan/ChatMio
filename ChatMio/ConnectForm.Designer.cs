namespace ChatMio
{
	partial class ConnectForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.cancelButton = new System.Windows.Forms.Button();
			this.connectButton = new System.Windows.Forms.Button();
			this.ipLabel = new System.Windows.Forms.Label();
			this.ipBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.cancelButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.cancelButton.Location = new System.Drawing.Point(229, 105);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(36, 36, 0, 0);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(145, 42);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "キャンセル";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// connectButton
			// 
			this.connectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.connectButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.connectButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.connectButton.Location = new System.Drawing.Point(52, 105);
			this.connectButton.Margin = new System.Windows.Forms.Padding(36, 36, 0, 0);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(145, 42);
			this.connectButton.TabIndex = 1;
			this.connectButton.Text = "接続";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
			// 
			// ipLabel
			// 
			this.ipLabel.AutoSize = true;
			this.ipLabel.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.ipLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.ipLabel.Location = new System.Drawing.Point(52, 40);
			this.ipLabel.Margin = new System.Windows.Forms.Padding(36, 36, 0, 0);
			this.ipLabel.Name = "ipLabel";
			this.ipLabel.Size = new System.Drawing.Size(111, 28);
			this.ipLabel.TabIndex = 10;
			this.ipLabel.Text = "IPアドレス";
			// 
			// ipBox
			// 
			this.ipBox.BackColor = System.Drawing.Color.Pink;
			this.ipBox.Font = new System.Drawing.Font("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.ipBox.ForeColor = System.Drawing.Color.Red;
			this.ipBox.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.ipBox.Location = new System.Drawing.Point(174, 40);
			this.ipBox.Margin = new System.Windows.Forms.Padding(36);
			this.ipBox.Name = "ipBox";
			this.ipBox.Size = new System.Drawing.Size(200, 31);
			this.ipBox.TabIndex = 0;
			this.ipBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ipBox.WordWrap = false;
			// 
			// ConnectForm
			// 
			this.AcceptButton = this.connectButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Crimson;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(419, 187);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.connectButton);
			this.Controls.Add(this.ipLabel);
			this.Controls.Add(this.ipBox);
			this.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "接続";
			this.Load += new System.EventHandler(this.ConnectForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.Label ipLabel;
		private System.Windows.Forms.TextBox ipBox;
	}
}