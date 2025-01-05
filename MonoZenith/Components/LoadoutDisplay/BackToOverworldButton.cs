using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Components.LoadoutDisplay;

public class BackToOverworldButton
{
    private static readonly Texture2D BackToOverworldTexture = 
        LoadImage("Images/LoadoutDisplay/Buttons/to-overworld.png");
    private readonly SoundEffectInstance _backToOverworldSoundEffect = 
        DataManager.GetInstance().EndPlayerTurnSound.CreateInstance();

    /// <summary>
    /// The dimensions of the grace.
    /// </summary>
    private Vector2 _dimensions;

    /// <summary>
    /// The position of the grace on the screen.
    /// </summary>
    private Vector2 _position;

    public bool IsHovered()
    {
        var mousePosition = GetMousePosition();
        return mousePosition.X >= _position.X &&
               mousePosition.X <= _position.X + _dimensions.X &&
               mousePosition.Y >= _position.Y &&
               mousePosition.Y <= _position.Y + _dimensions.Y;
    }

    public bool IsClicked() => IsHovered() && GetMouseButtonDown(MouseButtons.Left);
    
    private void UpdatePositionAndDimensions()
    {
        _dimensions = new Vector2(
            BackToOverworldTexture.Width * AppSettings.Scaling.ScaleFactor * 0.15f, 
            BackToOverworldTexture.Height * AppSettings.Scaling.ScaleFactor * 0.15f);
        _position = new Vector2(30 * AppSettings.Scaling.ScaleFactor, 30 * AppSettings.Scaling.ScaleFactor);
    }
    
    public void Update()
    {
        Console.WriteLine(GetMouseButtonDown(MouseButtons.Left));
        UpdatePositionAndDimensions();
        if (!IsClicked()) return;
        _backToOverworldSoundEffect.Play();
        ShowLoadoutDisplay(false);
    }
    
    public void Draw()
    {
        var alpha = IsHovered() ? 1f : 0.75f;
        DrawImage(
            BackToOverworldTexture, 
            _position, 
            AppSettings.Scaling.ScaleFactor * 0.15f, 
            alpha: alpha);
    }
}