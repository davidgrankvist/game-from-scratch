using GameFromScratch.App.Gameplay.LevelGameplay.Context;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems.Devices
{
    internal class DeviceSwitcherSystem : ISystem
    {
        private readonly PlayerDevice[] devices;
        private int currentDevice;

        public DeviceSwitcherSystem()
        {
            devices = (PlayerDevice[])Enum.GetValues(typeof(PlayerDevice));
            currentDevice = 0;
        }

        public void Initialize(GameContext context)
        {
            for (var i = 0; i < devices.Length; i++)
            {
                if (devices[i] == context.State.ActiveDevice)
                {
                    currentDevice = i;
                    break;
                }
            }
        }

        public void Update(GameContext context)
        {
            var state = context.State;

            if (state.IsActiveInput(PlayerInputFlags.NextDevice))
            {
                currentDevice = (currentDevice + 1) % devices.Length;
                state.ActiveDevice = devices[currentDevice];
            }

            if (state.IsActiveInput(PlayerInputFlags.PrevDevice))
            {
                currentDevice = currentDevice > 0 ? currentDevice - 1 : devices.Length - 1;
                state.ActiveDevice = devices[currentDevice];
            }
        }
    }
}
