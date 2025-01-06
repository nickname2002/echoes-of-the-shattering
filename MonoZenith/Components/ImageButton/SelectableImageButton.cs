using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Components;

public class SelectableImageButton : ImageButton
{
    private readonly Texture2D _selectedTexture;
    
    public bool Selected { get; set; }
    
    public SelectableImageButton(
        Vector2 pos, 
        Texture2D defaultTexture, Texture2D selectedTexture, 
        Action onClickAction = null, 
        float scale = 1) : 
        base(pos, defaultTexture, onClickAction, scale)
    {
        Selected = false;
        _selectedTexture = selectedTexture;
    }
    
    protected override void RecalculateSize()
    {
        var texture = Selected ? _selectedTexture : _defaultTexture;
        Width = (int)(texture.Width * _scale);
        Height = (int)(texture.Height * _scale);
    }

    public override void Draw()
    {
        var texture = Selected ? _selectedTexture : _defaultTexture;
        Game.DrawImage(texture, Position, _scale, alpha: IsHovered() ? 0.5f : 1f);
    }
}