using System;
using MonoZenith.Players;

namespace MonoZenith.Support.Managers;

public class BuffManager
{
    public GameState State { get; }
    public Player Owner { get; }
    public Player OpposingPlayer { get; }
    public Buff Buff { get; set; }
    public Buff Debuff { get; set; }
    
    public BuffManager(GameState state, Player owner)
    {
        State = state;
        Owner = owner;
        OpposingPlayer = Owner.OpposingPlayer;
    }
    
    /// <summary>
    /// Update the state of the buff.
    /// </summary>
    public void Update()
    {
        Buff?.PerformEffect();
        Debuff?.PerformEffect();
    }
}