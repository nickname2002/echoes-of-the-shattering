using System.Globalization;
using Microsoft.Xna.Framework;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

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
