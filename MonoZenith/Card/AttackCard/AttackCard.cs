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

    public override bool IsAffordable()
    {
        return _owner.Stamina >= _staminaCost;
    }
}

public class LightSwordAttackCard : AttackCard
{
    public LightSwordAttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _staminaCost = 10f;
        _damage = 10;
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack.CreateInstance();
        _name = "LightSwordAttackCard";
    }
}

public class HeavySwordAttackCard : AttackCard
{
    public HeavySwordAttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _staminaCost = 20f;
        _damage = 20;
        _soundOnPlay = DataManager.GetInstance(_game).HeavySwordAttack.CreateInstance();
        _name = "HeavySwordAttackCard";
    }
}
