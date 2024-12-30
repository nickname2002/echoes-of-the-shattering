using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Components.OverworldScreen;

public class SiteOfGraceButton
{
    /// <summary>
    /// Textures
    /// </summary>
    private static readonly Texture2D GraceTexture = DataManager.GetInstance().SiteOfGraceButton;
    private readonly Texture2D _graceHoverOutlineTexture = DataManager.GetInstance().SiteOfGraceButtonHover;

    /// <summary>
    /// The dimensions of the grace.
    /// </summary>
    private readonly Vector2 _dimensions = new(GraceTexture.Width, GraceTexture.Height);
    public Vector2 Dimensions =>
        new(
            _dimensions.X * AppSettings.Scaling.ScaleFactor * 0.4f,
            _dimensions.Y * AppSettings.Scaling.ScaleFactor * 0.4f
        );

    /// <summary>
    /// The position of the grace on the screen.
    /// </summary>
    private Vector2 _position;
    public Vector2 Position
    {
        get =>
            new(
                _position.X * AppSettings.Scaling.ScaleFactor,
                _position.Y * AppSettings.Scaling.ScaleFactor
            );
        set => _position = value;
    }
    
    /// <summary>
    /// Level associated with the grace.
    /// </summary>
    public Level Level { get; set; }

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
        return IsHovered();
    }

    public void Draw()
    {
        // Draw outline when hovered
        if (IsHovered())
        {
            DrawImage(
                _graceHoverOutlineTexture,
                Position,
                AppSettings.Scaling.ScaleFactor * 0.7f);
        }
        
        // Draw grace image
        DrawImage(
            GraceTexture,
            new Vector2(
                Position.X + 2 * AppSettings.Scaling.ScaleFactor,
                Position.Y + 1 * AppSettings.Scaling.ScaleFactor),
            AppSettings.Scaling.ScaleFactor * 0.4f);
        
        // Draw name
        try
        {
            DrawText(
                Level.Enemy.Name,
                Position + new Vector2(
                    GraceTexture.Width * AppSettings.Scaling.ScaleFactor * 0.4f / 2 - 
                    DataManager.GetInstance().CardFont.MeasureString(Level.Enemy.Name).X / 2,
                    GraceTexture.Height * AppSettings.Scaling.ScaleFactor * 0.4f + 5 * AppSettings.Scaling.ScaleFactor),
                DataManager.GetInstance().CardFont,
                Color.White,
                AppSettings.Scaling.ScaleFactor);   
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}