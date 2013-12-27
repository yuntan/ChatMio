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
	class ChatServer : ChatBase
	{

		private string _hostName;												//自分のマシンのホスト名

		private int _myIpIndex;													//AddressIndexの内部変数
		private TcpListener _tcpListener;
		private Thread _svrThread;												//サーバースレッド

		public IPAddress MyAddress { get; private set; }						//自分のマシンのIPアドレス
		public IPAddress[] AddressList { get; private set; }					//選択可能なIPアドレスのリスト
		public int AddressIndex													//AddressList用Index
		{
			get { return _myIpIndex; }
			set
			{
				if (AddressList == null) { return; }							//AddressListが空の場合設定不可
				if (value < AddressList.Length) {								//値がAddressListの大きさを超えていないかチェック
					_myIpIndex = value;
					MyAddress = AddressList[value];
				}
			}
		}

		/// <summary>
		/// コンストラクタにて変数設定、接続処理
		/// </summary>
		public ChatServer ()
		{
			_hostName = Dns.GetHostName();										// 自分のマシンのホスト名
			MyDebug.WriteLine(this, "自分のマシンのホスト名 {0}", _hostName);

			AddressList = Dns.GetHostEntry(_hostName).AddressList;				// 自分のマシンのIPアドレスを取得
			MyAddress = AddressList.Last();										//アドレスのリストの最後の項目を指定しておく
		}

		/// <summary>
		/// サーバーをスタート&待機
		/// </summary>
		public override void Start ()
		{
			MyDebug.WriteLine(this, "{0}としてサーバー開始", MyAddress);

			if (_svrThread != null) { _svrThread.Abort(); }

			_svrThread = new Thread(this.ServerThread);
			_svrThread.IsBackground = true;										//バックグラウンドスレッドとして設定
			_svrThread.Start();													//別スレッドで接続待機開始
		}

		/// <summary>
		/// 待機用スレッド
		/// </summary>
		private void ServerThread ()
		{
			MyDebug.WriteLine(this, "待機用スレッド開始");

			try {
				_tcpListener = new TcpListener(MyAddress, _port);
				MyDebug.WriteLine(this, "listenerを開始");
				_tcpListener.Start();											// 接続を待機(ここで処理が止まる)

				_tcpClient = _tcpListener.AcceptTcpClient();					// 接続要求があった場合受け入れ
				MyDebug.WriteLine(this, "接続要求あり、受け入れ");

				_netStream = _tcpClient.GetStream();							//NetworkStreamを取得

				if (!_isConnected) {										//これまで接続されていなかった場合
					IPAddress clientIP = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Address;
					MyDebug.WriteLine(this, "{0}と接続試行中", clientIP);
					MyDebug.WriteLine(this, "ユーザーデータを送信");
					SendUserData();												//ユーザー情報送信
				}

				while (true) {
					//送られてきたデータを受信

					var memStream = new MemoryStream();							//一時格納用MemoryStream
					var buff = new byte[256];
					while (_netStream.DataAvailable) {
						MyDebug.WriteLine(this, "_netStream.DataAvailable");
						int readSize = _netStream.Read(buff, 0, buff.Length);
						if (readSize == 0) {									//読み取りサイズが0の場合切断されたと判断
							MyDebug.WriteLine(this, "readSize == 0  接続をクローズ");
							InvokeChatClosedEvent();							//イベント発行
							Stop();												//サーバーを停止
							return;
						}
						memStream.Write(buff, 0, readSize);						//読み取ったサイズ分だけMemoryStreamに移す
					}

					byte[] bytCmd = memStream.ToArray();						//MemoryStreamからByteArrayに移す
					memStream.Close();											//MemoryStreamを閉じる
					if (bytCmd.Length != 0) {									//文字列を受け取っていた場合
						MyDebug.WriteLine(this, "バイト長 != 0  コマンドの受信を確認");
						ParseCommand(bytCmd);

						if (!_isConnected) {									// 接続フラグが立っていなかった場合
							IPAddress clientIP = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Address;
							MyDebug.WriteLine(this, "{0}と接続完了を確認", clientIP);
							InvokeConnectedEvent(clientIP.ToString());			//接続済みイベントを発行
							_isConnected = true;								//接続済みフラグを立てる
						}
					}
					Thread.Sleep(1000);
				}
			}
			catch (ThreadAbortException e) {
				MyDebug.WriteLine(this, "サーバースレッドが外部により強制終了 {0}", e);
			}
			catch (SystemException e) {
				MyDebug.WriteLine(this, "サーバースレッドが何らかの例外により終了", e);
			}
		}

		/// <summary>
		/// 切断する
		/// </summary>
		public override void Stop ()
		{
			MyDebug.WriteLine(this, "サーバー停止処理を実行");

			if (_isConnected) {													//接続されていた場合
				var buff = new byte[10];	  

				_utf8.GetBytes("@:").CopyTo(buff, 0);							//コマンドの先頭
				BitConverter.GetBytes((Int16) (++_lastID)).CopyTo(buff, 2);		//ID
				BitConverter.GetBytes((Int16) 2).CopyTo(buff, 4);				//CMD
				BitConverter.GetBytes(0).CopyTo(buff, 6);						//DATALEN

				SendCommand(buff);												//切断コマンドを送信
			}

			if (_svrThread != null) { _svrThread.Interrupt(); }					//サーバースレッドを中断

			if (_netStream != null) { _netStream.Close(); }						//ストリームを閉じる
			if (_tcpClient != null) { _tcpClient.Close(); }						//クライアント停止
			if (_tcpListener != null) { _tcpListener.Stop(); }					//Listenerをストップ

			MyDebug.WriteLine(this, "サーバー停止処理完了");
		}
	} //end of ChatServer (class)
} //end of ChatMio (namespace)
