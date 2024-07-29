using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRBotReborn
{
	internal class Permission
	{
		public static List<string> SensitiveList= new List<string>();
		public static Dictionary<Int64,Int64> BanList = new Dictionary<Int64, Int64>();
		public static List<string> RecallList= new List<string>();
		public static Dictionary<Int64, PermissionLevel> PermissionList = new Dictionary<Int64, PermissionLevel>();
		public static Dictionary<Int64,GroupFunctions> GroupFunctionsList = new Dictionary<Int64, GroupFunctions>();
		public static void TempBan(Int64 qid, Int64 time)
		{
			BanList.Add(qid,DateTimeOffset.Now.ToUnixTimeMilliseconds() + time*1000);
		}
		public static void PermBan(Int64 qid)
		{
			BanList[qid] = -1;
		}
		public static bool IsBanned(Int64 qid) {
			if (BanList.ContainsKey(qid))
			{
				if (BanList[qid] > DateTimeOffset.Now.ToUnixTimeMilliseconds() || BanList[qid] == -1)
				{
					return true;
				}
				else
				{
					BanList.Remove(qid);
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		public static void SwitchFuctions(Int64 qid, string func, bool status)
		{
			if (GroupFunctionsList.ContainsKey(qid))
			{
				if (func == "jrz")
				{
					GroupFunctionsList[qid].jrz = status;
				}
			}
			else
			{
				GroupFunctionsList.Add(qid, new GroupFunctions());
				if (func == "jrz")
				{
					GroupFunctionsList[qid].jrz = status;
				}
			}
		}
		public static GroupFunctions GetFunctions(Int64 qid)
		{
			if (GroupFunctionsList.ContainsKey(qid))
			{
				return GroupFunctionsList[qid];
			}
			else
			{
				return new GroupFunctions();
			}
		}
		public static bool SpamCheck(Int64 qid)
		{
			if (BotManagement.GetMessageInterval(qid) < 3000)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool SensitiveCheck(string msg)
		{
			return SensitiveList.Any(msg.Contains);
		}
		public static void UpdatePermission(Int64 qid, PermissionLevel level)
		{
			if (PermissionList.ContainsKey(qid))
			{
				PermissionList[qid] = level;
			}
			else
			{
				PermissionList.Add(qid, level);
			}
		}
		public static PermissionLevel GetPermission(Int64 qid)
		{
			if (PermissionList.ContainsKey(qid))
			{
				return PermissionList[qid];
			}
			else
			{
				return PermissionLevel.Normal;
			}
		}
		public class GroupFunctions {
			public GroupFunctions() 
			{
				jrz = false;
			}
			private static string[] func =
			{
				"jrz"
			};
			public override string ToString()
			{
				string str=func[0] + "=" + jrz;
				return str;
			}
			public static GroupFunctions Parse(string str)
			{
				string[] s = str.Split('0');
				GroupFunctions gf = new GroupFunctions();
				for(int i=0; i < func.Length; i++)
				{
					switch(i)
					{
						case 0:
							gf.jrz = bool.Parse(s[i].Split('=')[1]);
							break;
					}
				}
				return gf;
			}
			public bool jrz { get; set; }
		}
		public enum PermissionLevel
		{
			Blacklist,
			Normal,
			Trusted,
			Admin,
			Owner
		}
	}
}
