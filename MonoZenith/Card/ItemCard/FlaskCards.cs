using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Card;

public class FlaskOfCeruleanTearsCard : ItemCard
{
    public readonly float FocusBoost;
    
    public FlaskOfCeruleanTearsCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        FocusBoost = 30;
        _frontTexture = DataManager.GetInstance(_game).CardFlaskCerulean;
        _soundOnPlay = DataManager.GetInstance(_game).FlaskOfCeruleanTears.CreateInstance();
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
        _owner.Focus = _owner.Focus + FocusBoost + Buff > 30f ? 30f 
            : _owner.Focus + FocusBoost + Buff;
    }
}

public class FlaskOfCrimsonTearsCard : ItemCard
{
    public readonly float HealthBoost;
    
    public FlaskOfCrimsonTearsCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        HealthBoost = 75;
        _frontTexture = DataManager.GetInstance(_game).CardFlaskCrimson;
        _soundOnPlay = DataManager.GetInstance(_game).FlaskOfCrimsonTears.CreateInstance();
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
        _owner.Health = _owner.Health + HealthBoost + Buff > 100f ? 100f 
            : _owner.Health + HealthBoost + Buff;
    }
}