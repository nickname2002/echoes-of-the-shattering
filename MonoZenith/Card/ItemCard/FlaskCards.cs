using MonoZenith.Engine.Support;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Card;

public class FlaskOfCeruleanTearsCard : ItemCard
{
    public readonly float FocusBoost;
    
    public FlaskOfCeruleanTearsCard()
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
    
    public FlaskOfCrimsonTearsCard()
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
    public int doubleBuff;

    public FlaskOfWondrousPhysickCard()
    {
        HealthBoost = 50;
        FocusBoost = 15;
        doubleBuff = 1;
        _frontTexture = DataManager.GetInstance().CardWondrousPhysick;
        _soundOnPlay = DataManager.GetInstance().WondrousPhysickSound.CreateInstance();
        _description.Add("Restore " + HealthBoost + " HP");
        _description.Add("and " + FocusBoost + " FP.");
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
        _owner.Health = _owner.Health + HealthBoost * doubleBuff > 100f ? 100f
            : _owner.Health + HealthBoost * doubleBuff;
        _owner.Focus = _owner.Focus + FocusBoost * doubleBuff > 30f ? 30f
            : _owner.Focus + FocusBoost * doubleBuff;
    }

    protected override void UpdateDescription()
    {
        if (Buff != 0 && doubleBuff == 1)
        {
            doubleBuff = 2;
        }
        else if (Buff == 0 && doubleBuff == 2)
        {
            doubleBuff = 1;
        }
        _description[0] = "Restore " + (HealthBoost * doubleBuff) + " HP";
        _description[1] = "and " + (FocusBoost * doubleBuff) + " FP.";
    }
}


public class WarmingStoneCard : ItemCard
{
    public readonly float HealthBoost;

    public WarmingStoneCard()
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
        GetGameState(),
        _owner.BuffManager,
        2,
        (int)(HealthBoost + Buff)));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Restore " + (HealthBoost + Buff) + " HP";
    }
}