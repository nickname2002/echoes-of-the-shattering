using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Components;

public sealed class EndTurnButton : Button
{
    private readonly Texture2D _activeIdleTexture;
    private readonly Texture2D _activeHoverTexture;
    private readonly Texture2D _disabledTexture;
    private Texture2D _currentTexture;
    private GameState _gameState;
    private float textureScale;

    public EndTurnButton(Game g, GameState gs, float scale = 1f) : 
        base(g, Vector2.Zero, 0, 0, "", 0, Color.Black, Color.Black, 0, Color.Black)
    {
        _activeIdleTexture = DataManager.GetInstance(g).EndTurnButtonIdleTexture;
        _activeHoverTexture = DataManager.GetInstance(g).EndTurnButtonHoverTexture;
        _disabledTexture = DataManager.GetInstance(g).EndTurnButtonDisabledTexture;
        _currentTexture = _activeIdleTexture;
        _gameState = gs;
        textureScale = scale * 0.25f * AppSettings.Scaling.ScaleFactor;
        UpdateDimensions();
        Position = new Vector2(
            Game.ScreenWidth - Width - 25 * AppSettings.Scaling.ScaleFactor, 
            Game.ScreenHeight / 2 - Height / 2);

        var endTurnSound = DataManager.GetInstance(g).EndTurnSound;

        SetOnClickAction(() =>
        {
            if (_gameState.CurrentPlayer is not HumanPlayer) 
                return;
            
            endTurnSound.Play();
            _gameState.CurrentPlayer.MoveCardsFromHandToReserve();
            _gameState.SwitchTurn();
        });
    }

    private void UpdateDimensions()
    {
        Width = (int)(_activeIdleTexture.Width * textureScale);
        Height = (int)(_activeIdleTexture.Height * textureScale);
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
            true when IsHovered() => _activeHoverTexture,
            true => _activeIdleTexture,
            false => _disabledTexture
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
