namespace GameFromScratch.App
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var bitmapDrawer = new Win32BitmapDrawer();
			var game = new Game(bitmapDrawer);
			var fpsThrottler = new FpsThrottler();
			var windowManager = new Win32WindowManager(bitmapDrawer);

			windowManager.CreateWindow();

			while (windowManager.IsRunning)
			{
				windowManager.ProcessMessage();

				if (!fpsThrottler.PollIsReady())
				{
					continue;
				}

				// process game things
				game.RunFrame();

				// render
				bitmapDrawer.Draw();
			}
		}
	}
}
