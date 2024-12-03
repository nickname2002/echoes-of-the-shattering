using System.Globalization;
using Microsoft.Xna.Framework;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith.Card.AttackCard;

public class AttackCard : Card
{
    protected Player _enemy;
    protected float _staminaCost;
    protected float _damage;
    
    public float Damage => _damage;
    
    protected AttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _enemy = owner.OpposingPlayer;
        _staminaCost = 0f;
        _damage = 0;
        _soundOnPlay = null;
        _name = "BaseAttackCard";
    }
    
    /// <summary>
    /// Lower the stamina of the owner.
    /// </summary>
    protected void LowerPlayerStamina()
    {
        _owner.Stamina -= _staminaCost;
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
        return _owner.Stamina >= _staminaCost;
    }

    protected override void DrawMetaData()
    {
        // Calculate the top-left positions for the icon and text
        float scaleCost = 0.5f;
        float x = _costStaminaTexture.Width * 0.4f;
        float y = _costStaminaTexture.Height * 0.4f;
        Vector2 scaleVector = new Vector2(x, y) * _scale * scaleCost;
        Vector2 textOffset = _staminaCost >= 10 ? new Vector2(16, 24) : new Vector2(6, 24);

        // Draw the stamina cost icon
        _game.DrawImage(
            _costStaminaTexture,
            _position - scaleVector,
            _scale * scaleCost
        );
        
        // Draw the stamina cost text
        _game.DrawText(
            _staminaCost.ToString(CultureInfo.CurrentCulture),
            _position - textOffset * _scale,
            DataManager.GetInstance(_game).CardFont,
            Color.White
        );
    }
}

// Basic attack cards - Light and Heavy Attack

public class LightSwordAttackCard : AttackCard
{
    public LightSwordAttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _staminaCost = 10f;
        _damage = 10;
        _frontTexture = DataManager.GetInstance(_game).CardLightAttack;
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
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
    public HeavySwordAttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _staminaCost = 20f;
        _damage = 20;
        _frontTexture = DataManager.GetInstance(_game).CardHeavyAttack;
        _soundOnPlay = DataManager.GetInstance(_game).HeavySwordAttack.CreateInstance();
        _name = "HeavySwordAttackCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

// New Attack Cards

public class UnsheatheCard : AttackCard
{
    public UnsheatheCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 10f;
        _damage = 0;
        _frontTexture = DataManager.GetInstance(_game).CardUnsheathe;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "UnsheatheCard";
        _description.Add("Next card deals");
        _description.Add("double damage");
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();

    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class BloodhoundStepCard : AttackCard
{
    public BloodhoundStepCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 25f;
        _damage = 20;
        _frontTexture = DataManager.GetInstance(_game).CardBloodhound;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "BloodhoundStepCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Ignore next");
        _description.Add("enemy attack");
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

public class QuickstepCard : AttackCard
{
    public QuickstepCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 5f;
        _damage = 0;
        _frontTexture = DataManager.GetInstance(_game).CardQuickstep;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "QuickstepCard";
        _description.Add("Ignore next");
        _description.Add("enemy attack");
    }
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();

    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class EndureCard : AttackCard
{
    public EndureCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 10f;
        _damage = 0;
        _frontTexture = DataManager.GetInstance(_game).CardEndure;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "EndureCard";
        _description.Add("Reduce damage taken");
        _description.Add("by half next turn");
    }
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();

    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}

public class DoubleSlashCard : AttackCard
{
    public DoubleSlashCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 25f;
        _damage = 20;
        _frontTexture = DataManager.GetInstance(_game).CardDoubleSlash;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "DoubleSlashCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Next attack costs");
        _description.Add("10 less stamina");
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

public class WarCryCard : AttackCard
{
    public WarCryCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 10f;
        _damage = 5;
        _frontTexture = DataManager.GetInstance(_game).CardWarCry;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "DoubleSlashCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("+10 damage to");
        _description.Add("all cards this turn");
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

public class StormcallerCard : AttackCard
{
    public StormcallerCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 20f;
        _damage = 15;
        _frontTexture = DataManager.GetInstance(_game).CardStormcaller;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "StormcallerCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Reduce stamina of");
        _description.Add("enemy by 10 next turn");
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

public class RallyingStandardCard : AttackCard
{
    public RallyingStandardCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 15;
        _damage = 0;
        _frontTexture = DataManager.GetInstance(_game).CardRallyingStandard;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "RallyingStandardCard";
        _description.Add("+10 damage to all");
        _description.Add("cards for 2 turns");
    }
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();

    }
}

public class ICommandTheeKneelCard : AttackCard
{
    public ICommandTheeKneelCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 30f;
        _damage = 30;
        _frontTexture = DataManager.GetInstance(_game).CardCommandKneel;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "ICommandTheeKneelCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Reduce stamina of");
        _description.Add("enemy by 20 next turn");
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

public class WaterfowlDanceCard : AttackCard
{
    public WaterfowlDanceCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 30f;
        _damage = 40;
        _frontTexture = DataManager.GetInstance(_game).CardWaterfowlDance;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "DoubleSlashCard";
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("Ignore next");
        _description.Add("3 enemy attacks");
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

// Psuedo Attack Cards, classified as Item cards

public class ThrowingDaggerCard : AttackCard
{
    public ThrowingDaggerCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 0f;
        _damage = 5f;
        _frontTexture = DataManager.GetInstance(_game).CardThrowingDagger;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
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
    public PoisonPotCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _staminaCost = 0f;
        _damage = 5f;
        _frontTexture = DataManager.GetInstance(_game).CardPoisonPot;
        //TODO CHANGE
        _soundOnPlay = DataManager.GetInstance(_game).FlaskOfCrimsonTears.CreateInstance();
        _description.Add("Deal " + _damage + " damage.");
        _description.Add("For 2 turns");
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.OpposingPlayer.BuffManager.Debuff = new PoisonEffectDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        (int)_damage,
        2);
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