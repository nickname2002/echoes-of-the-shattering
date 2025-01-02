using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;
using System;
using System.Linq;
using static MonoZenith.Game;

namespace MonoZenith.Card.AttackCard;

/// <summary>
/// Representing all cards which require mana on use.
/// </summary>
public class MagicCard : AttackCard
{
    protected Texture2D _costFocusTexture;
    protected float _focusCost;
    
    protected MagicCard(GameState state, Player owner) : 
        base(state, owner)
    {
        _costFocusTexture = DataManager.GetInstance().CardCostFocus;
        _focusCost = 0;
    }
    
    /// <summary>
    /// Lower the mana of the owner.
    /// </summary>
    protected void LowerPlayerMana()
    {
        _owner.Focus -= _focusCost;
    }

    public override bool IsAffordable()
    {
        return base.IsAffordable() && _owner.Focus >= _focusCost;
    }

    public override void IsReductionOrEvasionActive()
    {
        if (_owner.OpposingPlayer.BuffManager.Buffs.OfType<DamageEvasionBuff>().Any())
        {
            _debuff = _damage + Buff;
        }
        else if (_owner.OpposingPlayer.BuffManager.Buffs.OfType<ThopsBuff>().Any())
        {
            _debuff = _damage + Buff;
        }
        else if (_owner.OpposingPlayer.BuffManager.Buffs.OfType<DamageReductionBuff>().Any())
        {
            _debuff = (_damage + Buff) / 2;
        }
        else
        {
            _debuff = 0;
        }
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        LowerPlayerMana();
    }

    protected override void DrawMetaData()
    {
        // Draw the stamina cost
        base.DrawMetaData();

        // Calculate the top-right positions for the cost icon and text
        float scaleCost = 0.5f;
        float x = _costStaminaTexture.Width * 0.6f;
        float y = _costStaminaTexture.Height * 0.4f;
        Vector2 scaleVector = new Vector2(x, y) * _scale * scaleCost;
        Vector2 textOffset = _focusCost >= 10 ? new Vector2(32, 24) : new Vector2(20, 24);

        // Draw the focus cost icon
        DrawImage(
            _costFocusTexture,
            _position - scaleVector + new Vector2(_width, 0),
            _scale * scaleCost
        );
        
        // Draw the focus cost text
        DrawText(
            _focusCost.ToString(),
            _position - textOffset * _scale + new Vector2(_width, 0),
            DataManager.GetInstance().CardFont,
            Color.CornflowerBlue
        );
    }
}

// Basic Magic Card - Glintstone Pebble

public class GlintStonePebbleCard : MagicCard
{
    public GlintStonePebbleCard(GameState state, Player owner) : 
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardGlintPebble;
        _soundOnPlay = DataManager.GetInstance().GlintPebbleSound.CreateInstance();
        _focusCost = 3;
        StaminaCost = 5;
        OriginalStaminaCost = StaminaCost;
        _damage = 15;
        _name = "GlintStonePebbleCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + _totalDamage + " damage.";
    }
}

// Reward Magic Cards

public class GlintbladePhalanxCard : MagicCard
{
    public GlintbladePhalanxCard(GameState state, Player owner) :
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardGlintPhalanx;
        _soundOnPlay = DataManager.GetInstance().GlintPhalanxSound.CreateInstance();
        _focusCost = 6;
        StaminaCost = 5;
        OriginalStaminaCost = StaminaCost;
        _damage = 10;
        _name = "GlintbladePhalanxCard";
        _description.Add("Deal " + _damage + " damage");
        _description.Add("for 2 turns.");
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        LowerPlayerMana();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new PoisonEffectDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2,
        (int)_totalDamage));
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + _totalDamage + " damage";
    }
}

public class ThopsBarrierCard : MagicCard
{
    public ThopsBarrierCard(GameState state, Player owner) :
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardThopsBarrier;
        _soundOnPlay = DataManager.GetInstance().ThopsBarrierSound.CreateInstance();
        _focusCost = 6;
        StaminaCost = 5;
        OriginalStaminaCost = StaminaCost;
        _damage = 0;
        _name = "ThopsBarrierCard";
        _description.Add("Ignore all magic");
        _description.Add("attacks next turn.");
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        LowerPlayerMana();

        _owner.BuffManager.Buffs.Add(new ThopsBuff(
        _state,
        _owner.BuffManager,
        1,
        100));

        foreach (Card card in Owner.OpposingPlayer.DeckStack.Cards)
        {
            card.CheckEnemyBuffs();
        }
    }
}

public class GreatGlintStoneCard : MagicCard
{
    public GreatGlintStoneCard(GameState state, Player owner) :
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardGreatShard;
        _soundOnPlay = DataManager.GetInstance().GreatShardSound.CreateInstance();
        _focusCost = 5;
        StaminaCost = 10;
        _damage = 20;
        _name = "GreatGlintStoneShardCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + _totalDamage + " damage.";
    }
}

public class CarianGreatSwordCard : MagicCard
{
    public CarianGreatSwordCard(GameState state, Player owner) :
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardCarianGSword;
        _soundOnPlay = DataManager.GetInstance().CarianGSwordSound.CreateInstance();
        _focusCost = 8;
        StaminaCost = 20;
        OriginalStaminaCost = StaminaCost;
        _damage = 30;
        _name = "CarianGreatSwordCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + _totalDamage + " damage.";
    }
}

// Boss Reward Magic Card

public class CometAzurCard : MagicCard
{
    public CometAzurCard(GameState state, Player owner) :
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardCometAzur;
        _soundOnPlay = DataManager.GetInstance().CometAzurSound.CreateInstance();
        _focusCost = 25;
        StaminaCost = 30;
        OriginalStaminaCost = StaminaCost;
        _damage = 45;
        _name = "CometAzurCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + _totalDamage + " damage.";
    }
}

public class MoonlightGreatswordCard : MagicCard
{
    public MoonlightGreatswordCard(GameState state, Player owner) :
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardMoonlight;
        _soundOnPlay = DataManager.GetInstance().MoonlightSound.CreateInstance();
        _focusCost = 25;
        StaminaCost = 30;
        OriginalStaminaCost = StaminaCost;
        _damage = 40;
        _name = "MoonlightGreatswordCard";
        _description.Add("Deal " + _damage + " damage and");
        _description.Add("skip enemy\'s");
        _description.Add("next turn");
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new MoonlightDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2));
        Owner.OpposingPlayer.SkipTurn = true;
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + _totalDamage + " damage.";
    }
}