using SRBotReborn;

namespace SRbot
{
	internal class MainThread
	{
		public static Thread ReadMsg = new Thread(new ThreadStart(BotEngine.RequestGetMsg));
		public static Thread ProcessGroupMsg = new Thread(new ThreadStart(BotEngine.ProcessGroupMsg));
		public static void Main(string[] args)
		{
			Configure.LoadAll();
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