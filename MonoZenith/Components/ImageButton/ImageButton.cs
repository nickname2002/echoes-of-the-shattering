using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
// ReSharper disable InconsistentNaming

namespace MonoZenith.Components;

public class ImageButton : Button
{
    protected readonly Texture2D _defaultTexture;
    protected readonly float _scale;
    
    public ImageButton(
        Vector2 pos, 
        Texture2D defaultTexture, 
        Action onClickAction = null, 
        float scale=1,
        SoundEffectInstance soundOnClick = null) : 
        base(Game.Instance, pos, 
            0, 0, "", 1, 
            Color.Black, Color.Black, 0, Color.Black)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        SetOnClickAction(onClickAction);
        _defaultTexture = defaultTexture;
        _scale = scale;
    }
    
    protected virtual void RecalculateSize()
    {
        Width = (int)(_defaultTexture.Width * _scale);
        Height = (int)(_defaultTexture.Height * _scale);
    }
    
    public override void Update(GameTime deltaTime)
    {
        RecalculateSize();
        base.Update(deltaTime);
    }
    
    public override void Draw()
    {
        var texture = _defaultTexture;
        Game.DrawImage(texture, Position, _scale, alpha: IsHovered() ? 0.5f : 1f);
    }
}