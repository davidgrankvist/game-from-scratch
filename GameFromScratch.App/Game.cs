namespace GameFromScratch.App
{
	internal class Game
	{
		private IWindowManager windowManager;
		private IGraphics2D graphics2D;
		private FpsThrottler fpsThrottler;

		private long animationLastTick = DateTime.MinValue.Ticks;
		private byte animationRed = 0;
		private int animationSign = 1;

		public Game(IWindowManager windowManager, IGraphics2D graphics2D)
		{
			this.windowManager = windowManager;
			this.graphics2D = graphics2D;
			fpsThrottler = new FpsThrottler();
		}

		public void Run()
		{
			windowManager.CreateWindow();

			while (windowManager.IsRunning)
			{
				windowManager.ProcessMessage();

				if (!fpsThrottler.PollIsReady())
				{
					continue;
				}

				Update();
			}
		}

		public void Update()
		{
			RunTestAnimation();
		}

		private void RunTestAnimation()
		{
			var ellapsed = DateTime.UtcNow.Ticks - animationLastTick;
			if (ellapsed > 1000)
			{
				if (animationRed == 120)
				{
					animationSign = -1;
				}
				else if (animationRed == 0)
				{
					animationSign = 1;
				}
				animationRed = (byte)(animationRed + animationSign);
				graphics2D.Fill(animationRed, 0, 0);

				animationLastTick = DateTime.UtcNow.Ticks;

				graphics2D.Commit();
			}
		}
	}
}
