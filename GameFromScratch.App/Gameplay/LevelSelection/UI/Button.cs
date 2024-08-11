using GameFromScratch.App.Framework.Graphics;
using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.Common;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.LevelSelection.UI
{
    internal class Button
    {
        public Vector2 Position;
        public Vector2 Bounds;
        public Color Color;
        public Color HoverColor;

        public string Text = "";
        public Color TextColor;
        public int FontSize;

        public Action OnClick = () => { };

        private bool IsHovering;

        public void Update(GameTools tools)
        {
            CheckInput(tools.Input);
            Draw(tools.Graphics);
        }

        private void CheckInput(IInputBuffer input)
        {
            IsHovering = CheckIsHovering(input.MousePosition);
            if (IsHovering && input.IsPressed(KeyCode.MouseLeft))
            {
                OnClick();
            }
        }

        private bool CheckIsHovering(Vector2 mousePosition)
        {
            var topLeft = Position;
            var bottomRight = Position + Bounds;

            return mousePosition.X >= topLeft.X && mousePosition.X <= bottomRight.X
                && mousePosition.Y >= topLeft.Y && mousePosition.Y <= bottomRight.Y;
        }

        private void Draw(IGraphics2D graphics)
        {
            var background = IsHovering ? HoverColor : Color;
            graphics.DrawRectangle(Position, Bounds.X, Bounds.Y, background);

            // roughly centralizes the text within the button
            var buttonCenter = Position + Bounds / 2;
            var textX = buttonCenter.X - FontSize * Text.Length / 3.5f;
            var textY = buttonCenter.Y - FontSize / 2f;
            var textTopLeft = new Vector2(textX, textY);
            graphics.DrawText(Text, FontSize, TextColor, textTopLeft);
        }
    }
}
