using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Components;
using MonoZenith.Components.MainMenuScreen;
using MonoZenith.Engine.Support;
using MonoZenith.Support;

namespace MonoZenith.Screen;

public class MainMenuScreen : Screen
{
    private Texture2D _mainMenuBackdrop;
    private float _mainMenuScale;
    private SoundEffectInstance _mainMenuMusic;

    // TODO: Create special MainMenuButton class (subclass of Button) 
    private MainMenuOptionButton _startButton;
    private MainMenuOptionButton _settingsButton;
        
    public MainMenuScreen(Game game) : base(game)
    {
        _mainMenuBackdrop = DataManager.GetInstance(game).MainMenuBackdrop;  
        _mainMenuScale = 0.7f;
        
        // Start button
        _startButton = new MainMenuOptionButton(
            _game, 
            _game.ScreenHeight / 2 + 250, 
            "Start Game",
            StartGame);
        
        // Settings dummy button
        _settingsButton = new MainMenuOptionButton(
            _game, 
            _game.ScreenHeight / 2 + 325, 
            "Settings",
            () => Console.WriteLine("Settings button clicked")
        );
        
        _mainMenuMusic = DataManager.GetInstance(game).MainMenuMusic;
        _mainMenuMusic.IsLooped = true;
        _mainMenuMusic.Play();
    }

    /// <summary>
    /// Start the game when the start button is clicked.
    /// </summary>
    private void StartGame()
    {
        _mainMenuMusic.Stop();
        _game.ActiveScreen = Screens.GAME;
    }

    public override void Update(GameTime deltaTime)
    {
        _startButton.Update(deltaTime);
        _settingsButton.Update(deltaTime);
    }

    public override void Draw()
    {
        // Draw tifo
       _game.DrawImage(
           _mainMenuBackdrop,
           new Vector2(
               _game.ScreenWidth / 2 - _mainMenuBackdrop.Width / 2 * _mainMenuScale,
               _game.ScreenHeight / 2 - _mainMenuBackdrop.Height / 2 * _mainMenuScale - 100 * _mainMenuScale),
           _mainMenuScale);
       
       // Draw start button
       _startButton.Draw();
       _settingsButton.Draw();
    }
}