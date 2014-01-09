using System;
using System.Collections.Generic;
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
					var log = new LogData();
					try {
						logNames[i] = logNames[i].Substring(2);
						var date = String.Format("20{0}年{1}月{2}日",			//チャットの日付
								logNames[i].Substring(8, 2),					//ファイル名から年を取り出す
								int.Parse(logNames[i].Substring(10, 2)),		//ファイル名から月を取り出す
								int.Parse(logNames[i].Substring(12, 2)));		//ファイル名から日を取り出す
						log.Date = date;										//リストに日付を追加
						string herName = logNames[i].Substring(22, logNames[i].Length - 26);//ファイル名からチャット相手の名前を取り出す
						log.HerName = herName;									//リストに名前を追加
						log.FileName = logNames[i];								//リストにファイル名を追加
					}
					catch (SystemException e) {
						MyDebug.WriteLine(null, "ChatLog.LogList ファイル名のフォーマットが不正\n{0}", e);
					}
					ret[i] = log;
				}
				return ret;
			}
		}

		public static void Print (string fileName, bool showPreview)
		{
			var date = String.Format("20{0}年{1}月{2}日",						//チャットの日付
					fileName.Substring(8, 2),									//ファイル名から年を取り出す
					int.Parse(fileName.Substring(10, 2)),						//ファイル名から月を取り出す
					int.Parse(fileName.Substring(12, 2)));						//ファイル名から日を取り出す
			string herName = fileName.Substring(22, fileName.Length - 26);		//ファイル名からチャット相手の名前を取り出す

			var pd = new PrintDocument();

			pd.PrintPage += delegate(object sender, PrintPageEventArgs e) {		//実際に印刷を行うデリゲートを追加
				#region 印刷を行うデリゲート
				var pen = new Pen(Color.Black, 1);								//デフォルトで幅1のペンを作成
				const int paperWidth = 210, paperHeight = 297;					//用紙の幅、高さ
				e.PageSettings.PaperSize = new PaperSize("A4", 210, 297);		//用紙サイズはA4
				e.Graphics.PageUnit = GraphicsUnit.Millimeter;					//ミリ単位で扱う

				//タイトルをセンタリングして描画
				const string title = "ChatMio チャットログ";
				var font = new Font("ＭＳ ゴシック", 18);						//フォントサイズを18に
				SizeF size = e.Graphics.MeasureString(title, font, paperWidth);	//描画時の文字列の長さを計測
				int x = Convert.ToInt32(paperWidth / 2 - size.Width / 2);		//X座標を求める
				e.Graphics.DrawString(title, font, Brushes.Black, x, 30);		//描画する

				//サブタイトルをセンタリングして描画
				string subTitle = String.Format("{0}  {1}との会話", date, herName);
				font = new Font("ＭＳ ゴシック", 15);							//フォントサイズを15に
				size = e.Graphics.MeasureString(subTitle, font, paperWidth);	//描画時の文字列の長さを計測
				x = Convert.ToInt32(paperWidth / 2 - size.Width / 2);			//X座標を求める
				e.Graphics.DrawString(subTitle, font, Brushes.Black, x, 60);	//描画する

				//チャットログを囲むボックスを描画
				const int rectX = 10, rectY = 50,								//(x,y)=(10,50)	幅190 高さ500
						rectW = paperWidth - rectX * 2, rectH = 500;
				e.Graphics.DrawRectangle(pen, rectX, rectY, rectW, rectH);		//描画する

				//チャットログを描画
				font = new Font("ＭＳ ゴシック", 12);							//フォントサイズを12に
				const int lineHeight = 12 + 5;									//一行の高さ
				int linesPerPage = rectH / lineHeight,							//1ページに入れることができる行数
					currentLine = 0;											//～行目
				string s;
				using (var r = new StreamReader(fileName)) {
					while (linesPerPage > 0 || r.EndOfStream) {					//行数オーバー又はファイル末尾まで来たら終了
						s = r.ReadLine();										//一行読む
						e.Graphics.DrawString(s, font, Brushes.Black,			//描画する
								rectX + 5, rectY + currentLine * lineHeight);
						linesPerPage--; currentLine++;
					}
					if (!r.EndOfStream) { } //TODO 2ページ目以降
				}
				#endregion
			};

			if (showPreview) {
				var ppd = new PrintPreviewDialog { Document = pd };				//プレビューダイアログ
				ppd.ShowDialog();												//ダイアログを表示
			}
			else { pd.Print(); }												//印刷する
		}
	}

	public class LogData
	{
		[DisplayName("日付")]
		public string Date { get; set; }

		[DisplayName("相手")]
		public string HerName { get; set; }

		[DisplayName("ファイル名")]
		public string FileName { get; set; }
	}
}
