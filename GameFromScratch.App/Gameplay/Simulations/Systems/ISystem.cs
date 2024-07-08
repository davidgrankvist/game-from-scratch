namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal interface ISystem
    {
        public void Initialize();

        public void Update(SimulationContext context);
    }
}
