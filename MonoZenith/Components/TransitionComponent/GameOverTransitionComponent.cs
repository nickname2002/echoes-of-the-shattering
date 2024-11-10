#nullable enable
using System;
using Microsoft.Xna.Framework;
using MonoZenith.Support;

namespace MonoZenith.Components;

public class GameOverTransitionComponent : TransitionComponent
{
    // TODO: Combine this with the TransitionComponent class
    
    private readonly Action? _actionAfterFadeOut;
    
    /// <summary>
    /// The content displayed by the transition component.
    /// </summary>
    public string Content
    {
        get => _content;
        set => _content = value;
    }

    /// <summary>
    /// The color of the content displayed by the transition component.
    /// </summary>
    public Color Color
    {
        get => _color;
        set => _color = value;
    }
    
    public GameOverTransitionComponent(
        Game g, 
        string content, 
        Color color,
        Action? callback=null) : base(g, content, color)
    {
        Position = new Vector2(
            Game.ScreenWidth / 2f - Width / 2f, 
            Game.ScreenHeight / 2f - Height / 2f);
        
        _fadeInDuration = 1f;
        _displayDuration = 4f;
        _actionAfterFadeOut = callback;
    }
    
    public override void Update(GameTime deltaTime)
    {
        Position = new Vector2(
            Game.ScreenWidth / 2f - Width / 2f, 
            Game.ScreenHeight / 2f - Height / 2f);
        
        base.Update(deltaTime);
        
        // Callback after fade-out stage is complete
        if (_isFadingOut && !_fadeOutTimer.IsActive())
            _actionAfterFadeOut?.Invoke();
    }
    
    protected override void Express(GameTime deltaTime)
    {
        if (!_isFadingOut)
        {
            // Fade-in stage
            if (_fadeInTimer.IsActive())
            {
                _alpha = MathHelper.Clamp(_fadeInTimer.GetProgress(), 0f, 1f); // Increment alpha over time
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
                _fadeOutTimer.ResetTimer(); // Reset the fade-out timer when fading starts
            }
        }
        else
        {
            // Fade-out stage
            if (_fadeOutTimer.IsActive())
            {
                _alpha = MathHelper.Clamp(1f - _fadeOutTimer.GetProgress(), 0f, 1f); // Decrease alpha over time
                _fadeOutTimer.Update(deltaTime);
            }
        }
    }
}