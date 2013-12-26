﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace ChatMio
{
	abstract class ChatBase
	{
		//接続完了時のイベントとデリゲート
		public delegate void ConnectedHandler (object obj, ConnectedEventArgs e);
		public event ConnectedHandler Connected;

		/// <summary>
		/// 派生クラスからConnectedを発行するためのメソッド
		/// </summary>
		/// <param name="s">IPアドレス</param>
		protected void InvokeConnectedEvent (string s)
		{
			if (Connected != null) {
				Connected(this, new ConnectedEventArgs(s));
			}
		}

		//接続失敗時のイベントとデリゲート
		public delegate void ConnectionFailedHander (object obj);
		public event ConnectionFailedHander ConnectionFailed;

		/// <summary>
		/// 派生クラスからConnectionFailedを発行するためのメソッド
		/// </summary>
		/// <param name="s">IPアドレス</param>
		protected void InvokeConnectionFailedEvent ()
		{
			if (ConnectionFailed != null) {
				ConnectionFailed(this);
			}
		}

		// チャットメッセージ受信時のイベントとデリゲート
		public delegate void MsgReceivedHandler (object obj, MsgReceivedEventArgs e);
		public event MsgReceivedHandler MsgReceived;

		//ユーザーデータ受信時のイベントとデリゲート
		public delegate void UserDataReceivedHandler (object obj, UserDataReceivedEventArgs e);
		public event UserDataReceivedHandler UserDataReceived;

		//チャット終了時のイベントとデリゲート
		public delegate void ChatClosedHandler (object obj);
		public event ChatClosedHandler ChatClosed;

		/// <summary>
		/// 派生クラスからChatClosedを発行するためのメソッド
		/// </summary>
		protected void InvokeChatClosedEvent ()
		{
			if (ChatClosed != null) {
				ChatClosed(this);
			}
		}

		//レスポンス受信時のイベントとデリゲート
		public delegate void ResponseReceivedHandler (object obj);
		public event ResponseReceivedHandler ResponseReceived;

		//Ping受信時のイベントとデリゲート
		public delegate void PingReceivedHandler (object obj, PingEventArgs e);
		public event PingReceivedHandler PingReceived;

		protected Encoding _utf8 = Encoding.UTF8;
		protected int _port = 3100;												//使用するポート
		protected TcpClient _tcpClient;
		protected NetworkStream _netStream;
		protected bool _userDataReceived = false;
		protected int _lastID = 0;
		protected bool _isConnected = false;

		abstract public void Start ();
		abstract public void Stop ();

		/// <summary>
		/// 受信したコマンドをパースする
		/// </summary>
		/// <param name="bytRes">受信したコマンド</param>
		protected void ParseCommand (byte[] bytRes)
		{
			byte[] bytCmd = new byte[0];										//コンパイルエラーを防ぐため空配列を入れとく
			bool flag = false;													//先頭発見フラグ
			for (int i = 0; i < bytRes.Length; i++) {
				if (bytRes[i] == _utf8.GetBytes("@")[0]
						&& bytRes[i + 1] == _utf8.GetBytes(":")[0]) {		    //コマンドの先頭を発見
					bytCmd = new byte[bytRes.Length - i];
					Array.Copy(bytRes, i, bytCmd, 0, bytCmd.Length);			//先頭の前で切る
					flag = true;												//先頭発見フラグを立てる
					break;
				}
			}

			if (!flag) {														//コマンドの先頭が見つからなかった場合
				Debug.WriteLine("コマンドの先頭が見つからなかったためパース終了");
				return;															//終了
			}

			int id = BitConverter.ToInt16(bytCmd, 2);							//IDを取り出し
			_lastID = id;
			int cmd = BitConverter.ToInt16(bytCmd, 4);							//CMDを取り出し
			int dataLen = BitConverter.ToInt32(bytCmd, 6);						//DATALENを取り出し

			switch (cmd) {
				case 0:															//Message
					Debug.WriteLine("コマンドを受信 ID: {0}, CMD: {1} (メッセージ), DATALEN: {2}", id, cmd, dataLen);

					if (_userDataReceived) {
						SendResponse(0, id);									//ユーザー情報受取済みなら"成功"を返す
						Debug.WriteLine("応答(成功)完了");
					}
					else {
						SendResponse(2, id);									//受け取っていなかったら"Not Ready"
						Debug.WriteLine("応答(Not Ready)完了");
					}

					if (MsgReceived != null) {									//イベント発行
						MsgReceived(this, new MsgReceivedEventArgs(_utf8.GetString(bytCmd, 10, dataLen)));
					}
					break;

				case 1:															//UserInfo
					Debug.WriteLine("コマンドを受信 ID: {0}, CMD: {1} (ユーザー情報), DATALEN: {2}", id, cmd, dataLen);

					UserData data;
					try {
						data = ParseUserData(bytCmd);
						_userDataReceived = true;								//データ受取完了フラグを立てる
						SendResponse(1, id);									//"Ready"を返す
						Debug.WriteLine("応答(Ready)完了");

						if (UserDataReceived != null) {							//イベント発行
							UserDataReceived(this, new UserDataReceivedEventArgs(data));
						}
					}
					catch {
						Debug.WriteLine("ユーザー情報パース失敗");				//"失敗"を返す
						SendResponse(255, id);
						Debug.WriteLine("応答(失敗)完了");
					}
					break;

				case 2:															//Close
					Debug.WriteLine("コマンドを受信 ID: {0}, CMD: {1} (終了), DATALEN: {2}", id, cmd, dataLen);
					if (ChatClosed != null) {
						ChatClosed(this);										//イベント発行
					}
					this.Stop();												//通信を閉じる
					Debug.WriteLine("通信終了");
					break;

				case 3:															//Response
					Debug.WriteLine("コマンドを受信 ID: {0}, CMD: {1} (応答), DATALEN: {2}", id, cmd, dataLen);
					int statusID = BitConverter.ToInt16(bytCmd, 10);
					int packetID = BitConverter.ToInt16(bytCmd, 12);

					if (statusID == 2) {										//ユーザー情報の要求の場合
						Debug.WriteLine("ユーザー情報が要求されました");
						if (SendUserData()) { Debug.WriteLine("ユーザー情報送信完了"); }
						else { Debug.Fail("ユーザー情報送信失敗"); }
					}

					// TODO PACKET_ID検証

					if (ResponseReceived != null) {
						ResponseReceived(this);									//イベント発行
					}
					break;

				case 4:															//Ping
					Debug.WriteLine("コマンドを受信 ID: {0}, CMD: {1} (Ping), DATALEN: {2}", id, cmd, dataLen);
					SendResponse(0, id);										//"成功"を返す
					Debug.WriteLine("応答(成功)完了");

					if (PingReceived != null) {
						PingReceived(this, new PingEventArgs(ChatStatus.Connected));
					}
					break;
			}
		}

		protected UserData ParseUserData (byte[] bytCmd)
		{
			int info_num = BitConverter.ToInt16(bytCmd, 10);					//含まれるユーザー情報の数

			int offset = 12;													//データ読み取り開始位置
			var data = new UserData { };

			for (int i = 0; i < info_num; i++) {								//ユーザー情報の数だけ繰り返す
				int info_id = BitConverter.ToInt16(bytCmd, offset);				//情報の種類
				offset += 2;
				int info_len = BitConverter.ToInt32(bytCmd, offset);			//INFO_BODYの長さ
				offset += 4;

				switch (info_id) {
					case 1:														//ユーザー名
						data.Name = _utf8.GetString(bytCmd, offset, info_len);
						break;
					case 2:														//出身地
						data.IsFrom = (PrefEnum) Enum.Parse(typeof(PrefEnum),
								_utf8.GetString(bytCmd, offset, info_len));
						break;
					case 3:														//フォントの色
						data.TextColor = (System.Drawing.KnownColor)
								BitConverter.ToInt32(bytCmd, offset);
						break;
					case 4:														//フォントサイズ
						data.FontSize = BitConverter.ToSingle(bytCmd, offset) / 10;
						break;
					default:
						// TODO その他の情報、ユーザー定義情報パース実装
						break;
				}
				offset += info_len;
			}
			return data;
		}

		/// <summary>
		/// コマンドを送信
		/// </summary>
		/// <param name="bytCmd">送りたいコマンド</param>
		/// <returns>成功失敗</returns>
		protected bool SendCommand (byte[] bytCmd)
		{
			try {
				_netStream.Write(bytCmd, 0, bytCmd.Length);						//ストリームにコマンドを書き出す
				return true;
			}
			catch {
				return false;
			}
		}

		/// <summary>
		/// メッセージを送信
		/// </summary>
		/// <param name="msg">送りたいメッセージ</param>
		/// <returns>成功失敗</returns>
		public bool SendMessage (string msg)									//serverからclientにメッセージを送信
		{
			byte[] buff = new byte[10 + _utf8.GetByteCount(msg)];

			_utf8.GetBytes("@:").CopyTo(buff, 0);								//コマンドの先頭
			BitConverter.GetBytes((Int16) (++_lastID)).CopyTo(buff, 2);			//ID
			BitConverter.GetBytes((Int16) 0).CopyTo(buff, 4);					//CMD
			BitConverter.GetBytes(_utf8.GetByteCount(msg)).CopyTo(buff, 6);		//DATALEN
			_utf8.GetBytes(msg).CopyTo(buff, 10);								//BODY

			return SendCommand(buff);
		}

		/// <summary>
		/// 自分のユーザーデータを送信
		/// </summary>
		/// <returns>成功失敗</returns>
		public bool SendUserData ()
		{
			UserData data;
			if (!UserInfo.Read(Properties.Settings.Default.LastUser, out data)) {//自分のユーザーデータを取得
				return false;
			}

			byte[] bodyBuff = new byte[1000];
			int index = 0; int c;
			BitConverter.GetBytes((Int16) 4).CopyTo(bodyBuff, index);			//INFO_NUM
			index += 2;

			// 1: 名前
			BitConverter.GetBytes((Int16) 1).CopyTo(bodyBuff, index);			//INFO_ID
			index += 2;
			c = _utf8.GetByteCount(data.Name);
			BitConverter.GetBytes(c).CopyTo(bodyBuff, index);					//INFO_LEN	
			index += 4;
			_utf8.GetBytes(data.Name).CopyTo(bodyBuff, index);					//INFO_BODY
			index += c;

			// 2: 出身地
			BitConverter.GetBytes((Int16) 2).CopyTo(bodyBuff, index);			//INFO_ID
			index += 2;
			c = _utf8.GetByteCount(data.IsFrom.ToString());
			BitConverter.GetBytes(c).CopyTo(bodyBuff, index);					//INFO_LEN	
			index += 4;
			_utf8.GetBytes(data.IsFrom.ToString()).CopyTo(bodyBuff, index);		//INFO_BODY
			index += c;

			// 3: 文字色
			BitConverter.GetBytes((Int16) 3).CopyTo(bodyBuff, index);			//INFO_ID
			index += 2;
			c = 4;
			BitConverter.GetBytes(c).CopyTo(bodyBuff, index);					//INFO_LEN	
			index += 4;
			BitConverter.GetBytes((int) data.TextColor).CopyTo(bodyBuff, index);//INFO_BODY
			index += c;

			// 4: 文字サイズ
			BitConverter.GetBytes((Int16) 4).CopyTo(bodyBuff, index);			//INFO_ID
			index += 2;
			c = 4;
			BitConverter.GetBytes(c).CopyTo(bodyBuff, index);					//INFO_LEN	
			index += 4;
			BitConverter.GetBytes(data.FontSize * 10).CopyTo(bodyBuff, index);	//INFO_BODY
			index += c;

			// UNDONE その他の情報、ユーザー定義情報

			byte[] buff = new byte[10 + index + 1];

			_utf8.GetBytes("@:").CopyTo(buff, 0);								//コマンドの先頭
			BitConverter.GetBytes((Int16) (++_lastID)).CopyTo(buff, 2);			//ID
			BitConverter.GetBytes((Int16) 1).CopyTo(buff, 4);					//CMD
			BitConverter.GetBytes(index + 1).CopyTo(buff, 6);					//DATALEN
			Array.Copy(bodyBuff, 0, buff, 10, index + 1);						//BODY

			return SendCommand(buff);
		}

		/// <summary>
		/// 応答を送信
		/// </summary>
		/// <param name="statusID">応答の種類 0:成功 1:Ready 2:NotReady 255:失敗</param>
		/// <param name="packetID">応答の対象コマンドID</param>
		/// <returns></returns>
		public bool SendResponse (int statusID, int packetID)
		{
			if (statusID != 0 && statusID != 1 && statusID != 2 && statusID != 255) {//規定外のSTATUS_IDでないか確認
				return false;
			}

			byte[] buff = new byte[14];

			_utf8.GetBytes("@:").CopyTo(buff, 0);								//コマンドの先頭
			BitConverter.GetBytes((Int16) (++_lastID)).CopyTo(buff, 2);			//ID
			BitConverter.GetBytes((Int16) 3).CopyTo(buff, 4);					//CMD
			BitConverter.GetBytes(4).CopyTo(buff, 6);							//DATALEN

			BitConverter.GetBytes((Int16) statusID).CopyTo(buff, 10);			//STATUS_ID
			BitConverter.GetBytes((Int16) packetID).CopyTo(buff, 12);			//PACKET_ID

			return SendCommand(buff);
		}

		/// <summary>
		/// Pingを送信
		/// </summary>
		/// <returns>成功失敗</returns>
		public bool SendPing ()
		{
			byte[] buff = new byte[10];

			_utf8.GetBytes("@:").CopyTo(buff, 0);								//コマンドの先頭
			BitConverter.GetBytes((Int16) (++_lastID)).CopyTo(buff, 2);			//ID
			BitConverter.GetBytes((Int16) 4).CopyTo(buff, 4);					//CMD
			BitConverter.GetBytes(0).CopyTo(buff, 6);							//DATALEN

			return SendCommand(buff);
		}
	} //end of ChatBase (class)
} //end of ChatMio (namespace)
