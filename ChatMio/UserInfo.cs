using System;
using System.Xml;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq.Mapping;
using System.Diagnostics;

namespace ChatMio
{
	/// <summary>
	/// ユーザーデータ格納用クラス
	/// </summary>
	[Table(Name = "UserData")]
	public class UserData											
	{
		[DisplayName("ユーザー名"), Column]
		public string Name { get; set; }

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

	public enum PrefEnum													// 都道府県のEnum
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

	public class UserInfo													// ユーザーデータ処理用クラス
	{
		private static bool CreateSqlDB ()									//mdf,ldfファイルを生成する
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
		/// <returns></returns>
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

		public static int Count ()
		{
			try {
				var xmlDoc = new XmlDocument();
				xmlDoc.Load("UserInfo.xml");

				int count = 0;
				var users = xmlDoc.SelectNodes("//User");
				foreach (XmlElement user in users) {
					count++;
				}

				return count;
			}
			catch (SystemException) {
				return -1;
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

		public static bool Read (string name, out UserData ret)
		{
			ret = new UserData();
			try {
				var xmlDoc = new XmlDocument();
				xmlDoc.Load("UserInfo.xml");

				string xPath = "//User[Name='" + name + "']";
				var user = xmlDoc.SelectSingleNode(xPath) as XmlElement;
				if (user != null) {
					ret.Name = user["Name"].InnerText;
					ret.Pass = user["Pass"].InnerText;
					ret.IsFrom = (PrefEnum) Enum.Parse(typeof(PrefEnum),
							user["IsFrom"].InnerText);
					ret.TextColor = (KnownColor) Enum.Parse(typeof(KnownColor),
							user["TextColor"].InnerText);
					ret.FontSize = int.Parse(user["FontSize"].InnerText);
				}
				else {
					throw new KeyNotFoundException();
				}

				return true;
			}
			catch (SystemException) {
				ret.Name = null; ret.IsFrom = PrefEnum.Hokkaido; ret.Pass = null;
				ret.TextColor = KnownColor.Red; ret.FontSize = 0;

				return false;
			}
		}

		public static bool ReadAll (out UserData[] datas)
		{
			try {
				var xmlDoc = new XmlDocument();
				xmlDoc.Load("UserInfo.xml");

				int count = Count();
				if (count > 0) {
					datas = new UserData[count];
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
					datas[i].FontSize = int.Parse(user["FontSize"].InnerText);
					i++;
				}

				return true;
			}
			catch (SystemException) {
				datas = new UserData[0];
				return false;
			}
		}

		public static bool Remove (string name)
		{
			try {
				var xmlDoc = new XmlDocument();
				xmlDoc.Load("UserInfo.xml");

				string xPath = "//User[Name='" + name + "']";
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
	} // end of UserInfo (class)
} // end of ChatMio (namespace)
