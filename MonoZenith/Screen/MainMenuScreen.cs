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
    private static MainMenuOptionButton _continueButton;
    private static MainMenuOptionButton _quitButton;
    private static MainMenuOptionButton _creditsButton;
    
    public MainMenuScreen()
    {
        _mainMenuBackdrop = DataManager.GetInstance().MainMenuBackdrop;  
        _mainMenuScale = 0.7f * AppSettings.Scaling.ScaleFactor;
        var startButtonSound = DataManager.GetInstance().StartButtonSound.CreateInstance();
        _mainMenuMusic = DataManager.GetInstance().MainMenuMusic.CreateInstance();
        _mainMenuMusic.IsLooped = true;
        _mainMenuMusic.Play();
        
        // Continue game button
        _continueButton = new MainMenuOptionButton(
            Instance, 
            ScreenHeight / 2f + (int)(250 * AppSettings.Scaling.ScaleFactor),
            "Continue game",
            ContinueGame,
            startButtonSound);
        
        // Start button
        _startButton = new MainMenuOptionButton(
            Game.Instance, 
            ScreenHeight / 2f + (int)(300 * AppSettings.Scaling.ScaleFactor), 
            "Start new game",
            StartNewGame,
            startButtonSound);
        
        // Quit to Desktop button
        _quitButton = new MainMenuOptionButton(
            Game.Instance, 
            ScreenHeight / 2f + (int)(350 * AppSettings.Scaling.ScaleFactor), 
            "Quit to desktop",
            BackToDesktop,
            startButtonSound);
        
        // Credits button
        _creditsButton = new MainMenuOptionButton(
            Game.Instance, 
            ScreenHeight / 2f + (int)(400 * AppSettings.Scaling.ScaleFactor), 
            "Credits",
            ToCreditsScreen,
            startButtonSound);
    }
    
    public override void Unload(float fadeSpeed = 0.05f, Action onUnloadComplete = null)
    {
        float musicFadeOutSpeed = fadeSpeed;
        
        if (_mainMenuMusic.Volume >= musicFadeOutSpeed)
        {
            _mainMenuMusic.Volume -= musicFadeOutSpeed;
            return;
        }
        
        _mainMenuMusic.Stop(); 
        onUnloadComplete?.Invoke();
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
        _quitButton.Update(deltaTime);
        _creditsButton.Update(deltaTime);
        if (!HasSaveFile) return;
        _continueButton.Update(deltaTime);
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
        
        _startButton.Draw();
        _quitButton.Draw();
        _creditsButton.Draw();
        if (!HasSaveFile) return;   
        _continueButton.Draw();
    }
}