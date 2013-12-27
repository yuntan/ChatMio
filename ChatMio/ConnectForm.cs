using System;
using System.Windows.Forms;
using System.Net;

namespace ChatMio
{
	public partial class ConnectForm : Form
	{
		public ConnectForm ()
		{
			InitializeComponent();
		}

		private void ConnectForm_Load (object sender, EventArgs e)			//フォームロード時
		{
			ipBox.Text = Properties.Settings.Default.LastIP;				//configファイルから最後に使用されたIPアドレスを取得
		}

		private void connectButton_Click (object sender, EventArgs e)		//接続ボタン押下時
		{
			IPAddress ip;
			if (IPAddress.TryParse(ipBox.Text.Trim(), out ip)) {			//有効なIPアドレスが入力された時
				Properties.Settings.Default.LastIP = ipBox.Text.Trim();		//configファイルにIPアドレスを保存
				Properties.Settings.Default.Save();

				DialogResult = DialogResult.OK;								//IPが入力されたことを親に知らせる
				this.Close();												//閉じる
			}
			else {
				MessageBox.Show(this, "IPアドレスを入力してください", "エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void cancelButton_Click (object sender, EventArgs e)		//cancelButton押下時
		{
			DialogResult = DialogResult.Cancel;								//IPが入力されなかったことを親に知らせる
			this.Close();													//閉じる
		}
	}
}
