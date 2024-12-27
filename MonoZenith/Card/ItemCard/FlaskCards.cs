using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith.Card;

public class FlaskOfCeruleanTearsCard : ItemCard
{
    public readonly float FocusBoost;
    
    public FlaskOfCeruleanTearsCard(GameState state, Player owner) : 
        base(state, owner)
    {
        FocusBoost = 30;
        _frontTexture = DataManager.GetInstance().CardFlaskCerulean;
        _soundOnPlay = DataManager.GetInstance().FlaskCeruleanSound.CreateInstance();
        _description.Add("Restore " + FocusBoost + " FP.");
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        BoostPlayerFocus();
    }

    /// <summary>
    /// Boos the mana of the owner.
    /// </summary>
    private void BoostPlayerFocus()
    {
        _owner.Focus = _owner.Focus + FocusBoost + Buff > 30f ? 30f 
            : _owner.Focus + FocusBoost + Buff;
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Restore " + (FocusBoost + Buff) + " FP.";
    }
}

public class FlaskOfCrimsonTearsCard : ItemCard
{
    public readonly float HealthBoost;
    
    public FlaskOfCrimsonTearsCard(GameState state, Player owner) : 
        base(state, owner)
    {
        HealthBoost = 75;
        _frontTexture = DataManager.GetInstance().CardFlaskCrimson;
        _soundOnPlay = DataManager.GetInstance().FlaskCrimsonSound.CreateInstance();
        _description.Add("Restore " + HealthBoost + " HP.");
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        HealPlayer();
    }

    /// <summary>
    /// Heal the owner.
    /// </summary>
    private void HealPlayer()
    {
        _owner.Health = _owner.Health + HealthBoost + Buff > 100f ? 100f 
            : _owner.Health + HealthBoost + Buff;
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Restore " + (HealthBoost + Buff) + " HP.";
    }
}

public class FlaskOfWondrousPhysickCard : ItemCard
{
    public readonly float HealthBoost;
    public readonly float FocusBoost;

    public FlaskOfWondrousPhysickCard(GameState state, Player owner) :
        base(state, owner)
    {
        HealthBoost = 50;
        FocusBoost = 15;
        _frontTexture = DataManager.GetInstance().CardWondrousPhysick;
        _soundOnPlay = DataManager.GetInstance().WondrousPhysickSound.CreateInstance();
        _description.Add("Restore " + HealthBoost + " HP and " + FocusBoost + ".");
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        HealAndRestorePlayer();
    }

    /// <summary>
    /// Heal the owner.
    /// </summary>
    private void HealAndRestorePlayer()
    {
        _owner.Health = _owner.Health + HealthBoost + Buff > 100f ? 100f
            : _owner.Health + HealthBoost + Buff;
        _owner.Focus = _owner.Focus + FocusBoost + Buff > 30f ? 30f
            : _owner.Focus + FocusBoost + Buff;
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Restore " + (HealthBoost + Buff) + " HP and " + (FocusBoost + Buff) + ".";
    }
}


public class WarmingStoneCard : ItemCard
{
    public readonly float HealthBoost;

    public WarmingStoneCard(GameState state, Player owner) :
        base(state, owner)
    {
        HealthBoost = 10;
        _frontTexture = DataManager.GetInstance().CardWarmingStone;
        _soundOnPlay = DataManager.GetInstance().WarmingStoneSound.CreateInstance();
        _description.Add("Restore " + HealthBoost + " HP");
        _description.Add("For 2 turns.");
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.BuffManager.Buffs.Add(new HealingEffectBuff(
        _state,
        _owner.BuffManager,
        2,
        (int)(HealthBoost + Buff)));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Restore " + (HealthBoost + Buff) + " HP";
    }
}