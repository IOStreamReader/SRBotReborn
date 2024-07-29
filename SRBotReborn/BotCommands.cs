using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRBotReborn
{
	public static class BotCommands
	{
		public static void ProcessCommand(object rawmsg)
		{
			BotEngine.GroupMessage msg = (BotEngine.GroupMessage)rawmsg;
			if (false)
			{
			}
			else if (msg.message.StartsWith("/func") && Permission.GetPermission(msg.sender.user_id) >= Permission.PermissionLevel.Admin)
			{
				string args = msg.message.Substring(6);
				string func = args.Substring(0, args.IndexOf(' '));
				string status = args.Substring(args.IndexOf(' ') + 1);
				if (status == "on")
				{
					Permission.SwitchFuctions(msg.group_id, func, true);
					BotEngine.SendGroupMsg(msg.group_id, "已在本群开启" + func);
					BotManagement.Log("已在群" + msg.group_id.ToString() + "开启" + func, BotManagement.LogLevel.Info);
				}
				else if (status == "off")
				{
					Permission.SwitchFuctions(msg.group_id, func, false);
					BotEngine.SendGroupMsg(msg.group_id, "已在本群关闭" + func);
					BotManagement.Log("已在群" + msg.group_id.ToString() + "关闭" + func, BotManagement.LogLevel.Info);
				}
				else
				{
					BotEngine.SendGroupMsg(msg.group_id, "参数错误");
				}
			}
			else if (msg.message.Contains("贾") || msg.message.Contains("jrz"))
			{
				if (Permission.GetFunctions(msg.group_id).jrz)
					jrz(msg.group_id, msg.sender);
			}
			else if (msg.message == "tyt")
			{
				tyt(msg.group_id);
			}
			else if (msg.message.StartsWith(".livedebug") && (msg.sender.user_id == 2484713081 || Permission.GetPermission(msg.sender.user_id) == Permission.PermissionLevel.Owner))
			{
				livedebug(msg);
			}
			else if (msg.message.StartsWith(".r"))
			{
				string args = msg.message.Substring(2);
				roll(args, msg.group_id);
			}
			else if (msg.message.StartsWith("/nbnhhsh"))
			{
				string abbrev = msg.message.Substring(9);
				nbnhhsh(abbrev, msg.group_id);
			}
			else if (msg.message.StartsWith("/5kcy"))
			{
				string args = msg.message.Substring(6);
				string text1 = args.Substring(0, args.IndexOf(' '));
				string text2 = args.Substring(args.IndexOf(' ') + 1);
				fivekcy(text1, text2, msg.group_id);
			}
			else if (msg.message.StartsWith("/ulk"))
			{
				string args= msg.message.Substring(5);
				string reigon, dc;
				reigon = "all";
				if (args.Split(" ").Length == 1)
					dc = args.Trim();
				else
				{
					reigon=args.Split(" ")[1];
					dc = args.Split(" ")[0];
				}
				SendEurekaStatus(msg.group_id, dc, reigon);
			}
			else if (msg.message.StartsWith("/ffrand"))
			{
				BotManagement.Log("Generating ffrand", BotManagement.LogLevel.Info);
				string[] args= msg.message.Substring(8).Split(' ');
				ffrand(msg.group_id, args);
			}
		}
		public static void jrz(long gid,BotEngine.Sender sender)
		{
			string nickname=sender.card==""?sender.nickname:sender.card;
			Random random = new Random();
			long jrz = random.Next() % 1919810;
			BotEngine.SendGroupMsg(gid, nickname+"贾了一下！");
			Thread.Sleep(1000);
			BotEngine.SendGroupMsg(gid, "[CQ:image,file=https://dev.zapic.moe/RandomPic/yys/?seed=" + jrz.ToString() + "]");
			BotManagement.Log("Image sent https://dev.zapic.moe/RandomPic/yys/?seed=" + jrz.ToString(), BotManagement.LogLevel.Info);
		}
		public static void tyt(long gid)
		{
			BotEngine.SendGroupMsg(gid, "[CQ:face,id=218]");
			BotManagement.Log("tyt了一下", BotManagement.LogLevel.Info);
		}
		public static void roll(string args, Int64 gid)
		{
			Random random = new Random((int)DateTimeOffset.Now.ToUnixTimeMilliseconds());
			try
			{
				int posd = args.IndexOf('d');
				int count = 1;
				if (posd != 0)
					count = int.Parse(args.Substring(0, posd));
				if (count < 10)
				{
					int[] result = new int[count];
					string diceparam = args.Substring(args.IndexOf('d') + 1);
					if (diceparam == "F" || diceparam == "f")
					{
						int[] dice = { -1, -1, 0, 0, 1, 1 };
						for (int i = 0; i < count; i++)
						{
							result[i] = dice[random.Next() % 5];
						}
					}
					else if (diceparam == "F.1" || diceparam == "f.1")
					{
						int[] dice = { -1, 0, 0, 0, 0, 1 };
						for (int i = 0; i < count; i++)
						{
							result[i] = dice[random.Next() % 5];
						}
					}
					else
					{
						int max = int.Parse(diceparam);
						for (int i = 0; i < count; i++)
						{
							result[i] = random.Next() % max + 1;
						}
					}
					BotEngine.SendGroupMsg(gid, "投掷结果为" + string.Join(" ", result));
				}
				else
				{
					BotEngine.SendGroupMsg(gid, "投掷次数过多");
					BotManagement.Log("投掷次数过多", BotManagement.LogLevel.Warning);
				}
			}
			catch
			{
				BotManagement.Log("投掷参数错误", BotManagement.LogLevel.Warning);
			}
		}
		public static void nbnhhsh(string text,Int64 gid)
		{
			List<string> result = ExternalAPI.GethhshList(text);
			if (result.Count == 0)
			{
				BotEngine.SendGroupMsg(gid, "未找到对应缩写");
			}
			else
			{
				BotEngine.SendGroupMsg(gid, "可能的全称有：" + string.Join(" ", result));
			}
			BotManagement.Log("nbnhhsh " + text, BotManagement.LogLevel.Info);
		}
		public static void fivekcy(string text1,string text2, Int64 gid)
		{
			string url= "https://gsapi.cbrx.io/image?top="+text1+"&bottom="+text2+ "&type=png&noalpha=true";
			string filename="5kcy_"+text1+"_"+text2+".png";
			ExternalAPI.WritePicCache(url,filename);
			Thread.Sleep(500);
			BotEngine.SendGroupMsg(gid, "[CQ:image,file="+ExternalAPI.GetPicCache(filename)+"]");
			BotManagement.Log("Image sent [CQ:image,file=" + ExternalAPI.GetPicCache(filename) + "]", BotManagement.LogLevel.Info);
			Thread.Sleep(500);
			File.Delete(ExternalAPI.GetPicCache(filename));
		}
		public static void DeleteMsg(BotEngine.GroupMessage msg)
		{
			BotEngine.DeleteMsg(msg.message_id);
			BotManagement.Log("已删除消息" + msg.message_id, BotManagement.LogLevel.Info);
		}
		public static bool CheckRecall(BotEngine.GroupMessage msg)
		{
			string str = msg.message;
			string trimmed = str.ToLower().Trim();
			foreach (string recall in Permission.RecallList)
			{
				if (trimmed.Contains(recall))
				{
					return true;
				}
			}
			return false;
		}
		public static void SendEurekaStatus(Int64 gid,string rawdc,string reigon)
		{
			string dc;
			switch (rawdc)
			{
				case "鸟":
					dc = "1";
					break;
				case "猪":
					dc = "2";
					break;
				case "猫":
					dc = "3";
					break;
				case "狗":
					dc = "4";
					break;
				default:
					BotEngine.SendGroupMsg(gid, "参数错误，正确使用方法：/ulk 大区[鸟/猪/猫/狗] 可选:地图[风/冰/火/水] ");
					BotManagement.Log("大区参数错误:"+rawdc, BotManagement.LogLevel.Warning);
					return;
			}
			List<ExternalAPI.EurekaData> ulkstatus=ExternalAPI.ProcessEurekaData(ExternalAPI.GetEurekaList(dc));
			string message = rawdc+"区"+reigon+"岛nm触发情况如下：\n";
			string filter;
			switch (reigon)
			{
				case "风":
					filter = "常风之地";
					break;
				case "冰":
					filter = "恒冰之地";
					break;
				case "火":
					filter = "涌火之地";
					break;
				case "水":
					filter = "丰水之地";
					break;
				case "all":
					filter = "all";
					break;
				default:
					BotEngine.SendGroupMsg(gid, "参数错误，正确使用方法：/ulk 大区[鸟/猪/猫/狗] 可选:地图[风/冰/火/水] ");
					BotManagement.Log("地图参数错误:"+reigon, BotManagement.LogLevel.Warning);
					return;
			}
			foreach(ExternalAPI.EurekaData data in ulkstatus)
			{
				if(data.Reigon==filter||filter=="all")
				{
					int TimeLength= data.Time.Substring(data.Time.IndexOf("（")+1).IndexOf("）");
					message += data.Time.Substring(data.Time.IndexOf("（")+1,TimeLength)+":";
					message += data.Name;
					if (data.HP != "0%")
						message += "  剩余血量 " + data.HP;
					message += '\n';
				}
			}
			BotEngine.SendGroupMsg(gid, message);
			BotManagement.Log("Sent Eureka status", BotManagement.LogLevel.Info);
		}
		public static void ffrand(Int64 gid, string[] args)
		{
			try
			{
				Random random = new Random((int)DateTimeOffset.Now.ToUnixTimeMilliseconds());
				int jobs = 19;
				foreach(string arg in args)
				{
					if (arg.StartsWith("-ver"))
					{
						switch (arg.Substring(4))
						{
							case "2":
								jobs = 10;
								break;
							case "3":
								jobs = 13;
								break;
							case "4":
								jobs = 15;
								break;
							case "5":
								jobs = 17;
								break;
							case "6":
								jobs = 19;
								break;
							case "7":
								jobs = 21;
								break;
						}
					}
				}
				BotManagement.Log("ffrand with "+jobs.ToString()+" jobs", BotManagement.LogLevel.Info);
                switch (args[0])
				{
					case "job":
						int job = random.Next() % jobs;
						BotManagement.Log("rand="+job.ToString(), BotManagement.LogLevel.Info);
						string name=ExternalAPI.JobName[job];
						string role=ExternalAPI.RoleName[ExternalAPI.JobsRole[job]];
						BotEngine.SendGroupMsg(gid,"按职业随机的结果为："+name+"("+role+")");
						BotManagement.Log("ffrand job in"+gid.ToString()+" "+name, BotManagement.LogLevel.Info);
						break;
					case "role":
						int roleid = random.Next() % 5;
						string rolename = ExternalAPI.RoleName[roleid];
						List<string> joblist = new List<string>();
						for (int i = 0; i < jobs; i++)
						{
							if (ExternalAPI.JobsRole[i] == roleid)
								joblist.Add(ExternalAPI.JobName[i]);
						}
						int jobid = random.Next() % joblist.Count;
						string jobresult = joblist[jobid];
						BotEngine.SendGroupMsg(gid, "按职能随机的结果为：" + rolename + " 中的 " + jobresult);
						BotManagement.Log("ffrand role" + gid.ToString() + " " + jobresult, BotManagement.LogLevel.Info);
						break;
					case "112":
						int r=random.Next() % 5;
						List<int> candidate=new List<int>();
						for(int i = 0; i < jobs; i++)
						{
							if (r == 0)
								if (ExternalAPI.JobsRole[i]==0)
									candidate.Add(i);
							else if(r==1)
								if(ExternalAPI.JobsRole[i]==1)
									candidate.Add(i);
							else
								candidate.Add(i);
						}
						BotManagement.Log(candidate.Count.ToString()+"candidates", BotManagement.LogLevel.Info);
						int j=random.Next()%candidate.Count;
						BotEngine.SendGroupMsg(gid,"按1:1:2随机到的职业为："+ExternalAPI.JobName[candidate[j]]);
						BotManagement.Log("ffrand 112", BotManagement.LogLevel.Info);
						break;
					case "125":
						int r2 = random.Next() % 8;
						List<int> candidate2 = new List<int>();
						for (int i = 0; i < jobs; i++)
						{
							if (r2 == 0)
								if (ExternalAPI.JobsRole[i] == 0)
									candidate2.Add(i);
								else if (r2 == 1)
									if (ExternalAPI.JobsRole[i] == 1 || ExternalAPI.JobsRole[i]==2)
										candidate2.Add(i);
									else
										candidate2.Add(i);
						}
						int j2 = random.Next() % candidate2.Count;
						BotEngine.SendGroupMsg(gid, "按1:2:5随机到的职业为：" + ExternalAPI.JobName[candidate2[j2]]);
						BotManagement.Log("ffrand 125", BotManagement.LogLevel.Info);
						break;
					default:
						BotEngine.SendGroupMsg(gid, "参数错误");
						break;
				}
			}
			catch(Exception e) {
				BotEngine.SendGroupMsg(gid, "参数错误");
				BotManagement.Log(e.StackTrace, BotManagement.LogLevel.Warning);
				return;
			}
		}
		public static void livedebug(BotEngine.GroupMessage msg)
		{
			string cmdargs = msg.message.Substring(11);
			if (cmdargs.StartsWith("permset"))
			{
				cmdargs = cmdargs.Substring(8);
				string[] args = cmdargs.Split(' ');
				try
				{
					Permission.PermissionLevel level = (Permission.PermissionLevel)Enum.Parse(typeof(Permission.PermissionLevel), args[1]);
					Permission.UpdatePermission(Int64.Parse(args[0]), level);
					BotEngine.SendGroupMsg(msg.group_id, "已经将" + args[0] + "的权限设置为" + args[1]);
					BotManagement.Log("已经将" + args[0] + "的权限设置为" + args[1], BotManagement.LogLevel.Info);
				}
				catch
				{
					BotEngine.SendGroupMsg(msg.group_id, "参数错误");
					BotManagement.Log("权限参数" + cmdargs + "错误", BotManagement.LogLevel.Warning);
				}
			}
			else if (cmdargs.StartsWith("permget"))
			{
				cmdargs = cmdargs.Substring(8);
				try
				{
					BotEngine.SendGroupMsg(msg.group_id, "用户" + cmdargs + "的权限为" + Permission.PermissionList[Int64.Parse(cmdargs)]);
					BotManagement.Log("获取权限" + cmdargs + "的权限为" + Permission.PermissionList[Int64.Parse(cmdargs)], BotManagement.LogLevel.Info);
				}
				catch
				{
					BotEngine.SendGroupMsg(msg.group_id, "参数错误");
					BotManagement.Log("获取权限参数"+cmdargs+"错误", BotManagement.LogLevel.Warning);
				}
			}
			else if (cmdargs == "saveall")
			{
				Configure.SaveAll();
				BotEngine.SendGroupMsg(msg.group_id, "全部配置已保存");
				BotManagement.Log("全部配置已保存", BotManagement.LogLevel.Info);
			}
			else if (cmdargs.StartsWith("getraw"))
			{
				cmdargs = cmdargs.Substring(6);
				BotEngine.SendGroupMsg(msg.group_id, cmdargs);
			}
			else if (cmdargs.StartsWith("ban"))
			{
				cmdargs = cmdargs.Substring(4);
				try
				{
					Permission.PermBan(Int64.Parse(cmdargs));
					BotEngine.SendGroupMsg(msg.group_id, "已经永久封禁" + cmdargs);
					BotManagement.Log("已经永久封禁" + cmdargs, BotManagement.LogLevel.Info);
				}
				catch
				{
					BotEngine.SendGroupMsg(msg.group_id, "参数错误");
					BotManagement.Log("封禁参数"+cmdargs+"错误", BotManagement.LogLevel.Warning);
				}
			}
			else if (cmdargs.StartsWith("tempban"))
			{
				cmdargs = cmdargs.Substring(8);
				string[] args = cmdargs.Split(' ');
				try
				{
					Permission.TempBan(Int64.Parse(args[0]), Int64.Parse(args[1]));
					BotEngine.SendGroupMsg(msg.group_id, "已经封禁" + args[0] + " " + args[1] + "秒");
					BotManagement.Log("已经封禁" + args[0] + " " + args[1] + "秒", BotManagement.LogLevel.Info);
				}
				catch
				{
					BotEngine.SendGroupMsg(msg.group_id, "参数错误");
					BotManagement.Log("封禁参数"+args+"错误", BotManagement.LogLevel.Warning);
				}
			}
			else if (cmdargs.StartsWith("sensitive"))
			{
				cmdargs = cmdargs.Substring(10);
				Permission.SensitiveList.Add(cmdargs);
				BotEngine.SendGroupMsg(msg.group_id, "已经添加敏感词" + cmdargs);
				BotManagement.Log("已经添加敏感词" + cmdargs, BotManagement.LogLevel.Info);
			}
			else if (cmdargs.StartsWith("unsensitive"))
			{
				cmdargs = cmdargs.Substring(12);
				Permission.SensitiveList.Remove(cmdargs);
				BotEngine.SendGroupMsg(msg.group_id, "已经移除敏感词" + cmdargs);
				BotManagement.Log("已经移除敏感词" + cmdargs, BotManagement.LogLevel.Info);
			}
			else if (cmdargs.StartsWith("recall"))
			{
				cmdargs = cmdargs.Substring(7);
				Permission.RecallList.Add(cmdargs);
				BotEngine.SendGroupMsg(msg.group_id, "已经添加撤回词" + cmdargs);
				BotManagement.Log("已经添加撤回词" + cmdargs, BotManagement.LogLevel.Info);
			}
			else if (cmdargs.StartsWith("unrecall"))
			{
				cmdargs = cmdargs.Substring(9);
				Permission.RecallList.Remove(cmdargs);
				BotEngine.SendGroupMsg(msg.group_id, "已经移除撤回词" + cmdargs);
				BotManagement.Log("已经移除撤回词" + cmdargs, BotManagement.LogLevel.Info);
			}
		}
	}
}
