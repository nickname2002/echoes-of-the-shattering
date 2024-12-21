using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Components.MainMenuScreen;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen;

public class MainMenuScreen : Screen
{
    private static Texture2D _mainMenuBackdrop;
    private float _mainMenuScale;
    private static SoundEffectInstance _mainMenuMusic;
    private static MainMenuOptionButton _startButton;
    private static MainMenuOptionButton _settingsButton;

    public MainMenuScreen()
    {
        _mainMenuBackdrop = DataManager.GetInstance().MainMenuBackdrop;  
        _mainMenuScale = 0.7f * AppSettings.Scaling.ScaleFactor;
        var startButtonSound = DataManager.GetInstance().StartButtonSound.CreateInstance();
        _mainMenuMusic = DataManager.GetInstance().MainMenuMusic.CreateInstance();
        _mainMenuMusic.IsLooped = true;
        _mainMenuMusic.Play();
        
        // Start button
        _startButton = new MainMenuOptionButton(
            Game.Instance, 
            ScreenHeight / 2f + (int)(250 * AppSettings.Scaling.ScaleFactor), 
            "Start Game",
            StartGame,
            startButtonSound);

        // Settings button
        _settingsButton = new MainMenuOptionButton(
            Instance, 
            ScreenHeight / 2f + (int)(325 * AppSettings.Scaling.ScaleFactor),
            "Settings",
            () => Console.WriteLine("Settings button clicked")
        );
    }
    
    public override void Unload()
    {
        float musicFadeOutSpeed = 0.015f;
        
        if (_mainMenuMusic.Volume >= musicFadeOutSpeed)
        {
            _mainMenuMusic.Volume -= musicFadeOutSpeed;
            return;
        }
        
        _mainMenuMusic.Stop(); 
    }
    
    public override void Load()
    {
        _mainMenuMusic.Volume = 1;
        StartFadeIn(() => _mainMenuMusic.Play());
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
            (ScreenWidth - scaledWidth) / 2,
            (ScreenHeight - scaledHeight) / 2 - 100 * _mainMenuScale); 
        
        // Draw the backdrop
        DrawImage(_mainMenuBackdrop, position, _mainMenuScale);
    
        // Draw start and settings buttons
        _startButton.Draw();
        _settingsButton.Draw();
    }
}