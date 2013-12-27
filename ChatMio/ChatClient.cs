using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatMio
{
	class ChatClient : ChatBase
	{
		private readonly IPAddress _ipAddr;										//相手マシンのIPアドレス
		private Thread _connThread;												//接続用スレッド
		private Thread _readThread;												//待機用スレッド

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
			MyDebug.WriteLine(this, "クライアントスタート");

			_connThread = new Thread(this.ConnectThread);						//接続用スレッドを作成
			_connThread.Start();												//スレッドスタート
		}

		private void ConnectThread ()											//接続用スレッド
		{
			MyDebug.WriteLine(this, "接続用スレッド開始");

			try {
				_tcpClient.Connect(_ipAddr, _port);								//サーバーと接続
				_netStream = _tcpClient.GetStream();

				IPAddress serverIP = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Address;
				MyDebug.WriteLine(this, "{0}と接続完了", serverIP);
				InvokeConnectedEvent(serverIP.ToString());						//イベント発行
				_isConnected = true;											//接続済みフラグを立てる

				if (SendUserData()) { MyDebug.WriteLine(this, "ユーザー情報送信完了"); }//ユーザー情報送信
				else { MyDebug.WriteLine(this, "ユーザー情報送信失敗"); }

				_readThread = new Thread(ReadThread);
				_readThread.Start();												//コマンド受信用スレッドスタート
			}
			catch (ThreadAbortException e) {
				MyDebug.WriteLine(this, "接続用スレッドが外部により強制終了 {0}", e);
			}
			catch (SystemException e) {
				MyDebug.WriteLine(this, "接続用スレッドが何らかの例外により終了 {0}", e);
				InvokeConnectionFailedEvent();
			}
		}

		private void ReadThread ()												//待機用スレッド
		{
			MyDebug.WriteLine(this, "待機用スレッド開始");

			try {
				while (true) {
					if (_tcpClient.Available > 0) {								//受け取れるデータが存在する場合
						MyDebug.WriteLine(this, "データ受信開始");
						var buff = new byte[_tcpClient.Available];
						_netStream.Read(buff, 0, buff.Length);					//ストリームから読み取り
						ParseCommand(buff);										//コマンドをパース
					}
					Thread.Sleep(1000);											//1sごとに実行
				}
			}
			catch (ThreadAbortException e) {
				MyDebug.WriteLine(this, "待機用スレッドが外部により強制終了 {0}", e);
			}
			catch (SystemException e) {
				MyDebug.WriteLine(this, "待機用スレッドが何らかの例外により終了 {0}", e);
			}
		}

		/// <summary>
		/// チャットを終了する
		/// </summary>
		public override void Stop ()
		{
			MyDebug.WriteLine(this, "クライアント停止処理を実行");

			if (_isConnected) {												//接続されていた場合
				var buff = new byte[10];

				_utf8.GetBytes("@:").CopyTo(buff, 0);						//コマンドの先頭
				BitConverter.GetBytes((Int16) (++_lastID)).CopyTo(buff, 2);	//ID
				BitConverter.GetBytes((Int16) 2).CopyTo(buff, 4);			//CMD
				BitConverter.GetBytes(0).CopyTo(buff, 6);					//DATALEN

				SendCommand(buff);											//切断コマンドを送信
			}
			if (_connThread != null) { _connThread.Abort(); }				//接続用スレッド中断
			if (_readThread != null) { _readThread.Abort(); }				//待機用スレッド中断

			if (_netStream != null) { _netStream.Close(); }					//ストリームを閉じる
			if (_tcpClient != null) { _tcpClient.Close(); }					//クライアント停止
			MyDebug.WriteLine(this, "切断完了");
		}
	}
}
