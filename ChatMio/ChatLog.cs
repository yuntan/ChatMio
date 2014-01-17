using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
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

		/// <summary>
		/// 保存済みのチャットログの一覧を取得
		/// </summary>
		public static LogData[] LogList
		{
			get
			{
				string[] logNames = Directory.GetFiles(@".\", "chatmio_*.txt");	//ログファイルを検索
				return logNames.Select(x => ParseLogFileName(x.Substring(2))).ToArray();//ファイル名をパースし配列にし返す
			}
		}

		/// <summary>
		/// チャットログを印刷する
		/// </summary>
		/// <param name="fileName">印刷したいチャットログのファイル名</param>
		/// <param name="showPreview">プレビューを表示するか否か</param>
		public static void Print (string fileName, bool showPreview)
		{
			LogData log = ParseLogFileName(fileName);							//ファイル名をパース
			string textToPrint;													//印刷するテキスト
			using (var r = new StreamReader(fileName)) {
				textToPrint = r.ReadToEnd();									//ファイルからテキストをすべて読み込む
			}
			int page = 1;

			var pd = new PrintDocument();				   						//PrindDocumentのインスタンス

			var pSize = (from PaperSize size in pd.PrinterSettings.PaperSizes	//A4の用紙サイズを取得
						where size.Kind == PaperKind.A4
						select size).FirstOrDefault();

			if (pSize == default(PaperSize)) {									//A4未対応のプリンターだったら
				MessageBox.Show("プリンターがA4に対応していません", "用紙サイズのエラー",//エラーを吐く
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;															//終了
			}

			pd.PrintPage += delegate(object sender, PrintPageEventArgs e) {		//実際に印刷を行うデリゲートを追加
				#region 印刷を行うデリゲート
				var pen = new Pen(Color.Black, 1);								//デフォルトで幅1のペンを作成
				Font font; SizeF size; int x;
				const int paperWidth = 210, paperHeight = 297;					//用紙の幅、高さ
				//e.PageSettings.PaperSize = new PaperSize("A4", 210, 297);		//用紙サイズはA4
				e.PageSettings.PaperSize = pSize;
				e.PageSettings.Landscape = false;								//縦向き
				e.Graphics.PageUnit = GraphicsUnit.Millimeter;					//ミリ単位で扱う

				if (page == 1) {
					//タイトルをセンタリングして描画
					const string title = "ChatMio チャットログ";
					font = new Font("ＭＳ ゴシック", 21.0F);					//フォントサイズを21に
					size = e.Graphics.MeasureString(title, font, paperWidth);	//描画時の文字列の長さを計測
					x = Convert.ToInt32(paperWidth / 2 - size.Width / 2);		//X座標を求める
					e.Graphics.DrawString(title, font, Brushes.Black, x, 10);	//描画する

					//サブタイトルをセンタリングして描画
					string subTitle = String.Format("{0}  {1}との会話", log.Date, log.HerName);
					font = new Font("ＭＳ ゴシック", 15.0F);					//フォントサイズを15に
					size = e.Graphics.MeasureString(subTitle, font, paperWidth);//描画時の文字列の長さを計測
					x = Convert.ToInt32(paperWidth / 2 - size.Width / 2);		//X座標を求める
					e.Graphics.DrawString(subTitle, font, Brushes.Black, x, 25);//描画する
				}

				//チャットログを囲むボックスを描画
				var rect = new Rectangle(10, (page == 1 ? 40 : 10),				//描画するボックスの位置・大きさ
						paperWidth - 10 - 10, paperHeight - (page == 1 ? 40 : 10) - 20);
				MyDebug.WriteLine(null, "rect = {0}", rect);
				e.Graphics.DrawRectangle(pen, rect);							//描画する

				//チャットログを描画
				font = new Font("ＭＳ ゴシック", 12.0F);						//フォントサイズを12に
				var drawRect = new RectangleF(rect.X + 5, rect.Y + 5,			//チャットログを描画する領域
						rect.Width - 10, rect.Height - 10);
				int charsInPage, linesInPage;									//描画された文字数、描画された行数
				e.Graphics.MeasureString(textToPrint, font,						//文字数、行数を測定
						new SizeF(drawRect.Width, drawRect.Height - 5),			//描画領域よりも少し狭く設定
						new StringFormat(), out charsInPage, out linesInPage);
				/* .Netのバグ
				 * 内容がWrapされた時正しく文字数、行数を測定できない
				 */
				MyDebug.WriteLine(null, "charsInPage = {0}, linesInPage = {1}", charsInPage, linesInPage);
				e.Graphics.DrawString(textToPrint.Substring(0, charsInPage),	//描画する
						font, Brushes.Black, drawRect);
				textToPrint = textToPrint.Substring(charsInPage);
				e.HasMorePages = (textToPrint.Length > 0);						//描画するテキストがまだ残っていたら次ページも印刷

				//ページ数を描画
				size = e.Graphics.MeasureString(								//描画時の文字列の長さを計測
						page.ToString(CultureInfo.InvariantCulture), font, paperWidth);
				x = Convert.ToInt32(paperWidth / 2 - size.Width / 2);			//X座標を求める
				e.Graphics.DrawString(											//描画する   
						page++.ToString(CultureInfo.InvariantCulture), font, Brushes.Black, x, 280);
				#endregion
			};

			if (showPreview) {
				var ppd = new PrintPreviewDialog { Document = pd };				//プレビューダイアログ
				ppd.ShowDialog();												//ダイアログを表示
			}
			else { pd.Print(); }												//印刷する
		}

		/// <summary>
		/// チャットログのファイル名からチャットが行われた日付、相手を取り出す
		/// </summary>
		/// <param name="fileName">対象のファイル名</param>
		/// <returns>取り出した情報</returns>
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
				log.FileSize = new FileInfo(fileName).Length;					//リストにファイルサイズを追加
			}
			catch (SystemException e) {
				MyDebug.WriteLine(null, "ChatLog.ParseLogFileName ファイル名のフォーマットが不正\n{0}", e);
			}
			return log;
		}
	}

	/// <summary>
	/// チャットログの行われた日付、終了時刻、相手、ファイル名を格納するクラス
	/// </summary>
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

		[DisplayName("ファイルサイズ(B)")]
		public long FileSize { get; set; }
	}
}
