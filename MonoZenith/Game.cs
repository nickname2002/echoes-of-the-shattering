using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Screen;
using MonoZenith.Support;

namespace MonoZenith;

public partial class Game
{
    public Screens ActiveScreen;
    private MainMenuScreen _mainMenuScreen;
    private GameScreen _gameScreen;
    private PauseScreen _pauseScreen; 

    // Add the FadeEffect instance
    private FadeEffectManager _fadeEffect;
    private bool _isFadingIn;

    /* Initialize game vars and load assets. */
    public void Init()
    {
        // Screen Setup
        SetScreenSize(1600, 900);
        SetWindowTitle("Echoes of the Shattering");

        ActiveScreen = Screens.MAIN_MENU;  
        _mainMenuScreen = new MainMenuScreen(this);
        _gameScreen = new GameScreen(this);
        _pauseScreen = new PauseScreen(this);

        // Initialize the FadeEffect with starting alpha at 1 (fully visible)
        _fadeEffect = new FadeEffectManager(1.0f, 0.005f);

        // Start with a fade-in when the game starts
        StartFadeIn();
    }

    // Trigger a fade-in effect
    public void StartFadeIn()
    {
        _isFadingIn = true;
        _fadeEffect.StartFadeIn(() => _isFadingIn = false); 
    }

    /* Update game logic. */
    public void Update(GameTime deltaTime)
    {
        // Update the active screen
        switch (ActiveScreen)
        {
            case Screens.GAME:
                _gameScreen.Update(deltaTime);
                break;

            case Screens.MAIN_MENU:
                _mainMenuScreen.Update(deltaTime);
                break;

            case Screens.PAUSE:
                _pauseScreen.Update(deltaTime);
                break;

            default:
                _gameScreen.Update(deltaTime);
                break;
        }

        // Update fade effect if active
        if (_isFadingIn)
        {
            _fadeEffect.Update();
        }
    }
    
    /* Draw objects/backdrop. */
    public void Draw()
    {
        // Draw the active screen
        switch (ActiveScreen)
        {
            case Screens.GAME:
                _gameScreen.Draw();
                break;
            
            case Screens.MAIN_MENU:
                _mainMenuScreen.Draw();
                break;

            case Screens.PAUSE:
                _pauseScreen.Draw();
                break;

            default:
                _gameScreen.Draw();
                break;
        }

        // Draw fade-in effect if it's still fading
        if (_isFadingIn)
        {
            _fadeEffect.DrawFadeEffect(_facade, Color.Black);  // Use a black fade
        }
    }
}