using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Support;
using static MonoZenith.Game;

namespace MonoZenith.Components.OverworldScreen;

public class RegionSelectButton
{
    private const float ScaleFactor = 0.2f;

    public bool Selected { get; set; } = false;
    
    /// <summary>
    /// Textures
    /// </summary>
    public Texture2D SelectedTexture { get; set; }
    public Texture2D HoveredTexture { get; set; }
    public Texture2D InactiveTexture { get; set; }

    /// <summary>
    /// Position & Dimensions
    /// </summary>
    public Vector2 Position { get; set; }
    public Vector2 Dimensions { get; set; }
    
    /// <summary>
    /// Region associated with the grace.
    /// </summary>
    public Region Region { get; set; }

    public bool IsHovered()
    {
        var mousePosition = GetMousePosition();
        return mousePosition.X >= Position.X &&
               mousePosition.X <= Position.X + Dimensions.X &&
               mousePosition.Y >= Position.Y &&
               mousePosition.Y <= Position.Y + Dimensions.Y; 
    }
    
    public bool IsClicked()
    {
        return IsHovered() && GetMouseButtonDown(MouseButtons.Left);
    }

    public void Update(Vector2 position)
    {
        Position = position;
        Dimensions = new Vector2(
            SelectedTexture.Width * AppSettings.Scaling.ScaleFactor * ScaleFactor, 
            SelectedTexture.Height * AppSettings.Scaling.ScaleFactor * ScaleFactor);
    }
    
    public void Draw()
    {
        bool regionActive = Screen.OverworldScreen.LevelManager.RegionActive(Region);
        
        if (IsHovered() && regionActive)
        {
            DrawImage(HoveredTexture, Position, AppSettings.Scaling.ScaleFactor * ScaleFactor);
            return;
        }
        
        if (Selected && regionActive)
        {
            DrawImage(SelectedTexture, Position, AppSettings.Scaling.ScaleFactor * ScaleFactor);
            return;
        }
        
        float opacity = regionActive ? 1.0f : 0.5f;
        DrawImage(
            InactiveTexture, 
            Position, 
            AppSettings.Scaling.ScaleFactor * ScaleFactor,
            alpha: opacity);
    }
}