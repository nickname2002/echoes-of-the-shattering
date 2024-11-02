using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoZenith.Components.Indicator;
using MonoZenith.Engine.Support;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoZenith.Players
{
    internal sealed class HumanPlayer : Player
    {
        private Card.Card _lastHoveredCard;
        private CardStackIndicator _deckIndicator;
        private CardStackIndicator _reserveIndicator;
        private ItemIndicator _spiritAshIndicator;
        
        public HumanPlayer(Game game, GameState state, string name) : base(game, state, name)
        {
            _handxPos = game.ScreenWidth / 2f;
            _handyPos = game.ScreenHeight / 1.38f;
            PlayerPosition = new Vector2(
                game.ScreenWidth * 0.05f, 
                game.ScreenHeight * 0.915f);
            PlayerIcon = DataManager.GetInstance(game).Player;
            InitializeState(game, state);
        }

        public override void InitializeState(Game game, GameState state)
        {
            base.InitializeState(game, state);

            // Initialize indicators
            _deckIndicator = new CardStackIndicator(
                game, state, 
                new Vector2(
                    _game.ScreenWidth - 185 * AppSettings.Scaling.ScaleFactor, 
                    _game.ScreenHeight - 200 * AppSettings.Scaling.ScaleFactor), 
                DataManager.GetInstance(_game).DeckIndicator,
                _deckStack);
            _reserveIndicator = new CardStackIndicator(
                game, state, 
                new Vector2(
                    _game.ScreenWidth - 100 * AppSettings.Scaling.ScaleFactor, 
                    _game.ScreenHeight - 253 * AppSettings.Scaling.ScaleFactor), 
                DataManager.GetInstance(_game).ReserveIndicator,
                _reserveCardStack);
            _spiritAshIndicator = new ItemIndicator(
                game, state, 
                new Vector2(
                    _game.ScreenWidth - 100 * AppSettings.Scaling.ScaleFactor, 
                    _game.ScreenHeight - 147 * AppSettings.Scaling.ScaleFactor), 
                DataManager.GetInstance(_game).MimicTearIndicatorDisabled);
        }
        
        /// <summary>
        /// Get the selected card from the hand.
        /// </summary>
        /// <returns>Selected card, or null if no card is hovered or clicked.</returns>
        public Card.Card GetSelectedCard()
        {
            // Check if the last hovered card was clicked and return it if true
            if (_lastHoveredCard != null && _lastHoveredCard.IsClicked())
            {
                Console.WriteLine($"Hovered card selected: {_lastHoveredCard}");
                return _lastHoveredCard;
            }

            // If no hovered card was clicked, check for any other clicked cards
            List<Card.Card> clickedCards = _handStack.Cards.Where(c => c.IsClicked()).ToList();
    
            // Print names of all clicked cards
            foreach (var card in clickedCards)
            {
                Console.WriteLine($"- {card}");
            }

            // Return the first clicked card, or null if no cards were clicked
            return clickedCards.Count switch
            {
                0 => null,
                >= 1 => clickedCards[0],
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void PerformTurn(GameState state)
        {
            base.PerformTurn(state);

            if (TryPlayCard())
                return;

            _spiritAshIndicator.Update(state.GameTime);
        }

        /// <summary>
        /// Attempt to play the selected card.
        /// </summary>
        /// <returns>Whether a valid card was played.</returns>
        public bool TryPlayCard()
        {
            var selectedCard = GetSelectedCard();

            // If no card is selected or the card is not valid, return false.
            if (selectedCard == null) 
                return false;

            // If the card conditions are not met, return false.
            if (!selectedCard.IsAffordable())
            {
                return false;
            }
                
            PlayCard(selectedCard);
            return true;
        }

        public override void Update(GameTime deltaTime)
        {
            _handStack.Update(deltaTime);
            _deckIndicator.Update(deltaTime);
            _reserveIndicator.Update(deltaTime);
        }

        public override void DrawPlayerHealthAndName()
        {
            // Setup offsets and positions for name and player bars
            Vector2 playerOffset = GetOffset(PlayerCurrent, Scale);
            Vector2 namePosition = PlayerPosition + new Vector2(playerOffset.X * 1.2f, 0);
            Vector2 shadowPosition = new(1.25f, 1.25f);
            int healthHeight = (int)(PlayerCurrent.Height * Scale * 0.05f);
            int healthWidth = (int)(_game.ScreenWidth * 0.9f);
            int barWidth = (int)(_game.ScreenWidth * 0.3f);
            Vector2 barOffset = new Vector2(0, healthHeight + 4);
            Vector2 healthPosition = PlayerPosition + new Vector2(0, playerOffset.Y - healthHeight * 4.5f) - new Vector2(1, 1);
            Vector2 edgePosition = healthPosition - new Vector2(1, 1);

            // Draw name
            _game.DrawText(Name, namePosition + shadowPosition, PlayerFont, Color.DarkGray);
            _game.DrawText(Name, namePosition, PlayerFont, Color.White);

            // Draw Health bar with current health points
            _game.DrawRectangle(Color.Goldenrod, edgePosition, healthWidth + 2, healthHeight + 2);
            _game.DrawRectangle(Color.DarkGray, healthPosition, healthWidth, healthHeight);
            _game.DrawRectangle(Color.DarkRed, healthPosition, (int)(healthWidth * (Health / 100f)), healthHeight);

            // Draw Focus bar with current focus points
            _game.DrawRectangle(Color.Goldenrod, edgePosition + barOffset, barWidth + 2, healthHeight + 2);
            _game.DrawRectangle(Color.DarkGray, healthPosition + barOffset, barWidth, healthHeight);
            _game.DrawRectangle(Color.MediumBlue, healthPosition + barOffset, (int)(barWidth * (Focus / 30f)), healthHeight);

            // Draw Stamina bar with current stamina points
            _game.DrawRectangle(Color.Goldenrod, edgePosition + barOffset * 2, barWidth + 2, healthHeight + 2);
            _game.DrawRectangle(Color.DarkGray, healthPosition + barOffset * 2, barWidth, healthHeight);
            _game.DrawRectangle(Color.ForestGreen, healthPosition + barOffset * 2, (int)(barWidth * (Stamina / 30f)),
                healthHeight);
        }

        protected override void DrawPlayerUi()
        {
            base.DrawPlayerUi();
            _deckIndicator.Draw();
            _reserveIndicator.Draw();
            _spiritAshIndicator.Draw();
        }
    }
}
