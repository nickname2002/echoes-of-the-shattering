#nullable enable
using Microsoft.Xna.Framework;
using MonoZenith.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public GraceMenu GraceMenu;
        public GameTime GameTime;
        private Player? _currentPlayer;
        private Player? _currentWinner;
        private readonly HumanPlayer _player;
        private readonly NpcPlayer _npc;
        public DrawableCardsStack DrawableCards;
        public CardStack PlayedCards;
        public Region CurrentRegion;
        private readonly SpriteFont _componentFont;
        public int Combo;
        public int Skip;
        
        public Player CurrentPlayer => _currentPlayer?? _player;
        public Player OpposingPlayer => _currentPlayer == _player? _npc : _player;

        public GameState(Game game)
        {
            _game = game;
            GraceMenu = new GraceMenu(_game, this);
            GameTime = new GameTime();
            _player = new HumanPlayer(_game, this, "Player");
            _npc = new NpcPlayer(_game, this, "NPC");
            _currentPlayer = null;
            DrawableCards = new DrawableCardsStack(_game, this);
            PlayedCards = new CardStack(_game, this);
            CurrentRegion = Region.LIMGRAVE;   // TODO: Set to random region.
            _componentFont = DataManager.GetInstance(game).ComponentFont;
            Combo = 0;
            Skip = 0;
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
            float height = _game.ScreenHeight / 2f;
            
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
            
            // Determine the starting player
            DetermineStartingPlayer();
            
            // Perform the effect of the first card played
            RegionCard firstCard = (RegionCard)PlayedCards.Cards.First();
            Console.WriteLine($"The first card played is: {firstCard}");
            firstCard.PerformEffect(this);
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
            
            if (_npc.Hand.Count != 0)
                return null;
            
            _currentWinner = _npc;
            return _npc;
        }

        /// <summary>
        /// Switches the turn to the next player.
        /// </summary>
        public void SwitchTurn()
        {
            // Turn does not get switched if the opposing player
            // has to skip turns
            if (Skip != 0)
            {
                Skip--;
                return;
            }

            _currentPlayer = _currentPlayer == _player? _npc : _player;
            Console.WriteLine($"Turn: {_currentPlayer.Name}");
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
            GraceMenu.Update(deltaTime);
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
            
            GraceMenu.Draw();
            
            // Draw cards in play
            DrawableCards.Draw();
            PlayedCards.Draw();

            // Draw player cards
            _player.Draw();
            _npc.Draw();

            // TODO: Remove when no longer needed
            // Draw text data
            _game.DrawText(
                $"Current region: {CurrentRegion}", 
                new Vector2(_game.ScreenWidth - 450, _game.ScreenHeight / 2), 
                _componentFont, Color.White);
        }
    }
}
