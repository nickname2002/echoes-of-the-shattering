using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Support.Managers;

public class LevelManager
{
    public List<Level> Levels { get; }
    public Level CurrentLevel { get; set; }

    public LevelManager(Game g, GameState s)
    {
        Levels = new List<Level>
        {
            new()
            {
                Backdrop = DataManager.GetInstance(g).Backdrop,
                SoundTrack = DataManager.GetInstance(g).LimgraveMusic.CreateInstance(),
                Enemy = new NpcPlayer(g, s, "Varré")
            }
        };
        
        // TODO: Change later when user can change levels
        CurrentLevel = Levels[0];
    }
}