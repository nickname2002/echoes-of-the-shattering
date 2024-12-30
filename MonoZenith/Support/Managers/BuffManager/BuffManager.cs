using System;
using System.Collections.Generic;
using System.Diagnostics;
using MonoZenith.Players;

namespace MonoZenith.Support.Managers;

public class BuffManager
{
    public GameState State { get; }
    public Player Owner { get; }
    public Player OpposingPlayer { get; }
    public List<Buff> Buffs { get; set; }
    public List<Buff> Debuffs { get; set; }
    
    public BuffManager(GameState state, Player owner)
    {
        State = state;
        Owner = owner;
        OpposingPlayer = Owner.OpposingPlayer;
        Buffs = new List<Buff>();
        Debuffs = new List<Buff>();
    }
    
    /// <summary>
    /// Update the state of the buff.
    /// </summary>
    public void Update()
    {
        foreach(Buff buff in Buffs.ToArray())
        {
            buff?.PerformEffect();
        }
        foreach(Buff debuff in Debuffs.ToArray())
        {
            debuff?.PerformEffect();
        }
    }
}