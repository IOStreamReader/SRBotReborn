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
				return transList;
			}
		}
	}
}
