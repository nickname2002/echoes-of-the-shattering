using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;

namespace MonoZenith.Components;

public abstract class TransitionComponent : Component
{
    protected string _content;
    protected Color _color;
    protected SpriteFont _font;
    protected float _alpha;
    protected bool _isFadingOut;

    protected Timer _fadeInTimer;
    protected Timer _displayTimer;
    protected Timer _fadeOutTimer;
    
    protected float _fadeInDuration = 0.5f; 
    protected float _displayDuration = 1f; 
    protected float _fadeOutDuration = 0.5f; 

    protected TransitionComponent(Game g, string content, Color c) : base(g, Vector2.Zero, 0, 0)
    {
        _content = content;
        _color = c;
        _font = DataManager.GetInstance(Game).TransitionComponentFont;

        Width = (int)_font.MeasureString(_content).X;
        Height = (int)_font.MeasureString(_content).Y;

        _alpha = 0;
        _isFadingOut = false;

        _fadeInTimer = new Timer(_fadeInDuration);
        _displayTimer = new Timer(_displayDuration);
        _fadeOutTimer = new Timer(_fadeOutDuration);
    }

    /// <summary>
    /// Express the transition component.
    /// </summary>
    /// <param name="deltaTime">The time since the last update.</param>
    protected abstract void Express(GameTime deltaTime);
    
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
    
    public override void Update(GameTime deltaTime)
    {
        // Update dimensions
        Width = (int)_font.MeasureString(_content).X;
        Height = (int)_font.MeasureString(_content).Y;
        
        // Express the transition component
        Express(deltaTime); 
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

        // Draw the actual transition name text
        Game.DrawText(
            _content,
            Position,
            _font,
            new Color(_color, _alpha));
    }   
}