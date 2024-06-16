namespace GameFromScratch.App
{
	internal class Game
	{
		private Win32BitmapDrawer bitmapDrawer;

		private long lastTick = DateTime.MinValue.Ticks;
		private const int TARGET_FPS = 60;
		private const int TICKS_PER_SECOND = 10_000_000;
		private const double TARGET_NANOSECONDS_PER_FRAME = TICKS_PER_SECOND / TARGET_FPS;

		private byte animationRed = 0;
		private int animationSign = 1;

		public Game(Win32BitmapDrawer bitmapDrawer)
		{
			this.bitmapDrawer = bitmapDrawer;
		}

		public bool RunFrame()
		{
			// throttle
			var ellapsed = DateTime.UtcNow.Ticks - lastTick;
			if (ellapsed < TARGET_NANOSECONDS_PER_FRAME)
			{
				return false;
			}

			RunTestAnimation(ellapsed);

			lastTick = DateTime.UtcNow.Ticks;
			return true;
		}

		private void RunTestAnimation(long ellapsed)
		{
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
			}

			bitmapDrawer.Fill(animationRed, 0, 0);
		}
	}
}
