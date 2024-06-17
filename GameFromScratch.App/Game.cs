namespace GameFromScratch.App
{
	internal class Game
	{
		private Win32BitmapDrawer bitmapDrawer;

		private long animationLastTick = DateTime.MinValue.Ticks;
		private byte animationRed = 0;
		private int animationSign = 1;

		public Game(Win32BitmapDrawer bitmapDrawer)
		{
			this.bitmapDrawer = bitmapDrawer;
		}

		public void RunFrame()
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
				bitmapDrawer.Fill(animationRed, 0, 0);

				animationLastTick = DateTime.UtcNow.Ticks;
			}
		}
	}
}
