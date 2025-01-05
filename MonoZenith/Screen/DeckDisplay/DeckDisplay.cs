using Microsoft.Xna.Framework;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen.DeckDisplay;

public class DeckDisplay
{
    public void Update(GameTime deltaTime)
    {
        
    }
    
    public void Draw()
    {   
        DrawText(
            "Deck Display", 
            new Vector2(ScreenWidth / 2f - 50 * AppSettings.Scaling.ScaleFactor, ScreenHeight / 2f), 
            DataManager.GetInstance().HeaderFont,
            Color.White);
    }
}