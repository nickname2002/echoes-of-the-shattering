using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Components;

public class EndTurnButton : Button
{
    private Texture2D activeIdleTexture;
    private Texture2D activeHoverTexture;
    private Texture2D disabledTexture;
    private Texture2D _currentTexture;
    private GameState _gameState;
    private float textureScale;
    
    public EndTurnButton(Game g, GameState gs) : 
        base(g, Vector2.Zero, 0, 0, "", 0, Color.Black, Color.Black, 0, Color.Black)
    {
        activeIdleTexture = DataManager.GetInstance(g).EndTurnButtonIdleTexture;
        activeHoverTexture = DataManager.GetInstance(g).EndTurnButtonHoverTexture;
        disabledTexture = DataManager.GetInstance(g).EndTurnButtonDisabledTexture;
        _currentTexture = activeIdleTexture;
        _gameState = gs;
        textureScale = 0.25f;
        UpdateDimensions();
        Position = new Vector2(
            Game.ScreenWidth - Width - 50, 
            Game.ScreenHeight / 2 - Height / 2);

        SetOnClickAction(() => 
        {
            if (_gameState.CurrentPlayer is HumanPlayer)
            {
                _gameState.SwitchTurn();
            }
        });
    }

    private void UpdateDimensions()
    {
        Width = (int)(activeIdleTexture.Width * textureScale);
        Height = (int)(activeIdleTexture.Height * textureScale);
    }

    /// <summary>
    /// Determines which texture to display for the end turn button,
    /// based on whether the current player is a human player and if
    /// the mouse is hovering over the button.
    /// </summary>
    private void DetermineCurrentTexture()
    {
        bool isHumanPlayer = _gameState.CurrentPlayer is HumanPlayer;
        _currentTexture = isHumanPlayer switch
        {
            true when IsHovered() => activeHoverTexture,
            true => activeIdleTexture,
            false => disabledTexture
        };
    }
    
    public override void Update(GameTime deltaTime)
    {
        base.Update(deltaTime);
        DetermineCurrentTexture();
    }

    public override void Draw()
    {
        Game.DrawImage(_currentTexture, Position, textureScale);
    }
}
