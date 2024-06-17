using GameFromScratch.App.Framework;

namespace GameFromScratch.App.Gameplay
{
	internal class TestAnimation
	{
		private readonly IGraphics2D graphics;

		private long animationLastTick = DateTime.MinValue.Ticks;
		private byte animationRed = 0;
		private int animationSign = 1;

		public TestAnimation(IGraphics2D graphics)
		{
			this.graphics = graphics;
		}

		public void Update()
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
				graphics.Fill(animationRed, 0, 0);

				animationLastTick = DateTime.UtcNow.Ticks;

				graphics.Commit();
			}
		}
	}
}
