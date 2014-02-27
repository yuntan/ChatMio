using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace ChatMio
{
	public partial class UserListForm : Form
	{
        /// <summary>
        /// dataGridViewに表示される内容
        /// </summary>
		private UserData[] _users;

        #region コンストラクタ
        public UserListForm ()
		{
			InitializeComponent();

			//Test();															//ソートテストを実行
		}
        #endregion //コンストラクタ

        #region private string[] SelectedUsers 選択されているデータのユーザー名の配列を取得
        /// <summary>
        /// 現在選択されているデータのユーザー名の配列を取得
        /// </summary>
        private string[] SelectedUsers 
        {
            get{
                var rows = dataGridView.SelectedRows.Cast<DataGridViewRow>();   //選択されている行
                var cellsInCells = rows.Select(r => r.Cells.Cast<DataGridViewCell>()); //選択されている行のセルの行ごとのコレクションのコレクション
                var cells = cellsInCells.Select(                                //列名がNameのセルのみ抜き出しコレクション化
                        cells2 => cells2.Where(c => c.OwningColumn.DataPropertyName == "Name").First());
                return cells.Select(c => c.Value.ToString()).ToArray();         //セルの値を抜き出し配列に加工
            }
        }
        #endregion //SelectedUsers

        #region private void ModifyUserData ユーザー情報を変更する
        /// <summary>
        /// ユーザー情報を変更する
        /// </summary>
        /// <param name="name">変更するユーザーのユーザー名</param>
        private void ModifyUserData (string name)
        {
            var regForm = new RegisterForm(name);						        //選択されたセルがある行のユーザー名を指定
            regForm.ShowDialog(this);										    //変更フォームを表示

            if (!UserInfo.ReadAll(out _users)) { 							    //ユーザー情報の取得ができなかった場合
                MessageBox.Show("ユーザー情報の取得に失敗しました");		    //ダイアログを表示
                return;
            }
            dataGridView.DataSource = SortUserData(_users);					    //dataGridViewのデータを更新
        }
        #endregion

        #region GUIのイベントハンドラ
        private void UserListForm_Load (object sender, EventArgs e)				//フォームロード時
		{
            Cursor = Cursors.WaitCursor;                                        //待ち状態のカーソルに変更

			if (Properties.Settings.Default.Pyon) { 							//管理者権限でログインしていた場合
				modifyButton.Visible = true;									//modifyButtonを可視化する
                removeButton.Visible = true;									//removeButtonを可視化する
			}									
			else {																//管理者権限でなかった場合
				modifyButton.Visible = false;									//modifyButtonを不可視化する
				removeButton.Visible = false;									//removeButtonを不可視化する
			}

			if (!UserInfo.ReadAll(out _users)) { 								//ユーザー情報の取得ができなかった場合
				MessageBox.Show("ユーザー情報の取得に失敗しました");			//ダイアログを表示
				return;
			}
			dataGridView.DataSource = SortUserData(_users);						//dataGridViewにデータをセット

            //foreach (var column in dataGridView.Columns.Cast<DataGridViewColumn>()) {
            //    column.SortMode = DataGridViewColumnSortMode.Automatic;         //各列をユーザーがソート可能にする
            //}
            Cursor = Cursors.Default;                                           //通常のカーソルに変更
        }

		private void modifyButton_Click (object sender, EventArgs e)			//変更ボタン押下時
		{
            string[] names = SelectedUsers;                                     //選択中のユーザーを取得
			if (names.Count() == 1) {											//選択中のユーザーが一人だけだった場合
                ModifyUserData(names.First());                                  //ユーザー情報を変更
            }
			else {																//セルが選択されていない又は2つ以上選択されている時
				MessageBox.Show(this, "ユーザーを一人選択してください", "エラー",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void removeButton_Click (object sender, EventArgs e)			//削除ボタン押下時
		{
            string[] names = SelectedUsers;                                     //選択中のユーザーを取得

            if (names.Count() == 0) {                                           //何も選択されていなかったら
                MessageBox.Show(this, "ユーザー情報が選択されていません。",     //エラーメッセージを表示
                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                return;                                                         //終了
            }
            if (MessageBox.Show(this,                                           //確認用ダイアログを表示
                    "以下のユーザーの情報を削除してよろしいですか？\n"
                    + String.Join("\n", names),
                    "ユーザー情報削除", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) == DialogResult.Cancel) {  //Cancelボタン押下時
                return;														    //何もせず終了
            }

            Cursor = Cursors.WaitCursor;                                        //待ち状態のカーソルに変更

            names.Select(n => UserInfo.Remove(n));                              
            foreach (string name in names) {
                if (!UserInfo.Remove(name)) {                                   //ユーザー情報を削除
                    MessageBox.Show(this,                                       //削除に失敗したらエラーメッセージを表示
                            "ユーザー情報の削除に失敗しました。",
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                    return;                                                     //終了
                }
            }
            if (!UserInfo.ReadAll(out _users)) { 							    //ユーザー情報の取得ができなかった場合
                MessageBox.Show("ユーザー情報の取得に失敗しました");		    //エラーメッセージを表示
                return;                                                         //終了
            }
            dataGridView.DataSource = SortUserData(_users);					    //dataGridViewを更新
            Cursor = Cursors.Default;                                           //通常のカーソルに変更
		}

		private void printButton_Click (object sender, EventArgs e)				//印刷ボタン押下時
		{
            var form = new PrintUsersForm();
            form.ShowDialog();
		}

        private void dataGridView_CellContentDoubleClick (object sender, DataGridViewCellEventArgs e) //表のデータのダブルクリック時
        {
            if (!Properties.Settings.Default.Pyon) { 							//管理者権限でのログインでなかった場合
                return;                                                         //何もしない
            }									
            if (e.RowIndex < 1) { return; }                                     //ColumnHeaderがダブルクリックされた場合終了
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];                //ダブルクリックされた行
            var cells = row.Cells.Cast<DataGridViewCell>();                     //行に含まれるセルのコレクション
            string name = cells.Where(c => c.OwningColumn.DataPropertyName == "Name") //列名がNameのセルの値を抜き出し
                    .First().Value.ToString();
            ModifyUserData(name);                                               //ユーザー情報を変更する
        }

#endregion //GUIのイベントハンドラ

        #region ソート関連
        /// <summary>
        /// ソートテスト用関数
        /// </summary>
		private void Test ()
		{
			foreach (int i in new int[] { 100, 1000, 10000, 100000, 1000000 }) {
				Console.WriteLine("データ数: {0}", i);
				UserData[] testData = GenerateTestData(i);

				Console.WriteLine("自作アルゴリズムでソート");
				var sw = new Stopwatch();
				sw.Start();
				UserData[] myData = SortUserData(testData);
				sw.Stop();
				Console.WriteLine("経過時間: {0}ms", sw.ElapsedMilliseconds);
				Console.WriteLine();

				Console.WriteLine("システムのアルゴリズムでソート");
                sw.Reset(); sw.Start();
				Array.Sort(testData, new UserDataComp());
				//UserData[] ud = SortUserData(testData, 2);
				sw.Stop();
				Console.WriteLine("経過時間: {0}ms", sw.ElapsedMilliseconds);
				Console.WriteLine();

				Console.WriteLine("データを検証");
				for (int j = 0; j < testData.Length; j++) {
					if (myData[j].Name.CompareTo(testData[j].Name) != 0) {
						Console.WriteLine("{0}番目のデータに不整合を発見", j + 1);
						Console.WriteLine("自作 : {0}, システム: {1}", myData[j].Name, testData[j].Name);
					}
				}
				Console.WriteLine("検証終了");
				Console.WriteLine();
				Console.WriteLine();
			}
		}

		/// <summary>
		/// UserDataをソートする
		/// </summary>
		/// <param name="data"></param>
		/// <param name="methodId">ソートに用いるアルゴリズム</param>
		/// <returns></returns>
		private UserData[] SortUserData (UserData[] data)
		{
			var root = new TreeNode(data[0]);								//0番目の項目をルートとする
			TreeNode current;

			for (int i = 1; i < data.Length; i++) {							//i番目の項目に対する処理
				current = root;
				while (true) {
					if (data[i].Name.CompareTo(current.Data.Name) < 0) {	//i番目の項目がcurrentより前にあった場合
						if (current.LeftChild != null) {					//currentの左側の子が空でない場合
							current = current.LeftChild;					//currentを左下に移動
							continue;										//続行
						}
						else {												//currentの左側の子が空の場合
							current.LeftChild = new TreeNode(data[i]);		//currentの左側に自分を入れる
							current.LeftChild.Parent = current;				//自分の親をcurrentにする
							break;											//ループから抜ける
						}
					}
					else {													//i番目の項目がcurrentより後ろにあった場合
						if (current.RightChild != null) {					//currentの右側の子が空でない場合
							current = current.RightChild;					//currentを右下に移動
							continue;										//続行
						}
						else {												//currentの右側の子が空の場合
							current.RightChild = new TreeNode(data[i]);		//currentの右側に自分を入れる
							current.RightChild.Parent = current;			//自分の親をcurrentにする
							break;											//ループから抜ける
						}
					}
				}
			}

			current = root;
			while (true) {													//ループで先頭を探す
				if (current.LeftChild != null) { current = current.LeftChild; }
				else { break; }												//currentが先頭を示す
			}

			var ret = new UserData[data.Length];							//返り値となる配列
			ret[0] = current.Data;											//先頭を配列に追加

			for (int i = 1; i < ret.Length; i++) {							//自分の次の要素を探すループ
				if (current.RightChild != null) {							//右に子供がいる場合
					current = current.RightChild;							//右に進む
					while (true) {
						if (current.LeftChild != null) {					//左に子供がいる場合
							current = current.LeftChild;					//左に進む
							continue;										//ループ
						}
						else {												//左に子供がいなかった場合
							ret[i] = current.Data;							//発見！
							break;											//ループを抜ける
						}
					}
				}
				else {														//右に子供がいなかった場合
					while (true) {
						TreeNode before = current;							//一つ前の要素
						current = current.Parent;							//親に進む
						if (current.LeftChild != null
							&& current.LeftChild.Equals(before)) {			//一つ前が親の左だった場合
							ret[i] = current.Data;							//発見！
							break;											//ループを抜ける
						}
						else {												//一つ前が親の右だった場合
							continue;										//ループ
						}
					}
				}
			}

			return ret;
		}

		private class TreeNode
		{
			public TreeNode Parent { get; set; }
			public TreeNode LeftChild { get; set; }
			public TreeNode RightChild { get; set; }
			public UserData Data { get; private set; }

			public TreeNode (UserData d)
			{
				Data = d;
			}
		}

		private class UserDataComp : IComparer
		{
			public int Compare (object x, object y)
			{
				return ((UserData) x).Name.CompareTo(((UserData) y).Name);
			}
		}

		static UserData[] GenerateTestData (int count)
		{
			var ret = new UserData[count];
			for (int i = 0; i < count; i++) {
				ret[i] = new UserData();
				ret[i].Name = System.IO.Path.GetRandomFileName();
			}
			return ret;
        }
        #endregion //ソート関連
    }
}
