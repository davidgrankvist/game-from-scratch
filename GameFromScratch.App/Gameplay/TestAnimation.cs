using GameFromScratch.App.Framework;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay
{
    internal class TestAnimation
    {
        private readonly IGraphics2D graphics;

        private long animationLastTick = DateTime.MinValue.Ticks;
        private byte backgroundColor = 0;
        private int backgroundColorDelta = 1;

        private Vector2 rectanglePos = new Vector2(50, 50);
        private Vector2 rectangleVelocity = new Vector2(2f, 1.5f);

        public TestAnimation(IGraphics2D graphics)
        {
            this.graphics = graphics;
        }

        public void Update()
        {
            AnimateBackground();
            AnimateRectangle();

            graphics.Commit();
        }

        private void AnimateBackground()
        {
            var ellapsed = DateTime.UtcNow.Ticks - animationLastTick;
            if (ellapsed > 1000)
            {
                animationLastTick = DateTime.UtcNow.Ticks;
                if (backgroundColor == 120)
                {
                    backgroundColorDelta = -1;
                }
                else if (backgroundColor == 0)
                {
                    backgroundColorDelta = 1;
                }

                backgroundColor = (byte)(backgroundColor + backgroundColorDelta);
                graphics.Fill(Color.FromArgb(backgroundColor, 0, 0));
            }
        }

        private void AnimateRectangle()
        {
            if (rectanglePos.X > 400 || rectanglePos.X < 50)
            {
                rectangleVelocity.X = -rectangleVelocity.X;
            }
            if (rectanglePos.Y > 300 || rectanglePos.Y < 50)
            {
                rectangleVelocity.Y = -rectangleVelocity.Y;
            }

            rectanglePos += rectangleVelocity;
            graphics.DrawRectangle(rectanglePos, 100, 100, Color.Green);
        }
    }
}
