using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatMio
{
    public partial class PrintUsersForm : Form
    {
        #region コンストラクタ
        public PrintUsersForm ()
        {
            InitializeComponent();
        }
        #endregion //コンストラクタ

        #region UIのイベントハンドラ
        private void PrintUsersForm_Load (object sender, EventArgs e)
        {
            comboBox.Items.AddRange(Enum.GetNames(typeof(PrefEnum)));		//comboBoxに要素を追加
        }

        private void printButton_Click (object sender, EventArgs e)
        {
            if (comboBox.Text == "") {                                      //comboBoxが空の時
                MessageBox.Show(this, "出身地を選択してください");          //メッセージを表示
                return;                                                     //終了
            }

            if (MessageBox.Show(this,                                       //確認ダイアログを表示
                    "印刷しますか？\n印刷にはExcelを使用します。",
                    "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk,
                    MessageBoxDefaultButton.Button2) == DialogResult.OK) {
                Cursor = Cursors.WaitCursor;                                //待ち状態のカーソルに変更
                UserInfo.Print((PrefEnum) Enum.Parse(typeof(PrefEnum), comboBox.Text)); //印刷
                Cursor = Cursors.Default;                                   //通常のカーソルに変更
            }
        }

        private void cancelButton_Click (object sender, EventArgs e)
        {
            Close();                                                        //閉じる
        }
        #endregion //UIのイベントハンドラ
    }
}
