namespace GameFromScratch.App.Gameplay.Simulations.Entities
{
    [Flags]
    internal enum EntityFlag
    {
        /*
         * Use powers of 2 so that bitwise AND/OR can be used
         * to compose flags.
         */
        None = 0,
        Test1 = 1 << 0,
        Test2 = 1 << 1,
    }
}
