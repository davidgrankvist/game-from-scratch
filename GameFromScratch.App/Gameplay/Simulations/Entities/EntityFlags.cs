namespace GameFromScratch.App.Gameplay.Simulations.Entities
{
    [Flags]
    internal enum EntityFlags
    {
        /*
         * Use powers of 2 so that bitwise AND/OR can be used
         * to compose flags.
         */
        None = 0,
        Solid = 1 << 0,
        Move = 1 << 1,
        Render = 1 << 2,
        MovePeriodic = 1 << 3,
        Player = 1 << 4,
    }
}
