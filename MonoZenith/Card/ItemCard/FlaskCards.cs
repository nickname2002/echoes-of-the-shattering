using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Card;

public class FlaskOfCeruleanTearsCard : ItemCard
{
    private readonly float _manaBoost;
    
    public FlaskOfCeruleanTearsCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _manaBoost = 75;
        _soundOnPlay = DataManager.GetInstance(_game).FlaskOfCeruleanTears;
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        BoostPlayerMana();
    }

    /// <summary>
    /// Boos the mana of the owner.
    /// </summary>
    private void BoostPlayerMana()
    {
        _owner.Mana += _manaBoost;
    }
}

public class FlaskOfCrimsonTearsCard : ItemCard
{
    private readonly float _healthBoost;
    
    public FlaskOfCrimsonTearsCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _healthBoost = 75;
        _soundOnPlay = DataManager.GetInstance(_game).FlaskOfCrimsonTears;
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
        _owner.Health += _healthBoost;
    }
}