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
			if (msg.message.Contains("贾") || msg.message.StartsWith("jrz"))
			{
				jrz(msg.group_id, msg.sender);
			}
			else if (msg.message == "tyt")
			{
				tyt(msg.group_id);
			}
			else if (msg.message.StartsWith("livedebug") && (msg.sender.user_id == 2484713081||Permission.GetPermission(msg.sender.user_id)==Permission.PermissionLevel.Owner))
			{
				livedebug(msg);
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
			BotManagement.Log(sender.user_id.ToString()+"贾了一下", BotManagement.LogLevel.Info);
		}
		public static void tyt(long gid)
		{
			BotEngine.SendGroupMsg(gid, "[CQ:face,id=218]");
			BotManagement.Log("tyt了一下", BotManagement.LogLevel.Info);
		}
		public static void DeleteMsg(BotEngine.GroupMessage msg)
		{
			BotEngine.DeleteMsg(msg.message_id);
			BotManagement.Log("已删除消息" + msg.message_id, BotManagement.LogLevel.Info);
		}
		public static void livedebug(BotEngine.GroupMessage msg)
		{
			string cmdargs = msg.message.Substring(10);
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
			else if (msg.message.StartsWith("permget"))
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
			else if (msg.message == "saveall")
			{
				Configure.SaveSensitive();
				Configure.SavePermissionList();
				Configure.SaveBanList();
				BotEngine.SendGroupMsg(msg.group_id, "全部配置已保存");
				BotManagement.Log("全部配置已保存", BotManagement.LogLevel.Info);
			}
			else if (msg.message.StartsWith("getraw"))
			{
				cmdargs = cmdargs.Substring(6);
				BotEngine.SendGroupMsg(msg.group_id, cmdargs);
			}
			else if (msg.message.StartsWith("ban"))
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
			else if (msg.message.StartsWith("tempban"))
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
			else if (msg.message.StartsWith("sensitive"))
			{
				cmdargs = cmdargs.Substring(10);
				Permission.SensitiveList.Add(cmdargs);
				BotEngine.SendGroupMsg(msg.group_id, "已经添加敏感词" + cmdargs);
				BotManagement.Log("已经添加敏感词" + cmdargs, BotManagement.LogLevel.Info);
			}
			else if (msg.message.StartsWith("unsensitive"))
			{
				cmdargs = cmdargs.Substring(12);
				Permission.SensitiveList.Remove(cmdargs);
				BotEngine.SendGroupMsg(msg.group_id, "已经移除敏感词" + cmdargs);
				BotManagement.Log("已经移除敏感词" + cmdargs, BotManagement.LogLevel.Info);
			}
			else if (msg.message.StartsWith("recall"))
			{
				cmdargs = cmdargs.Substring(7);
				Permission.RecallList.Add(cmdargs);
				BotEngine.SendGroupMsg(msg.group_id, "已经添加撤回词" + cmdargs);
				BotManagement.Log("已经添加撤回词" + cmdargs, BotManagement.LogLevel.Info);
			}
			else if (msg.message.StartsWith("unrecall"))
			{
				cmdargs = cmdargs.Substring(9);
				Permission.RecallList.Remove(cmdargs);
				BotEngine.SendGroupMsg(msg.group_id, "已经移除撤回词" + cmdargs);
				BotManagement.Log("已经移除撤回词" + cmdargs, BotManagement.LogLevel.Info);
			}
		}
	}
}
