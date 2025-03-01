
using Microsoft.Xna.Framework;

namespace MonoZenith.Components;

public abstract class Component
{
    protected Vector2 Position;
    protected int Width;
    protected int Height;
    
    protected Component(Vector2 pos, int width, int height)
    {
        this.Position = pos;
        this.Width = width;
        this.Height = height;
    }
    
    public abstract void Update(GameTime deltaTime);
    public abstract void Draw();
}