using GameFromScratch.App.Gameplay.Common.Entities;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems.Devices
{
    internal class GrapplingHookDeviceSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            var state = context.State;
            if (state.InputFlags.HasFlag(PlayerInputFlags.UseDevicePress) && state.ActiveDevice == PlayerDevice.GrapplingHook)
            {
                FireHook(context);
            }
        }

        private void FireHook(GameContext context)
        {
            var repo = context.State.Repository;
            var player = repo.Player;
            var input = context.Tools.Input;

            var mousePosition = input.MousePosition;
            var hookSpeed = player.Speed;
            var hookVelocity = Vector2.Normalize(input.MousePosition - player.Position) * hookSpeed;

            var grapplingHook = new Entity
            {
                Flags = EntityFlags.Hook | EntityFlags.Solid | EntityFlags.Move | EntityFlags.Render,
                Position = player.Position,
                Speed = hookSpeed,
                Velocity = hookVelocity,
                Bounds = new Vector2(10, 10),
                Color = Color.Red,
            };

            repo.Add(grapplingHook);
        }
    }
}
