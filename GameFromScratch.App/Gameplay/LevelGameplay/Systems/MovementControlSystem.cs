using GameFromScratch.App.Framework.Maths;
using GameFromScratch.App.Gameplay.Common.Entities;
using GameFromScratch.App.Gameplay.LevelGameplay.Context;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelGameplay.Systems
{
    internal class MovementControlSystem : ISystem
    {
        public void Initialize(GameContext context)
        {
        }

        public void Update(GameContext context)
        {
            if (context.State.HookAttached)
            {
                UpdateGrapplingHookMovement(context);
            }
            else
            {
                UpdateRegularMovement(context);
            }
        }

        private static void UpdateRegularMovement(GameContext context)
        {
            var state = context.State;
            var player = state.Repository.Player;

            var playerVx = player.Velocity.X;
            var playerVy = player.Velocity.Y;

            var isTouchingGround = playerVy == 0;

            // prevent sliding on the floor
            if (isTouchingGround)
            {
                playerVx = 0;
            }

            if (state.IsActiveInput(PlayerInputFlags.MoveJump) && isTouchingGround)
            {
                playerVy = -player.JumpSpeed * context.State.GravitySign;
            }
            if (state.IsActiveInput(PlayerInputFlags.MoveLeft))
            {
                playerVx = -player.Speed;
            }
            if (state.IsActiveInput(PlayerInputFlags.MoveRight))
            {
                playerVx = player.Speed;
            }

            player.Velocity = new Vector2(playerVx, playerVy);
        }

        private static void UpdateGrapplingHookMovement(GameContext context)
        {
            var state = context.State;
            var player = state.Repository.Player;

            player.Velocity = Vector2.Zero;

            var grapplingHook = state.Repository.Query(EntityFlags.Hook).First();
            var hookPosition = grapplingHook.Position;
            var hookDirection = Vector2.Normalize(hookPosition - player.Position);

            if (state.IsActiveInput(PlayerInputFlags.MoveUp))
            {
                // pull
                player.Velocity = hookDirection * player.Speed;
            }
            if (state.IsActiveInput(PlayerInputFlags.MoveDown))
            {
                // push
                player.Velocity = hookDirection * -player.Speed;
            }

            var ropeToVerticalAxisAngle = MathF.Atan2(player.Position.X - hookPosition.X, player.Position.Y - hookPosition.Y);
            var ropeLength = Vector2.Distance(hookPosition, player.Position);
            var gravity = 1350f;
            var angularAcceleration = -gravity / ropeLength * MathF.Sin(ropeToVerticalAxisAngle);
            var inputTorque = 5f;
            var maxAngularSpeed = 5f;
            var dampenFactor = 0.005f;

            var deltaGravity = angularAcceleration * state.DeltaTime;
            var deltaSwing = 0f;

            if (state.IsActiveInput(PlayerInputFlags.MoveLeft))
            {
                // clockwise
                deltaSwing = -inputTorque * state.DeltaTime;
            }
            if (state.IsActiveInput(PlayerInputFlags.MoveRight))
            {
                // counter clockwise
                deltaSwing = inputTorque * state.DeltaTime;
            }

            var nextAngularVelocity = (1 - dampenFactor) * (player.AngularVelocity + deltaGravity + deltaSwing);
            var cappedAngularVelocity = MathExtensions.Clamp(nextAngularVelocity, -maxAngularSpeed, maxAngularSpeed);
            player.AngularVelocity = cappedAngularVelocity;

            var nextAngle = ropeToVerticalAxisAngle + player.AngularVelocity * state.DeltaTime;
            var x = hookPosition.X + ropeLength * MathF.Sin(nextAngle);
            var y = hookPosition.Y + ropeLength * MathF.Cos(nextAngle);
            // TODO(bug): consider player collision during pendulum movement
            player.Position = new Vector2(x, y);
        }
    }
}
