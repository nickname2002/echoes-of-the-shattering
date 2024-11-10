#nullable enable
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Card.CardStack;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith
{
    public class GameState
    {
        private readonly Game _game;
        public GameTime GameTime;
        private Player? _currentPlayer;
        private Player? _currentWinner;
        
        /// <summary>
        /// Transition components for the turn and game over messages.
        /// </summary>
        private readonly TransitionComponent _turnTransitionComponentHuman;
        private readonly TransitionComponent _turnTransitionComponentNpc;
        private TransitionComponent? _activeTurnTransitionComponent;
        private readonly TransitionComponent _gameOverTransitionComponent;
        
        private readonly HumanPlayer _player;
        private readonly NpcPlayer _npc;
        public readonly CardStack PlayedCards;
        private readonly EndTurnButton _endTurnButton;
        
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

        public GameState(Game game)
        {
            _game = game;
            GameTime = new GameTime();
            _player = new HumanPlayer(_game, this, "Player");
            _npc = new NpcPlayer(_game, this, "NPC");
            _currentPlayer = null;

            var turnTransitionComponentFont = DataManager.GetInstance(_game).TransitionComponentFont;
            var gameOverTransitionComponentFont = DataManager.GetInstance(_game).GameOverTransitionComponentFont;
            _turnTransitionComponentHuman = new TransitionComponent(
                _game, "YOUR TURN", Color.White, turnTransitionComponentFont);
            _turnTransitionComponentNpc = new TransitionComponent(
                _game, "ENEMY TURN", Color.White, turnTransitionComponentFont);
            _activeTurnTransitionComponent = null;
            _gameOverTransitionComponent = new TransitionComponent(
                _game, "YOU DIED", new Color(255, 215, 0), gameOverTransitionComponentFont,
                1f, 3f, 1f,
                _game.BackToMainMenu);
            
            PlayedCards = new CardStack(_game, this, true);
            _playerDeathSound = DataManager.GetInstance(game).PlayerDeathSound.CreateInstance();
            _enemyDeathSound = DataManager.GetInstance(game).EnemyDeathSound.CreateInstance();
            _startPlayerTurnSound = DataManager.GetInstance(game).PlayerTurnSound.CreateInstance();
            InitializeState();
            _endTurnButton = new EndTurnButton(_game, this);
        }
        
        /// <summary>
        /// Initialize the game state.
        /// </summary>
        public void InitializeState()
        {
            PlayedCards.Clear();
            
            // Update the position of the played cards
            PlayedCards.UpdatePosition(
                _game.ScreenWidth / 2f, 
                _game.ScreenHeight / 2f - Card.Card.Height / 2f);

            // Determine the starting player
            DetermineStartingPlayer();

            // Reset transition components
            _turnTransitionComponentHuman.Reset();
            _turnTransitionComponentNpc.Reset();
            _gameOverTransitionComponent.Reset();
            
            // Initialize players
            _player.InitializeState(_game, this);
            _npc.InitializeState(_game, this);
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
            
            if (HasWinner() != null)
            {
                _gameOverTransitionComponent.Update(deltaTime);
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
            _game.DrawImage(
                DataManager.GetInstance(_game).Backdrop, 
                Vector2.Zero, 
                AppSettings.Scaling.ScaleFactor);

            if (HasWinner() != null)
            {
                DisplayGameOverMessage();
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
