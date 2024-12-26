using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZenith.Screen;
using MonoZenith.Support;
using MonoZenith.Support.Managers;

namespace MonoZenith;

public partial class Game
{
    public static Screens ActiveScreen;
    private static MainMenuScreen _mainMenuScreen;
    private static GameScreen _gameScreen;
    private static OverworldScreen _overworldScreen;
    private static PauseScreen _pauseScreen;
    private static List<Screen.Screen> _screens;

    public static GameScreen GetGameScreen() => _gameScreen;
    public static GameState GetGameState() => _gameScreen.GameState;
    
    /* Initialize game vars and load assets. */
    public static void Init()
    {
        // Screen Setup
        SetScreenSizeAutomatically();
        // SetScreenSize(1600, 900);
        // SetScreenFullScreen(true);
        SetWindowTitle("Echoes of the Shattering");
    }

    /// <summary>
    /// Initialize all screens.
    /// </summary>
    public static void InitializeScreens()
    {
        ActiveScreen = Screens.MAIN_MENU;  
        _mainMenuScreen = new MainMenuScreen();
        _gameScreen = new GameScreen();
        _overworldScreen = new OverworldScreen();
        _pauseScreen = new PauseScreen();
        _screens = new List<Screen.Screen>
        {
            _mainMenuScreen,
            _gameScreen,
            _overworldScreen,
            _pauseScreen
        };

        // Start with a fade-in when the game starts
        StartFadeIn();
    }

    private static void UnloadOnFadeOut(Screen.Screen screenToUnload)
    {
        if (IsFadingOut)
            screenToUnload.Unload();
    }
    
    /// <summary>
    /// Callback method to return to the main menu.
    /// </summary>
    public static void BackToMainMenu()
    {
        _gameScreen.Unload();
        StartFadeOut(() =>
        {
            _mainMenuScreen.Load();
            ActiveScreen = Screens.MAIN_MENU;
        });
    }
    
    /// <summary>
    /// Callback method to start the game.
    /// </summary>
    public static void StartGame()
    {
        _mainMenuScreen.Unload();
        StartFadeOut(() =>
        {
            _overworldScreen.Load();
            ActiveScreen = Screens.OVERWORLD;
        });
    }
    
    /// <summary>
    /// Load second phase of a level.
    /// </summary>
    public static void TryLoadSecondPhase()
    {
        if (LevelManager.CurrentLevel.SecondPhase == null) return;
        if (GetGameState().CurrentLevel == LevelManager.CurrentLevel.SecondPhase) return;
        
        Console.WriteLine("Load second phase...");
        
        _gameScreen.Unload();
        StartFadeOut(onFadeOutComplete: () =>
        {
            LevelManager.CurrentLevel.SecondPhase.Initialize(GetGameState());
            GetGameState().SetLevel(LevelManager.CurrentLevel.SecondPhase);
            GetGameScreen().SetBackgroundMusic(LevelManager.CurrentLevel.SecondPhase.SoundTrack);
            _gameScreen.Load(); 
        }); 
    }
    
    /* Update game logic. */
    public static void Update(GameTime deltaTime)
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

            case Screens.OVERWORLD:
                UnloadOnFadeOut(_mainMenuScreen);
                _gameScreen.Unload();
                _overworldScreen.Update(deltaTime);
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
    public static void Draw()
    {
        // Draw the active screen
        switch (ActiveScreen)
        {
            case Screens.GAME:
                _gameScreen.Draw();
                break;
            
            case Screens.OVERWORLD:
                _overworldScreen.Draw();
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