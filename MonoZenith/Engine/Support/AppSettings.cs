using System;

namespace MonoZenith.Engine.Support;

public static class AppSettings
{
    public static class Scaling
    {
        public static float ScaleFactor { get; set; } = 1.0f;
        public static int OriginalScreenHeight => 900;
        public static int OriginalScreenWidth { get; } = 1600;
        
        public static void UpdateScaleFactor(int currentWidth, int currentHeight)
        {
            ScaleFactor = Math.Min(
                (float)currentWidth / OriginalScreenWidth, 
                (float)currentHeight / OriginalScreenHeight);
        }
    }
}