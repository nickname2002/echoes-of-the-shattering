using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Card.AttackCard;

/// <summary>
/// Representing all cards which require mana on use.
/// </summary>
public class MagicCard : AttackCard
{
    protected float _manaCost;
    
    protected MagicCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _manaCost = 0;
    }

    /// <summary>
    /// Lower the mana of the owner.
    /// </summary>
    protected void LowerPlayerMana()
    {
        _owner.Mana -= _manaCost;
    }

    protected override bool IsPlayable()
    {
        return base.IsPlayable() && _owner.Mana >= _manaCost;
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        LowerPlayerMana();
    }
}

public class GlintStonePebbleCard : MagicCard
{
    public GlintStonePebbleCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _soundOnPlay = DataManager.GetInstance(_game).GlintStonePebble;
        _manaCost = 10;
        _staminaCost = 20;
        _damage = 15;
    }
}