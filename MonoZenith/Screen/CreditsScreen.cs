using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Components.MainMenuScreen;
using MonoZenith.Engine.Support;

namespace MonoZenith.Screen;

public class CreditsScreen : Screen
{
    private readonly Texture2D _creditsTexture;
    private Vector2 _position;

    private readonly MainMenuOptionButton _backButton;

    public CreditsScreen()
    {
        _creditsTexture = Game.LoadImage("Images/credits.png");
        _position = Vector2.Zero;

        var buttonSound = DataManager.GetInstance().StartButtonSound.CreateInstance();
        _backButton = new MainMenuOptionButton(
            Game.Instance,
            Game.ScreenHeight - (int)(100 * AppSettings.Scaling.ScaleFactor), 
            "Back to Main Menu",
            Game.BackToMainMenu,
            buttonSound
        );
        
        // Set the position of the back button to the left side of the screen
        _backButton.ChangePosition(new Vector2(
            Game.ScreenWidth / 7f,
            Game.ScreenHeight - (int)(100 * AppSettings.Scaling.ScaleFactor))
        );
    }

    public override void Unload(float fadeSpeed = 0.05f, Action onUnloadComplete = null)
    {
        
    }

    public override void Load()
    {
        Game.StartFadeIn();
        _position = new Vector2(0, 0);
    }

    public override void Update(GameTime deltaTime)
    {
        _backButton.Update(deltaTime);
    }

    public override void Draw()
    {
        Game.DrawImage(_creditsTexture, _position, AppSettings.Scaling.ScaleFactor); 
        _backButton.Draw();
    }
}