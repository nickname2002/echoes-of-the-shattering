#nullable enable
using Microsoft.Xna.Framework;
using MonoZenith.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card.CardStack;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support;

namespace MonoZenith
{
    public class GameState
    {
        private readonly Game _game;
        public GameTime GameTime;
        private Player? _currentPlayer;
        private Player? _currentWinner;
        private readonly HumanPlayer _player;
        private readonly NpcPlayer _npc;
        public readonly CardStack PlayedCards;
        private readonly SpriteFont _componentFont;
        private readonly EndTurnButton _endTurnButton;
        private CardStack _playedCardStack;
        private readonly SoundEffectInstance _playerDeathSound;
        private readonly SoundEffectInstance _enemyDeathSound;

        public Player CurrentPlayer => _currentPlayer?? _player;
        public Player OpposingPlayer => _currentPlayer == _player? _npc : _player;

        public GameState(Game game)
        {
            _game = game;
            GameTime = new GameTime();
            _player = new HumanPlayer(_game, this, "Player");
            _npc = new NpcPlayer(_game, this, "NPC");
            _currentPlayer = null;
            PlayedCards = new CardStack(_game, this);
            _componentFont = DataManager.GetInstance(game).ComponentFont;
            _playerDeathSound = DataManager.GetInstance(game).PlayerDeathSound;
            _enemyDeathSound = DataManager.GetInstance(game).EnemyDeathSound;
            InitializeState();
            _endTurnButton = new EndTurnButton(_game, this);
            _playedCardStack = new CardStack(_game, this);
        }

        /// <summary>
        /// Initialize the game state.
        /// </summary>
        private void InitializeState()
        {
            // Calculate positions of the decks
            float drawableX = _game.ScreenWidth / 2.2f;
            float playedX = _game.ScreenWidth / 1.8f;
            float height = _game.ScreenHeight / 2f;
            
            PlayedCards.ChangePosition(playedX, height);
            PlayedCards.ChangePosition(playedX, height);

            // Determine the starting player
            DetermineStartingPlayer();

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
                return _player;
            }

            if (_player.Health > 0)
                return null;

            if (_currentWinner == null)
                _playerDeathSound.Play();
            _currentWinner = _npc;
            return _npc;
        }

        /// <summary>
        /// Switches the turn to the next player.
        /// </summary>
        public void SwitchTurn()
        {
            _currentPlayer = _currentPlayer == _player? _npc : _player;
        }

        private void DisplayWinnerMessage()
        {
            // Text to be displayed
            string winnerText = $"{_currentWinner?.Name} wins!";

            // Measure the size of the text
            Vector2 textSize = _componentFont.MeasureString(winnerText);

            // Calculate the position to center the text
            Vector2 position = new Vector2(
                (_game.ScreenWidth / 2f) - (textSize.X / 2),  // Center horizontally
                (_game.ScreenHeight / 2f) - (textSize.Y / 2)  // Center vertically
            );

            // Draw the text at the calculated position
            _game.DrawText(winnerText, position, _componentFont, Color.White);
        }
        
        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public void Update(GameTime deltaTime)
        {
            GameTime = deltaTime;
            
            if (HasWinner() != null)
            {
                return;
            }
            
            _currentPlayer?.PerformTurn(this);
            _endTurnButton.Update(deltaTime);
            
            // Update players
            _player.Update(deltaTime);
            _npc.Update(deltaTime);
        }
        
        /// <summary>
        /// Draw the game state.
        /// </summary>
        public void Draw()
        {
            // Draw backdrop
            _game.DrawImage(DataManager.GetInstance(_game).Backdrop, Vector2.Zero);

            if (HasWinner() != null)
            {
                DisplayWinnerMessage();
                return;
            }
            
            // Draw cards in play
            PlayedCards.Draw();

            // Draw player cards
            _player.Draw();
            _npc.Draw();
            _endTurnButton.Draw();
        }
    }
}
