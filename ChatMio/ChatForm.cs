using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using ChatMio.Properties;

namespace ChatMio
{
	public partial class ChatForm : Form
	{
		private ChatBase _chat;													//サーバー／クライアントオブジェクト格納用変数
		private bool _isConnected;												//接続済みフラグ
		private UserData _me = new UserData();									//自分のUserData
		private UserData _she = new UserData();									//相手のUserData
		private int _chatTextIndex;												//chatTextの末尾

		private delegate void ChangeTitleBarTextCallback (string text);			//タイトルバーのテキストを変えるためのデリゲート
		private delegate void SetPostBtnCallback (bool enable);					//投稿ボタンの有効無効を切り替えるためのデリゲート
		private delegate void SetConnectMenuCallback (bool enable);				//接続メニューの有効無効を切り替えるためのデリゲート
		private delegate void SetExitMenusCallback (bool enable);				//終了メニューの有効無効を切り替えるためのデリゲート
		private delegate void SetCursorCallback (bool waiting);					//カーソルの見た目を切り替えるためのデリゲート
		private delegate void ShowReconnectDialogCallback ();					//再接続するか尋ねるダイアログを表示するためのデリゲート
		private delegate void AppendMsgCallback (UserData user, string msg);	//相手又は自分のメッセージを表示する際のデリゲート
		private delegate void AppendSystemMsgCallback (string msg);				//アプリからのメッセージを表示する際のデリゲート

		public ChatForm ()
		{
			_chatTextIndex = 0;
			_isConnected = false;
			InitializeComponent();
			UserInfo.Read(Settings.Default.LastUser, out _me);					//_meに自分のUserDataを格納
		}

		private void ChatForm_Load (object sender, EventArgs e)					//フォームロード時
		{
			SetPostBtn(false);													//投稿ボタンを無効化する

			var addrListForm = new AddressListForm(Dns.GetHostEntry(Dns.GetHostName()).AddressList);
			addrListForm.ShowDialog(this);										//IPアドレスダイアログを開く

			StartServer();														//サーバースタート
		}

		private void StartServer ()
		{
			_chat = new ChatServer();											//ChatServerのインスタンス化
			((ChatServer) _chat).AddressIndex = Settings.Default.LastIPIndex;	//使用するIPアドレスをセット

			//デリゲートの登録
			_chat.Connected += Connected;
			_chat.ConnectionFailed += ConnectionFailed;
			_chat.MsgReceived += MsgReceived;
			_chat.UserDataReceived += UserDataReceived;
			_chat.ChatClosed += ChatClosed;
			_chat.ResponseReceived += ResponseReceived;
			_chat.PingReceived += PingReceived;

			_chat.Start();														//ChatServer.Startを実行

			statusLabel.Text = String.Format("{0}として接続を待機しています…", ((ChatServer) _chat).MyAddress);
		}

		private void StartClient (string ipAddr)
		{
			_chat = new ChatClient(ipAddr);										//ChatClientのインスタンス化

			//デリゲートの登録
			_chat.Connected += Connected;
			_chat.ConnectionFailed += ConnectionFailed;
			_chat.MsgReceived += MsgReceived;
			_chat.UserDataReceived += UserDataReceived;
			_chat.ChatClosed += ChatClosed;
			_chat.ResponseReceived += ResponseReceived;
			_chat.PingReceived += PingReceived;

			_chat.Start();														//ChatClient.Startを実行

			statusLabel.Text = String.Format("{0}に接続中です…", ipAddr);		//statusLabel更新
			SetCursor(true);													//カーソルを待機中のものに変更
		}

		private void ChatForm_FormClosing (object sender, FormClosingEventArgs e)//フォームが閉じられようとした時
		{
			if (_isConnected) {													//チャットが接続中なら
				if (MessageBox.Show(this,										//確認ダイアログを表示
						"チャットを切断してログアウトしますか？", "警告",
						MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
						MessageBoxDefaultButton.Button2)
						== DialogResult.No) { 									//Noの場合
					e.Cancel = true;											//フォームが閉じられるのを阻止
					return;
				}

				ChatLog.Save(_she.Name, chatBox.Text.Substring(_chatTextIndex));//チャットログを保存する
			}

			if (_chat != null) { _chat.Stop(); }							//サーバー又はクライアントを停止
			_chat = null;													//オブジェクトを破棄
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
			else { statusLabel.Text = "接続されていません"; }					//接続済みでない場合
		}

		private void AppendMsg (UserData user, string msg)						//chatBoxにメッセージを追加するメソッド
		{
			if (chatBox.InvokeRequired) {										//非UIスレッドからの呼び出し時
				var d = new AppendMsgCallback(AppendMsg);
				Invoke(d, new object[] { user, msg });							//UIスレッドでInvoke
			}
			else {
				int iLength = chatBox.Text.Length;
				chatBox.AppendText(String.Format("  ::{0}::\r\n", user.Name));	//chatBoxにユーザー名を追加
				chatBox.Select(iLength, user.Name.Length + 6);					//編集したい文字を選択
				chatBox.SelectionColor = Color.FromKnownColor(user.TextColor);	//文字色を赤に変更
				chatBox.SelectionFont = 										//文字サイズを11に変更
						new Font(chatBox.SelectionFont.FontFamily,
						11, FontStyle.Bold);

				iLength = chatBox.Text.Length;
				chatBox.AppendText(String.Format("{0}\r\n\r\n", msg));			//chatBoxにメッセージを追加
				chatBox.Select(iLength, msg.Length);							//編集したい文字を選択
				chatBox.SelectionColor = Color.FromKnownColor(user.TextColor);	//文字色を変更
				chatBox.SelectionFont = 										//文字サイズを変更
						new Font(chatBox.SelectionFont.FontFamily,
						user.FontSize, FontStyle.Bold);

				iLength = chatBox.Text.Length;
				chatBox.Select(iLength, iLength);								//選択を解除
			}
		}

		private void AppendSystemMsg (string msg)								//chatBoxにシステムメッセージを追加するメソッド
		{
			if (chatBox.InvokeRequired) {										//非UIスレッドからの呼び出し時
				var d = new AppendSystemMsgCallback(AppendSystemMsg);
				Invoke(d, new object[] { msg });								//UIスレッドでInvoke
			}
			else {
				int iLength = chatBox.Text.Length;
				chatBox.AppendText(String.Format("  /* {0} */\r\n", msg));		//chatBoxにユーザー名を追加
				chatBox.Select(iLength, msg.Length + 8);						//編集したい文字を選択
				chatBox.SelectionColor = Color.FromKnownColor(KnownColor.Red);	//文字色を赤に変更
				chatBox.SelectionFont = 										//文字サイズを12に変更
						new Font(chatBox.SelectionFont.FontFamily,
						12, FontStyle.Italic);									//スタイルをイタリック体に

				iLength = chatBox.Text.Length;
				chatBox.Select(iLength, iLength);								//選択を解除
			}
		}

		private void menuButton_Click (object sender, EventArgs e)				//メニューボタンクリック時
		{
			menuStrip.Show(menuButton, new Point(0, 0));						//メニューを表示
		}

		private void connectMenu_Click (object sender, EventArgs e)				//「接続」または「切断」メニューが選択されたとき
		{
			if (!_isConnected) {												//接続されていない場合
				var connForm = new ConnectForm();								//接続ダイアログ表示
				if (connForm.ShowDialog() == DialogResult.OK) {					//接続する場合
					SetConnectMenu(false);										//接続メニューをクリック不可にする
					_chat.Stop();												//サーバーを停止
					StartClient(Settings.Default.LastIP);						//クライアントを起動
				}
			}
			else {																//接続済みの場合
				if (_chat != null) { _chat.Stop(); }							//サーバー又はクライアントを停止

				AppendSystemMsg(String.Format("{0}との接続を切断しました", _she.Name));

				ChangeTitleBarText(String.Format("ChatMio"));					//タイトルバーのテキストを更新
				connectMenu.Text = "接続";										//メニューのテキストを更新
				SetPostBtn(false);												//投稿ボタンを使えないようにする
				_isConnected = false;											//接続済みフラグを下ろす

				AppendSystemMsg(												//チャットログを書き出し
						String.Format("\"{0}\"としてチャットログを保存しました",
						ChatLog.Save(_she.Name, chatBox.Text.Substring(_chatTextIndex))));
				chatBox.AppendText("\r\n\r\n");									//空行を2行追加

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

			if (UserInfo.Read(Settings.Default.LastUser, out _me)) {			//変更が成功していたら
				_chat.SendUserData();											//相手にユーザーデータを再送信
			}
		}

		private void removeUserMenu_Click (object sender, EventArgs e)			//「ユーザー情報を削除」メニュークリック時
		{
			if (MessageBox.Show(this, "ユーザー情報を削除してよろしいですか？", //確認用ダイアログを表示
					"ユーザー情報削除", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button2) == DialogResult.OK) {		// OKボタン押下時
				if (UserInfo.Remove(_me.Name)) {								// 削除成功時
					MessageBox.Show(this, "ユーザー情報を削除しました", "",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
					Close();												// フォームを閉じる
				}
				else {															// 削除失敗時
					MessageBox.Show(this, "処理が失敗しました", "失敗",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void logoutMenu_Click (object sender, EventArgs e)				// ログアウトメニュークリック時
		{
			if (_chat != null) { _chat.Stop(); }								// サーバー／クライアントを停止
			_chat = null;														// サーバー／クライアントを破棄
			Close();															// フォームを閉じる
		}

		private void exitMenu_Click (object sender, EventArgs e)				// 終了メニュークリック時
		{
			if (_chat != null) { _chat.Stop(); }								// サーバー／クライアントを停止
			_chat = null;														// サーバー／クライアントを破棄
			Program.Exit = true;												// アプリケーション終了フラグを立てる
			Close();
		}

		private void ChangeTitleBarText (string str)							//タイトルバーのテキストを変える
		{
			if (postButton.InvokeRequired) {									//非UIスレッドからの呼び出し時
				Invoke(new ChangeTitleBarTextCallback(ChangeTitleBarText),		//UIスレッドでInvoke
						new object[] { str });
			}
			else {
				Text = str;														//タイトルバーのテキスト変更
			}
		}

		private void SetPostBtn (bool enable)									//postButtonの有効無効を切り替える
		{
			if (postButton.InvokeRequired) {									//非UIスレッドからの呼び出し時
				Invoke(new SetPostBtnCallback(SetPostBtn),						//UIスレッドでInvoke
						new object[] { enable });
			}
			else {
				postButton.Enabled = enable;									//postButton切り替え
				postBox.Enabled = enable;
			}
		}

		private void SetConnectMenu (bool enable)								//接続メニューの有効無効を切り替える
		{
			if (InvokeRequired) {												//非UIスレッドからの呼び出し時
				Invoke(new SetConnectMenuCallback(SetConnectMenu),				//UIスレッドでInvoke
						new object[] { enable });
			}
			else {
				connectMenu.Enabled = enable;									//connectMenu切り替え
			}
		}

		private void SetExitMenus (bool enable)									//ログアウト・終了メニューの有効無効を切り替える
		{
			if (InvokeRequired) {												//非UIスレッドからの呼び出し時
				Invoke(new SetExitMenusCallback(SetExitMenus),					//UIスレッドでInvoke
						new object[] { enable });
			}
			else {
				logoutMenu.Enabled = enable;									//logoutMenu切り替え
				exitMenu.Enabled = enable;										//exitMenu切り替え
			}
		}

		private void SetCursor (bool waiting)									//カーソルの見た目を切り替える
		{
			if (InvokeRequired) {												//非UIスレッドからの呼び出し時
				Invoke(new SetCursorCallback(SetCursor),						//UIスレッドでInvoke
						new object[] { waiting });
			}
			else {
				if (waiting) {													//待ち状態の場合
					Cursor = Cursors.WaitCursor;								//待ち状態のカーソルに切り替え
					chatBox.Cursor = Cursors.WaitCursor;
				}
				else {															//待ち状態でない場合
					Cursor = Cursors.Default;									//通常のカーソルに切り替え
					chatBox.Cursor = Cursors.IBeam;
				}
			}
		}

		private void ShowReconnectDialog ()
		{
			if (InvokeRequired) {												//非UIスレッドからの呼び出し時
				Invoke(new ShowReconnectDialogCallback(ShowReconnectDialog));	//UIスレッドでInvoke
			}
			else {
				if (MessageBox.Show(this, "接続に失敗しました\n再試行しますか？",//やり直すかどうか尋ねる
						"失敗", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error,
						MessageBoxDefaultButton.Button2)
						== DialogResult.Retry) {								//再試行が選択された場合
					connectMenu.PerformClick();									//接続ダイアログ再表示
				}
			}
		}

		/* --- Server,Clientのイベントハンドラ --- */
		private void Connected (object obj, ConnectedEventArgs e)				//接続成功時
		{
			AppendSystemMsg(
					String.Format(
					_chat.GetType() == typeof(ChatServer) ? 					//サーバー／クライアントで異なるメッセージを出す
					"{0}からの接続を受け付けました" : "{0}への接続が完了しました",
					_she.Name));

			ChangeTitleBarText(String.Format("ChatMio  /*{0}との会話*/", _she.Name));//タイトルバーのテキストを更新
			statusLabel.Text = String.Format("{0}に接続完了", e.IpAddr);		//statusLabel更新
			connectMenu.Text = "切断";										  	//メニューのテキストを更新
			SetPostBtn(true);													//投稿ボタンを使えるようにする
			SetExitMenus(false);												//終了メニュー無効化
			_isConnected = true;												//接続済みフラグを立てる
			SetCursor(false);													//カーソルを通常のものにする
			SetConnectMenu(true);												//接続メニューを有効化する
		}

		private void ConnectionFailed (object obj)								//接続失敗時
		{
			connectMenu.Text = "接続";											//メニューのテキストを更新
			SetPostBtn(false);													//投稿ボタンを使えないようにする
			_isConnected = false;												//接続済みフラグを下ろす
			SetCursor(false);													//カーソルを通常のものにする
			SetConnectMenu(true);												//接続メニューを有効化する

			StartServer();														//サーバースタート

			ShowReconnectDialog();												//再試行するかどうか尋ねる
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

			ChangeTitleBarText(String.Format("ChatMio"));						//タイトルバーのテキストを更新
			connectMenu.Text = "接続";										  	//メニューのテキストを更新
			SetPostBtn(false);													//投稿ボタンを使えないようにする
			_isConnected = false;												//接続済みフラグを下ろす

			AppendSystemMsg(													//チャットログを書き出し
					String.Format("\"{0}\"としてチャットログを保存しました\r\n\r\n",
					ChatLog.Save(_she.Name, chatBox.Text.Substring(_chatTextIndex))));
			chatBox.AppendText("\r\n\r\n");										//空行を2行追加
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
