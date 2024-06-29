using GameFromScratch.App.Framework.Input;
using System.Runtime.Versioning;

namespace GameFromScratch.App.Platform.Win32Platform
{
    [SupportedOSPlatform("windows7.0")]
    internal class Win32InputBuffer : InputBuffer
    {
        public void HandleKeyDown(byte vk, int state)
        {
            var key = ToKeyCode(vk);
            if (!key.HasValue)
            {
                return;
            }

            SetKeyState(key.Value, true);
        }

        public void HandleKeyUp(byte vk, int state)
        {
            var key = ToKeyCode(vk);
            if (!key.HasValue)
            {
                return;
            }

            SetKeyState(key.Value, false);
        }

        /*
         * Converts Windows virtual key codes to platform independent key codes.
         * See https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
         */
        private static KeyCode? ToKeyCode(byte vk)
        {
            switch (vk)
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

        private void SetKeyState(KeyCode key, bool isDown)
        {
            buffer[(int)key] = isDown;
        }
    }
}
