using GameFromScratch.App.Gameplay.Common.Entities;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems.Devices
{
    internal class GrapplingHookDeviceSystem : ISystem
    {
        private Entity grapplingHook;
        private bool isActive;

        public GrapplingHookDeviceSystem()
        {
            grapplingHook = new Entity
            {
                Flags = EntityFlags.Hook | EntityFlags.Solid | EntityFlags.Move | EntityFlags.Render,
                Velocity = Vector2.Zero,
                Bounds = new Vector2(10, 10),
                Color = Color.Red,
            };
        }

        public void Initialize(GameContext context)
        {
            var player = context.State.Repository.Player;
            grapplingHook.Speed = player.Speed * 1.2f;
        }

        public void Update(GameContext context)
        {
            var state = context.State;
            if (state.InputFlags.HasFlag(PlayerInputFlags.UseDevicePress) && state.ActiveDevice == PlayerDevice.GrapplingHook)
            {
                if (isActive)
                {
                    DetachHook(context);
                }
                else
                {
                    FireHook(context);
                }
            }
        }

        private void FireHook(GameContext context)
        {
            var repo = context.State.Repository;
            var player = repo.Player;
            var input = context.Tools.Input;

            var hookVelocity = Vector2.Normalize(input.MousePosition - player.Position) * grapplingHook.Speed;

            grapplingHook.Position = player.Position;
            grapplingHook.Velocity = hookVelocity;

            repo.Add(grapplingHook);

            isActive = true;
            // global hook mode is enabled on collision
        }

        private void DetachHook(GameContext context)
        {
            var state = context.State;
            var repo = state.Repository;
            var player = repo.Player;

            // deactive / despawn hook
            isActive = false;
            state.HookAttached = false;
            repo.Remove(grapplingHook);
            player.AngularVelocity = 0;

            // TODO(feature): maintain tangential velocity when detaching
        }
    }
}
