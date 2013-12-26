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
	public partial class RegisterForm : Form
	{
		private UserData _user;														//UserData格納用変数
		private bool _modifyMode;													//新規登録ではなく修正の場合true

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
		}

		private void colorBox_DrawItem (object sender, DrawItemEventArgs e)			//owner-draw用メソッド
		{
			if (e.Index < 0 || e.Index > 173) { return; }							//e.Indexが不正な値でないかチェック

			e.DrawBackground();														//背景を描画

			Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2,				//正方形を描画
					e.Bounds.Height - 4, e.Bounds.Height - 4);
			e.Graphics.FillRectangle(												//単色で正方形を塗りつぶす
					new SolidBrush(Color.FromKnownColor((KnownColor) e.Index + 1)), rectangle);

			e.Graphics.DrawString(Enum.GetNames(typeof(KnownColor))[e.Index],		//文字を描画
					new Font("メイリオ", 12), System.Drawing.Brushes.Red, 
					new RectangleF(rectangle.Width + 4, e.Bounds.Y,
					e.Bounds.Width - rectangle.Width - 4, e.Bounds.Height));

			e.DrawFocusRectangle();													//フォーカス時のフィードバックを描画
		}

		private void registerButton_Click (object sender, EventArgs e)				//登録ボタン押下時
		{
			errorProvider.Clear();													//エラー表示をクリア

			//項目を確認
			if (nameBox.Text == "") {												//nameBoxが空の時
				errorProvider.SetError(nameBox, "ユーザー名を入力してください");
				return;
			}
			if (prefectureBox.Text == "") {											//prefectureBoxが空の時
				errorProvider.SetError(prefectureBox, "出身地を選択してください");
				return;
			}
			if (colorBox.Text == "") {												//colorBoxが空の時
				errorProvider.SetError(colorBox, "文字色を選択してください");
				return;
			}
			if (passBox.Text == "") {												//passBoxが空の時
				errorProvider.SetError(passBox, "パスワードを入力してください");
				return;
			}
			if (passBox.Text == Properties.Settings.Default.Piyo) {					//パスワードが管理者のものと重複していた場合
				errorProvider.SetError(passBox, "違うパスワードにしてください");
				return;
			}

			var data = new UserData();
			if ((!UserInfo.Read(nameBox.Text, out data) && !_modifyMode)			//ユーザー情報の重複がなくかつ修正モードでない場合
					|| _modifyMode) {												//又は修正モードの場合
				data.Name = nameBox.Text;											//各コントロールから値を取得してdataに代入
				data.Pass = passBox.Text;
				data.IsFrom = (PrefEnum) Enum.Parse(typeof(PrefEnum), prefectureBox.Text);
				data.TextColor = (System.Drawing.KnownColor) Enum.Parse(
					typeof(System.Drawing.KnownColor), colorBox.Text);
				data.FontSize = 12;

				if (_modifyMode) { 													//修正モードの場合
					if (UserInfo.Remove(_user.Name)) {								//登録前に元の情報を削除
						Properties.Settings.Default.LastUser = data.Name;
					}
					else {															//削除失敗時
						MessageBox.Show(this, "何かがおかしいです！", "登録失敗",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}

				if (UserInfo.Write(data)) {
					if (MessageBox.Show(this, "ユーザー登録が完了しました。", "登録完了",
										MessageBoxButtons.OK, MessageBoxIcon.Information)
							== DialogResult.OK) {
						Properties.Settings.Default.LastUser = data.Name;			//LastUserとして記録
						this.Close();												//閉じる
						return;
					}
				}
				else {																//データの書き込みに失敗した場合
					MessageBox.Show(this, "データの書き込みに失敗しました", "登録失敗",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else {																	//ユーザー名が重複しておりかつ修正モードでない場合
				MessageBox.Show(this, "ユーザー名が重複しています。", "登録失敗",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void clearButton_Click (object sender, EventArgs e)					//クリアボタン押下時
		{																				
			nameBox.Text = "";														//nameBoxを空に
			prefectureBox.SelectedIndex = -1;										//prefectureBoxを空に
			colorBox.SelectedIndex = -1;											//colorBoxを空に
			passBox.Text = "";														//passBoxを空に
		}
	}
}
