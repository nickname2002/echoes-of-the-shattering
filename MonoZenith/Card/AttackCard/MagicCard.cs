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
    protected Texture2D _costFocusTexture;
    protected float _focusCost;
    
    protected MagicCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _costFocusTexture = DataManager.GetInstance(_game).CardCostFocus;
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
        _game.DrawImage(
            _costFocusTexture,
            _position - scaleVector + new Vector2(_width, 0),
            _scale * scaleCost
        );
        
        // Draw the focus cost text
        _game.DrawText(
            _focusCost.ToString(),
            _position - textOffset * _scale + new Vector2(_width, 0),
            DataManager.GetInstance(_game).CardFont,
            Color.CornflowerBlue
        );
    }
}

public class GlintStonePebbleCard : MagicCard
{
    public GlintStonePebbleCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _frontTexture = DataManager.GetInstance(_game).CardGlintPebble;
        _soundOnPlay = DataManager.GetInstance(_game).GlintStonePebble.CreateInstance();
        _focusCost = 2;
        _staminaCost = 5;
        _damage = 15;
        _name = "GlintStonePebbleCard";
        _description.Add("Deal " + _damage + " damage.");
    }

    protected override void UpdateDescription()
    {
        _description[0] = "Deal " + (_damage + Buff) + " damage.";
    }
}