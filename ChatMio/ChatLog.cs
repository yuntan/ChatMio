using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

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
			string fileName = String.Format(									//ファイル名を指定
					"chatmio_{0}_{1}.txt", DateTime.Now.ToString("yyMMdd_HHmmss"), user);
			using (var w = new StreamWriter(fileName)) {
				w.Write(str);													//ファイル書き込み
			}

			return fileName;													//ファイル名を返す
		}

		public static LogData[] LogList
		{
			get
			{
				string[] logNames = Directory.GetFiles(@".\", "chatmio_*.txt");	//ログファイルを検索
				var ret = new LogData[logNames.Length];							//返り値
				for (int i = 0; i < logNames.Length; i++) {
					var log = ParseLogFileName(logNames[i].Substring(2));		//ファイル名をパース
					ret[i] = log;												//配列に追加
				}
				return ret;														//配列を返す
			}
		}

		public static void Print (string fileName, bool showPreview)
		{
			LogData log = ParseLogFileName(fileName);							//ファイル名をパース

			var pd = new PrintDocument();				   						//PrindDocumentのインスタンス
			pd.PrintPage += delegate(object sender, PrintPageEventArgs e) {		//実際に印刷を行うデリゲートを追加
				#region 印刷を行うデリゲート
				var pen = new Pen(Color.Black, 1);								//デフォルトで幅1のペンを作成
				const int paperWidth = 210, paperHeight = 297;					//用紙の幅、高さ
				e.PageSettings.PaperSize = new PaperSize("A4", 210, 297);		//用紙サイズはA4
				e.Graphics.PageUnit = GraphicsUnit.Millimeter;					//ミリ単位で扱う

				//タイトルをセンタリングして描画
				const string title = "ChatMio チャットログ";
				var font = new Font("ＭＳ ゴシック", 21);						//フォントサイズを21に
				SizeF size = e.Graphics.MeasureString(title, font, paperWidth);	//描画時の文字列の長さを計測
				int x = Convert.ToInt32(paperWidth / 2 - size.Width / 2);		//X座標を求める
				e.Graphics.DrawString(title, font, Brushes.Black, x, 10);		//描画する

				//サブタイトルをセンタリングして描画
				string subTitle = String.Format("{0}  {1}との会話", log.Date, log.HerName);
				font = new Font("ＭＳ ゴシック", 15);							//フォントサイズを15に
				size = e.Graphics.MeasureString(subTitle, font, paperWidth);	//描画時の文字列の長さを計測
				x = Convert.ToInt32(paperWidth / 2 - size.Width / 2);			//X座標を求める
				e.Graphics.DrawString(subTitle, font, Brushes.Black, x, 25);	//描画する

				//チャットログを囲むボックスを描画
				const int rectX = 10, rectY = 40,								//(x,y)=(10,40)
						rectW = paperWidth - rectX * 2, rectH = paperHeight - rectY - 10;
				e.Graphics.DrawRectangle(pen, rectX, rectY, rectW, rectH);		//描画する

				//チャットログを描画
				font = new Font("ＭＳ ゴシック", 12);							//フォントサイズを12に
				const int lineHeight = 6;										//一行の高さ
				int linesPerPage = (rectH - 10) / lineHeight,					//1ページに入れることができる行数
					currentLine = 0;											//～行目
				using (var r = new StreamReader(fileName)) {
					while (linesPerPage > 0 && !r.EndOfStream) {				//行数オーバー又はファイル末尾まで来たら終了
						string s = r.ReadLine();
						e.Graphics.DrawString(s, font, Brushes.Black,			//描画する
								rectX + 5, rectY + currentLine * lineHeight + 5);
						linesPerPage--; currentLine++;
					}
					if (!r.EndOfStream) {
						e.HasMorePages = true;
					} //TODO 2ページ目以降
				}
				#endregion
			};

			if (showPreview) {
				var ppd = new PrintPreviewDialog { Document = pd };				//プレビューダイアログ
				ppd.ShowDialog();												//ダイアログを表示
			}
			else { pd.Print(); }												//印刷する
		}

		private static LogData ParseLogFileName (string fileName)
		{
			var log = new LogData();
			try {
				var date = String.Format("20{0}年{1}月{2}日",					//チャットの日付
						fileName.Substring(8, 2),								//ファイル名から年を取り出す
						int.Parse(fileName.Substring(10, 2)),					//ファイル名から月を取り出す
						int.Parse(fileName.Substring(12, 2)));					//ファイル名から日を取り出す
				log.Date = date;												//リストに日付を追加
				var time = String.Format("{0}時{1}分{2}秒",						//チャットの終了時刻
						int.Parse(fileName.Substring(15, 2)),					//ファイル名から時を取り出す
						int.Parse(fileName.Substring(17, 2)),					//ファイル名から分を取り出す
						int.Parse(fileName.Substring(19, 2)));					//ファイル名から秒を取り出す
				log.EndTime = time;												//リストに終了時刻を追加
				string herName = fileName.Substring(22, fileName.Length - 26);	//ファイル名からチャット相手の名前を取り出す
				log.HerName = herName;											//リストに名前を追加
				log.FileName = fileName;										//リストにファイル名を追加
			}
			catch (SystemException e) {
				MyDebug.WriteLine(null, "ChatLog.ParseLogFileName ファイル名のフォーマットが不正\n{0}", e);
			}
			return log;
		}
	}

	public class LogData
	{
		[DisplayName("日付")]
		public string Date { get; set; }

		[DisplayName("終了時刻")]
		public string EndTime { get; set; }

		[DisplayName("相手")]
		public string HerName { get; set; }

		[DisplayName("ファイル名")]
		public string FileName { get; set; }
	}
}
