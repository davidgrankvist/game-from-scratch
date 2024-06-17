namespace GameFromScratch.App.Framework
{
    internal interface IWindowManager
    {
        public bool IsRunning { get; }

        public void CreateWindow();

        public void ProcessMessage();
    }
}
