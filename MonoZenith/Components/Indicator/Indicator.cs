using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Components.Indicator;

public abstract class Indicator : Component
{
    protected GameState _gameState;
    protected Texture2D _texture;
    protected Vector2 _position;
    
    protected Indicator(GameState gs, Vector2 pos, Texture2D texture) : 
        base(pos, 0, 0)
    {
        _gameState = gs;
        _position = pos;
        _texture = texture;
    } 

    /// <summary>
    /// Calculate a scale factor that will make the indicator exactly 20 pixels wide.
    /// </summary>
    /// <returns>The scale factor.</returns>
    protected float GetScale()
    {
        float satisfiedWidth = 75 * AppSettings.Scaling.ScaleFactor;
        return satisfiedWidth / _texture.Width;
    }

    /// <summary>
    /// Update the indicator
    /// </summary>
    /// <param name="deltaTime">The time since the last update.</param>
    public override void Update(GameTime deltaTime)
    {
        Width = (int)(_texture.Width * GetScale());
        Height = (int)(_texture.Height * GetScale());
    }

    /// <summary>
    /// Draw the indicator
    /// </summary>
    public override void Draw()
    {
        DrawImage(_texture, _position, GetScale());
    }
}