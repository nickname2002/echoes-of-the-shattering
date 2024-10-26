using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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
        _enemy = owner == state.CurrentPlayer ? state.OpposingPlayer : state.CurrentPlayer;
        _staminaCost = 0f;
        _damage = 0;
        _soundOnPlay = null;
        _name = "BaseAttackCard";
    }
    
    public override bool IsAffordable() => _owner.Stamina >= _staminaCost;
    
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
        _enemy.Health -= _damage;
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        ApplyEnemyDamage();
    }

    protected override bool IsPlayable()
    {
        return _owner.Stamina >= _staminaCost;
    }
}

public class LightSwordAttackCard : AttackCard
{
    public LightSwordAttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack;
        _damage = 10;
        _staminaCost = 10;
    }
}

public class HeavySwordAttackCard : AttackCard
{
    public HeavySwordAttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _soundOnPlay = DataManager.GetInstance(_game).HeavySwordAttack;
        _damage = 20;
        _staminaCost = 20;
    }
}
