using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Card;

public class ItemCard : Card
{   
    protected ItemCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _owner = owner;
    }
    
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
    }

    public override bool IsAffordable() => true;

    protected override void DrawMetaData()
    {
        return;
    }
}