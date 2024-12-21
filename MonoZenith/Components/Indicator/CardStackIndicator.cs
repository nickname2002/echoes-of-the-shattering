using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Components.Indicator;

public class CardStackIndicator : Indicator
{
    public CardStack _container;
    private readonly SpriteFont _font;
    private string _countToDisplay;
    
    public CardStackIndicator(GameState gs, Vector2 pos, Texture2D texture, CardStack cs) : 
        base(gs, pos, texture)
    {
        _container = cs;
        _font = DataManager.GetInstance().IndicatorFont;
        _countToDisplay = _container.Count.ToString();
    }

    public override void Update(GameTime deltaTime)
    {
        _countToDisplay = _container.Count.ToString();
    }

    public override void Draw()
    {
        base.Draw();
        DrawText(
            _countToDisplay, 
            _position + new Vector2(
                _texture.Width * GetScale() - 27.5f * AppSettings.Scaling.ScaleFactor, 
                _texture.Height * GetScale() - 35 * AppSettings.Scaling.ScaleFactor), 
            _font, 
            Color.White);
    }
}