using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Threading;

namespace ChatMio
{
	static class Program
	{
		/// <summary>
		/// アプリケーション終了フラグ
		/// </summary>
		public static bool Exit { get; set; }

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main ()
		{
			using (var mtx = new Mutex(false, "ChatMio")) {
				if (!mtx.WaitOne(0, false)) {
					MessageBox.Show("2つ以上のChatMioを同時に使うことはできません。",
							"複数起動禁止", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}

				Exit = false;													//終了フラグの初期値

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				//GenerateTestData(1000);

				while (true) {													//無限ループ
					var loginForm = new LoginForm();							
					DialogResult login = loginForm.ShowDialog();				//ログインフォームを表示

					if (login == DialogResult.OK) {								//ログイン成功時
						Application.Run(new ChatForm());						//ChatForm表示
					}
					else if (login == DialogResult.Cancel) {					//登録ボタン押下時
						var registerForm = new RegisterForm();
						registerForm.ShowDialog();								//RegisterForm表示
					}
					else if (login == DialogResult.Abort) {						//×ボタン押下時
						break;													//終了
					}

					if (Exit == true) { break; }								//フラグが立っていたら終了する
				}
			}
		}
	}
}
