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

	public class ConnectedEventArgs : EventArgs			
	{
		public readonly string IpAddr;					//相手のIPアドレス

		public ConnectedEventArgs (string s)
		{
			IpAddr = s;
		}
	}

	public class MsgReceivedEventArgs : EventArgs		
	{
		public readonly string Message;					//受信したメッセージ

		public MsgReceivedEventArgs (string msg)
		{
			Message = msg;
		}
	}

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

	public class PingEventArgs : EventArgs				
	{
		public readonly ChatStatus status;				//接続・未接続

		public PingEventArgs (ChatStatus arg)
		{
			status = arg;
		}
	}
}
