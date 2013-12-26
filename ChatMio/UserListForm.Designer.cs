namespace ChatMio
{
	partial class UserListForm
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.modifyButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView
			// 
			this.dataGridView.AllowUserToAddRows = false;
			this.dataGridView.AllowUserToDeleteRows = false;
			this.dataGridView.AllowUserToOrderColumns = true;
			this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView.BackgroundColor = System.Drawing.Color.Pink;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.Pink;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Red;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Pink;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Purple;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.GridColor = System.Drawing.Color.DeepPink;
			this.dataGridView.Location = new System.Drawing.Point(12, 12);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.ReadOnly = true;
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.Pink;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Red;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Pink;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Purple;
			this.dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridView.RowTemplate.Height = 21;
			this.dataGridView.Size = new System.Drawing.Size(414, 276);
			this.dataGridView.TabIndex = 25;
			// 
			// modifyButton
			// 
			this.modifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.modifyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.modifyButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.modifyButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.modifyButton.Location = new System.Drawing.Point(90, 294);
			this.modifyButton.Name = "modifyButton";
			this.modifyButton.Size = new System.Drawing.Size(165, 42);
			this.modifyButton.TabIndex = 26;
			this.modifyButton.Text = "修正";
			this.modifyButton.UseVisualStyleBackColor = true;
			this.modifyButton.Click += new System.EventHandler(this.modifyButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.removeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.removeButton.Font = new System.Drawing.Font("メイリオ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.removeButton.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.removeButton.Location = new System.Drawing.Point(261, 294);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(165, 42);
			this.removeButton.TabIndex = 27;
			this.removeButton.Text = "削除";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// UserListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Crimson;
			this.ClientSize = new System.Drawing.Size(438, 348);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.modifyButton);
			this.Controls.Add(this.dataGridView);
			this.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.Name = "UserListForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ユーザー情報一覧";
			this.Load += new System.EventHandler(this.UserListForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.Button modifyButton;
		private System.Windows.Forms.Button removeButton;
	}
}