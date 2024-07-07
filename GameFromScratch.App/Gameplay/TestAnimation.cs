using GameFromScratch.App.Framework;
using GameFromScratch.App.Framework.Graphics;
using GameFromScratch.App.Framework.Input;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay
{
    internal class TestAnimation
    {
        private readonly IGraphics2D graphics;
        private readonly InputBuffer input;
        private readonly Camera2D camera;

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

        private Vector2 playerPos = new Vector2(333, 333);

        public TestAnimation(IGraphics2D graphics, InputBuffer input, Camera2D camera)
        {
            this.graphics = graphics;
            this.input = input;
            this.camera = camera;
        }

        public void Update()
        {
            AnimateBackground();
            AnimateRectangle();
            AnimateCircle();
            AnimateTriangle();
            AnimateSpinningRectangle();
            AnimatePlayer();
            TestPressedReleased();
            TestViewport();
            TestMouse();

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

        private void AnimatePlayer()
        {
            var dx = 0;
            var dy = 0;
            var speed = 5;
            if (input.IsDown(KeyCode.W))
            {
                dy = -speed;
            }

            if (input.IsDown(KeyCode.A))
            {
                dx = -speed;
            }

            if (input.IsDown(KeyCode.S))
            {
                dy = speed;
            }

            if (input.IsDown(KeyCode.D))
            {
                dx = speed;
            }
            playerPos += new Vector2(dx, dy);

            graphics.DrawCircle(playerPos, 50, Color.Cyan);
        }

        private void TestPressedReleased()
        {
            if (input.IsPressed(KeyCode.A))
            {
                Console.WriteLine("Pressed A");
            }

            if (input.IsReleased(KeyCode.A))
            {
                Console.WriteLine("Released A");
            }
        }

        private void TestViewport()
        {
            camera.SetViewport(-100, 0);
        }

        private void TestMouse()
        {
            if (input.IsPressed(KeyCode.MouseLeft))
            {
                Console.WriteLine("Pressed mouse left");
            }
            if (input.IsReleased(KeyCode.MouseLeft))
            {
                Console.WriteLine("Released mouse left");
            }
            if (input.IsPressed(KeyCode.MouseRight))
            {
                Console.WriteLine("Pressed mouse right");
            }
            if (input.IsReleased(KeyCode.MouseRight))
            {
                Console.WriteLine("Released mouse right");
            }
        }
    }
}
