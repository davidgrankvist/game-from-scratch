using GameFromScratch.App.Win32Platform;

namespace GameFromScratch.App
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var bitmapDrawer = new Win32BitmapDrawer();
			var windowManager = new Win32WindowManager(bitmapDrawer);

			var game = new Game(windowManager, bitmapDrawer);
			game.Run();
		}
	}
}
