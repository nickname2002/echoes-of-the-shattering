using System;
using Microsoft.Xna.Framework;

namespace MonoZenith.Engine.Support;

public class FadeEffectManager
{
    private float _alpha;
    private readonly float _fadeSpeed;
    private bool _isFadingIn;
    private bool _isFadingOut;
    private Action _onFadeInComplete;
    private Action _onFadeOutComplete;

    public FadeEffectManager(float initialAlpha = 1.0f, float fadeSpeed = 0.01f)
    {
        _alpha = initialAlpha;
        _fadeSpeed = fadeSpeed;
        _isFadingIn = false;
        _isFadingOut = false;
    }

    public void StartFadeIn(Action onFadeInComplete = null)
    {
        _isFadingIn = true;
        _isFadingOut = false;
        _onFadeInComplete = onFadeInComplete;
    }

    public void StartFadeOut(Action onFadeOutComplete = null)
    {
        _isFadingIn = false;
        _isFadingOut = true;
        _onFadeOutComplete = onFadeOutComplete;
    }

    public void Update()
    {
        if (_isFadingIn)
        {
            _alpha -= _fadeSpeed;
            
            if (!(_alpha <= 0)) 
                return;
            
            _alpha = 0;
            _isFadingIn = false;
            _onFadeInComplete?.Invoke();
        }
        else if (_isFadingOut)
        {
            _alpha += _fadeSpeed;
            
            if (!(_alpha >= 1)) 
                return;
            
            _alpha = 1;
            _isFadingOut = false;
            _onFadeOutComplete?.Invoke();
        }
    }

    public void DrawFadeEffect(GameFacade game, Color color)
    {
        // Draw a fullscreen rectangle with the current alpha
        color *= _alpha;
        game.DrawRectangle(color, new Vector2(0, 0), game.ScreenWidth, game.ScreenHeight);
    }

    public bool IsFading => _isFadingIn || _isFadingOut;
}