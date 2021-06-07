using Microsoft.Xna.Framework;

public static class GameTimeExtension
{
    public static float GetDelta(this GameTime gt)
    {
        return (float)gt.ElapsedGameTime.TotalSeconds;
    }
}