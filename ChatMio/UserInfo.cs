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
using System.Data.Linq;

namespace ChatMio
{
    #region public class UserData ユーザーデータ格納用クラス
    /// <summary>
    /// ユーザーデータ格納用クラス
    /// </summary>
    [Table(Name = "UserData")]
    public class UserData
    {
        /// <summary>
        /// ユーザー名
        /// </summary>
        [DisplayName("ユーザー名"), Column]
        public string Name
        {
            get { return _name; }
            set { if (value != null) { _name = value.Trim(); } }			//前後の空白を除去する
        }

        /// <summary>
        /// Nameの内部用変数
        /// </summary>
        private string _name;

        /// <summary>
        /// パスワード
        /// </summary>
        [DisplayName("パスワード"), Browsable(false), Column]
        public string Pass { get; set; }

        /// <summary>
        /// 出身地
        /// </summary>
        [DisplayName("出身地"), Column]
        public PrefEnum IsFrom { get; set; }

        /// <summary>
        /// 文字色
        /// </summary>
        [DisplayName("文字色"), Column]
        public KnownColor TextColor { get; set; }

        /// <summary>
        /// フォントサイズ
        /// </summary>
        [DisplayName("フォントサイズ"), Column]
        public float FontSize { get; set; }

        public UserData ()													//初期化用コンストラクタ
        {
            Name = ""; Pass = ""; IsFrom = PrefEnum.Hokkaido;
            TextColor = KnownColor.White; FontSize = 12;
        }
    }
    #endregion ユーザーデータ格納用クラス

    #region public enum PrefEnum 都道府県のEnum
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
    #endregion

    #region public class UserInfo ユーザーデータ処理用クラス
    /// <summary>
    /// ユーザーデータ処理用クラス
    /// </summary>
    public class UserInfo
    {
        const string CONNECTION_STRING = "Server = {0}; Integrated security = SSPI; Database = ChatMioUserDB;";

        #region private static bool CreateSqlDB mdf,ldfファイルを生成する
        /// <summary>
        /// mdf,ldfファイルを生成する
        /// </summary>
        /// <returns>成功失敗</returns>
        private static bool CreateSqlDB ()
        {
            SqlConnection sqlConn1 = new SqlConnection(						//localhostのSQLServerへの接続
                    "Server = localhost; Integrated security = SSPI;　");
            SqlConnection sqlConn2 = new SqlConnection(                     //ChatMioUserDBへの接続
                    String.Format(CONNECTION_STRING, Properties.Settings.Default.SQLServer));

            const string tmp =  "CREATE DATABASE ChatMioUserDB ON PRIMARY " +//データベース作成用コマンド文字列
                                "(NAME = ChatMioUserDBData,	" +
                                "FILENAME = '{0}', " +
                                "SIZE = 5MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
                                "LOG ON (NAME = ChatMioUserDBLog, " +
                                "FILENAME = '{1}', " +
                                "SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%)";
            string mkDbCmd = String.Format(tmp, Path.GetFullPath(".\\ChatMioUserDB.mdf"), Path.GetFullPath(".\\ChatMioUserDB.ldf"));

            const string mkTblCmd = "CREATE TABLE UserData (" +				//テーブル作成用コマンド文字列
                                    "Name nvarchar(20) PRIMARY KEY, " +
                                    "Pass nvarchar(20), " +
                                    "IsFrom nvarchar(20), " +
                                    "TextColor nvarchar(30), " +
                                    "FontSize float )";

            var sqlCmd1 = new SqlCommand(mkDbCmd, sqlConn1);
            var sqlCmd2 = new SqlCommand(mkTblCmd, sqlConn2);

            bool res = false;												//結果フラグ
            try {
                sqlConn1.Open();											//接続開始
                sqlCmd1.ExecuteNonQuery();									//データベース作成コマンド実行
                MyDebug.WriteLine(null, "UserInfo.CreateSqlDB Database is created successfully");

                sqlConn2.Open();
                sqlCmd2.ExecuteNonQuery();									//テーブル作成コマンド実行
                MyDebug.WriteLine(null, "UserInfo.CreateSqlDB Table is created successfully");

                res = true;													//成功フラグを立てる

            }
            catch (Exception e) {									    	//何らかのエラー時
                MyDebug.WriteLine(null, "UserInfo.CreateSqlDB Create DB Error!\n{0}", e);
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
        #endregion CreateSqlDB

        #region public static bool Write ユーザーデータを書き込む
        /// <summary>
        /// ユーザーデータを書き込む
        /// </summary>
        /// <param name="data">書き込むデータ</param>
        /// <returns>書き込みの成功失敗</returns>
        public static bool Write (UserData data)
        {
            #region 古いXML用コード
            /*
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
             * */
            #endregion //古いXML用コード

            if (!File.Exists(".\\ChatMioUserDB.mdf")) {						//mdfが存在しない時
                MyDebug.WriteLine(null, "UserInfo.Write mdfが見つからなかったため、作成します");
                if (!CreateSqlDB()) {										//DBを生成
                    return false;                                           //失敗したら終了
                }
            }

            SqlConnection sqlConn = new SqlConnection(                     //ChatMioUserDBへの接続
                    String.Format(CONNECTION_STRING, Properties.Settings.Default.SQLServer));

            // データを書き込むSQLコマンド
            const string tmp =  "INSERT INTO UserData (Name, Pass, IsFrom, TextColor, FontSize) " +
                                "VALUES ('{0}', '{1}', '{2}', '{3}', {4})";
            string addDataCmd = String.Format(tmp, data.Name, data.Pass, data.IsFrom, data.TextColor, data.FontSize);

            var sqlCmd = new SqlCommand(addDataCmd, sqlConn);

            bool res = false;												//結果フラグ
            try {
                sqlConn.Open();											    //接続開始
                sqlCmd.ExecuteNonQuery();									//コマンド実行
                MyDebug.WriteLine(null, "UserInfo.Write success");
                res = true;                                                 //成功を返す
            }
            catch (Exception e) {								    		//何らかのエラー時
                MyDebug.WriteLine(null, "UserInfo.Write Error 不明なエラー\n{0}", e);
            }                                                             
            finally {
                if (sqlConn.State == ConnectionState.Open) {				//接続が開いている場合
                    sqlConn.Close();										//閉じる
                }
            }
            return res;                                                     //結果を返す
        }
        #endregion Write

        #region public static int Count 保存されているUserDataの数を取得
        /// <summary>
        /// 保存されているUserDataの数を取得
        /// </summary>
        public static int Count
        {
            get                                                             //値の取得のみ許可
            {
                #region 古いXML用コード
                /*
                try {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load("UserInfo.xml");

                    var users = xmlDoc.SelectNodes("//User");

                    return users != null ? users.Cast<XmlElement>().Count() : 0;
                }
                catch (SystemException) {
                    return 0;
                }
                 */
                #endregion 古いXML用コード

                if (!File.Exists(".\\ChatMioUserDB.mdf")) {					//mdfが存在しない時
                    MyDebug.WriteLine(null, "UserInfo.Count mdfが見つかりませんでした");
                    return 0;												//項目は0個
                }

                SqlConnection sqlConn = new SqlConnection(                     //ChatMioUserDBへの接続
                        String.Format(CONNECTION_STRING, Properties.Settings.Default.SQLServer));

                const string countDataCmd = "SELECT COUNT (*) FROM UserData";
                var sqlCmd = new SqlCommand(countDataCmd, sqlConn);

                int res = -1;												//結果フラグ
                try {
                    sqlConn.Open();											//接続開始
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                    sqlReader.Read();									    //コマンド実行
                    res = (int) sqlReader.GetValue(0);                      //コマンドの結果を読み取り
                    MyDebug.WriteLine(null, "UserInfo.Count success 項目数: {0}", res);
                }
                catch (Exception e) {								        //何らかのエラー時
                    MyDebug.WriteLine(null, "UserInfo.Count Error 不明なエラー\n{0}", e);
                }
                finally {
                    if (sqlConn.State == ConnectionState.Open) {			//接続が開いている場合
                        sqlConn.Close();									//閉じる
                    }
                }
                return res;
            }
        }
        #endregion Count

        #region public static bool Read ユーザー情報を読み込む
        /// <summary>
        /// ユーザー情報を読み込む
        /// </summary>
        /// <param name="name">読み込みたい情報のユーザー名</param>
        /// <param name="ret">読み込んだユーザー情報</param>
        /// <returns>ユーザーが存在するとtrue・存在しない、又は読み込みに失敗するとfalse</returns>
        public static bool Read (string name, out UserData ret)
        {
            #region 古いXML用コード
            /*
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
            catch (Exception e) {
                return false;
            }
             */
            #endregion 古いXML用コード

            ret = new UserData();

            if (!File.Exists(".\\ChatMioUserDB.mdf")) {					    //mdfが存在しない時
                MyDebug.WriteLine(null, "UserInfo.Read mdfが見つかりませんでした");
                return false;											    //終了
            }

            SqlConnection sqlConn = new SqlConnection(                     //ChatMioUserDBへの接続
                    String.Format(CONNECTION_STRING, Properties.Settings.Default.SQLServer));

            const string tmp = "SELECT * FROM UserData WHERE Name = '{0}'";     //データを検索し射影するコマンド
            string readDataCmd = String.Format(tmp, name);

            var sqlCmd = new SqlCommand(readDataCmd, sqlConn);

            bool res = false;												//結果フラグ
            try {
                sqlConn.Open();											    //接続開始
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                sqlReader.Read();									        //コマンド実行
                ret.Name = sqlReader.GetValue(0).ToString();                //名前を読み取り
                ret.Pass = sqlReader.GetValue(1).ToString();                //Passを読み取り
                ret.IsFrom = (PrefEnum) Enum.Parse(typeof(PrefEnum),        //出身地を読み取り
                        sqlReader.GetValue(2).ToString());
                ret.TextColor = (KnownColor) Enum.Parse(typeof(KnownColor), //色を読み取り
                        sqlReader.GetValue(3).ToString());
                ret.FontSize = float.Parse(sqlReader.GetValue(4).ToString());//フォントサイズを読み取り

                MyDebug.WriteLine(null, "UserInfo.Read success");
                res = true;                                                 //成功を返す
            }
            catch (InvalidOperationException e) {
                MyDebug.WriteLine(null, "UserInfo.Read Error 要素が見つからなかった、あるいは複数の要素が見つかりました\n{0}", e);
            }
            catch (Exception e) {								    		//何らかのエラー時
                MyDebug.WriteLine(null, "UserInfo.Read Error 不明なエラー\n{0}", e);
            }
            finally {
                if (sqlConn.State == ConnectionState.Open) {				//接続が開いている場合
                    sqlConn.Close();										//閉じる
                }
            }
            return res;                                                     //結果を返す
        }
        #endregion Read

        #region public static bool ReadAll ユーザー情報をすべて読み込む
        /// <summary>
        /// ユーザー情報をすべて読み込む
        /// </summary>
        /// <param name="datas">読み込んだユーザー情報の配列</param>
        /// <returns>読み込みの成功失敗</returns>
        public static bool ReadAll (out UserData[] datas)
        {
            #region 古いXML用コード
            /*
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
             */
            #endregion 古いXML用コード

            if (!File.Exists(".\\ChatMioUserDB.mdf")) {					    //mdfが存在しない時
                MyDebug.WriteLine(null, "UserInfo.ReadAll mdfが見つかりませんでした");
                datas = new UserData[0];
                return false;											    //終了
            }

            SqlConnection sqlConn = new SqlConnection(                     //ChatMioUserDBへの接続
                    String.Format(CONNECTION_STRING, Properties.Settings.Default.SQLServer));

            const string countDataCmd = "SELECT COUNT (*) FROM UserData";   //データ数を数えるコマンド
            const string readDataCmd = "SELECT * FROM UserData";            //データを検索し射影するコマンド

            var sqlCountCmd = new SqlCommand(countDataCmd, sqlConn);
            var sqlReadCmd = new SqlCommand(readDataCmd, sqlConn);

            bool res = false;												//結果フラグ
            try {
                sqlConn.Open();											    //接続開始
                SqlDataReader sqlReader1 = sqlCountCmd.ExecuteReader();
                sqlReader1.Read();
                int count = (int) sqlReader1.GetValue(0);
                MyDebug.WriteLine(null, "UserInfo.ReadAll 項目数: {0}", count);
                sqlReader1.Close();

                datas = new UserData[count];

                SqlDataReader sqlReader2 = sqlReadCmd.ExecuteReader();
                int i = 0;
                while (sqlReader2.Read()) {			                        //コマンド実行
                    datas[i] = new UserData();
                    datas[i].Name = sqlReader2.GetValue(0).ToString();      //名前を読み取り
                    datas[i].Pass = sqlReader2.GetValue(1).ToString();      //Passを読み取り
                    datas[i].IsFrom = (PrefEnum) Enum.Parse(typeof(PrefEnum),//出身地を読み取り
                            sqlReader2.GetValue(2).ToString());
                    datas[i].TextColor = (KnownColor) Enum.Parse(typeof(KnownColor),//色を読み取り
                            sqlReader2.GetValue(3).ToString());
                    datas[i].FontSize = float.Parse(sqlReader2.GetValue(4).ToString());//フォントサイズを読み取り
                    i++;
                }
                MyDebug.WriteLine(null, "UserInfo.ReadAll success");
                res = true;                                                 //成功を返す
            }
            catch (Exception e) {								    		//何らかのエラー時
                MyDebug.WriteLine(null, "ReadAll DB Error 不明なエラー\n{0}", e);
                datas = new UserData[0];
            }
            finally {
                if (sqlConn.State == ConnectionState.Open) {				//接続が開いている場合
                    sqlConn.Close();										//閉じる
                }
            }
            return res;                                                     //結果を返す
        }
        #endregion ReadAll

        #region public static bool Remove ユーザー情報を削除する
        /// <summary>
        /// ユーザー情報を削除する
        /// </summary>
        /// <param name="name">削除するユーザーの名前</param>
        /// <returns>削除の成功失敗</returns>
        public static bool Remove (string name)
        {
            #region 古いXML用コード
            /*
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
             */
            #endregion 古いXML用コード

            if (!File.Exists(".\\ChatMioUserDB.mdf")) {					    //mdfが存在しない時
                MyDebug.WriteLine(null, "UserInfo.Remove mdfが見つかりませんでした");
                return false;											    //終了
            }

            SqlConnection sqlConn = new SqlConnection(                     //ChatMioUserDBへの接続
                    String.Format(CONNECTION_STRING, Properties.Settings.Default.SQLServer));

            const string tmp = "DELETE FROM UserData WHERE Name = '{0}'";   //データを検索し消去するコマンド
            string delDataCmd = String.Format(tmp, name);

            var sqlCmd = new SqlCommand(delDataCmd, sqlConn);

            bool res = false;												//結果フラグ
            try {
                sqlConn.Open();											    //接続開始
                sqlCmd.ExecuteNonQuery();									//コマンド実行
                Debug.WriteLine("UserInfo.Remove success");
                res = true;                                                 //成功を返す
            }
            catch (Exception e) {								    		//何らかのエラー時
                MyDebug.WriteLine(null, "UserInfo.Remove Error 不明なエラー\n{0}", e);
            }
            finally {
                if (sqlConn.State == ConnectionState.Open) {				//接続が開いている場合
                    sqlConn.Close();										//閉じる
                }
            }
            return res;                                                     //結果を返す
        }
        #endregion Remove

        #region public static void Print 出身地から抽出したユーザーのリストを印刷
        /// <summary>
        /// 出身地から抽出したユーザーのリストを印刷する
        /// </summary>
        /// <param name="pref">出身地</param>
        public static void Print (PrefEnum pref)
        {
            var xlsApp = new Excel.Application { Visible = true };			//Excelを開く

            //Excelファイルを新規作成
            var xlsBook = xlsApp.Workbooks.Add();							//Excelファイルを新規作成

            ////シートを新規作成
            //var xlsSheet = xlsBook.Worksheets.Add();
            var xlsSheet = xlsBook.Sheets[1] as Excel.Worksheet;			//作業シートを選択
            //xlsSheet.Name = sheetName;
            xlsSheet.Select();

            //B2からD2までを結合して中央揃えしタイトルを入れる
            var xlsRange = xlsSheet.Range["B2", "D2"];			           	//B2からD2までを選択
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
            xlsRange = xlsSheet.Range["B5", "D5"];						    //B5からD5までを選択
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
            xlsSheet.PageSetup.PrintTitleRows = @"$5:$5";			    	//行タイトルを設定

            //印刷
            xlsSheet.PrintOut();

            //終了時の処理
            xlsBook.Close(false);
            xlsApp.Quit();
            Marshal.ReleaseComObject(xlsRange);
            Marshal.ReleaseComObject(xlsSheet);
            Marshal.ReleaseComObject(xlsBook);
            Marshal.ReleaseComObject(xlsApp);
            xlsRange = null; xlsSheet = null; xlsBook = null; xlsApp = null;
            GC.Collect();
        }
        #endregion Print

        #region static void GenerateTestData テストデータ生成用関数
        /// <summary>
        /// テストデータ生成用関数
        /// </summary>
        /// <param name="count">生成したいデータ数</param>
        private static void GenerateTestData (int count = 1000)
        {
            if (!File.Exists(".\\ChatMioUserDB.mdf")) {						//mdfが存在しない時
                MyDebug.WriteLine(null, "UserInfo.GenerateTestData mdfが見つからなかったため、作成します");
                if (!CreateSqlDB()) {										//DBを生成
                    return;                                                 //失敗したら終了
                }
            }

            SqlConnection sqlConn = new SqlConnection(                     //ChatMioUserDBへの接続
                    String.Format(CONNECTION_STRING, Properties.Settings.Default.SQLServer));

            // データを書き込むSQLコマンド
            const string addDataCmd =   "INSERT INTO UserData (Name, Pass, IsFrom, TextColor, FontSize) " +
                                        "VALUES ('{0}', '{1}', '{2}', '{3}', {4})";

            var sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;

            var rand = new Random();                                        //乱数用クラスをセットアップ

            try {
                sqlConn.Open();											    //接続開始

                for (int i = 0; i < count; i++) {
                    string tmp = String.Format(addDataCmd,
                            Path.GetRandomFileName(),
                            "pass",
                            (PrefEnum) (rand.Next() % 47),
                            (KnownColor) (rand.Next() % 174 + 1),
                            rand.Next() % 28 + 3);

                    sqlCmd.CommandText = tmp;
                    sqlCmd.ExecuteNonQuery();
                    if ((i + 1) % 1000 == 0) {
                        MyDebug.WriteLine(null, "UserInfo.GenerateTestData {0}件目", i + 1);
                    }
                }
            }
            catch (Exception e) {								    		//何らかのエラー時
                MyDebug.WriteLine(null, "UserInfo.GenerateTestData Error 不明なエラー\n{0}", e);
            }
            finally {
                if (sqlConn.State == ConnectionState.Open) {				//接続が開いている場合
                    sqlConn.Close();										//閉じる
                }
            }
        }
        #endregion GenerateTestData
    }
    #endregion UserInfo
} // end of ChatMio (namespace)
