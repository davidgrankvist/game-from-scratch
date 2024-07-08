using GameFromScratch.App.Framework.Maths;
using System.Numerics;

namespace GameFromScratch.App.Framework.Input
{
    internal class InputBuffer : IInputBuffer, IInputBufferWriter
    {
        private readonly bool[] isKeyDown;
        private readonly bool[] isKeyPrevDown;

        private Vector2 mousePosition;
        public Vector2 MousePosition { get => mousePosition; }

        private readonly Camera2D camera;

        public InputBuffer(Camera2D camera)
        {
            var numKeyCodes = Enum.GetValues(typeof(KeyCode)).Length;
            isKeyDown = new bool[numKeyCodes];
            isKeyPrevDown = new bool[numKeyCodes];

            mousePosition = Vector2.Zero;
            this.camera = camera;
        }

        public void SetKeyState(KeyCode key, bool isDown)
        {
            isKeyDown[(int)key] = isDown;
        }

        public void SetMousePosition(Vector2Int pixelPosition)
        {
            mousePosition = camera.FromPixel(pixelPosition);
        }

        public void Refresh()
        {
            Array.Copy(isKeyDown, isKeyPrevDown, isKeyDown.Length);
        }

        public bool IsDown(KeyCode key)
        {
            return isKeyDown[(int)key];
        }

        public bool IsPressed(KeyCode key)
        {
            return isKeyDown[(int)key] && !isKeyPrevDown[(int)key];
        }

        public bool IsReleased(KeyCode key)
        {
            return !isKeyDown[(int)key] && isKeyPrevDown[(int)key];
        }
    }
}
