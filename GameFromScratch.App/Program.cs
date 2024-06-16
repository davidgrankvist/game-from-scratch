namespace GameFromScratch.App
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var windowManager = new Win32WindowManager();
			windowManager.CreateWindow();
		}
	}
}
