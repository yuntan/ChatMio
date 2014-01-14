using System;
using System.IO;
using System.Diagnostics;

namespace ChatMio
{
	class MyDebug
	{
		private static bool _usingLogFile;								//ログファイルが使用中であることを示すフラグ

		/// <summary>
		/// デバッグ出力とログファイルにログを吐く
		/// </summary>
		/// <param name="obj">this or null</param>
		/// <param name="msg">デバッグメッセージ</param>
		/// <param name="args">代入文字列</param>
		public static void WriteLine (object obj, string msg, params object[] args)
		{
			string message = String.Format("//{0}// {1}", obj ?? "null", String.Format(msg, args));
			Debug.WriteLine(message);									//デバッグ出力
			WriteLog(message);											//ログファイルに追記
		}

		/// <summary>
		/// ログファイルにログを吐く
		/// </summary>
		/// <param name="message">吐きたいログ</param>
		private static void WriteLog (string message)
		{
			string fileName = String.Format("{0}.log", DateTime.Now.ToString("yyMMdd"));

			if (!_usingLogFile) {										//ログファイルが使用中でなければ
				_usingLogFile = true;									//使用中フラグを立てる
				using (var w = new StreamWriter(fileName, true)) {
					w.WriteLine("{0}  {1}", DateTime.Now.ToString("HH:mm:ss"), message);
				}
				_usingLogFile = false;									//使用中フラグを下ろす
			}
		}
	}
}
