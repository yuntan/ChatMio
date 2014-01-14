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

		/// <summary>
		/// テストデータ生成用関数
		/// </summary>
		/// <param name="count">生成したいデータ数</param>
		static void GenerateTestData (int count)
		{
			var xmlDoc = new XmlDocument();
			XmlElement xmlRoot;

			xmlRoot = xmlDoc.CreateElement("UserInfo");
			xmlDoc.AppendChild(xmlRoot);

			for (int i = 0; i < count; i++) {
				var data = new UserData();
				data.Name = Path.GetRandomFileName();

				XmlElement userElem = xmlDoc.CreateElement("User");
				xmlRoot.AppendChild(userElem);

				XmlElement nameElem = xmlDoc.CreateElement("Name");
				nameElem.InnerText = data.Name;
				userElem.AppendChild(nameElem);

				XmlElement passElem = xmlDoc.CreateElement("Pass");
				passElem.InnerText = data.Pass;
				userElem.AppendChild(passElem);

				XmlElement isFromElem = xmlDoc.CreateElement("IsFrom");
				isFromElem.InnerText = data.IsFrom.ToString();
				userElem.AppendChild(isFromElem);

				XmlElement textColorElem = xmlDoc.CreateElement("TextColor");
				textColorElem.InnerText = data.TextColor.ToString();
				userElem.AppendChild(textColorElem);

				XmlElement fontSizeElem = xmlDoc.CreateElement("FontSize");
				fontSizeElem.InnerText = data.FontSize.ToString();
				userElem.AppendChild(fontSizeElem);

				if ((i + 1) % 10000 == 0) { Debug.WriteLine("{0}件目", i+1); }
			}

			try {
				xmlDoc.Save("UserInfo.xml");
				Debug.WriteLine("書き出し完了");
			}
			catch (SystemException) {
			}
		}
	}
}
