using GameFromScratch.App.Framework.Maths;

namespace GameFromScratch.App.Framework.Input
{
    /// <summary>
    /// Input buffer write operations to use in the platform code.
    /// </summary>
    internal interface IInputBufferWriter
    {
        public void SetKeyState(KeyCode key, bool isDown);

        public void SetMousePosition(Vector2Int pixelPosition);
    }
}
