using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace ChatMio
{
	/// <summary>
	/// ユーザーデータ格納用クラス
	/// </summary>
	[Table(Name = "UserData")]
	public class UserData
	{
		[DisplayName("ユーザー名"), Column]
		public string Name {
			get { return _name; }
			set { if (value != null) { _name = value.Trim(); } }			//前後の空白を除去する
		}

		private string _name;

		[DisplayName("パスワード"), Browsable(false), Column]
		public string Pass { get; set; }

		[DisplayName("出身地"), Column]
		public PrefEnum IsFrom { get; set; }

		[DisplayName("文字色"), Column]
		public KnownColor TextColor { get; set; }

		[DisplayName("フォントサイズ"), Column]
		public float FontSize { get; set; }

		public UserData ()													//初期化用コンストラクタ
		{
			Name = ""; Pass = ""; IsFrom = PrefEnum.Hokkaido;
			TextColor = KnownColor.White; FontSize = 12;
		}
	}

	/// <summary>
	/// 都道府県のEnum
	/// </summary>
	public enum PrefEnum
	{
		Hokkaido, Aomori, Iwate, Miyagi, Akita, Yamagata,
		Fukushima, Ibaragi, Tochigi, Gunnma, Saitama, Chiba,
		Tokyo, Kanagawa, Nigata, Toyama, Ishikawa, Fukui,
		Yamanashi, Nagano, Gifu, Shizuoka, Aichi, Mie,
		Shiga, Kyoto, Osaka, Hyogo, Nara, Wakayama,
		Tottori, Shimane, Okayama, Hiroshima, Yamaguchi, Tokushima,
		Kagawa, Ehime, Kouchi, Fukuoka, Saga, Nagasaki,
		Kumamoto, Oita, Miyazaki, Kagoshima, Okinawa
	}

	/// <summary>
	/// ユーザーデータ処理用クラス
	/// </summary>
	public class UserInfo
	{
		/// <summary>
		/// mdf,ldfファイルを生成する
		/// </summary>
		/// <returns>成功失敗</returns>
		private static bool CreateSqlDB ()
		{
			SqlConnection sqlConn1 = new SqlConnection(						//localhostのSQLServerへの接続
					"Server = localhost; Integrated security = SSPI;　");
			SqlConnection sqlConn2 = new SqlConnection(						//ChatMioUserDBへの接続
					"Server = localhost; Integrated security = SSPI; Database = ChatMioUserDB;");

			string str1 = "CREATE DATABASE ChatMioUserDB ON PRIMARY " +		//データベース作成用コマンド文字列
						"(NAME = ChatMioUserDBData,	" +
						"FILENAME = '" + Path.GetFullPath(".\\ChatMioUserDB.mdf") + "', " +
						"SIZE = 5MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
						"LOG ON (NAME = ChatMioUserDBLog, " +
						"FILENAME = '" + Path.GetFullPath(".\\ChatMioUserDB.ldf") + "', " +
						"SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%)";

			string str2 = "CREATE TABLE UserData (" +						//テーブル作成用コマンド文字列
							"Name nvarchar, " +
							"Pass nvarchar, " +
							"IsFrom nvarchar, " +
							"TextColor nvarchar, " +
							"FontSize int )";

			var sqlCmd1 = new SqlCommand(str1, sqlConn1);

			bool res = false;												//結果フラグ
			try {
				sqlConn1.Open();											//接続開始
				sqlCmd1.ExecuteNonQuery();									//データベース作成コマンド実行
				Debug.WriteLine("Database is created successfully");
				sqlConn1.Close();

				var sqlCmd2 = new SqlCommand(str2, sqlConn2);
				sqlConn2.Open();
				sqlCmd2.ExecuteNonQuery();									//テーブル作成コマンド実行
				Debug.WriteLine("Table is created successfully");

				res = true;													//成功フラグを立てる

			}
			catch (SystemException e) {										//何らかのエラー時
				Debug.WriteLine("Create Error! {0}", e);
			}
			finally {
				if (sqlConn1.State == ConnectionState.Open) {				//接続が開いている場合
					sqlConn1.Close();										//閉じる
				}
				if (sqlConn2.State == ConnectionState.Open) {				//接続が開いている場合
					sqlConn2.Close();										//閉じる
				}
			}

			return res;
		}

		/// <summary>
		/// ユーザーデータを書き込む
		/// </summary>
		/// <param name="data">書き込むデータ</param>
		/// <returns>成功失敗</returns>
		public static bool Write (UserData data)
		{
			var xmlDoc = new XmlDocument();
			XmlElement xmlRoot;

			try {
				xmlDoc.Load("UserInfo.xml");
				xmlRoot = xmlDoc.FirstChild as XmlElement;
			}
			catch {
				xmlRoot = xmlDoc.CreateElement("UserInfo");
				xmlDoc.AppendChild(xmlRoot);
			}

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

			try {
				xmlDoc.Save("UserInfo.xml");
				return true;
			}
			catch (SystemException) {
				return false;
			}

			/*
			if (!File.Exists(".\\ChatMioUserDB.mdf")) {						//mdfが存在しない時
				if (!CreateSqlDB()) {										//DB生成が失敗した場合
					return false;
				}
			}

			try {
				var db = new DataContext(									//データベースへ接続
						"Server = localhost; Integrated security = SSPI; Database = ChatMioUserDB;");

				var users = db.GetTable<UserData>();						//UserDataテーブルを取得
				users.InsertOnSubmit(data);									//テーブルに挿入
				db.SubmitChanges();											//変更
				Debug.WriteLine("Write UserData success");
				return true;
			}
			catch (SystemException e) {										//何らかのエラー時
				Debug.WriteLine("Write Error! {0}", e);
				return false;
			}
			 */
		}

		/// <summary>
		/// 保存されているUserDataの数を取得
		/// </summary>
		public static int Count
		{
			get
			{
				try {
					var xmlDoc = new XmlDocument();
					xmlDoc.Load("UserInfo.xml");

					var users = xmlDoc.SelectNodes("//User");

					return users != null ? users.Cast<XmlElement>().Count() : 0;
				}
				catch (SystemException) {
					return 0;
				}
				/*
				if (!File.Exists(".\\ChatMioUserDB.mdf")) {						//mdfが存在しない時
					return 0;													//項目は0個
				}

				try {
					var db = new DataContext(									//データベースへ接続
							"Server = localhost; Integrated security = SSPI; Database = ChatMioUserDB;");

					var users = db.GetTable<UserData>();						//UserDataテーブルを取得
					return users.Count();										//項目数を返す
				}
				catch (SystemException e) {
					Debug.WriteLine("Count Error! {0}", e);
					return -1;
				}
				 */
			}
		}

		/// <summary>
		/// ユーザー情報を読み込む
		/// </summary>
		/// <param name="name">読み込みたい情報のユーザー名</param>
		/// <param name="ret">読み込んだユーザー情報</param>
		/// <returns>成功失敗</returns>
		public static bool Read (string name, out UserData ret)
		{
			ret = new UserData();
			try {
				var xmlDoc = new XmlDocument();
				xmlDoc.Load("UserInfo.xml");

				string xPath = "//User[Name='" + name.Trim() + "']";
				var user = xmlDoc.SelectSingleNode(xPath) as XmlElement;
				if (user != null) {
					ret.Name = user["Name"].InnerText;
					ret.Pass = user["Pass"].InnerText;
					ret.IsFrom = (PrefEnum) Enum.Parse(typeof(PrefEnum),
							user["IsFrom"].InnerText);
					ret.TextColor = (KnownColor) Enum.Parse(typeof(KnownColor),
							user["TextColor"].InnerText);
					ret.FontSize = float.Parse(user["FontSize"].InnerText);
				}
				else {
					throw new KeyNotFoundException();
				}

				return true;
			}
			catch (SystemException) {
				ret = new UserData();
				return false;
			}
		}

		/// <summary>
		/// ユーザー情報をすべて読み込む
		/// </summary>
		/// <param name="datas">読み込んだユーザー情報の配列</param>
		/// <returns>成功失敗</returns>
		public static bool ReadAll (out UserData[] datas)
		{
			try {
				var xmlDoc = new XmlDocument();
				xmlDoc.Load("UserInfo.xml");

				int c = Count;
				if (c > 0) {
					datas = new UserData[c];
				}
				else {
					datas = new UserData[0];
					return false;
				}

				var users = xmlDoc.SelectNodes("//User");

				int i = 0;
				foreach (XmlElement user in users) {
					datas[i] = new UserData();
					datas[i].Name = user["Name"].InnerText;
					datas[i].Pass = user["Pass"].InnerText;
					datas[i].IsFrom = (PrefEnum) Enum.Parse(typeof(PrefEnum), user["IsFrom"].InnerText);
					datas[i].TextColor = (KnownColor) Enum.Parse(typeof(KnownColor), user["TextColor"].InnerText);
					datas[i].FontSize = float.Parse(user["FontSize"].InnerText);
					i++;
				}

				return true;
			}
			catch (SystemException) {
				datas = new UserData[0];
				return false;
			}
		}

		/// <summary>
		/// ユーザー情報を削除する
		/// </summary>
		/// <param name="name">削除するユーザーの名前</param>
		/// <returns>成功失敗</returns>
		public static bool Remove (string name)
		{
			try {
				var xmlDoc = new XmlDocument();
				xmlDoc.Load("UserInfo.xml");

				string xPath = "//User[Name='" + name.Trim() + "']";
				var oldUser = xmlDoc.SelectSingleNode(xPath);

				if (oldUser != null) {
					xmlDoc.DocumentElement.RemoveChild(oldUser);
					xmlDoc.Save("UserInfo.xml");
				}
				else {
					throw new KeyNotFoundException();
				}

				return true;
			}
			catch (SystemException) {
				return false;
			}
		}

		public static void Print (PrefEnum pref)
		{
			var xlsApp = new Excel.Application { Visible = true };			//Excelを開く
			//string fileName = String.Format(@".\{0}.xlsx", user.Name);		
			////TODO ファイル名の確認
			//const string sheetName = "UserData";

			//Excelファイルを新規作成
			var xlsBook = xlsApp.Workbooks.Add();							//Excelファイルを新規作成
			//xlsBook.SaveAs(fileName);
			//Marshal.ReleaseComObject(xlsBook);
			//xlsBook = null;

			////Excelファイルを開く
			//xlsBook = xlsApp.Workbooks.Open(fileName);

			////シートを新規作成
			//var xlsSheet = xlsBook.Worksheets.Add();
			var xlsSheet = xlsBook.Sheets[1] as Excel.Worksheet;								//作業シートを選択
			//xlsSheet.Name = sheetName;
			xlsSheet.Select();

			//シートを開く
			//xlsSheet = xlsBook.Worksheets[sheetName] as Excel.Worksheet;

			//セルに文字列を設定
			//var xlsRange = xlsSheet.Cells[1, 1];
			//xlsRange.Value = "testtest";
			//Marshal.ReleaseComObject(xlsRange);
			//xlsRange = null;

			//B2からD2までを結合して中央揃えしタイトルを入れる
			var xlsRange = xlsSheet.Range["B2", "D2"];				//B2からD2までを選択
			xlsRange.MergeCells = true;										//結合
			xlsRange.HorizontalAlignment = Excel.Constants.xlCenter;		//中央揃え
			xlsRange.Font.Bold = true;										//太字
			xlsRange.Font.Size = 16;										//サイズ
			xlsRange.Value2 = "ChatMio";									//タイトルを入れる

			//B3からD3までを結合して中央揃えし抽出条件を入れる
			xlsRange = xlsSheet.Range["B3", "D3"];							//B3からD3までを選択
			xlsRange.MergeCells = true;										//結合
			xlsRange.HorizontalAlignment = Excel.Constants.xlCenter;		//中央揃え
			xlsRange.Font.Size = 12;										//サイズ
			xlsRange.Value2 = String.Format("出身地{0}のユーザー一覧", pref);//タイトルを入れる

			//B5からD5に表のヘッダを入れる
			xlsRange = xlsSheet.Range["B5", "D5"];						//B5からD5までを選択
			xlsRange.Borders.Item[Excel.XlBordersIndex.xlEdgeBottom].LineStyle
					= Excel.XlLineStyle.xlContinuous;						//横線を引っ張る
			xlsSheet.Range["B5"].Value2 = "名前";
			xlsSheet.Range["C5"].Value2 = "文字色";
			xlsSheet.Range["D5"].Value2 = "フォントサイズ";

			//表の内容を書き込む
			UserData[] datas;
			ReadAll(out datas);
			int row = 6;
			var q = datas.Where(data => data.IsFrom == pref);
			foreach (var d in q) { 
				xlsSheet.Cells[row, 2].Value2 = d.Name;
				xlsSheet.Cells[row, 3].Value2 = d.TextColor.ToString();
				xlsSheet.Cells[row++, 4].Value2 = d.FontSize;
			}

			//列を調整する
			xlsSheet.Columns["B"].ColumnWidth = 20;							
			xlsSheet.Columns["B"].NumberFormatLocal = "@";					//カラムBの表示形式を文字列にする
			xlsSheet.Columns["C"].ColumnWidth = 20;
			xlsSheet.Columns["C"].NumberFormatLocal = "@";					//カラムCの表示形式を文字列にする
			xlsSheet.Columns["D"].ColumnWidth = 11;
			xlsSheet.Columns["D"].NumberFormat = "0.0";						//カラムDの表示形式を小数点第一位までの小数とする

			//印刷設定
			xlsSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;//横向きに印刷
			xlsSheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;		//用紙サイズをA4に
			xlsSheet.PageSetup.PrintTitleRows = @"$5:$5";				//行タイトルを設定

			//印刷
			xlsSheet.PrintOut();

			//終了時の処理
			xlsBook.Close(false);
			xlsApp.Quit();
			Marshal.ReleaseComObject(xlsSheet);
			Marshal.ReleaseComObject(xlsBook);
			Marshal.ReleaseComObject(xlsApp);
			xlsSheet = null; xlsBook = null; xlsApp = null;
			GC.Collect();
		}
	} // end of UserInfo (class)
} // end of ChatMio (namespace)
