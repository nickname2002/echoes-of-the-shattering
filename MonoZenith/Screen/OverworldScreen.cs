using System;
using Microsoft.Xna.Framework;
using MonoZenith.Support;
using MonoZenith.Support.Managers;

namespace MonoZenith.Screen;

public class OverworldScreen : Screen
{
    public LevelManager LevelManager { get; set; }

    public OverworldScreen(Game game) : base(game)
    {
        LevelManager = new LevelManager(game, game.GetGameState());
    }

    public override void Unload()
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
        LevelManager.CurrentLevel.Initialize(_game, _game.GetGameState());
        _game.GetGameScreen().SetBackgroundMusic(LevelManager.CurrentLevel.SoundTrack);
        _game.GetGameState().SetLevel(LevelManager.CurrentLevel);
        _game.GetGameScreen().Load();
        _game.ActiveScreen = Screens.GAME;
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