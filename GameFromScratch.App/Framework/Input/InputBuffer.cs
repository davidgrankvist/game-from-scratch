namespace GameFromScratch.App.Framework.Input
{
    internal class InputBuffer
    {
        private readonly bool[] isKeyDown;
        private readonly bool[] isKeyPrevDown;

        public InputBuffer()
        {
            var numKeyCodes = Enum.GetValues(typeof(KeyCode)).Length;
            isKeyDown = new bool[numKeyCodes];
            isKeyPrevDown = new bool[numKeyCodes];
        }

        // make buffer operations public so that the platform code can access them
        public void SetKeyState(KeyCode key, bool isDown)
        {
            isKeyDown[(int)key] = isDown;
        }

        /*
         * Refresh previous states every frame in order to support IsPressed/IsReleased.
         *
         * Example:
         * If you release a key and then press nothing, then the previous state is "down".
         * Since no further input events are received it needs to be refreshed manually to "up".
         */
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
