#nullable enable
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Support;

namespace MonoZenith.Components;

public class TransitionComponent : Component
{
    private bool _isFadingOut;
    private readonly Action? _actionAfterFadeOut;
    
    /// <summary>
    /// Transition component properties
    /// </summary>
    private string _content;
    private Color _color;
    private readonly SpriteFont _font;
    private float _alpha;

    /// <summary>
    /// Timers for each stage of the transition.
    /// </summary>
    private readonly Timer _fadeInTimer;
    private readonly Timer _displayTimer;
    private readonly Timer _fadeOutTimer;

    public TransitionComponent(
        Game g,
        string content,
        Color color,
        SpriteFont font,
        float fadeInDuration = 0.5f,
        float displayDuration = 1f,
        float fadeOutDuration = 0.5f,
        Action? actionAfterFadeOut = null) : base(Vector2.Zero, 0, 0)
    {
        _isFadingOut = false;
        _actionAfterFadeOut = actionAfterFadeOut;
        
        _content = content;
        _color = color;
        _font = font;
        _alpha = 0;
        
        _fadeInTimer = new Timer(fadeInDuration);
        _displayTimer = new Timer(displayDuration);
        _fadeOutTimer = new Timer(fadeOutDuration);
        
        UpdatePosition();
    }

    /// <summary>
    /// The content of the transition component.
    /// </summary>
    public string Content
    {
        set
        {
            _content = value;
            UpdateDimensions();
        }
    }

    /// <summary>
    /// The color of the transition component.
    /// </summary>
    public Color Color
    {
        set => _color = value;
    }

    /// <summary>
    /// Update the position of the transition component.
    /// </summary>
    private void UpdatePosition()
    {
        Position = new Vector2(
            Game.ScreenWidth / 2f - Width / 2f,
            Game.ScreenHeight / 2f - Height / 2f);
    }

    /// <summary>
    /// Update the dimensions of the transition component.
    /// </summary>
    private void UpdateDimensions()
    {
        Width = (int)_font.MeasureString(_content).X;
        Height = (int)_font.MeasureString(_content).Y;
        UpdatePosition();
    }

    public override void Update(GameTime deltaTime)
    {
        UpdateDimensions();

        if (!_isFadingOut)
        {
            // Fade-in stage
            if (_fadeInTimer.IsActive())
            {
                _alpha = MathHelper.Clamp(_fadeInTimer.GetProgress(), 0f, 1f);
                _fadeInTimer.Update(deltaTime);
            }
            else if (_displayTimer.IsActive())
            {
                // Fully visible stage
                _alpha = 1f;
                _displayTimer.Update(deltaTime);
            }
            else
            {
                // Start fading out
                _isFadingOut = true;
                _fadeOutTimer.ResetTimer();
            }
        }
        else
        {
            // Fade-out stage
            if (_fadeOutTimer.IsActive())
            {
                _alpha = MathHelper.Clamp(1f - _fadeOutTimer.GetProgress(), 0f, 1f);
                _fadeOutTimer.Update(deltaTime);
            }
            else
            {
                // Callback after fade-out stage is complete
                _actionAfterFadeOut?.Invoke();
            }
        }
    }

    public override void Draw()
    {
        if (_alpha < 0.1f)
        {
            return;
        }

        // Draw border (transparent black)
        Game.DrawRectangle(
            new Color(Color.Black, 0.2f * _alpha),
            Position - new Vector2(25, 0),
            Width + 50,
            Height);

        // Draw line (transparent gray)
        Game.DrawRectangle(
            new Color(Color.LightGray, 0.3f * _alpha),
            Position - new Vector2(25, -Height),
            Width + 50,
            2);

        // Draw text shadow
        Game.DrawText(
            _content,
            Position + new Vector2(2, 2),
            _font,
            new Color(Color.DarkGray, 0.7f * _alpha));

        // Draw the actual transition text
        Game.DrawText(
            _content,
            Position,
            _font,
            new Color(_color, _alpha));
    }

    /// <summary>
    /// Reset the transition component.
    /// </summary>
    public void Reset()
    {
        _alpha = 0;
        _isFadingOut = false;
        _fadeInTimer.ResetTimer();
        _displayTimer.ResetTimer();
        _fadeOutTimer.ResetTimer();
    }
}