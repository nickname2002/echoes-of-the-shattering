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

    /// <summary>
    /// Responsible for saving and loading game data.
    /// </summary>
    private static SaveManager _saveManager;
    public bool HasSaveFile => _saveManager.HasSaveFile;
    
    public static GameScreen GetGameScreen() => _gameScreen;
    public static GameState GetGameState() => _gameScreen.GameState;
    public static OverworldScreen GetOverworldScreen() => _overworldScreen;
    
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

        _saveManager = new SaveManager();
        _saveManager.LoadGame();
        
        // Start with a fade-in when the game starts
        StartFadeIn();
    }

    public static void SaveGame()
    {
        _saveManager.SaveGame();
    }
    
    private static void UnloadOnFadeOut(Screen.Screen screenToUnload)
    {
        if (IsFadingOut)
            screenToUnload.Unload();
    }

    public static void ToGameScreen()
    {
        _overworldScreen.Unload();
        StartFadeOut(() =>
        {
            _gameScreen.Load();
            ActiveScreen = Screens.GAME;
        });
    }
    
    /// <summary>
    /// Callback method to return to the overworld.
    /// </summary>
    public static void BackToOverworld()
    {
        _gameScreen.Unload();
        StartFadeOut(() =>
        {
            _overworldScreen.Load();
            ActiveScreen = Screens.OVERWORLD;
        });
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
    /// Callback method to start game on a new save file.
    /// </summary>
    public static void StartNewGame()
    {
        _saveManager.RemoveSaveFile();
        _saveManager.LoadGame();
        ContinueGame();
    }
    
    /// <summary>
    /// Callback method to start the game on an existing save file.
    /// </summary>
    public static void ContinueGame()
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
        // Update the active screen
        switch (ActiveScreen)
        {
            case Screens.GAME:
                UnloadOnFadeOut(_gameScreen);
                _mainMenuScreen.Unload(0.03f);
                _overworldScreen.Unload();
                if (IsFadingIn || IsFadingOut) { _fadeEffect.Update(); return; }
                _gameScreen.Update(deltaTime);
                break;

            case Screens.OVERWORLD:
                UnloadOnFadeOut(_overworldScreen);
                _mainMenuScreen.Unload(0.03f);
                _gameScreen.Unload();
                if (IsFadingIn || IsFadingOut) { _fadeEffect.Update(); return; }
                _overworldScreen.Update(deltaTime);
                break;
            
            case Screens.MAIN_MENU:
                UnloadOnFadeOut(_mainMenuScreen);
                _gameScreen.Unload(); 
                if (IsFadingIn || IsFadingOut) { _fadeEffect.Update(); return; }
                _mainMenuScreen.Update(deltaTime);
                break;

            case Screens.PAUSE:
                _mainMenuScreen.Unload();
                if (IsFadingIn || IsFadingOut) { _fadeEffect.Update(); return; }
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