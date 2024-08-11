using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Graphics;
using GameFromScratch.App.Framework.Input;

namespace GameFromScratch.App.Gameplay.Common
{
    internal class GameTools
    {
        public readonly IGraphics2D Graphics;
        public readonly IInputBuffer Input;
        public readonly Camera2D Camera;

        public GameTools(IGraphics2D graphics, IInputBuffer input, Camera2D camera)
        {
            Graphics = graphics;
            Input = input;
            Camera = camera;
        }
    }
}
