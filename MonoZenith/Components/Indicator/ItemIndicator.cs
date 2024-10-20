using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Components.Indicator;

public class ItemIndicator : Indicator
{
    // TODO: Add sounds later
    
    public ItemIndicator(Game g, GameState gs, Vector2 pos, Texture2D tex) : base(g, gs, pos, tex)
    {   
        
    }

    public override void Update(GameTime deltaTime)
    {
        base.Update(deltaTime);
    }

    /// <summary>
    /// Check if the mouse is currently hovering over the item.
    /// </summary>
    /// <returns>True if the mouse is hovering over the item, false otherwise.</returns>
    public bool IsHovered()
    {
        return _game.GetMousePosition().X > Position.X && _game.GetMousePosition().X < Position.X + Width && 
               _game.GetMousePosition().Y > Position.Y && _game.GetMousePosition().Y < Position.Y + Height;
    }
    
    /// <summary>
    /// Check if the left mouse button is currently pressed and the mouse is hovering over the item.
    /// </summary>
    /// <returns>True if the left mouse button is pressed and the mouse is hovering over the item, false otherwise.</returns>
    public bool IsClicked()
    {
        return IsHovered() && _game.GetMouseButtonDown(MouseButtons.Left);
    }
}