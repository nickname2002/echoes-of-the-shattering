#nullable enable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Players;
using MonoZenith.Screen.RewardPanel;

namespace MonoZenith.Support.Managers;

public class Level
{
    [JsonIgnore]
    public Texture2D Backdrop { get; init; }
    
    [JsonIgnore]
    public SoundEffectInstance SoundTrack { get; init; }
    
    [JsonIgnore]
    public NpcPlayer Enemy { get; init; }
    
    [JsonIgnore]
    public Reward? LevelReward { get; set; }
    
    [JsonIgnore]
    public List<Card.Card> EnemyDeck { get; init; }

    public string EnemyName => Enemy.Name;
    public bool Unlocked { get; set; } = false;
    public bool RewardCollected { get; set; }
    
    public Level? SecondPhase { get; init; }
    public List<SoundEffectInstance> VoiceLinesBattleStart = new();
    public List<SoundEffectInstance> VoiceLinesBattleLoss = new();
    public List<SoundEffectInstance> VoiceLinesBattleVictory = new();

    public Level()
    {
        RewardCollected = (LevelReward == null);
    }

    /// <summary>
    /// Reset the state of the Level.
    /// </summary>
    /// <param name="s">The game state.</param>
    public void Initialize(GameState s)
    {
        Enemy.CardsInDeck = EnemyDeck;
        Enemy.InitializeState(s);
        SoundTrack.IsLooped = true;
    }
}