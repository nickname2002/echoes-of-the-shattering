using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Items;

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
    
    public SpiritAshIndicator(Game g, GameState gs, Vector2 pos, Texture2D tex, SpiritAsh ash, bool hasHumanOwner = true)
        : base(g, gs, pos, tex)
    {   
        _spiritAsh = ash;
        _soundOnClick = DataManager.GetInstance(g).SpiritAshSummonSound.CreateInstance();
        IsActive = true;
        _hasHumanOwner = hasHumanOwner;
        
        TextureEnabled = DataManager.GetInstance(g).AshIndicatorEnabled;
        TextureDisabled = DataManager.GetInstance(g).AshIndicatorDisabled;
        TextureHovered = DataManager.GetInstance(g).AshIndicatorHovered;
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
        return _game.GetMousePosition().X > Position.X && _game.GetMousePosition().X < Position.X + Width && 
               _game.GetMousePosition().Y > Position.Y && _game.GetMousePosition().Y < Position.Y + Height;
    }
    
    /// <summary>
    /// Check if the left mouse button is currently pressed and the mouse is hovering over the item.
    /// </summary>
    /// <returns>True if the left mouse button is pressed and the mouse is hovering over the item, false otherwise.</returns>
    public bool IsClicked()
    {
        return IsHovered() && _game.GetMouseButtonDown(MouseButtons.Left);
    }
    
    public override void Draw()
    {
        Texture2D textureToDraw = IsActive
            ? (IsHovered() && _hasHumanOwner ? TextureHovered : TextureEnabled)
            : TextureDisabled;

        float spiritAshScale = IsActive
            ? (IsHovered() && _hasHumanOwner ? 1.0f : 0.7f)
            : 0.25f;

        _game.DrawImage(textureToDraw, _position, GetScale());
        _spiritAsh.Draw(_position, spiritAshScale);
    }
}