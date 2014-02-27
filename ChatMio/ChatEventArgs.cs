using System;

namespace ChatMio
{
	/// <summary>
	/// 接続・未接続を示すEnum
	/// </summary>
	public enum ChatStatus
	{
		Connected,
		DisConnected
	}

	/// <summary>
	/// 接続相手のIPアドレスを格納するEventArgs
	/// </summary>
	public class ConnectedEventArgs : EventArgs			
	{
		public readonly string IpAddr;					//相手のIPアドレス

		public ConnectedEventArgs (string s)
		{
			IpAddr = s;
		}
	}

	/// <summary>
	/// 受信したメッセージを格納するEventArgs
	/// </summary>
	public class MsgReceivedEventArgs : EventArgs		
	{
		public readonly string Message;					//受信したメッセージ

		public MsgReceivedEventArgs (string msg)
		{
			Message = msg;
		}
	}

	/// <summary>
	/// 相手のUserDataを格納するEventArgs
	/// </summary>
	public class UserDataReceivedEventArgs : EventArgs
	{
		public readonly UserData Data;					//相手のUserData

		public UserDataReceivedEventArgs (UserData d)
		{
			Data = new UserData();
			Data.Name = d.Name;
			Data.IsFrom = d.IsFrom;
			Data.TextColor = d.TextColor;
			Data.FontSize = d.FontSize;
		}
	}

	/// <summary>
	/// 接続・未接続を示すEnumを格納するEventArgs
	/// </summary>
	public class PingEventArgs : EventArgs				
	{
		public readonly ChatStatus status;				//接続・未接続

		public PingEventArgs (ChatStatus arg)
		{
			status = arg;
		}
	}
}
