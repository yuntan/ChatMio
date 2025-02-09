﻿using System;
using System.Windows.Forms;

namespace ChatMio
{
	public partial class LoginForm : Form
	{
		// 0: 画面を破棄 1: ログイン成功 2: 登録画面へ
		private int _status = 0;											//結果判別用フラグ

		public LoginForm ()
		{
			InitializeComponent();
		}

        private void LoginForm_Load (object sender, EventArgs e)			//フォームロード時
		{
			nameBox.Text = Properties.Settings.Default.LastUser;			//最後に使用したユーザーを読み込み表示
		}

		private void registerButton_Click (object sender, EventArgs e)		//登録ボタン押下時
		{
			_status = 2;													//登録画面へ進むためフラグを2にする
			Close();														//フォームを閉じる
		}

		private void loginButton_Click (object sender, EventArgs e)			//ログインボタン押下時
		{
			errorProvider.Clear();											//エラー表示をクリア

			bool errFlag = false;											//入力内容が不正であることを示すフラグ
			if (passBox.Text == "") {										//passBoxが空の時
				errorProvider.SetError(passBox, "パスワードを入力してください");//エラー表示
				passBox.Focus();											//passBoxをフォーカス
				errFlag = true;												//errFlagを立てる
			}
			if (nameBox.Text == "") {										//nameBoxが空の時
				errorProvider.SetError(nameBox, "ユーザー名を入力してください");//エラー表示
				nameBox.Focus();											//nameBoxをフォーカス
				errFlag = true;												//errFlagを立てる
			}

			if (errFlag) { return; }										//errFlagが立っていたら終了

			//errorFlagが立っていない場合
			UserData data;
			if (UserInfo.Read(nameBox.Text, out data)) {					//ユーザー情報が存在した場合
				if (passBox.Text == Properties.Settings.Default.Piyo) {		//パスワードが管理者のものだった場合
					_status = 1;											//ログイン成功フラグを立てる

					Properties.Settings.Default.LastUser = nameBox.Text;	//最後にログインしたユーザーとして保存
					Properties.Settings.Default.Pyon = true;				//管理者フラグオン
					Properties.Settings.Default.Save();

					Close();
					return;
				}
				if (passBox.Text == data.Pass) {							//パスワードが一致していた場合
					_status = 1;											//ログイン成功フラグを立てる

					Properties.Settings.Default.LastUser = nameBox.Text;	//最後にログインしたユーザーとして保存
					Properties.Settings.Default.Pyon = false;				//管理者フラグオフ
					Properties.Settings.Default.Save();

					Close();
					return;
				}

				//パスワードが間違っていた場合
				MessageBox.Show(this, "パスワードが間違っています。", "ログイン失敗",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				passBox.Focus();											//passBoxをフォーカス
				passBox.SelectAll();										//passBox内を全選択
				return;
			}

			//ユーザーが存在していなかった場合
			MessageBox.Show(this, "ユーザー名が間違っています。", "ログイン失敗",
				MessageBoxButtons.OK, MessageBoxIcon.Error);
			nameBox.Focus();												//nameBoxをフォーカス
			nameBox.SelectAll();											//nameBox内を全選択
		}

		private void LoginForm_FormClosed (object sender, FormClosedEventArgs e)
		{
			if (_status == 0) {												//×ボタンが押された時
				this.DialogResult = DialogResult.Abort;						//DialogResultをAbortにして閉じる
			}
			else if (_status == 1) {										//ログインが成功した時
				this.DialogResult = DialogResult.OK;						//DialogResultをOKにして閉じる
			}
			else if (_status == 2) {										//登録ボタン押下時
				this.DialogResult = DialogResult.Cancel;					//DialogResultをCancelにして閉じる
			}
		}
	}
}
