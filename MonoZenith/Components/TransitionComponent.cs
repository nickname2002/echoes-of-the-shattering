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
    /// Original duration of each stage of the transition.
    /// </summary>
    private readonly float _originalFadeInTimerDuration;
    private readonly float _originalDisplayTimerDuration;
    private readonly float _originalFadeOutTimerDuration;
    
    /// <summary>
    /// Timers for each stage of the transition.
    /// </summary>
    private Timer _fadeInTimer;
    private Timer _displayTimer;
    private Timer _fadeOutTimer;

    public TransitionComponent(string content,
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
        
        _originalFadeInTimerDuration = fadeInDuration;
        _originalDisplayTimerDuration = displayDuration;
        _originalFadeOutTimerDuration = fadeOutDuration;
        
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
                _fadeOutTimer = new Timer(_originalFadeOutTimerDuration);
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
    /// Temporarily set the duration of each stage of the transition.
    /// This is only of effect for the next transition. The duration will be
    /// reset to the original values after the transition is complete.
    /// </summary>
    /// <param name="fadeInDuration">Duration of the fade-in stage.</param>
    /// <param name="displayDuration">Duration of the fully visible stage.</param>
    /// <param name="fadeOutDuration">Duration of the fade-out stage.</param>
    public void SetTempTransitionTimers(float fadeInDuration, float displayDuration, float fadeOutDuration)
    {
        _fadeInTimer = new Timer(fadeInDuration);
        _displayTimer = new Timer(displayDuration);
        _fadeOutTimer = new Timer(fadeOutDuration);
    }
    
    /// <summary>
    /// Reset the transition component.
    /// </summary>
    public void Reset()
    {
        _alpha = 0;
        _isFadingOut = false;
        _fadeInTimer = new Timer(_originalFadeInTimerDuration);
        _displayTimer = new Timer(_originalDisplayTimerDuration);
        _fadeOutTimer = new Timer(_originalFadeOutTimerDuration);
    }
}