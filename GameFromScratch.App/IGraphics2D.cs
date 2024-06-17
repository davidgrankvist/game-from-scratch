namespace GameFromScratch.App
{
	internal interface IGraphics2D
	{
		public void Fill(byte r, byte g, byte b);

		public void Resize(int width, int height);

		public void Commit();
	}
}
