using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith.Components;

public sealed class EndTurnButton : Button
{
    private readonly Texture2D _activeIdleTexture;
    private readonly Texture2D _activeHoverTexture;
    private readonly Texture2D _disabledTexture;
    private SoundEffectInstance _retrieveCardsSound;
    private readonly SoundEffectInstance _endPlayerTurnSound;
    private Texture2D _currentTexture;
    private GameState _gameState;
    private float textureScale;

    public EndTurnButton(Game g, GameState gs, float scale = 1f) : 
        base(g, Vector2.Zero, 0, 0, "", 0, Color.Black, Color.Black, 0, Color.Black)
    {
        _activeIdleTexture = DataManager.GetInstance(g).EndTurnButtonIdleTexture;
        _activeHoverTexture = DataManager.GetInstance(g).EndTurnButtonHoverTexture;
        _disabledTexture = DataManager.GetInstance(g).EndTurnButtonDisabledTexture;
        _retrieveCardsSound = DataManager.GetInstance(g).RetrieveCardsSound.CreateInstance();
        _endPlayerTurnSound = DataManager.GetInstance(g).EndPlayerTurnSound.CreateInstance();
        _currentTexture = _activeIdleTexture;
        _gameState = gs;
        textureScale = scale * 0.25f * AppSettings.Scaling.ScaleFactor;
        UpdateDimensions();
        Position = new Vector2(
            Game.ScreenWidth - Width - 25 * AppSettings.Scaling.ScaleFactor, 
            Game.ScreenHeight / 2f - Height / 2f);
        
        SetOnClickAction(() =>
        {
            _endPlayerTurnSound.Play();
            
            if (_gameState.TurnManager.CurrentPlayer == null) return;
            
            _gameState.TurnManager.CurrentPlayer.MoveCardsFromHandToReserve();
            _gameState.TurnManager.CurrentPlayer.MoveCardsFromPlayedToReserve();
            _gameState.TurnManager.CurrentPlayer.ResetPlayerStamina();
            
            _gameState.TurnManager.SwitchingTurns = true;
            _retrieveCardsSound.Play();
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
        bool isHumanPlayer = _gameState.TurnManager.CurrentPlayer is HumanPlayer;
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
