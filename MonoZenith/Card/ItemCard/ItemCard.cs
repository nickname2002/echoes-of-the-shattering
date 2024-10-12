using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Card;

public class ItemCard : Card
{
    public ItemCard(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name) : 
        base(game, state, position, texture, activeTexture, name)
    {
    }

    public override void PerformEffect()
    {
        throw new System.NotImplementedException();
    }

    protected override void DrawMetaData()
    {
        throw new System.NotImplementedException();
    }
}