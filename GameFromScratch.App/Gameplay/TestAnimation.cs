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

        private Vector2 circlePos = new Vector2(100, 100);

        private Vector2 triangleA = new Vector2(200, 125);
        private Vector2 triangleB = new Vector2(250, 100);
        private Vector2 triangleC = new Vector2(225, 220);

        private Vector2 spinningRectanglePos = new Vector2(300, 100);
        private float spinningRectangleAngle = 0f;

        public TestAnimation(IGraphics2D graphics)
        {
            this.graphics = graphics;
        }

        public void Update()
        {
            AnimateBackground();
            AnimateRectangle();
            AnimateCircle();
            AnimateTriangle();
            AnimateSpinningRectangle();

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

        private void AnimateCircle()
        {
            graphics.DrawCircle(circlePos, 50, Color.Blue);
        }

        private void AnimateTriangle()
        {
            graphics.DrawTriangle(triangleA, triangleB, triangleC, Color.Chocolate);
        }

        private void AnimateSpinningRectangle()
        {
            spinningRectangleAngle += 0.01f;
            var rectangleCenter = spinningRectanglePos + new Vector2(25, 25);
            graphics.DrawRectangleRotated(spinningRectanglePos, 50, 50, Color.Beige, spinningRectangleAngle, rectangleCenter);
        }
    }
}
