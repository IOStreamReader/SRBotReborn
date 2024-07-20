using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRBotReborn
{
	public static class BotManagement
	{
		public static Dictionary<Int64,long> LastMeaasge= new Dictionary<Int64, long>();
		public static long GetMessageInterval(Int64 qid)
		{
			if (!LastMeaasge.ContainsKey(qid))
			{
				return 999999;
			}
			long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			long last=LastMeaasge[qid];
			return now - last;
		}
		public static void UpdateMessageTime(Int64 qid)
		{
			if(!LastMeaasge.ContainsKey(qid))
			{
				LastMeaasge.Add(qid, DateTimeOffset.Now.ToUnixTimeMilliseconds());
			}
			LastMeaasge[qid] = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		}
		public static void Log(string msg, LogLevel level)
		{
			ConsoleColor current = Console.ForegroundColor;
			switch(level)
			{
				case LogLevel.Debug:
					Console.ForegroundColor = ConsoleColor.Gray;
					break;
				case LogLevel.Info:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case LogLevel.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case LogLevel.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
			}
			Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [" + level.ToString() + "] " + msg);
			Console.ForegroundColor = current;
		}
		public enum LogLevel
		{
			Debug,
			Info,
			Warning,
			Error
		}
	}
}
