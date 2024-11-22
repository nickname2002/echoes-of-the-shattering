using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Card;

public class FlaskOfCeruleanTearsCard : ItemCard
{
    private readonly float _focusBoost;
    
    public FlaskOfCeruleanTearsCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _focusBoost = 30;
        _frontTexture = DataManager.GetInstance(_game).CardFlaskCerulean;
        _soundOnPlay = DataManager.GetInstance(_game).FlaskOfCeruleanTears.CreateInstance();
        _description = "Restore " + _focusBoost + " FP.";
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        BoostPlayerFocus();
    }

    /// <summary>
    /// Boos the mana of the owner.
    /// </summary>
    private void BoostPlayerFocus()
    {
        _owner.Focus = _owner.Focus + _focusBoost > 30f ? 30f : _owner.Focus + _focusBoost;
    }
}

public class FlaskOfCrimsonTearsCard : ItemCard
{
    private readonly float _healthBoost;
    
    public FlaskOfCrimsonTearsCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _healthBoost = 75;
        _frontTexture = DataManager.GetInstance(_game).CardFlaskCrimson;
        _soundOnPlay = DataManager.GetInstance(_game).FlaskOfCrimsonTears.CreateInstance();
        _description = "Restore " + _healthBoost + " HP.";
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        HealPlayer();
    }

    /// <summary>
    /// Heal the owner.
    /// </summary>
    private void HealPlayer()
    {
        _owner.Health = _owner.Health + _healthBoost > 100f ? 100f : _owner.Health + _healthBoost;
    }
}