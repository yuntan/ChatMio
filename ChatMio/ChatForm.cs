using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace ChatMio
{
	public partial class ChatForm : Form
	{
		private ChatBase _chat;													//サーバー／クライアントオブジェクト格納用変数
		private bool _isConnected = false;										//接続済みフラグ
		private UserData _me;													//自分のUserData
		private UserData _she;													//相手のUserData
		private int _chatTextIndex = 0;											//chatTextの末尾

		private delegate void OpenAddrListCallback ();							//IPアドレスのリストを開くためのデリゲート
		private delegate void SetPostBtnCallback (bool enable);					//投稿ボタンの有効無効を切り替えるためのデリゲート
		private delegate void SetExitMenusCallback (bool enable);				//終了メニューの有効無効を切り替えるためのデリゲート
		private delegate void AppendMsgCallback (UserData user, string msg);	//相手又は自分のメッセージを表示する際のデリゲート
		private delegate void AppendSystemMsgCallback (string msg);				//アプリからのメッセージを表示する際のデリゲート

		public ChatForm ()
		{
			InitializeComponent();
			UserInfo.Read(Properties.Settings.Default.LastUser, out _me);		//_meに自分のUserDataを格納
		}																		

		private void ChatForm_Load (object sender, EventArgs e)					//フォームロード時
		{
			postButton.Enabled = false;											//投稿ボタンを無効化する

			_chat = new ChatServer();											//ChatServerのインスタンス化
			var addrListForm = new AddressListForm(((ChatServer) _chat).AddressList);
			addrListForm.ShowDialog(this);										//IPアドレスダイアログを開く

			StartServer();														//サーバースタート
		}

		private void StartServer ()
		{
			_chat = new ChatServer();											//ChatServerのインスタンス化
			((ChatServer) _chat).AddressIndex = Properties.Settings.Default.LastIPIndex;//使用するIPアドレスをセット

			//デリゲートの登録
			_chat.Connected += new ChatServer.ConnectedHandler(this.Connected);
			_chat.ConnectionFailed += new ChatServer.ConnectionFailedHander(this.ConnectionFailed);
			_chat.MsgReceived += new ChatServer.MsgReceivedHandler(this.MsgReceived);
			_chat.UserDataReceived += new ChatServer.UserDataReceivedHandler(this.UserDataReceived);
			_chat.ChatClosed += new ChatServer.ChatClosedHandler(this.ChatClosed);
			_chat.ResponseReceived += new ChatServer.ResponseReceivedHandler(this.ResponseReceived);
			_chat.PingReceived += new ChatServer.PingReceivedHandler(this.PingReceived);

			_chat.Start();														//ChatServer.Startを実行

			statusLabel.Text = String.Format("{0}として接続を待機しています…", ((ChatServer) _chat).MyAddress);
		}

		private void StartClient (string ipAddr)
		{
			_chat = new ChatClient(ipAddr);										//ChatClientのインスタンス化

			//デリゲートの登録
			_chat.Connected += new ChatClient.ConnectedHandler(this.Connected);
			_chat.ConnectionFailed += new ChatClient.ConnectionFailedHander(this.ConnectionFailed);
			_chat.MsgReceived += new ChatClient.MsgReceivedHandler(this.MsgReceived);
			_chat.UserDataReceived += new ChatClient.UserDataReceivedHandler(this.UserDataReceived);
			_chat.ChatClosed += new ChatClient.ChatClosedHandler(this.ChatClosed);
			_chat.ResponseReceived += new ChatClient.ResponseReceivedHandler(this.ResponseReceived);
			_chat.PingReceived += new ChatClient.PingReceivedHandler(this.PingReceived);

			_chat.Start();														//ChatClient.Startを実行

			statusLabel.Text = String.Format("{0}に接続中です…", ipAddr);		//statusLabel更新
		}

		/* --- GUIのイベントハンドラ --- */
		private void postButton_Click (object sender, EventArgs e)				//投稿ボタン押下時
		{
			if (_isConnected) {													//接続済みの場合
				if (postBox.Text != "") {
					_chat.SendMessage(postBox.Text);							//メッセージコマンドを送信

					AppendMsg(_me, postBox.Text);								//chatBoxに追記

					postBox.Text = "";											//投稿ボックスを空にする
				}
			}
			else { statusLabel.Text = "接続されていません"; }				    //接続済みでない場合
		}

		private void AppendMsg (UserData user, string msg)						//chatBoxにメッセージを追加するメソッド
		{
			if (chatBox.InvokeRequired) {										//非UIスレッドからの呼び出し時
				var d = new AppendMsgCallback(AppendMsg);
				this.Invoke(d, new object[] { user, msg });						//UIスレッドでInvoke
			}
			else {
				int iLength = chatBox.Text.Length;
				chatBox.AppendText(String.Format("{0}: ", user.Name));			//chatBoxにユーザー名を追加
				chatBox.Select(iLength, user.Name.Length + 2);					//編集したい文字を選択
				chatBox.SelectionColor = Color.FromKnownColor(KnownColor.Red);	//文字色を赤に変更
				chatBox.SelectionFont = 										//文字サイズを11に変更
						new Font(chatBox.SelectionFont.FontFamily,
						11, FontStyle.Italic);

				iLength += user.Name.Length + 2;
				chatBox.AppendText(String.Format("{0}\r\n", msg));				//chatBoxにメッセージを追加
				chatBox.Select(iLength, msg.Length);							//編集したい文字を選択
				chatBox.SelectionColor = Color.FromKnownColor(user.TextColor);	//文字色を変更
				chatBox.SelectionFont = 										//文字サイズを変更
						new Font(chatBox.SelectionFont.FontFamily,
						user.FontSize, FontStyle.Bold);

				chatBox.Select(0, 0);											//選択を解除
			}
		}																		
		private void AppendSystemMsg (string msg)								//chatBoxにシステムメッセージを追加するメソッド
		{
			if (chatBox.InvokeRequired) {										//非UIスレッドからの呼び出し時
				var d = new AppendSystemMsgCallback(AppendSystemMsg);
				this.Invoke(d, new object[] { msg });							//UIスレッドでInvoke
			}
			else {
				int iLength = chatBox.Text.Length;
				chatBox.AppendText(String.Format("  {0}\r\n", msg));			//chatBoxにユーザー名を追加
				chatBox.Select(iLength, msg.Length + 2);						//編集したい文字を選択
				chatBox.SelectionColor = Color.FromKnownColor(KnownColor.Red);	//文字色を赤に変更
				chatBox.SelectionFont = 										//文字サイズを12に変更
						new Font(chatBox.SelectionFont.FontFamily,
						12, FontStyle.Italic);

				chatBox.Select(0, 0);											//選択を解除
			}
		}

		private void menuButton_Click (object sender, EventArgs e)				//メニューボタンクリック時
		{
			menuStrip.Show(menuButton, new Point(0, 0));						//メニューを表示
		}

		private void connectMenu_Click (object sender, EventArgs e)				//「接続」または「切断」メニューが選択されたとき
		{
			if (!_isConnected) {												//接続されていない場合
				var connForm = new ConnectForm();
				if (connForm.ShowDialog() == DialogResult.OK) {
					_chat.Stop();												//サーバーを停止
					StartClient(Properties.Settings.Default.LastIP);			//クライアントを起動
				}
			}
			else {																//接続済みの場合
				if (_chat != null) { _chat.Stop(); }							//サーバー又はクライアントを停止

				AppendSystemMsg(String.Format("{0}との接続を切断しました", _she.Name));

				connectMenu.Text = "接続";										//メニューのテキストを更新
				SetPostBtn(false);												//投稿ボタンを使えないようにする
				_isConnected = false;											//接続済みフラグを下ろす

				AppendSystemMsg(String.Format("\"{0}\"としてチャットログを保存しました",//チャットログを書き出し 
						ChatLog.Save(_she.Name, chatBox.Text.Substring(_chatTextIndex))));

				SetExitMenus(true);												//終了メニューを有効化

				StartServer();													//サーバーを開始
			}
		}

		private void userListMenu_Click (object sender, EventArgs e)			//「ユーザー一覧」メニュークリック時
		{
			var usrLstForm = new UserListForm();
			usrLstForm.ShowDialog();											//ユーザー一覧フォーム表示
		}

		private void modifyMenu_Click (object sender, EventArgs e)
		{
			var regForm = new RegisterForm(_me.Name);
			regForm.ShowDialog();												//変更用フォームを立ち上げる

			if (UserInfo.Read(Properties.Settings.Default.LastUser, out _me)) {	//変更が成功していたら
				_chat.SendUserData();											//相手にユーザーデータを再送信
			}
		}

		private void removeUserMenu_Click (object sender, EventArgs e)			//「ユーザー情報を削除」メニュークリック時
		{
			if (MessageBox.Show(this, "ユーザー情報を削除してよろしいですか？", //確認用ダイアログを表示
					"ユーザー情報削除", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button2) == DialogResult.OK) {		//OKボタン押下時
				if (UserInfo.Remove(_me.Name)) {								//削除成功時
					MessageBox.Show(this, "ユーザー情報を削除しました", "",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
					this.Close();												//フォームを閉じる
				}
				else {															//削除失敗時
					MessageBox.Show(this, "処理が失敗しました", "失敗",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void logoutMenu_Click (object sender, EventArgs e)				//ログアウトメニュークリック時
		{
			this.Close();														//フォームを閉じる
		}

		private void exitMenu_Click (object sender, EventArgs e)				//終了メニュークリック時
		{
			Program.Exit = true;												//アプリケーション終了フラグを立てる
			this.Close();
		}

		private void SetPostBtn (bool enable)									//postButtonの有効無効を切り替える
		{
			if (postButton.InvokeRequired) {									//非UIスレッドからの呼び出し時
				var d = new SetPostBtnCallback(SetPostBtn);
				this.Invoke(d, new object[] { enable });						//UIスレッドでInvoke
			}
			else {
				postButton.Enabled = enable;									//postButton切り替え
			}
		}

		private void SetExitMenus (bool enable)									//ログアウト・終了メニューの有効無効を切り替える
		{
			if (this.InvokeRequired) {											//非UIスレッドからの呼び出し時
				var d = new SetExitMenusCallback(SetExitMenus);
				this.Invoke(d, new object[] { enable });						//UIスレッドでInvoke
			}
			else {
				logoutMenu.Enabled = enable;									//logoutMenu切り替え
				exitMenu.Enabled = enable;										//exitMenu切り替え
			}
		}

		/* --- Server,Clientのイベントハンドラ --- */
		private void Connected (object obj, ConnectedEventArgs e)				//接続成功時
		{
			statusLabel.Text = String.Format("{0}に接続完了", e.IpAddr);		//statusLabel更新
			connectMenu.Text = "切断";										  	//メニューのテキストを更新
			SetPostBtn(true);													//投稿ボタンを使えるようにする
			SetExitMenus(false);												//終了メニュー無効化
			_isConnected = true;												//接続済みフラグを立てる
		}

		private void ConnectionFailed (object obj)								//接続失敗時
		{
			statusLabel.Text = "サーバーへの接続に失敗しました";				//statusLabel更新
			connectMenu.Text = "接続";											//メニューのテキストを更新
			SetPostBtn(false);													//投稿ボタンを使えないようにする
			_isConnected = false;												//接続済みフラグを下ろす
		}

		private void MsgReceived (object server, MsgReceivedEventArgs e)		//メッセージ受信時
		{
			AppendMsg(_she, e.Message);											//chatBoxにメッセージ追加
		}

		private void UserDataReceived (object obj, UserDataReceivedEventArgs e)	//ユーザー情報受信時
		{
			_she = e.Data;														//ユーザー情報を変数に保存											   
		}

		private void ChatClosed (object obj)									//チャット終了時
		{
			AppendSystemMsg(String.Format("{0}が接続を切断しました", _she.Name));//chatBoxにシステムメッセージ追加

			connectMenu.Text = "接続";										  	//メニューのテキストを更新
			SetPostBtn(false);													//投稿ボタンを使えないようにする
			_isConnected = false;												//接続済みフラグを下ろす
				
			AppendSystemMsg(String.Format("\"{0}\"としてチャットログを保存しました",//チャットログを書き出し 
					ChatLog.Save(_she.Name, chatBox.Text.Substring(_chatTextIndex))));
			_chatTextIndex = chatBox.Text.Length;								//_chatTextIndex更新

			SetExitMenus(true);													//終了メニュー有効化

			StartServer();														//サーバーを開始
		}

		private void ResponseReceived (object obj)								//応答受信時
		{

		}

		private void PingReceived (object obj, PingEventArgs e)					//Ping受信時
		{

		}
	}
}
