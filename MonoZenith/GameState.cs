#nullable enable
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Card.CardStack;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith
{
    public class GameState
    {
        public readonly Game Game;
        public GameTime GameTime;
        
        /// <summary>
        /// Transition components for the turn and game over messages.
        /// </summary>
        private readonly TransitionComponent _turnTransitionComponentHuman;
        private readonly TransitionComponent _turnTransitionComponentNpc;
        private TransitionComponent? _activeTurnTransitionComponent;
        private readonly TransitionComponent _gameOverTransitionComponent;
        
        public readonly TurnManager TurnManager;
        
        public readonly HumanPlayer Player;
        public readonly NpcPlayer Npc;
        
        public readonly CardStack PlayedCards;
        
        private readonly SoundEffectInstance _playerDeathSound;
        private readonly SoundEffectInstance _enemyDeathSound;
        private readonly SoundEffectInstance _startPlayerTurnSound;
        
        /// <summary>
        /// The current player.
        /// </summary>
        public Player CurrentPlayer => _currentPlayer ?? _player;
        
        /// <summary>
        /// The opposing player.
        /// </summary>
        public Player OpposingPlayer => _currentPlayer == _player ? _npc : _player;
        
        /// <summary>
        /// Triggered by the player to switch turns
        /// </summary>
        public bool SwitchingTurns { get; set; }
        
        public int RoundNumber { get; set; }

        public GameState(Game game)
        {
            Game = game;
            GameTime = new GameTime();
            TurnManager = new TurnManager(Game, this);
            _gameOverManager = new GameOverManager(Game);
            Player = new HumanPlayer(Game, this, "Player");
            Npc = new NpcPlayer(Game, this, "NPC");
            
            PlayedCards = new CardStack(Game, this, true);

            InitializeState();
        }
        
        /// <summary>
        /// Initialize the game state.
        /// </summary>
        public void InitializeState()
        {
            _currentWinner = null;
            TurnManager.InitializeState(Player, Npc);
            PlayedCards.Clear();
            
            // Initialize managers
            _gameOverManager.InitializeState();
            
            // Update the position of the played cards
            PlayedCards.UpdatePosition(
                Game.ScreenWidth / 2f, 
                Game.ScreenHeight / 2f - Card.Card.Height / 2f);

            // Determine the starting player
            DetermineStartingPlayer();

            // Reset transition components
            _turnTransitionComponentHuman.Reset();
            _turnTransitionComponentNpc.Reset();
            _gameOverTransitionComponent.Reset();
            
            // Initialize players
            Player.InitializeState(Game, this);
            Npc.InitializeState(Game, this);
        }

        /// <summary>
        /// Check if there is a winner.
        /// </summary>
        /// <returns>The winning player, or null if there is no winner.</returns>
        public Player? HasWinner()
        {
            if (_npc.Health < 1)
            {
                if (_currentWinner == null)
                    _enemyDeathSound.Play();

                _currentWinner = _player;
                _gameOverTransitionComponent.Content = "ENEMY FELLED";
                _gameOverTransitionComponent.Color = new Color(255, 215, 0);
                return _player;
            }

            if (_player.Health > 0)
                return null;

            if (_currentWinner == null)
                _playerDeathSound.Play();

            _currentWinner = _npc;
            _gameOverTransitionComponent.Content = "YOU DIED";
            _gameOverTransitionComponent.Color = new Color(180, 30, 30);
            return _npc;
        }

        /// <summary>
        /// Switches the turn to the next player.
        /// </summary>
        private void SwitchTurn()
        {
            SwitchingTurns = false;
            _currentPlayer = _currentPlayer == _player ? _npc : _player;
            _activeTurnTransitionComponent?.Reset();
            
            if (_currentPlayer is HumanPlayer)
            {
                _startPlayerTurnSound.Play();
                _activeTurnTransitionComponent = _turnTransitionComponentHuman;
                return;
            }

            _activeTurnTransitionComponent = _turnTransitionComponentNpc;
            RoundNumber++;
        }

        /// <summary>
        /// Displays the game over message.
        /// </summary>
        private void DisplayGameOverMessage()
        {
            _gameOverTransitionComponent.Draw();
        }
        
        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public void Update(GameTime deltaTime)
        {
            GameTime = deltaTime;
            
            // Make sure all cards are updated
            Player.Update(deltaTime);
            Npc.Update(deltaTime);
            PlayedCards.Update(deltaTime);
            
            // Update the turn manager
            TurnManager.Update(deltaTime, Player, Npc);
            if (TurnManager.SwitchingTurns) return;
            
            if (_gameOverManager.HasWinner(_player, _npc) != null)
            {
                _gameOverManager.UpdateGameOverTransition(deltaTime);
                return;
            }
            
            TurnManager.CurrentPlayer?.PerformTurn(this);
        }
        
        /// <summary>
        /// Draw the game state.
        /// </summary>
        public void Draw()
        {
            // Draw backdrop
            Game.DrawImage(
                DataManager.GetInstance(Game).Backdrop, 
                Vector2.Zero, 
                AppSettings.Scaling.ScaleFactor);
            
            if (_gameOverManager.HasWinner(_player, _npc) != null)
            {
                _gameOverManager.DisplayGameOverMessage();
                return;
            }
            
            // Draw cards in play
            PlayedCards.Draw();

            // Draw player cards
            Player.Draw();
            Npc.Draw();

            TurnManager.Draw();
        }
    }
}
