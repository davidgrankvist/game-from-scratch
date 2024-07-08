using System.Numerics;

namespace GameFromScratch.App.Framework.Input
{
    internal interface IInputBuffer
    {
        /// <summary>
        /// Refresh previous up/down states. Invoke each frame in order to support IsPressed/IsReleased.
        /// </summary>
        public void Refresh();

        public bool IsDown(KeyCode key);

        public bool IsPressed(KeyCode key);

        public bool IsReleased(KeyCode key);

        public Vector2 MousePosition { get; }
    }
}
