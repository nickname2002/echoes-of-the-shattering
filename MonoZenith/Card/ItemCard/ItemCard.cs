using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Players;

namespace MonoZenith.Card;

public class ItemCard : Card
{
    protected Player _owner;
    
    protected ItemCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name, Player owner) : 
        base(game, state, position, texture, activeTexture, name, owner)
    {
        _owner = owner;
    }

    public override void PerformEffect()
    {
        _soundOnPlay.Play();
    }
}