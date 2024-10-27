using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SRBotReborn
{
	public static class Configure
	{
		public static Thread Autosave= new Thread(new ThreadStart(Tick10m));
		public static void Tick10m()
		{
			SaveAll();
			Thread.Sleep(600000);
		}
		public static void LoadAll()
		{
			LoadSensitive();
			LoadBanList();
			LoadPermissionList();
			LoadRecallList();
			LoadGroupFunctionsList();
		}
		public static void SaveAll()
		{
			SaveSensitive();
			SaveBanList();
			SavePermissionList();
			SaveRecallList();
			SaveGroupFunctionsList();
		}
		public static void LoadSensitive()
		{
			if (!File.Exists("Sensitive.txt"))
				return;
			StreamReader sr = new StreamReader("Sensitive.txt");
			while (!sr.EndOfStream)
			{
				Permission.SensitiveList.Add(sr.ReadLine());
			}
			sr.Close();
		}
		public static void LoadBanList()
		{
			if (!File.Exists("BanList.txt"))
				return;
			StreamReader sr = new StreamReader("BanList.txt");
			while (!sr.EndOfStream)
			{
				string[] tmp = sr.ReadLine().Split(' ');
				Permission.BanList.Add(Int64.Parse(tmp[0]), Int64.Parse(tmp[1]));
			}
			sr.Close();
		}
		public static void LoadPermissionList()
		{
			if (!File.Exists("PermissionList.txt"))
				return;
			StreamReader sr = new StreamReader("PermissionList.txt");
			while (!sr.EndOfStream)
			{
				string tmp = sr.ReadLine();
				if (tmp.Contains(":") && tmp.Contains(","))
				{
					Int64 qid = Int64.Parse(tmp.Substring(tmp.IndexOf(":")));
					string[] perms=tmp.Substring(tmp.IndexOf(":")).Split(";");
					Permission.PermissionList.Add(qid, new Dictionary<long, Permission.PermissionLevel>());
					foreach (string perm in perms)
					{
						Permission.PermissionList[qid].Add(Int64.Parse(perm.Split(',')[0]), (Permission.PermissionLevel)Int64.Parse(perm.Split(',')[1]));
					}
				}
			}
			sr.Close();
		}
		public static void LoadRecallList()
		{
			if (!File.Exists("RecallList.txt"))
				return;
			StreamReader sr = new StreamReader("RecallList.txt");
			while (!sr.EndOfStream)
			{
				Permission.RecallList.Add(sr.ReadLine());
			}
			sr.Close();
		}
		public static void LoadGroupFunctionsList()
		{
			if (!File.Exists("GroupFunctionsList.txt"))
				return;
			StreamReader sr = new StreamReader("GroupFunctionsList.txt");
			while (!sr.EndOfStream)
			{
				string line= sr.ReadLine();
				string[] tmp = line.Split(':');
				Permission.GroupFunctionsList.Add(Int64.Parse(tmp[0]), Permission.GroupFunctions.Parse(tmp[1]));
			}
			sr.Close();
		}
		public static void SavePermissionList()
		{
			StreamWriter sw = new StreamWriter("PermissionList.txt");
			foreach (KeyValuePair<Int64, Dictionary<Int64,Permission.PermissionLevel>> kv in Permission.PermissionList)
			{
				sw.Write(kv.Key+":");
				foreach(KeyValuePair<Int64,Permission.PermissionLevel> qgp in kv.Value)
				{
					sw.Write(qgp.Key+","+qgp.Value.ToString()+";");
				}
			}
			sw.Close();
		}
		public static void SaveSensitive()
		{
			StreamWriter sw = new StreamWriter("Sensitive.txt");
			foreach (string s in Permission.SensitiveList)
			{
				sw.WriteLine(s);
			}
			sw.Close();
		}
		public static void SaveBanList()
		{
			StreamWriter sw = new StreamWriter("BanList.txt");
			foreach (KeyValuePair<Int64, Int64> kv in Permission.BanList)
			{
				sw.WriteLine(kv.Key + " " + kv.Value);
			}
			sw.Close();
		}
		public static void SaveRecallList()
		{
			StreamWriter sw = new StreamWriter("RecallList.txt");
			foreach (string s in Permission.RecallList)
			{
				sw.WriteLine(s);
			}
			sw.Close();
		}
		public static void SaveGroupFunctionsList()
		{
			StreamWriter sw = new StreamWriter("GroupFunctionsList.txt");
			foreach (KeyValuePair<Int64, Permission.GroupFunctions> kv in Permission.GroupFunctionsList)
			{
				sw.WriteLine(kv.Key + ":" + kv.Value.ToString());
			}
			sw.Close();
		}
	}
}
