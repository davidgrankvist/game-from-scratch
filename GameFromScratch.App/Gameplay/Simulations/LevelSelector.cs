using GameFromScratch.App.Framework.Input;
using GameFromScratch.App.Gameplay.Simulations.Levels;

namespace GameFromScratch.App.Gameplay.Simulations
{
    // TODO(feature): Add GUI. For now it's a simple print based level selector for prototyping. 
    internal class LevelSelector
    {
        private static readonly ILevel[] levels = [
            new ShrinkDeviceDemoLevel(),
            new GravityInverterDeviceDemoLevel(),
        ];

        public ILevel SelectedLevel { get => levels[selection]; }
        public bool IsReady { get => isReady; }

        private readonly IInputBuffer input;
        private int selection;
        private bool didPrintInitialSelection ;
        private bool isReady;

        public LevelSelector(IInputBuffer input)
        {
            this.input = input;
            selection = 0;
            didPrintInitialSelection = false;
            isReady = false;
        }

        public void Update()
        {
            if (!didPrintInitialSelection) 
            {
                PrintInstructions();
                PrintSelection();
                didPrintInitialSelection = true;
            }

            if (input.IsPressed(KeyCode.A))
            {
                selection = selection <= 0 ? levels.Length - 1 : selection - 1;
                PrintSelection();
            }
            if (input.IsPressed(KeyCode.D))
            {
                selection = (selection + 1) % levels.Length;
                PrintSelection();
            }
            if (input.IsPressed(KeyCode.S))
            {
                Console.WriteLine("Ready");
                isReady = true;
            }
        }

        private static void PrintInstructions()
        {
            var instructions = @"====== LEVEL SELECTOR ======
Controls:
Press A/D to select prev/next level
Press S to play
============================";
            Console.WriteLine(instructions);
        }

        private void PrintSelection()
        {
            Console.WriteLine("Selected: " + levels[selection].Name);
        }
    }
}
