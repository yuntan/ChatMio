using System;
using System.Windows.Forms;

namespace ChatMio
{
	public partial class LogListForm : Form
	{
		public LogListForm ()
		{
			InitializeComponent();
		}

		private void LogListForm_Load (object sender, EventArgs e)				//フォームロード時
		{
			dataGridView.DataSource = ChatLog.LogList;							//dataGridViewにデータをセット
		}

		private void printButton_Click (object sender, EventArgs e)				//「印刷」ボタンクリック時
		{
			DataGridViewSelectedCellCollection cells = dataGridView.SelectedCells;
			if (cells.Count == 1) {												//選択中のセルが1つあった場合
				string fileName =												//選択中のログのファイル名を取得
						dataGridView.Rows[cells[0].RowIndex].Cells[3].Value.ToString();
				ChatLog.Print(fileName, true);									//ログファイルのプレビューを表示
			}
			else {																//セルが選択されていない又は2つ以上選択されている時
				MessageBox.Show(this, "セルを一つ選択してください", "エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
