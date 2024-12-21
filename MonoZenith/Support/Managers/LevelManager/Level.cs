using System;
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
    public Reward Reward { get; init; }
    
    /// <summary>
    /// Reset the state of the Level.
    /// </summary>
    /// <param name="g">The game.</param>
    /// <param name="s">The game state.</param>
    public void Initialize(Game g, GameState s)
    {
        Enemy.InitializeState(s);
        SoundTrack.IsLooped = true;
    }
}