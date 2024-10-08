﻿namespace GameFromScratch.App.Gameplay.Common.Entities
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
        Player = 1 << 3,
        Goal = 1 << 4,
        Invert = 1 << 5,
        Hook =  1 << 6,
    }
}
