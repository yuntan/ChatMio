using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChatMio
{
	public partial class RegisterForm : Form
	{
		private UserData _user;														//UserData格納用変数
		private readonly bool _modifyMode;													//新規登録ではなく修正の場合true

		public RegisterForm ()
		{
			InitializeComponent();

			prefectureBox.Items.AddRange(Enum.GetNames(typeof(PrefEnum)));			//prefectureBoxに要素を追加
			colorBox.Items.AddRange(Enum.GetNames(typeof(KnownColor)));				//colorBoxに要素を追加
		}

		/// <summary>
		/// ユーザー情報修正用コンストラクタ
		/// </summary>
		/// <param name="name"></param>
		public RegisterForm (string name)
			: this()
		{
			_modifyMode = true;														//修正モードフラグを立てる
			if (!UserInfo.Read(name, out _user)) {									//ユーザー情報が見つからない場合
				this.Close();														//閉じる
				return;
			}

			nameBox.Text = _user.Name;												//ユーザー名をnameBoxにセット
			prefectureBox.SelectedIndex = (int) _user.IsFrom;						//出身地をprefectureBoxにセット
			colorBox.SelectedIndex = ((int) _user.TextColor) - 1;					//文字色をcolorBoxにセット
			sizeBox.Value = (decimal) _user.FontSize;
		}

		private void colorBox_DrawItem (object sender, DrawItemEventArgs e)			//owner-draw用メソッド
		{
			if (e.Index < 0 || e.Index > 173) { return; }							//e.Indexが不正な値でないかチェック

			e.DrawBackground();														//背景を描画

			var rectangle = new Rectangle(2, e.Bounds.Top + 2,						//正方形を描画
					e.Bounds.Height - 4, e.Bounds.Height - 4);
			e.Graphics.FillRectangle(												//単色で正方形を塗りつぶす
					new SolidBrush(Color.FromKnownColor((KnownColor) e.Index + 1)), rectangle);

			e.Graphics.DrawString(Enum.GetNames(typeof(KnownColor))[e.Index],		//文字を描画
					new Font("メイリオ", 12), Brushes.Red,
					new RectangleF(rectangle.Width + 4, e.Bounds.Y,
					e.Bounds.Width - rectangle.Width - 4, e.Bounds.Height));

			e.DrawFocusRectangle();													//フォーカス時のフィードバックを描画
		}

		private void sizeBox_ValueChanged (object sender, EventArgs e)				//sizeBoxの値が変更された時
		{
			sizePreviewBox.Font = 													//sizePreviewBoxの文字の大きさを変える
				new Font("メイリオ", ((float) (int) (sizeBox.Value * 10)) / 10, FontStyle.Bold);
		}

		private void registerButton_Click (object sender, EventArgs e)				//登録ボタン押下時
		{
			errorProvider.Clear();													//エラー表示をクリア

			bool errFlag = false;													//入力内容が不正であることを示すフラグ
			//項目を確認
			if (passBox.Text == "") {												//passBoxが空の時
				errorProvider.SetError(passBox, "パスワードを入力してください");
				passBox.Focus();													//passBoxをフォーカス
				errFlag = true;														//errFlagを立てる
			} 
			if (((int) (sizeBox.Value * 10)) % 5 != 0) {							//値が0.5刻みのものでなかった時
				errorProvider.SetError(sizeBox, "値は0.5刻みにしてください");
				sizeBox.Focus();													//sizeBoxをフォーカス
				errFlag = true;														//errFlagを立てる	
			}
			if (passBox.Text == Properties.Settings.Default.Piyo) {					//パスワードが管理者のものと重複していた場合
				errorProvider.SetError(passBox, "違うパスワードにしてください");
				passBox.Focus();													//passBoxをフォーカス
				passBox.SelectAll();												//passBox内を全選択
				errFlag = true;														//errFlagを立てる
			}
			if (colorBox.Text == "") {												//colorBoxが空の時
				errorProvider.SetError(colorBox, "文字色を選択してください");
				colorBox.Focus();													//colorBoxをフォーカス
				errFlag = true;														//errFlagを立てる
			}
			if (prefectureBox.Text == "") {											//prefectureBoxが空の時
				errorProvider.SetError(prefectureBox, "出身地を選択してください");
				prefectureBox.Focus();												//prefectureBoxをフォーカス
				errFlag = true;														//errFlagを立てる
			}
			if (nameBox.Text == "") {												//nameBoxが空の時
				errorProvider.SetError(nameBox, "ユーザー名を入力してください");
				nameBox.Focus();													//nameBoxをフォーカス
				errFlag = true;														//errFlagを立てる
			}

			if (errFlag) { return; }												//errFlagが立っていたら終了

			//errFlagが立っていない場合
			UserData data;
			if (UserInfo.Read(nameBox.Text, out data) && !_modifyMode) {			//ユーザー情報が重複しておりかつ修正モードでない場合
				MessageBox.Show(this, "ユーザー名が重複しています。", "登録失敗",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;																//終了
			}

			data = new UserData {													//フォームの内容を代入
				Name = nameBox.Text,
				Pass = passBox.Text,
				IsFrom = (PrefEnum) Enum.Parse(typeof(PrefEnum), prefectureBox.Text),
				TextColor = (KnownColor) Enum.Parse(
					typeof(KnownColor), colorBox.Text),
				FontSize = ((float) (int) (sizeBox.Value * 10)) / 10				//小数点第二位以下を切り捨てる
			};

			if (_modifyMode) { 														//修正モードの場合
				if (!UserInfo.Remove(_user.Name)) {									//登録前に元の情報を削除
					MessageBox.Show(this, "何かがおかしいです！", "登録失敗",		//削除失敗時
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			if (UserInfo.Write(data)) {												//データの書き込みが成功した場合
				MessageBox.Show(this, "ユーザー登録が完了しました。", "登録完了",
								MessageBoxButtons.OK, MessageBoxIcon.Information);
				if (!_modifyMode) { Properties.Settings.Default.LastUser = data.Name; }	//LastUserとして記録
				Close();															//閉じる
			}
			else {																	//データの書き込みに失敗した場合
				MessageBox.Show(this, "データの書き込みに失敗しました", "登録失敗",
				MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void clearButton_Click (object sender, EventArgs e)					//クリアボタン押下時
		{
			nameBox.Text = "";														//nameBoxを空に
			prefectureBox.SelectedIndex = -1;										//prefectureBoxを空に
			colorBox.SelectedIndex = -1;											//colorBoxを空に
			passBox.Text = "";														//passBoxを空に
			sizeBox.Value = 12m;													//sizeBoxをデフォルト値に
		}
	}
}
