using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Card;

public class FlaskOfCeruleanTearsCard : ItemCard
{
    private readonly float _manaBoost;
    
    public FlaskOfCeruleanTearsCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name, Player owner) : 
        base(game, state, position, texture, activeTexture, name, owner)
    {
        _manaBoost = 20;
        _soundOnPlay = DataManager.GetInstance(_game).FlaskOfCeruleanTears;
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
        _owner.Focus += _manaBoost;
    }
}

public class FlaskOfCrimsonTearsCard : ItemCard
{
    private readonly float _healthBoost;
    
    public FlaskOfCrimsonTearsCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name, Player owner) : 
        base(game, state, position, texture, activeTexture, name, owner)
    {
        _healthBoost = 20;
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