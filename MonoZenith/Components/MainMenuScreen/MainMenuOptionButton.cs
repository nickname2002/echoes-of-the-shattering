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
    
    public MainMenuOptionButton(Game g, float y, string content, Action a, SoundEffectInstance activationSound = null) : 
        base(g, Vector2.Zero, 0, 0, content, 1, Color.White, Color.Black, 0, Color.Black)
    {
        SetOnClickAction(a);
        _font = DataManager.GetInstance().StartMenuFont;
        _hoverIndicator = DataManager.GetInstance().MenuHoverIndicator;
        _hoverIndicatorScale = 0.75f * AppSettings.Scaling.ScaleFactor;
        Width = (int)_font.MeasureString(Content).X;
        Height = (int)_font.MeasureString(Content).Y;

        // Apply scaling factor to Y position
        Position = new Vector2(Game.ScreenWidth / 2f - Width / 2f, y); 
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
    
    public void ChangePosition(Vector2 position)
    {
        Position = position;
    }
    
    public override void Draw()
    {
        if (IsHovered())
        {
            DrawHoverIndicator();
        }

        DrawBorderContent();
    }

    private void DrawHoverIndicator()
    {
        Game.DrawImage(
            _hoverIndicator,
            new Vector2(
                Game.ScreenWidth / 2f - _hoverIndicator.Width * _hoverIndicatorScale / 2f,
                Position.Y - Height * _hoverIndicatorScale / 2 - 5 * AppSettings.Scaling.ScaleFactor),
            _hoverIndicatorScale);
    }
}