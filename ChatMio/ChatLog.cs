using System;
using System.IO;

namespace ChatMio
{
	class ChatLog
	{
		/// <summary>
		/// チャットログを保存するメソッド
		/// </summary>
		/// <param name="user">チャット相手のユーザー名</param>
		/// <param name="str">書き込む文字列</param>
		/// <returns>書き込んだファイルのファイル名</returns>
		public static string Save (string user, string str)				
		{
			string fileName = String.Format(							//ファイル名を指定
					"chatmio_{0}_{1}.txt", DateTime.Now.ToString("yyMMdd_HHmmss"), user);
			using (var w = new StreamWriter(fileName)) {
				w.Write(str);											//ファイル書き込み
			}

			return fileName;											//ファイル名を返す
		}
	}
}
