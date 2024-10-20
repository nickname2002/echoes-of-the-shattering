using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Components.Indicator;

public abstract class Indicator
{
    protected Game _game;
    protected GameState _gameState;
    protected Texture2D _texture;
    protected Vector2 _position;
    
    protected Indicator(Game g, GameState gs, Vector2 pos)
    {
        _game = g;
        _gameState = gs;
        _position = pos;
    }

    /// <summary>
    /// Calculate a scale factor that will make the indicator exactly 20 pixels wide.
    /// </summary>
    /// <returns>The scale factor.</returns>
    protected float GetScale()
    {
        const float satisfiedWidth = 75;
        return satisfiedWidth / _texture.Width;
    }
    
    /// <summary>
    /// Update the indicator
    /// </summary>
    /// <param name="deltaTime">The time since the last update.</param>
    public abstract void Update(GameTime deltaTime);

    /// <summary>
    /// Draw the indicator
    /// </summary>
    public virtual void Draw()
    {
        _game.DrawImage(_texture, _position, GetScale());
    }
}