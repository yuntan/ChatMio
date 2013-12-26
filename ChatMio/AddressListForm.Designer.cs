namespace ChatMio
{
	partial class AddressListForm
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
			this.okButton = new System.Windows.Forms.Button();
			this.addressListBox = new System.Windows.Forms.ListBox();
			this.label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.okButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.okButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.okButton.Location = new System.Drawing.Point(32, 283);
			this.okButton.Margin = new System.Windows.Forms.Padding(36, 36, 0, 0);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(165, 42);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// addressListBox
			// 
			this.addressListBox.BackColor = System.Drawing.Color.Pink;
			this.addressListBox.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.addressListBox.ForeColor = System.Drawing.Color.Red;
			this.addressListBox.FormattingEnabled = true;
			this.addressListBox.ItemHeight = 16;
			this.addressListBox.Location = new System.Drawing.Point(32, 96);
			this.addressListBox.Name = "addressListBox";
			this.addressListBox.Size = new System.Drawing.Size(165, 148);
			this.addressListBox.TabIndex = 0;
			// 
			// label
			// 
			this.label.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label.Location = new System.Drawing.Point(9, 9);
			this.label.Margin = new System.Windows.Forms.Padding(0);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(210, 64);
			this.label.TabIndex = 20;
			this.label.Text = "使用するIPアドレスを選択してください";
			this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// AddressListForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Crimson;
			this.ClientSize = new System.Drawing.Size(228, 360);
			this.ControlBox = false;
			this.Controls.Add(this.label);
			this.Controls.Add(this.addressListBox);
			this.Controls.Add(this.okButton);
			this.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "AddressListForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "IPアドレスの選択";
			this.Load += new System.EventHandler(this.AddressListForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.ListBox addressListBox;
		private System.Windows.Forms.Label label;
	}
}