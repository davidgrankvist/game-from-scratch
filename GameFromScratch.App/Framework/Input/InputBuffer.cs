namespace GameFromScratch.App.Framework.Input
{
    internal class InputBuffer
    {
        // make buffer protected so that the platform code can access it
        protected bool[] buffer;

        public InputBuffer()
        {
            var numKeyCodes = Enum.GetValues(typeof(KeyCode)).Length;
            buffer = new bool[numKeyCodes];
        }

        public bool IsDown(KeyCode key)
        {
            return buffer[(int)key];
        }

        // TODO(feature): add state for pressed/released
    }
}
