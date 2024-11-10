using Microsoft.Xna.Framework;

namespace MonoZenith.Components;

public class TurnTransitionComponent : TransitionComponent
{
    public TurnTransitionComponent(Game g, string content, Color c) : base(g, content, c)
    {
        Position = new Vector2(
            Game.ScreenWidth  / 2f - Width / 2f, 
            Game.ScreenHeight / 2f - Height / 2f);
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