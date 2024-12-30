#nullable enable
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Support.Managers;

public class TurnManager
{
    private TransitionComponent? _activeTurnTransitionComponent;
    private readonly TransitionComponent _turnTransitionComponentHuman;
    private readonly TransitionComponent _turnTransitionComponentNpc;
    private readonly SoundEffectInstance _startPlayerTurnSound;
    private readonly EndTurnButton _endTurnButton;
    
    /// <summary>
    /// The current player.
    /// </summary>
    public Player? CurrentPlayer { get; set; }
    
    /// <summary>
    /// The other player.
    /// </summary>
    public Player? OpposingPlayer { get; set; }

    /// <summary>
    /// Triggered by the player to switch turns
    /// </summary>
    public bool SwitchingTurns { get; set; }
        
    /// <summary>
    /// The current round
    /// </summary>
    public int RoundNumber { get; set; }

    public TurnManager(Game g, GameState s)
    {
        var turnTransitionComponentFont = DataManager.GetInstance().TransitionComponentFont;
        _turnTransitionComponentHuman = new TransitionComponent("YOUR TURN", Color.White, turnTransitionComponentFont);
        _turnTransitionComponentNpc = new TransitionComponent("ENEMY TURN", Color.White, turnTransitionComponentFont);
        _activeTurnTransitionComponent = null;
        _startPlayerTurnSound = DataManager.GetInstance().PlayerTurnSound.CreateInstance();
        _endTurnButton = new EndTurnButton(g, s);
    }
    
    public void InitializeState(Player player, Player npc)
    {
        DetermineStartingPlayer(player, npc);
        RoundNumber = 1;
    }
    
    /// <summary>
    /// Determine the starting player.
    /// </summary>
    private void DetermineStartingPlayer(Player player, Player npc)
    {
        Random rand = new Random();
        CurrentPlayer = rand.Next(0, 2) == 0 ? player : npc;
    }
    
    /// <summary>
    /// Switches the turn to the next player.
    /// </summary>
    private void SwitchTurn(Player player, Player npc)
    {
        SwitchingTurns = false;
        RoundNumber++;
        CurrentPlayer = CurrentPlayer == player ? npc : player;
        OpposingPlayer = OpposingPlayer == player ? npc : player;
        _activeTurnTransitionComponent?.Reset();
            
        if (CurrentPlayer is HumanPlayer)
        {
            _startPlayerTurnSound.Play();
            _activeTurnTransitionComponent = _turnTransitionComponentHuman;
            return;
        }

        _activeTurnTransitionComponent = _turnTransitionComponentNpc;
    }
    
    public void Update(GameTime gameTime, Player player, Player npc)
    {
        _endTurnButton.Update(gameTime);
        _activeTurnTransitionComponent?.Update(gameTime);
        
        // If the player is switching turns, wait for the player to finish moving cards
        if (SwitchingTurns)
        {
            if (CurrentPlayer is { HasAnyMovingCards: true })
                return;

            SwitchTurn(player, npc);
        }
    }

    public void Draw()
    {
        _endTurnButton.Draw();
 
        if (Game.GetGameState().TurnManager.RoundNumber == 1)
            return;
        
        _activeTurnTransitionComponent?.Draw();
    }
}