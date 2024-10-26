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
    protected void AppleEnemyDamage()
    {
        _enemy.Health -= _damage;
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
        LowerPlayerStamina();
        AppleEnemyDamage();
    }

    public override bool IsPlayable()
    {
        return _owner.Stamina >= _staminaCost;
    }
}

public class LightSwordAttackCard : AttackCard
{
    public LightSwordAttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _staminaCost = 5f;
        _damage = 5f;
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack;
    }
}

public class HeavySwordAttackCard : AttackCard
{
    public HeavySwordAttackCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _staminaCost = 10f;
        _damage = 20f;
        _soundOnPlay = DataManager.GetInstance(_game).HeavySwordAttack;
    }
}
