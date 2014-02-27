using System;
using System.Windows.Forms;
using System.Linq;

namespace ChatMio
{
	public partial class LogListForm : Form
    {
        #region コンストラクタ
        public LogListForm ()
		{
			InitializeComponent();
		}
        #endregion //コンストラクタ

        private void LogListForm_Load (object sender, EventArgs e)				//フォームロード時
		{
			dataGridView.DataSource = ChatLog.LogList;							//dataGridViewにデータをセット
		}

		private void printButton_Click (object sender, EventArgs e)				//「印刷」ボタンクリック時
		{
            if (dataGridView.SelectedRows.Count != 1) {                         // ログが一つだけ選択されているのでなかった場合
                MessageBox.Show(this, "ログを一つ選択してください", "エラー",   // メッセージを出す
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;                                                         // 終了
            }

            DataGridViewRow row = dataGridView.SelectedRows[0];                 // 選択されている行
            string fileName = row.Cells.Cast<DataGridViewCell>()
                    .Where(c => c.OwningColumn.DataPropertyName == "FileName")  // 列のプロパティがFileNameであるセルを選ぶ
                    .First().Value.ToString();
            ChatLog.Print(fileName, true);                                      // 印刷
		}

        private void dataGridView_CellContentDoubleClick (object sender, DataGridViewCellEventArgs e) // セルがダブルクリックされた時
        {
            if (e.RowIndex == -1) { return; }                                   // headerがダブルクリックされていた場合終了

            DataGridViewRow row = dataGridView.Rows[e.RowIndex];                // ダブルクリックされた行 
            string fileName = row.Cells.Cast<DataGridViewCell>()
                    .Where(c => c.OwningColumn.DataPropertyName == "FileName")  // 列のプロパティがFileNameであるセルを選ぶ
                    .First().Value.ToString();
            ChatLog.Print(fileName, true);                                      // 印刷
        }
	}
}
