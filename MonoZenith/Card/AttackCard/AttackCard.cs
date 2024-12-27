using System.Globalization;
using Microsoft.Xna.Framework;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Card.AttackCard;

public class AttackCard : Card
{
    protected Player _enemy;
    public float OriginalStaminaCost;
    public float StaminaCost;
    protected float _damage;
    
    public float Damage => _damage;
    
    protected AttackCard(GameState state, Player owner) : 
        base(state, owner)
    {
        _enemy = owner.OpposingPlayer;
        StaminaCost = 0f;
        OriginalStaminaCost = StaminaCost;
        _damage = 0;
        _soundOnPlay = null;
        _name = "BaseAttackCard";
    }
    
    /// <summary>
    /// Lower the stamina of the owner.
    /// </summary>
    protected void LowerPlayerStamina()
    {
        _owner.Stamina -= StaminaCost;
    }

    /// <summary>
    /// Apply damage to the enemy player.
    /// </summary>
    protected void ApplyEnemyDamage()
    {
        _enemy.Health -= _damage + Buff;
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        ApplyEnemyDamage();
    }

    public override bool IsAffordable()
    {
        return _owner.Stamina >= StaminaCost;
    }

    protected override void DrawMetaData()
    {
        // Calculate the top-left positions for the icon and text
        float scaleCost = 0.5f;
        float x = _costStaminaTexture.Width * 0.4f;
        float y = _costStaminaTexture.Height * 0.4f;
        Vector2 scaleVector = new Vector2(x, y) * _scale * scaleCost;
        Vector2 textOffset = StaminaCost >= 10 ? new Vector2(16, 24) : new Vector2(6, 24);

        // Draw the stamina cost icon
        DrawImage(
            _costStaminaTexture,
            _position - scaleVector,
            _scale * scaleCost
        );
        
        // Draw the stamina cost text
        DrawText(
            StaminaCost.ToString(CultureInfo.CurrentCulture),
            _position - textOffset * _scale,
            DataManager.GetInstance().CardFont,
            Color.White
        );
    }
}

// Basic attack cards - Light and Heavy Attack

public class LightSwordAttackCard : AttackCard
{
    public LightSwordAttackCard(GameState state, Player owner) : 
        base(state, owner)
    {
        StaminaCost = 10f;
        OriginalStaminaCost = StaminaCost;
        _damage = 10;
        _frontTexture = DataManager.GetInstance().CardLightAttack;
        _soundOnPlay = DataManager.GetInstance().LightSwordSound.CreateInstance();
        _name = "LightSwordAttackCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class HeavySwordAttackCard : AttackCard
{
    public HeavySwordAttackCard(GameState state, Player owner) : 
        base(state, owner)
    {
        StaminaCost = 20f;
        OriginalStaminaCost = StaminaCost;
        _damage = 20;
        _frontTexture = DataManager.GetInstance().CardHeavyAttack;
        _soundOnPlay = DataManager.GetInstance().HeavySwordSound.CreateInstance();
        _name = "HeavySwordAttackCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

// Reward Attack Cards

public class UnsheatheCard : AttackCard
{
    public UnsheatheCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 10f;
        OriginalStaminaCost = StaminaCost;
        _damage = 0;
        _frontTexture = DataManager.GetInstance().CardUnsheathe;
        _soundOnPlay = DataManager.GetInstance().UnsheatheSound.CreateInstance();
        _name = "UnsheatheCard";
        _description.Add("Next melee card deals");
        _description.Add("double damage");
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        _owner.BuffManager.Buffs.Add(new MeleeCardTwiceAsStrongBuff(
            _state, _owner.BuffManager));
    }
}

public class BloodhoundStepCard : AttackCard
{
    public BloodhoundStepCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 25f;
        OriginalStaminaCost = StaminaCost;
        _damage = 20;
        _frontTexture = DataManager.GetInstance().CardBloodhound;
        _soundOnPlay = DataManager.GetInstance().BloodhoundSound.CreateInstance();
        _name = "BloodhoundStepCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Ignore the next");
        _description.Add("enemy attack");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new DamageEvasionDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        1,
        1));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class QuickstepCard : AttackCard
{
    public QuickstepCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 10f;
        OriginalStaminaCost = StaminaCost;
        _damage = 0;
        _frontTexture = DataManager.GetInstance().CardQuickstep;
        _soundOnPlay = DataManager.GetInstance().QuickstepSound.CreateInstance();
        _name = "QuickstepCard";
        _description.Add("Ignore the next");
        _description.Add("enemy attack");
    }
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new DamageEvasionDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2,
        1));
    }
}

public class EndureCard : AttackCard
{
    public EndureCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 15f;
        OriginalStaminaCost = StaminaCost;
        _damage = 0;
        _frontTexture = DataManager.GetInstance().CardEndure;
        _soundOnPlay = DataManager.GetInstance().EndureSound.CreateInstance();
        _name = "EndureCard";
        _description.Add("Reduce damage taken");
        _description.Add("by half next turn");
    }
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new DamageReductionDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2,
        50));
    }
}

public class DoubleSlashCard : AttackCard
{
    public DoubleSlashCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 25f;
        OriginalStaminaCost = StaminaCost;
        _damage = 20;
        _frontTexture = DataManager.GetInstance().CardDoubleSlash;
        _soundOnPlay = DataManager.GetInstance().DoubleSlashSound.CreateInstance();
        _name = "DoubleSlashCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Next attack costs");
        _description.Add("10 less stamina");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.BuffManager.Buffs.Add(new CardStaminaBuff(
        _state,
        _owner.BuffManager,
        10));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class WarCryCard : AttackCard
{
    public WarCryCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 10f;
        OriginalStaminaCost = StaminaCost;
        _damage = 5;
        _frontTexture = DataManager.GetInstance().CardWarCry;
        _soundOnPlay = DataManager.GetInstance().WarCrySound.CreateInstance();
        _name = "DoubleSlashCard";
        _description.Add("Deal " + _damage + " damage and");
        _description.Add("+10 damage to");
        _description.Add("all cards next turn");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.BuffManager.Buffs.Add(new DamageIncreaseBuff(
        _state,
        _owner.BuffManager,
        1,
        10));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage and";
    }
}

public class StormcallerCard : AttackCard
{
    public StormcallerCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 20f;
        OriginalStaminaCost = StaminaCost;
        _damage = 15;
        _frontTexture = DataManager.GetInstance().CardStormcaller;
        _soundOnPlay = DataManager.GetInstance().StormcallerSound.CreateInstance();
        _name = "StormcallerCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Reduce stamina of");
        _description.Add("enemy by 10 next turn");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new StaminaEffectDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        1,
        10));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class RallyingStandardCard : AttackCard
{
    public RallyingStandardCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 15;
        OriginalStaminaCost = StaminaCost;
        _damage = 0;
        _frontTexture = DataManager.GetInstance().CardRallyingStandard;
        _soundOnPlay = DataManager.GetInstance().RallyingSound.CreateInstance();
        _name = "RallyingStandardCard";
        _description.Add("+ 10 damage to all");
        _description.Add("cards next 2 turns");
    }
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        base.PerformEffect();
        _owner.BuffManager.Buffs.Add(new DamageIncreaseBuff(
        _state,
        _owner.BuffManager,
        2,
        10));
    }
}

// Boss Reward Attack Cards

public class ICommandTheeKneelCard : AttackCard
{
    public ICommandTheeKneelCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 30f;
        OriginalStaminaCost = StaminaCost;
        _damage = 30;
        _frontTexture = DataManager.GetInstance().CardCommandKneel;
        _soundOnPlay = DataManager.GetInstance().CommandKneelSound.CreateInstance();
        _name = "ICommandTheeKneelCard";
        _description.Add("Deal " + _damage + " damage and");
        _description.Add("Reduce stamina of");
        _description.Add("enemy by 20 next turn");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new StaminaEffectDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        1,
        20));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage and";
    }
}

public class WaterfowlDanceCard : AttackCard
{
    public WaterfowlDanceCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 30f;
        OriginalStaminaCost = StaminaCost;
        _damage = 40;
        _frontTexture = DataManager.GetInstance().CardWaterfowlDance;
        _soundOnPlay = DataManager.GetInstance().WaterfowlDanceSound.CreateInstance();
        _name = "WaterfowlDanceCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Ignore the next");
        _description.Add("3 enemy attacks");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new DamageEvasionDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2,
        3));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class StarcallerCryCard : AttackCard
{
    public StarcallerCryCard(Game game, GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 30f;
        OriginalStaminaCost = StaminaCost;
        _damage = 40;
        _frontTexture = DataManager.GetInstance().CardStarcallerCry;
        //TODO: Change sound and effect
        _soundOnPlay = DataManager.GetInstance().StarcallerCrySound.CreateInstance();
        _name = "StarcallerCryCard";
        _description.Add("Deal " + _damage + " damage and");
        _description.Add("Reduce stamina of");
        _description.Add("enemy by 15 next 2 turns");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new StaminaEffectDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        ,
        15));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class CursedBloodSliceCard : AttackCard
{
    public CursedBloodSliceCard(Game game, GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 30f;
        OriginalStaminaCost = StaminaCost;
        _damage = 30;
        _frontTexture = DataManager.GetInstance().CardCursedSlice;
        //TODO: Change sound and effect
        _soundOnPlay = DataManager.GetInstance().CursedSliceSound.CreateInstance();
        _name = "CursedBloodSliceCard";
        _description.Add("Deal " + _damage + " damage.");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();

    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class BloodboonRitualCard : AttackCard
{
    public BloodboonRitualCard(Game game, GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 30f;
        OriginalStaminaCost = StaminaCost;
        _damage = 15;
        _frontTexture = DataManager.GetInstance().CardBloodboon;
        _soundOnPlay = DataManager.GetInstance().BloodboonSound1.CreateInstance();
        _name = "BloodboonRitualCard";
        _description.Add("Damage restores health.");
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("this and next 2 turns.");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new BloodboonDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2,
        (int)(_damage + Buff)));
        _owner.Health = _owner.Health + _damage > 100 ? 100 : _owner.Health + _damage;
    }

    protected override void UpdateDescription()
    {
        _description[1] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class DestinedDeathCard : AttackCard
{
    private int _healthReduction;

    public DestinedDeathCard(Game game, GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 30f;
        OriginalStaminaCost = StaminaCost;
        _damage = 10;
        _healthReduction = 20;
        _frontTexture = DataManager.GetInstance().CardDestinedDeath;
        //TODO: Change sound and effect
        _soundOnPlay = DataManager.GetInstance().DestinedDeathSound.CreateInstance();
        _name = "DestinedDeathCard";
        _description.Add("Enemy player has -" + _healthReduction + " max");
        _description.Add("health for next 2 turns");
        _description.Add("Deal " + _damage + " damage.");
    }
    public override void PerformEffect()
    {
        _owner.OpposingPlayer.OriginalHealth -= 
            _owner.OpposingPlayer.OriginalHealth * _healthReduction / 100;
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new DestinedDeathDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2));
        base.PerformEffect();
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class RegalRoarCard : AttackCard
{
    public RegalRoarCard(Game game, GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 30f;
        OriginalStaminaCost = StaminaCost;
        _damage = 30;
        _frontTexture = DataManager.GetInstance().CardRegalRoar;
        //TODO: Change sound and effect
        _soundOnPlay = DataManager.GetInstance().RegalRoarSound.CreateInstance();
        _name = "RegalRoarCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.BuffManager.Buffs.Add(new DamageIncreaseBuff(
        _state,
        _owner.BuffManager,
        2,
        10));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class WaveOfGoldCard : AttackCard
{
    public WaveOfGoldCard(Game game, GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 30f;
        OriginalStaminaCost = StaminaCost;
        _damage = 45;
        _frontTexture = DataManager.GetInstance().CardWaveOfGold;
        _soundOnPlay = DataManager.GetInstance().WaveOfGoldSound.CreateInstance();
        _name = "WaveOfGoldCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Removes enemy buffs.");
    }
    public override void PerformEffect()
    {
        base.PerformEffect();
        foreach(Buff buff in Owner.OpposingPlayer.BuffManager.Buffs)
        {
            if (buff is TurnBuff turnBuff)
            {
                turnBuff.RoundsLeft = 0;
                turnBuff.BuffRemoved();
            }
            else
            {
                buff.BuffRemoved();
            }
        }
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

// Psuedo Attack Cards, classified as Item cards

public class ThrowingDaggerCard : AttackCard
{
    public ThrowingDaggerCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 0f;
        OriginalStaminaCost = StaminaCost;
        _damage = 5f;
        _frontTexture = DataManager.GetInstance().CardThrowingDagger;
        _soundOnPlay = DataManager.GetInstance().ThrowingDaggerSound.CreateInstance();
        _name = "ThrowingDaggerCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }

    protected override void DrawMetaData()
    {
        return;
    }
}

public class PoisonPotCard : AttackCard
{
    public PoisonPotCard(GameState state, Player owner) :
        base(state, owner)
    {
        StaminaCost = 0f;
        OriginalStaminaCost = StaminaCost;
        _damage = 5f;
        _frontTexture = DataManager.GetInstance().CardPoisonPot;
        _soundOnPlay = DataManager.GetInstance().PoisonPotSound.CreateInstance();
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("next 2 turns");
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new PoisonEffectDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2,
        (int)(_damage + Buff)));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }

    protected override void DrawMetaData()
    {
        return;
    }
}