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
    protected float _focusCost;
    
    protected MagicCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name, Player owner) : 
        base(game, state, position, texture, activeTexture, name, owner)
    {
        _focusCost = 0;
    }

    /// <summary>
    /// Lower the mana of the owner.
    /// </summary>
    protected void LowerPlayerMana()
    {
        _owner.Focus -= _focusCost;
    }

    protected override bool IsPlayable()
    {
        return base.IsPlayable() && _owner.Focus >= _focusCost;
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        LowerPlayerMana();
    }
}

public class GlintStonePebbleCard : MagicCard
{
    public GlintStonePebbleCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name, Player owner) : 
        base(game, state, position, texture, activeTexture, name, owner)
    {
        _soundOnPlay = DataManager.GetInstance(_game).GlintStonePebble;
        _focusCost = 2;
    }
}