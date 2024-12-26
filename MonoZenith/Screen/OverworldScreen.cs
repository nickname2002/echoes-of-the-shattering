using System;
using Microsoft.Xna.Framework;
using MonoZenith.Support;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Screen;

public class OverworldScreen : Screen
{
    public LevelManager LevelManager { get; set; } = new();

    public override void Unload(float fadeSpeed = 0.015f, Action unOnloadComplete = null)
    {
        
    }

    public override void Load()
    {
        
    }

    /// <summary>
    /// Activate the selected level
    /// </summary>
    private void ActivateSelectedLevel()
    {
        LevelManager.CurrentLevel.Initialize(GetGameState());
        GetGameScreen().SetBackgroundMusic(LevelManager.CurrentLevel.SoundTrack);
        GetGameState().SetLevel(LevelManager.CurrentLevel);
        GetGameScreen().Load();
        ActiveScreen = Screens.GAME;
    }
    
    public override void Update(GameTime deltaTime)
    {
        // TODO: Update when overworld UI is implemented
        Console.WriteLine("In Overworld.");
        ActivateSelectedLevel();
    }
    
    public override void Draw()
    {
        
    }
}