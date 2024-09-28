#nullable enable
using Microsoft.Xna.Framework;
using MonoZenith.Card;
using MonoZenith.Classes.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;

namespace MonoZenith
{
    internal class GameState
    {
        private readonly Game _game;
        private Player? _currentPlayer;
        private Player? _currentWinner;
        private readonly HumanPlayer _player;
        private readonly NpcPlayer _npc;
        public DrawableCardsStack DrawableCards;
        public CardStack PlayedCards;
        public Region CurrentRegion;
        private readonly SpriteFont _componentFont;
        public int Combo;

        public GameState(Game game)
        {
            _game = game;
            _player = new HumanPlayer(_game, this, "Player");
            _npc = new NpcPlayer(_game, this, "NPC");
            _currentPlayer = null;
            DrawableCards = new DrawableCardsStack(_game, this);
            PlayedCards = new CardStack(_game, this);
            CurrentRegion = Region.LIMGRAVE;   // TODO: Set to random region.
            _componentFont = DataManager.GetInstance(game).ComponentFont;
            Combo = 0;
            InitializeState();
            
            Console.WriteLine(_player);
            Console.WriteLine(_npc);
        }

        /// <summary>
        /// Initialize the game state.
        /// </summary>
        private void InitializeState()
        {
            // Calculate positions of the decks
            float drawableX = _game.ScreenWidth / 2.2f;
            float playedX = _game.ScreenWidth / 1.8f;
            float height = _game.ScreenHeight / 2;
            
            DrawableCards = new DrawableCardsStack(_game, this);
            PlayedCards.ChangePosition(playedX, height);
            DrawableCards.ChangePosition(drawableX, height);
            PlayedCards.ChangePosition(playedX, height);

            // Play the first card in the game
            PlayedCards.AddToFront(DrawableCards.Pop());
            CurrentRegion = ((RegionCard)PlayedCards.Cards.First()).Region;

            // Initialize player hands
            _player.Hand = DrawableCards.GetSevenCards();
            _npc.Hand = DrawableCards.GetSevenCards();
            
            // For debugging purposes, make sure the human player starts
            _currentPlayer = _player;
            // DetermineStartingPlayer();
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
            if (_player.Hand.Count == 0)
            {
                _currentWinner = _player;
                return _player;
            }
            else if (_npc.Hand.Count == 0)
            {
                _currentWinner = _npc;
                return _npc;
            }

            return null;
        }

        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public void Update(GameTime deltaTime)
        {
            if (_currentPlayer is NpcPlayer)
            {
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
            _game.DrawImage(DataManager.GetInstance(_game).Backdrop, Vector2.Zero);

            // Draw cards in play
            DrawableCards.Draw();
            PlayedCards.Draw();

            // Draw player cards
            _player.Draw();
            _npc.Draw();

            // Draw text data
            _game.DrawText(
                $"Current player: {_currentPlayer.Name}", 
                new Vector2(_game.ScreenWidth - 450, _game.ScreenHeight / 2 - 25), 
                _componentFont, Color.White);
            _game.DrawText(
                $"Current region: {CurrentRegion}", 
                new Vector2(_game.ScreenWidth - 450, _game.ScreenHeight / 2), 
                _componentFont, Color.White);
        }
    }
}
