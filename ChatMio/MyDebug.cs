using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ChatMio
{
	class MyDebug
	{
		//public static void WriteLine (string msg, params object[] args)
		//{
		//	string message = String.Format(msg, args);
		//	Debug.WriteLine(message);
		//	WriteLog(message);
		//}

		public static void WriteLine (object obj, string msg, params object[] args)
		{
			string message = String.Format("//{0}// {1}", obj, String.Format(msg, args));
			Debug.WriteLine(message);
			WriteLog(message);
		}

		private static void WriteLog (string message)
		{
			string fileName = String.Format("{0}.log", DateTime.Now.ToString("yyMMdd"));

			using (var w = new StreamWriter(fileName, true)) {
				w.WriteLine("{0}  {1}", DateTime.Now.ToString("hh:mm:ss"), message);
			}
		}
	}
}
