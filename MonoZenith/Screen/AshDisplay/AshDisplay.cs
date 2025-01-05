using Microsoft.Xna.Framework;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen.AshDisplay;

public class AshDisplay
{
    public void Update(GameTime deltaTime)
    {
        
    }
    
    public void Draw()
    {   
        DrawText(
            "Ash Display", 
            new Vector2(ScreenWidth / 2f - 50 * AppSettings.Scaling.ScaleFactor, ScreenHeight / 2f), 
            DataManager.GetInstance().HeaderFont,
            Color.White);
    }
}