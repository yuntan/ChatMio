using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace ChatMio
{
	class ChatClient : ChatBase
	{
		private IPAddress _ipAddr;												//相手マシンのIPアドレス

		/// <summary>
		/// クライアントのコンストラクタ
		/// </summary>
		/// <param name="sIpAddr">接続先のIPアドレス</param>
		/// <exception cref="SocketException"></exception>
		public ChatClient (string sIpAddr)
		{
			_tcpClient = new TcpClient();
			_ipAddr = IPAddress.Parse(sIpAddr);
		}

		/// <summary>
		/// 接続開始メソッド
		/// </summary>
		public override void Start ()											
		{
			Debug.WriteLine("クライアントスタート");

			Thread connThread = new Thread(this.ConnectThread);					//接続用スレッドを作成
			connThread.Start();													//スレッドスタート
		}

		private void ConnectThread ()
		{
			try {
				_tcpClient.Connect(_ipAddr, _port);								//サーバーと接続
				_netStream = _tcpClient.GetStream();

				IPAddress serverIP = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Address;
				Debug.WriteLine("{0}と接続完了", serverIP);
				InvokeConnectedEvent(serverIP.ToString());						//イベント発行
				_isConnected = true;											//接続済みフラグを立てる

				if (SendUserData()) { Debug.WriteLine("ユーザー情報送信完了"); }//ユーザー情報送信
				else { Debug.WriteLine("ユーザー情報送信失敗"); }

				Thread readThread = new Thread(this.ReadThread);
				readThread.Start();												//コマンド受信用スレッドスタート
			}
			catch { InvokeConnectionFailedEvent(); }
		}

		private void ReadThread ()												//待機用スレッド
		{
			try {
				while (true) {
					if (_tcpClient.Available > 0) {								//受け取れるデータが存在する場合
						var buff = new byte[_tcpClient.Available];
						_netStream.Read(buff, 0, buff.Length);					//ストリームから読み取り
						try { ParseCommand(buff); }								//コマンドのパースを試みる
						catch { Debug.WriteLine("パース失敗"); }
					}
					Thread.Sleep(1000);											//1sごとに実行
				}
				
			}
			catch { }
		}

		/// <summary>
		/// チャットを終了する
		/// </summary>
		public override void Stop ()											
		{
			byte[] buff = new byte[10];

			_utf8.GetBytes("@:").CopyTo(buff, 0);								//コマンドの先頭
			BitConverter.GetBytes((Int16) (++_lastID)).CopyTo(buff, 2);			//ID
			BitConverter.GetBytes((Int16) 2).CopyTo(buff, 4);					//CMD
			BitConverter.GetBytes(0).CopyTo(buff, 6);							//DATALEN

			try { SendCommand(buff); }
			catch { }
			finally {
				if (_netStream != null) { _netStream.Close(); }					//ストリームを閉じる
				if (_tcpClient != null) { _tcpClient.Close(); }					//クライアント停止
				Debug.WriteLine("切断完了");
			}
		}
	}
}
