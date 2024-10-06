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
    private MainMenuOptionButton _startButton;
    private MainMenuOptionButton _settingsButton;

    public MainMenuScreen(Game game) : base(game)
    {
        _mainMenuBackdrop = DataManager.GetInstance(game).MainMenuBackdrop;  
        _mainMenuScale = 0.7f;
        var startButtonSound = DataManager.GetInstance(game).StartButtonSound;
        _mainMenuMusic = DataManager.GetInstance(game).MainMenuMusic;
        _mainMenuMusic.IsLooped = true;
        _mainMenuMusic.Play();
        
        Console.WriteLine(_mainMenuMusic.State);
        
        // Start button
        _startButton = new MainMenuOptionButton(
            _game, 
            _game.ScreenHeight / 2 + 250, 
            "Start Game",
            StartGame,
            startButtonSound);
        
        // Settings dummy button
        _settingsButton = new MainMenuOptionButton(
            _game, 
            _game.ScreenHeight / 2 + 325, 
            "Settings",
            () => Console.WriteLine("Settings button clicked")
        );
    }

    /// <summary>
    /// Start the game when the start button is clicked.
    /// </summary>
    private void StartGame()
    {
        _game.StartFadeOut(OnFadeOutComplete);
        return;

        void OnFadeOutComplete()
        {
            _game.StartFadeIn();
            _game.ActiveScreen = Screens.GAME;
        }
    }

    /// <summary>
    /// Remove side effects of the main menu screen.
    /// </summary>
    public override void Unload()
    {
        float musicFadeOutSpeed = 0.015f;

        if (_mainMenuMusic.Volume >= musicFadeOutSpeed)
        {
            _mainMenuMusic.Volume -= musicFadeOutSpeed;
        }
        else
        {
            _mainMenuMusic.Stop();
        }
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