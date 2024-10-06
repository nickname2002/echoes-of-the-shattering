using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        // Start with a fade-in when the game starts
        StartFadeIn();
    }

    private void UnloadOnFadeOut(Screen.Screen screenToUnload)
    {
        if (IsFadingOut)
            screenToUnload.Unload();
    }
    
    /* Update game logic. */
    public void Update(GameTime deltaTime)
    {
        // Update fade effect if active
        if (IsFadingIn || IsFadingOut)
        {
            _fadeEffect.Update();
        }
        
        // Update the active screen
        switch (ActiveScreen)
        {
            case Screens.GAME:
                UnloadOnFadeOut(_gameScreen);
                _mainMenuScreen.Unload();
                _gameScreen.Update(deltaTime);
                break;

            case Screens.MAIN_MENU:
                UnloadOnFadeOut(_mainMenuScreen);
                _gameScreen.Unload();
                _mainMenuScreen.Update(deltaTime);
                break;

            case Screens.PAUSE:
                _mainMenuScreen.Unload();
                _pauseScreen.Update(deltaTime);
                break;

            default:
                _gameScreen.Update(deltaTime);
                break;
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
        if (IsFadingIn || IsFadingOut)
        {
            _fadeEffect.DrawFadeEffect(_facade, Color.Black);
        }
    }
}