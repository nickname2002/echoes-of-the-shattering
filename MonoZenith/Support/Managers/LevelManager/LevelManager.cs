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

    public LevelManager(GameState s)
    {
        Levels = new List<Level>
        {
            // Test
            new()
            {
                Backdrop = DataManager.GetInstance().Backdrop,
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                Enemy = new NpcPlayer(s, "NPC"),
                Reward = new Reward(
                    DataManager.GetInstance().MimicTearAsh,
                    "Mimic Tear Ash",
                    typeof(MimicTearAsh))
            },
            
            // Test 2 (Limgrave theme)
            new()
            {
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                Enemy = new NpcPlayer(s, "Varré"),
                Reward = new Reward(
                    DataManager.GetInstance().WolvesAsh,
                    "Wolves Spirit Ash",
                    typeof(WolvesAsh))
            },
            
            // Test 3 (Caelid theme)
            new()
            {
                Backdrop = DataManager.GetInstance().CaelidBackdrop,
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                Enemy = new NpcPlayer(s, "Varré"),
                Reward = new Reward(
                    DataManager.GetInstance().RemembranceOfTheStarscourge,
                    "Radahn's Remembrance",
                    typeof(WolvesAsh))
            }
        };
        
        // TODO: Change later when user can change levels
        CurrentLevel = Levels[0];
    }
}