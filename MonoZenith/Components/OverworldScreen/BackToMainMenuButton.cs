using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Support;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Components.OverworldScreen;

public class BackToMainMenuButton
{
    private static readonly Texture2D BackToMainMenuTexture = DataManager.GetInstance().BackToMainMenuButton;
    private SoundEffectInstance _backToMainMenuSoundEffect = DataManager.GetInstance().StartButtonSound.CreateInstance();

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
            BackToMainMenuTexture.Width * AppSettings.Scaling.ScaleFactor * 0.15f, 
            BackToMainMenuTexture.Height * AppSettings.Scaling.ScaleFactor * 0.15f);
        _position = new Vector2(30 * AppSettings.Scaling.ScaleFactor, 30 * AppSettings.Scaling.ScaleFactor);
    }
    
    public void Update()
    {
        UpdatePositionAndDimensions();
        if (!IsClicked()) return;
        _backToMainMenuSoundEffect.Play();
        BackToMainMenu();
    }
    
    public void Draw()
    {
        var alpha = IsHovered() ? 1f : 0.75f;
        DrawImage(
            BackToMainMenuTexture, 
            _position, 
            AppSettings.Scaling.ScaleFactor * 0.15f, 
            alpha: alpha);
    }
}