#nullable enable
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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
        private Player? _currentPlayer;
        private Player? _currentWinner;

        private Texture2D _backdrop;
        
        private readonly GameOverManager _gameOverManager;
        
        /// <summary>
        /// Transition components for the turn and game over messages.
        /// </summary>
        private readonly TransitionComponent _turnTransitionComponentHuman;
        private readonly TransitionComponent _turnTransitionComponentNpc;
        private TransitionComponent? _activeTurnTransitionComponent;
        
        private readonly HumanPlayer _player;
        private NpcPlayer _npc;
        public readonly CardStack PlayedCards;
        private readonly EndTurnButton _endTurnButton;
        
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
            
            _gameOverManager = new GameOverManager(Game);
            
            _player = new HumanPlayer(Game, this, "Player");
            _npc = new NpcPlayer(Game, this, "NPC");
            _currentPlayer = null;

            _startPlayerTurnSound = DataManager.GetInstance(game).PlayerTurnSound.CreateInstance();
            var turnTransitionComponentFont = DataManager.GetInstance(Game).TransitionComponentFont;
            _turnTransitionComponentHuman = new TransitionComponent(
                Game, "YOUR TURN", Color.White, turnTransitionComponentFont);
            _turnTransitionComponentNpc = new TransitionComponent(
                Game, "ENEMY TURN", Color.White, turnTransitionComponentFont);
            _activeTurnTransitionComponent = null;
            
            PlayedCards = new CardStack(Game, this, true);

            InitializeState();
            _endTurnButton = new EndTurnButton(Game, this);
        }

        /// <summary>
        /// Set the current level
        /// </summary>
        /// <param name="level">The level</param>
        public void SetLevel(Level level)
        {
            _npc = level.Enemy;
            _backdrop = level.Backdrop;
        }
        
        /// <summary>
        /// Initialize the game state.
        /// </summary>
        public void InitializeState()
        {
            RoundNumber = 1;
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
            
            // Initialize players
            _player.InitializeState(Game, this);
            _npc.InitializeState(Game, this);
        }
        
        /// <summary>
        /// Determine the starting player.
        /// </summary>
        private void DetermineStartingPlayer()
        {
            // If there is a winner from the previous game, they go first.
            if (_currentWinner != null)
            {
                _currentPlayer = _currentWinner;
                _currentWinner = null;
                return;
            }

            // Otherwise, randomly select a player to go first.
            Random rand = new Random();
            
            if (rand.Next(0, 2) == 0)
            {
                _currentPlayer = _player;
            }
            else
            {
                _currentPlayer = _npc;
            }
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
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public void Update(GameTime deltaTime)
        {
            GameTime = deltaTime;
            
            // Make sure all cards are updated
            _player.Update(deltaTime);
            _npc.Update(deltaTime);
            _endTurnButton.Update(deltaTime);
            PlayedCards.Update(deltaTime);
            _activeTurnTransitionComponent?.Update(deltaTime);
            
            // If the player is switching turns, wait for the player to finish moving cards
            if (SwitchingTurns)
            {
                if (_currentPlayer is { HasAnyMovingCards: true })
                    return;

                SwitchTurn();
            }

            if (_gameOverManager.HasWinner(_player, _npc) != null)
            {
                _gameOverManager.UpdateGameOverTransition(deltaTime);
                return;
            }
            
            _currentPlayer?.PerformTurn(this);
        }
        
        /// <summary>
        /// Draw the game state.
        /// </summary>
        public void Draw()
        {
            // Draw backdrop
            Game.DrawImage(
                _backdrop, 
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
            _player.Draw();
            _npc.Draw();
            _endTurnButton.Draw();
            
            // Draw turn indicator
            _activeTurnTransitionComponent?.Draw();
        }
    }
}
