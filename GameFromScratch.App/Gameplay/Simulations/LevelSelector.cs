using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.Simulations.Levels;
using GameFromScratch.App.Gameplay.Simulations.UI;
using System.Drawing;
using System.Numerics;

namespace GameFromScratch.App.Gameplay.Simulations
{
    internal class LevelSelector
    {
        private static readonly ILevel[] levels = [
            new ShrinkDeviceDemoLevel(),
            new GravityInverterDeviceDemoLevel(),
            new MapInverterDeviceDemoLevel(),
        ];

        public ILevel SelectedLevel { get => levels[selection]; }
        public bool IsReady { get => isReady; }

        private readonly SimulationTools tools;
        private int selection;
        private bool isReady;

        private Button prevLevelButton;
        private Button nextLevelButton;
        private Button selectLevelButton;
        private Vector2 selectedLevelPosition;

        public LevelSelector(SimulationTools tools)
        {
            this.tools = tools;

            InitUI();
            Reset();
        }

        public void Reset()
        {
            selection = 0;
            isReady = false;
        }

        private void InitUI()
        {
            var mapSize = LevelUtils.MAP_SIZE;
            var center = mapSize / 2;
            var bounds = new Vector2(100, 50);
            var fontSize = 10;
            var textColor = Color.Yellow;

            selectedLevelPosition = center + new Vector2(-20, -50);

            prevLevelButton = new Button
            {
                Position = center + new Vector2(-110, 0),
                Bounds = bounds,
                Color = Color.Blue,
                HoverColor = Color.Green,

                Text = "Previous level",
                TextColor = textColor,
                FontSize = fontSize,

                OnClick = () => selection = selection <= 0 ? levels.Length - 1 : selection - 1,
            };

            nextLevelButton = new Button
            {
                Position = center + new Vector2(110, 0),
                Bounds = bounds,
                Color = Color.Blue,
                HoverColor = Color.Green,

                Text = "Next level",
                TextColor = textColor,
                FontSize = fontSize,

                OnClick = () => selection = (selection + 1) % levels.Length,
            };

            selectLevelButton = new Button
            {
                Position = center + new Vector2(0, 100),
                Bounds = bounds,
                Color = Color.Blue,
                HoverColor = Color.Green,

                Text = "Select level",
                TextColor = textColor,
                FontSize = fontSize,

                OnClick = () => isReady = true,
            };
        }

        public void Update()
        {
            var graphics = tools.Graphics;
            graphics.PixelMode = true;

            graphics.DrawText(levels[selection].Name, 16, Color.Blue, selectedLevelPosition);
            prevLevelButton.Update(tools);
            nextLevelButton.Update(tools);
            selectLevelButton.Update(tools);

            graphics.PixelMode = false;
        }
    }
}
