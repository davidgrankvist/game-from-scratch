namespace GameFromScratch.App.Gameplay.Simulations.Systems
{
    internal interface ISystem
    {
        public void Initialize(SimulationContext context);

        public void Update(SimulationContext context);
    }
}
