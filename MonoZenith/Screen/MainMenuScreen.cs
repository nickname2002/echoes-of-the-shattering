using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Support;

namespace MonoZenith.Screen;

public class MainMenuScreen : Screen
{
    private Texture2D _mainMenuBackdrop;
    private float _mainMenuScale;
    private Button _startButton;
    private Texture2D _hoverIndicator;
    private float _hoverIndicatorScale;
    
    public MainMenuScreen(Game game) : base(game)
    {
        _mainMenuBackdrop = DataManager.GetInstance(game).MainMenuBackdrop;  
        _hoverIndicator = DataManager.GetInstance(game).MainMenuHoverIndicator;
        _mainMenuScale = 0.7f;
        _hoverIndicatorScale = 0.2f;
        _startButton = new Button(
            game,
            new Vector2(_game.ScreenWidth / 2 - 125 / 2, _game.ScreenHeight / 2 + 300),
            125, 40,
            "Start Game", 1, Color.White,
            Color.Black, 0, Color.Black);
        _startButton.Font = DataManager.GetInstance(game).StartMenuFont;
        _startButton.SetOnClickAction(() => _game.ActiveScreen = Screens.GAME);
    }

    public override void Update(GameTime deltaTime)
    {
        _startButton.Update(deltaTime);
    }

    public override void Draw()
    {
        // Draw tifo
       _game.DrawImage(
           _mainMenuBackdrop,
           new Vector2(
               _game.ScreenWidth / 2 - _mainMenuBackdrop.Width / 2 * _mainMenuScale,
               _game.ScreenHeight / 2 - _mainMenuBackdrop.Height / 2 * _mainMenuScale - 50 * _mainMenuScale),
           _mainMenuScale);
       
       // Draw start button hover indicator
       if (_startButton.IsHovered())
       {
           _game.DrawImage(
               _hoverIndicator,
               new Vector2(
                   _game.ScreenWidth / 2 - _hoverIndicator.Width / 2 * _hoverIndicatorScale,
                   _game.ScreenHeight / 2 + 1150 * _hoverIndicatorScale),
               _hoverIndicatorScale);
       }
       
       // Draw start button
       _startButton.Draw();
    }
}