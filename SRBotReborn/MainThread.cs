using SRBotReborn;

namespace SRbot
{
	internal class MainThread
	{
		public static Thread ReadMsg = new Thread(new ThreadStart(BotEngine.RequestGetMsg));
		public static Thread ProcessGroupMsg = new Thread(new ThreadStart(BotEngine.ProcessGroupMsg));
		public static void Main(string[] args)
		{
			Console.WriteLine("SRBot is loading");
			Console.WriteLine("Build Version 241025Z");
			Console.WriteLine("Press ANY key to start the bot.");
			Console.ReadKey();
			Configure.LoadAll();
			Configure.Autosave.Start();
			ReadMsg.Start();
			ProcessGroupMsg.Start();
		}
		~MainThread()
		{
			Configure.SaveAll();
			ReadMsg.Abort();
			ProcessGroupMsg.Abort();
		}
	}
}