using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using MonoZenith.Players;
using MonoZenith.Screen.RewardPanel;

namespace MonoZenith.Support.Managers;

public class LevelManager
{
    public List<Level> Levels { get; }
    public Level CurrentLevel { get; set; }

    public LevelManager(Game g, GameState s)
    {
        Levels = new List<Level>
        {
            // Test
            new()
            {
                Backdrop = DataManager.GetInstance(g).Backdrop,
                SoundTrack = DataManager.GetInstance(g).LimgraveMusic.CreateInstance(),
                Enemy = new NpcPlayer(g, s, "NPC"),
                Reward = new Reward(
                    DataManager.GetInstance(g).MimicTearAsh,
                    "Mimic Tear Ash",
                    typeof(MimicTearAsh))
            },
            
            // Test 2 (Limgrave theme)
            new()
            {
                Backdrop = DataManager.GetInstance(g).LimgraveBackdrop,
                SoundTrack = DataManager.GetInstance(g).LimgraveMusic.CreateInstance(),
                Enemy = new NpcPlayer(g, s, "Varré"),
                Reward = new Reward(
                    DataManager.GetInstance(g).WolvesAsh,
                    "Wolves Spirit Ash",
                    typeof(WolvesAsh))
            },
            
            // Test 3 (Caelid theme)
            new()
            {
                Backdrop = DataManager.GetInstance(g).CaelidBackdrop,
                SoundTrack = DataManager.GetInstance(g).LimgraveMusic.CreateInstance(),
                Enemy = new NpcPlayer(g, s, "Varré"),
                Reward = new Reward(
                    DataManager.GetInstance(g).RemembranceOfTheStarscourge,
                    "Radahn's Remembrance",
                    typeof(WolvesAsh))
            }
        };
        
        // TODO: Change later when user can change levels
        CurrentLevel = Levels[0];
    }
}