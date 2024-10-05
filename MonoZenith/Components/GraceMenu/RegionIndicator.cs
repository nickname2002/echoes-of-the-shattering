using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;

namespace MonoZenith.Components
{
    public class RegionIndicator : Component
    {
        private string _regionName;
        private SpriteFont _font;
        private float _alpha;
        private bool _isFadingOut;

        private Timer _fadeInTimer;
        private Timer _displayTimer;
        private Timer _fadeOutTimer;

        // Timing settings
        private float _fadeInDuration = 1f; // 2 seconds to fade in
        private float _displayDuration = 3f; // Display for 3 seconds
        private float _fadeOutDuration = 1f; // 2 seconds to fade out

        public RegionIndicator(Game g, string regionName) : base(g, Vector2.Zero, 0, 0)
        {
            _regionName = regionName;
            _font = DataManager.GetInstance(Game).RegionIndicatorFont;

            Width = (int)_font.MeasureString(_regionName).X;
            Height = (int)_font.MeasureString(_regionName).Y;
            Position = new Vector2(
                Game.ScreenWidth * 0.8f - Width / 2, 
                Game.ScreenHeight / 2 - Height / 2);

            _alpha = 0;
            _isFadingOut = false;

            _fadeInTimer = new Timer(_fadeInDuration);
            _displayTimer = new Timer(_displayDuration);
            _fadeOutTimer = new Timer(_fadeOutDuration);
        }

        public void Express(GameTime deltaTime)
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

        public override void Update(GameTime deltaTime)
        {
            Express(deltaTime); // Pass deltaTime here
        }

        public override void Draw()
        {
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
                _regionName,
                Position + new Vector2(2, 2),
                _font,
                new Color(Color.DarkGray, 0.7f * _alpha));

            // Draw the actual region name text
            Game.DrawText(
                _regionName,
                Position,
                _font,
                new Color(Color.White, _alpha));
        }
    }
}