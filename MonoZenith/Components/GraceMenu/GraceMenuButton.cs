using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Components;

public class GraceMenuButton : Button
{
    private Texture2D _texture;
    private Texture2D _hoverTexture;
    
    public GraceMenuButton(
        Game g, Vector2 pos, Texture2D texture, Texture2D hoverTexture) : 
        base(g, pos, 103, 112, "", 1, Color.Black, Color.Black, 0, Color.Black)
    {
        _texture = texture;
        _hoverTexture = hoverTexture;
    }

    public override void Draw()
    {
        if (IsHovered())
        {
            Game.DrawImage(_hoverTexture, Position);
            return;
        }
        
        Game.DrawImage(_texture, Position);
    }
}