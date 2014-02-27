using System;
using System.Windows.Forms;
using System.Net;

namespace ChatMio
{
	public partial class AddressListForm : Form
	{
		private IPAddress[] _addressList;										//IPアドレスのリスト

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addrs">IPアドレスのリスト</param>
		public AddressListForm (IPAddress[] addrs)
		{
			InitializeComponent();
			_addressList = addrs;
		}

		private void AddressListForm_Load (object sender, EventArgs e)			//フォームロード時
		{
			addressListBox.Items.AddRange(_addressList);						//_addressListをlistBoxにセット
			int idx = Properties.Settings.Default.LastIPIndex;					//最後に使用されたIPアドレスのインデックスを取得
			if (idx < _addressList.Length) {									//インデックスが不正な値でないとき
				addressListBox.SelectedIndex = idx;								//インデックスの項目を選択
			}
		}

		private void okButton_Click (object sender, EventArgs e)				//okButton押下時
		{
			Properties.Settings.Default.LastIPIndex = addressListBox.SelectedIndex;//選択されている項目のインデックスを保存
			Properties.Settings.Default.Save();
			this.Close();														//フォームを閉じる
		}
	}
}
