using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;

namespace MonoZenith.Components;

public class Button : Component
{
    // Members
    protected readonly float _contentScale;
    
    protected Color _buttonColor;
    protected readonly Color _originalButtonColor;
    protected readonly Color _buttonHoverColor;

    protected SpriteFont _font;
    protected Action _callbackMethod;
    
    private readonly float _buttonDelay;
    private float _currentButtonDelay;
    
    // Properties
    public string Content;
    public Color ContentColor { get; }
    public Color ButtonColor => _buttonColor;
    public int BorderWidth { get; }
    public Color BorderColor { get; }

    public Button(
        Game g,
        Vector2 pos,
        int width, int height,
        string content, int contentScale, Color contentColor, Color buttonColor,
        int borderWidth, Color borderColor) :
        base(pos, width, height)
    {
        // Content 
        Content = content;
        ContentColor = contentColor;
        _contentScale = contentScale;
        
        // Button properties 
        _buttonColor = buttonColor;
        _originalButtonColor = _buttonColor;
        _buttonHoverColor = new Color(buttonColor.R + 50, buttonColor.G + 50, buttonColor.B + 50);
        _font = DataManager.GetInstance().ComponentFont;
        _callbackMethod = () => Game.DebugLog("");
        
        // Border properties 
        BorderWidth = borderWidth;
        BorderColor = borderColor;
        
        // Timers
        _buttonDelay = 150f;
        _currentButtonDelay = 0;
    }
    
    public virtual void SetOnClickAction(Action a)
    {
        _callbackMethod = a;
    }

    protected bool IsHovered()
    {
        Point mousePos = Game.GetMousePosition();

        // In X range
        if (mousePos.X > Position.X && mousePos.X < Position.X + Width)
        {
            // In Y range
            if (mousePos.Y > Position.Y && mousePos.Y < Position.Y + Height)
            {
                return true;
            }
        }

        return false;
    }
    
    public bool IsClicked()
    {
        return IsHovered() && Game.GetMouseButtonDown(MouseButtons.Left);
    }
    
    /* Manage button delay */
    private void UpdateTimers(GameTime deltaTime)
    {
        if (_currentButtonDelay <= 0)
        {
            _currentButtonDelay = 0;
            return;
        }
        
        _currentButtonDelay -= deltaTime.ElapsedGameTime.Milliseconds;
    }
    
    private bool ClickAllowed()
    {
        return _currentButtonDelay <= 0;
    }
    
    public override void Update(GameTime deltaTime)
    {
        UpdateTimers(deltaTime);
        _buttonColor = IsHovered() ? _buttonHoverColor : _originalButtonColor;
        
        // Decrease sensitivity of button
        if (IsClicked() && ClickAllowed())
        {
            _currentButtonDelay = _buttonDelay;
            _callbackMethod();
        }
    }

    private void DrawBorder()
    {
        float borderX = Position.X;
        float borderY = Position.Y;
        Vector2 borderPos = new Vector2(borderX, borderY);
        float borderRectWidth = Width + 2 * BorderWidth;
        float borderRectHeight = Height + 2 * BorderWidth;
        Game.DrawRectangle(BorderColor, borderPos, (int)borderRectWidth, (int)borderRectHeight);
    }
    
    protected void DrawBorderContent()
    {
        Game.DrawText(Content, Position, _font, ContentColor, _contentScale);
    }
    
    public override void Draw()
    {
        DrawBorder();
        Game.DrawRectangle(ButtonColor, Position, Width, Height);
        DrawBorderContent();
    }
}