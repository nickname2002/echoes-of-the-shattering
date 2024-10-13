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
    protected SoundEffectInstance _soundOnPlay;
    
    protected AttackCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name, Player owner) : 
        base(game, state, position, texture, activeTexture, name, owner)
    {
        _enemy = owner == state.CurrentPlayer ? state.OpposingPlayer : state.CurrentPlayer;
        _staminaCost = 0f;
        _damage = 0;
        _soundOnPlay = null;
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
}

public class LightSwordAttackCard : AttackCard
{
    protected LightSwordAttackCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name, Player owner) : 
        base(game, state, position, texture, activeTexture, name, owner)
    {
        _soundOnPlay = DataManager.GetInstance(_game).LightSwordAttack;
    }
}

public class HeavySwordAttackCard : AttackCard
{
    protected HeavySwordAttackCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name, Player owner) : 
        base(game, state, position, texture, activeTexture, name, owner)
    {
        _soundOnPlay = DataManager.GetInstance(_game).HeavySwordAttack;
    }
}