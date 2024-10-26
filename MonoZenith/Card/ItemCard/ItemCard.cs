using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Players;

namespace MonoZenith.Card;

public class ItemCard : Card
{   
    protected ItemCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _owner = owner;
    }

    public override bool IsAffordable() => true;

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
    }

    protected override bool IsPlayable() => true;
}