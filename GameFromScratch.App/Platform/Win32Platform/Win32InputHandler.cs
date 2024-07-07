using GameFromScratch.App.Framework.Input;
using System.Runtime.Versioning;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal class Win32InputHandler
    {
        private readonly InputBuffer inputBuffer;

        public Win32InputHandler(InputBuffer inputBuffer)
        {
            this.inputBuffer = inputBuffer;
        }
        public void HandleKeyDown(uint virtualKey, int state)
        {
            var key = FromVirtualKey(virtualKey);
            if (!key.HasValue)
            {
                return;
            }

            inputBuffer.SetKeyState(key.Value, true);
        }

        public void HandleKeyUp(uint virtualKey, int state)
        {
            var key = FromVirtualKey(virtualKey);
            if (!key.HasValue)
            {
                return;
            }

            inputBuffer.SetKeyState(key.Value, false);
        }

        /*
         * Converts Windows virtual key codes to platform independent key codes.
         * See https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
         */
        private static KeyCode? FromVirtualKey(uint virtualKey)
        {
            switch (virtualKey)
            {
                // WASD
                case 0x57:
                    return KeyCode.W;
                case 0x41:
                    return KeyCode.A;
                case 0x53:
                    return KeyCode.S;
                case 0x44:
                    return KeyCode.D;
                default:
                    return null;
            }
        }

        public void HandleMouseLeftDown(uint mouseVirtualKey, int position)
        {
            inputBuffer.SetKeyState(KeyCode.MouseLeft, true);
        }

        public void HandleMouseLeftUp(uint mouseVirtualKey, int position)
        {
            inputBuffer.SetKeyState(KeyCode.MouseLeft, false);
        }

        public void HandleMouseRightDown(uint mouseVirtualKey, int position)
        {
            inputBuffer.SetKeyState(KeyCode.MouseRight, true);
        }

        public void HandleMouseRightUp(uint mouseVirtualKey, int position)
        {
            inputBuffer.SetKeyState(KeyCode.MouseRight, false);
        }
    }
}
