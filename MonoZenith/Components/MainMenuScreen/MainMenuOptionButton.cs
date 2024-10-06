using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;

namespace MonoZenith.Components.MainMenuScreen;

public class MainMenuOptionButton : Button
{
    private readonly Texture2D _hoverIndicator;
    private readonly float _hoverIndicatorScale;
    private readonly SoundEffectInstance _activationSound;
    
    public MainMenuOptionButton(Game g, int y, string content, Action a, SoundEffectInstance activationSound = null) : 
        base(g, Vector2.Zero, 0, 0, content, 1, Color.White, Color.Black, 0, Color.Black)
    {
        SetOnClickAction(a);
        _font = DataManager.GetInstance(Game).StartMenuFont;
        _hoverIndicator = DataManager.GetInstance(Game).MainMenuHoverIndicator;
        _hoverIndicatorScale = 0.3f;
        Width = (int)_font.MeasureString(Content).X;
        Height = (int)_font.MeasureString(Content).Y;
        Position = new Vector2(Game.ScreenWidth / 2 - Width / 2, y);
        _buttonColor = Color.Black;
        _activationSound = activationSound;
    }

    public sealed override void SetOnClickAction(Action a)
    {
        _callbackMethod = CallBackMethod;
        return;
        
        void CallBackMethod()
        {
            _activationSound?.Play();
        
            // Start action after the sound is played
            if (_activationSound == null)
            {
                return;
            }
        
            a.Invoke();  
        }
    }
    
    /// <summary>
    /// Draw the indicator specifying the button is hovered.
    /// </summary>
    private void DrawHoverIndicator()
    {
        Game.DrawImage(
            _hoverIndicator,
            new Vector2(
                Game.ScreenWidth / 2 - _hoverIndicator.Width / 2 * _hoverIndicatorScale,
                Position.Y - Height * _hoverIndicatorScale / 2 - _hoverIndicatorScale * 40),
            _hoverIndicatorScale);
    }
    
    public override void Draw()
    {
        if (IsHovered())
        {
            DrawHoverIndicator();
        }

        DrawBorderContent();
    }
}