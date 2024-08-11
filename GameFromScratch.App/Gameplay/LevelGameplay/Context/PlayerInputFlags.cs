namespace GameFromScratch.App.Gameplay.LevelGameplay.Context
{
    [Flags]
    internal enum PlayerInputFlags
    {
        None = 0,
        MoveLeft = 1 << 0,
        MoveRight = 1 << 1,
        MoveJump = 1 << 2,
        NextDevice = 1 << 3,
        PrevDevice = 1 << 4,
        UseDevicePress = 1 << 5,
        UseDeviceHold = 1 << 6,
    }
}
