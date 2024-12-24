#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Players;
using MonoZenith.Screen.RewardPanel;

namespace MonoZenith.Support.Managers;

public class Level
{
    public Texture2D Backdrop { get; init; }
    public SoundEffectInstance SoundTrack { get; init; }
    public NpcPlayer Enemy { get; init; }
    public Reward? Reward { get; init; }
    public List<Card.Card> EnemyDeck { get; init; }
    public Level? SecondPhase { get; init; }

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