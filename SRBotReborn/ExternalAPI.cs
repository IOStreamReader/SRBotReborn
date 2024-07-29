using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json.Linq;

namespace SRBotReborn
{
	public static class ExternalAPI
	{
		private static string hhshAPI = "https://lab.magiconch.com/api/nbnhhsh/";

		public static List<string> GethhshList(string text)
		{
			using (var client = new HttpClient())
			{
				var data = new { text = text };
				var jsonData = JsonConvert.SerializeObject(data);
				var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

				var response = client.PostAsync(hhshAPI + "guess", content).Result;
				var result = response.Content.ReadAsStringAsync().Result;
				var jsonArray = JsonConvert.DeserializeObject(result) as JArray;
				var transList = new List<string>();
				if (jsonArray != null)
				{
					foreach (var item in jsonArray)
					{
						var transArray = item["trans"] as JArray;
						if (transArray != null)
						{
							foreach (var trans in transArray)
							{
								transList.Add(trans.ToString());
							}
						}
					}
				}
				return transList;
			}
		}

		public static byte[] DownloadImage(string url)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync(url).Result;
				var result = response.Content.ReadAsByteArrayAsync().Result;
				return result;
			}
		}

		public static void WritePicCache(string url, string filename)
		{
			if (!Directory.Exists("cache"))
			{
				Directory.CreateDirectory("cache");
			}
			var imageBytes = DownloadImage(url);
			File.WriteAllBytes("/root/cache/" + filename, imageBytes);
		}

		public static string GetPicCache(string filename)
		{
			return "/root/cache/" + filename;
		}

		public static List<string> GetEurekaList(string datacenter = "1")
		{
			string EurekaAPI = "https://eureka-tracker.tunnel.tidebyte.com:8129/?dc=" + datacenter;
			using (var client = new HttpClient())
			{
				var response = client.GetAsync(EurekaAPI).Result;
				var result = response.Content.ReadAsStringAsync().Result;
				// Parse the HTML content
				var htmlDocument = new HtmlAgilityPack.HtmlDocument();
				htmlDocument.LoadHtml(result);

				// Find the table element
				var table = htmlDocument.DocumentNode.SelectSingleNode("//table");
				var rows = table.SelectNodes("//tr");
				var eurekaList = new List<string>();
				// Iterate through each row and extract the cell values
				foreach (var row in rows)
				{
					var cells = row.SelectNodes("td");
					if (cells != null)
					{
						foreach (var cell in cells)
						{
							eurekaList.Add(cell.InnerText);
						}
					}
				}
				return eurekaList;
			}
		}
		public class EurekaData
		{
			public string Reigon { get; set; }
			public string Name { get; set; }
			public string HP { get; set; }
			public string Time { get; set; }
		}

		public static List<EurekaData> ProcessEurekaData(List<string> raw)
		{
			List<EurekaData> data= new List<EurekaData>();
			int count = raw.Count / 6;
			for (int i= 0; i < count; i++)
			{
				EurekaData current= new EurekaData();
				current.Reigon= raw[i * 6];
				current.Name= raw[i * 6 + 1];
				current.Time = raw[i * 6 + 2];
				current.HP= raw[i * 6 + 2];
				if(current.Name.Contains("兔"))
					continue;
				data.Add(current);
			}
			return data;
		}
		public enum FFjobs
		{
			PLD,
			WAR,
			WHM,
			SCH,
			MNK,
			DRG,
			NIN,
			BRD,
			SMN,
			BLM,
			DRK,
			AST,
			MCH,
			SAM,
			RDM,
			GNB,
			DNC,
			SGE,
			RPR,
			PCT,
			VRP
		};
		public static int[] JobsRole =
		{
			0,0,1,1,2,2,2,3,4,4,0,1,3,2,4,0,3,1,2,4,2
		};
		public static string[] RoleName = {"防护职业","治疗职业","近战职业","远程物理职业","远程魔法职业"};
		public static string[] JobName =
		{
			"骑士","战士","白魔法师","学者","武僧","龙骑士","忍者","吟游诗人","召唤师","黑魔法师","暗黑骑士","占星术士","机工士","武士","赤魔法师","绝枪战士","舞者","贤者","钐镰师","绘灵法师","蝰蛇剑士"
		};
	}
}
