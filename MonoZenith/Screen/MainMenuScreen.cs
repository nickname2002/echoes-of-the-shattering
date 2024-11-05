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
        _mainMenuScale = 0.7f * AppSettings.Scaling.ScaleFactor;
        var startButtonSound = DataManager.GetInstance(game).StartButtonSound.CreateInstance();
        _mainMenuMusic = DataManager.GetInstance(game).MainMenuMusic.CreateInstance();
        _mainMenuMusic.IsLooped = true;
        _mainMenuMusic.Play();
        
        // Start button
        _startButton = new MainMenuOptionButton(
            _game, 
            _game.ScreenHeight / 2 + (int)(250 * AppSettings.Scaling.ScaleFactor), 
            "Start Game",
            StartGame,
            startButtonSound);

        // Settings button
        _settingsButton = new MainMenuOptionButton(
            _game, 
            _game.ScreenHeight / 2 + (int)(325 * AppSettings.Scaling.ScaleFactor),
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
        _mainMenuScale = 0.7f * AppSettings.Scaling.ScaleFactor;
        _startButton.Update(deltaTime);
        _settingsButton.Update(deltaTime);
    }

    public override void Draw()
    {
        // Calculate scaled width and height based on scale factor
        float scaledWidth = _mainMenuBackdrop.Width * _mainMenuScale;
        float scaledHeight = _mainMenuBackdrop.Height * _mainMenuScale;

        // Center the backdrop on the screen
        Vector2 position = new Vector2(
            (_game.ScreenWidth - scaledWidth) / 2,
            (_game.ScreenHeight - scaledHeight) / 2 - 100 * _mainMenuScale); 
        
        // Draw the backdrop
        _game.DrawImage(_mainMenuBackdrop, position, _mainMenuScale);
    
        // Draw start and settings buttons
        _startButton.Draw();
        _settingsButton.Draw();
    }
}