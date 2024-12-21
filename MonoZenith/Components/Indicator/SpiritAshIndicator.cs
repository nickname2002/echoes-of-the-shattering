using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using static MonoZenith.Game;

namespace MonoZenith.Components.Indicator;

public class SpiritAshIndicator : Indicator
{
    private readonly SpiritAsh _spiritAsh;
    private readonly SoundEffectInstance _soundOnClick;
    private readonly bool _hasHumanOwner;
    public bool IsActive { get; set; }
    
    public Texture2D TextureEnabled { get; set; }
    public Texture2D TextureDisabled { get; set; }
    public Texture2D TextureHovered { get; set; }
    
    public SpiritAshIndicator(GameState gs, Vector2 pos, Texture2D tex, SpiritAsh ash, bool hasHumanOwner = true)
        : base(gs, pos, tex)
    {   
        _spiritAsh = ash;
        _soundOnClick = DataManager.GetInstance().SpiritAshSummonSound.CreateInstance();
        IsActive = true;
        _hasHumanOwner = hasHumanOwner;
        
        TextureEnabled = DataManager.GetInstance().AshIndicatorEnabled;
        TextureDisabled = DataManager.GetInstance().AshIndicatorDisabled;
        TextureHovered = DataManager.GetInstance().AshIndicatorHovered;
    }

    public override void Update(GameTime deltaTime)
    {
        base.Update(deltaTime);
        if (!IsClicked() || !IsActive) return;
        InvokeClickEvent(deltaTime);
    }

    /// <summary>
    /// Trigger the click event of the spirit ash.
    /// </summary>
    /// <param name="deltaTime">The delta time.</param>
    public void InvokeClickEvent(GameTime deltaTime)
    {
        if (!IsActive) return;
        _spiritAsh.Update(deltaTime);
        _soundOnClick.Play();
        IsActive = false;
    }
    
    /// <summary>
    /// Check if the mouse is currently hovering over the item.
    /// </summary>
    /// <returns>True if the mouse is hovering over the item, false otherwise.</returns>
    public bool IsHovered()
    {
        return GetMousePosition().X > Position.X && GetMousePosition().X < Position.X + Width && 
               GetMousePosition().Y > Position.Y && GetMousePosition().Y < Position.Y + Height;
    }
    
    /// <summary>
    /// Check if the left mouse button is currently pressed and the mouse is hovering over the item.
    /// </summary>
    /// <returns>True if the left mouse button is pressed and the mouse is hovering over the item, false otherwise.</returns>
    public bool IsClicked()
    {
        return IsHovered() && GetMouseButtonDown(MouseButtons.Left);
    }
    
    public override void Draw()
    {
        Texture2D textureToDraw = IsActive
            ? (IsHovered() && _hasHumanOwner ? TextureHovered : TextureEnabled)
            : TextureDisabled;

        float spiritAshScale = IsActive
            ? (IsHovered() && _hasHumanOwner ? 1.0f : 0.7f)
            : 0.25f;

        DrawImage(textureToDraw, _position, GetScale());
        _spiritAsh.Draw(_position, spiritAshScale);
    }
}