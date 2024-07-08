using GameFromScratch.App.Framework.Fps;
using GameFromScratch.App.Framework.Input;

namespace GameFromScratch.App.Framework
{
    internal interface IWindowManager
    {
        public bool IsRunning { get; }

        public void CreateWindow();

        public void ProcessMessages();

        public IInputBuffer Input { get; }

        public ISleeper Sleeper { get; }
    }
}
