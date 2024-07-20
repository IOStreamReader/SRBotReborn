using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SRBotReborn
{
	public static class BotEngine
	{
		static Queue<GroupMessage> RecvGroupMsgQueue = new Queue<GroupMessage>();
		static string url = @"http://127.0.0.1";
		public static HttpListener listener = new HttpListener();
		public static void RequestGetMsg()
		{
			listener.Prefixes.Add(url + @":8082/");
			listener.Start();
			while (true)
			{
				try
				{
					HttpListenerContext context = listener.GetContext();
					if (context.Request.HttpMethod == "POST")
					{
						byte[] data = new byte[context.Request.ContentLength64];
						context.Request.InputStream.Read(data, 0, data.Length);
						string rawMessage = Encoding.UTF8.GetString(data);
						string responseString = "200";
						context.Response.ContentType = "text/plain";
						context.Response.ContentEncoding = Encoding.UTF8; byte[] buffer = Encoding.UTF8.GetBytes(responseString);
						context.Response.OutputStream.Write(buffer, 0, buffer.Length);
						context.Response.Close();
						JsonSerializerSettings settings = new JsonSerializerSettings
						{
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						GroupMessage msg = JsonConvert.DeserializeObject<GroupMessage>(rawMessage, settings);
						if (msg != null && msg.message_type == "group")
						{
							RecvGroupMsgQueue.Enqueue(msg);
							BotManagement.Log("Received message from " + msg.sender.user_id + " in group " + msg.group_id + " : " + msg.message, BotManagement.LogLevel.Debug);
						}
					}
					Thread.Sleep(100);
				}
				catch (Exception e)
				{
					BotManagement.Log(e.Message, BotManagement.LogLevel.Error);
					Thread.Sleep(3000);
				}
			}
		}
		public static void ProcessGroupMsg()
		{
			while (true)
			{
				if (RecvGroupMsgQueue.Count > 0)
				{
					GroupMessage tmp = RecvGroupMsgQueue.Dequeue();
					bool Reply = true;
					if(Permission.IsBanned(tmp.sender.user_id)||Permission.SpamCheck(tmp.sender.user_id))
					{
						Reply = false;
					}
					if(Permission.SensitiveCheck(tmp.message))
					{
						Permission.TempBan(tmp.sender.user_id, 60);
						Reply = false;
					}
					if (Reply)
					{
						BotManagement.UpdateMessageTime(tmp.sender.user_id);
						ThreadPool.QueueUserWorkItem(new WaitCallback(BotCommands.ProcessCommand), tmp);
					}
				}
				Thread.Sleep(100);
			}
		}
		public static void SendGroupMsg(Int64 gid, string message)
		{
			SendPOSTRequest("group_id=" + gid.ToString() + "&message=" + message, "send_group_msg");
		}
		public static void DeleteMsg(Int32 mid)
		{
			SendPOSTRequest("message_id=" + mid.ToString(), "delete_msg");
		}
		public static void Mute(Int32 gid,Int64 qid,int duration)
		{
			SendPOSTRequest("group_id=" + gid.ToString() + "&user_id=" + qid.ToString() + "&duration=" + duration.ToString(), "set_group_ban");
		}
		public static void SendPOSTRequest(string content, string type)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + @":8083/"+type);
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				byte[] postBytes = Encoding.UTF8.GetBytes(content);
				request.ContentLength = postBytes.Length;
				Stream requestStream = request.GetRequestStream();
				requestStream.Write(postBytes, 0, postBytes.Length);
				requestStream.Close();
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream responseStream = response.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
				string responseString = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
			}
			catch (Exception e)
			{
				BotManagement.Log(e.Message, BotManagement.LogLevel.Error);
				Thread.Sleep(3000);
			}
		}
		public class Sender
		{
			public string nickname { get; set; }
			public Int64 user_id { get; set; }
			public string card { get; set; }
			public string sex { get; set; }
			public Int32 age { get; set; }
			public string area { get; set; }
			public string level { get; set; }
			public string role { get; set; }
			public string title { get; set; }
		}

		public class GroupMessage
		{
			public Int64 time { get; set; }
			public Int64 self_id { get; set; }
			public string post_type { get; set; }
			public string message_type { get; set; }
			public Int64 group_id { get; set; }
			public Int32 message_id { get; set; }
			public Int32 real_id { get; set; }
			public Sender sender { get; set; }
			public string message { get; set; }
			public string raw_message { get; set; }
			public string font { get; set; }
			public string message_seq { get; set; }
			public string sub_type { get; set; }

		}
	}
}
