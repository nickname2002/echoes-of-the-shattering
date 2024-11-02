using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support;
using static MonoZenith.Engine.Support.AppSettings;

namespace MonoZenith.Card.CardStack
{
    /// <summary>
    /// Index ^1: First card to draw
    /// Index 0: Last card to draw
    /// </summary>
    public class HandCardStack : CardStack
    {
        public readonly Texture2D CardBack;
        public readonly Texture2D CardFront;
        
        public HandCardStack(Game game, GameState state) : base(game, state)
        {
            // Load in Textures
            CardBack = DataManager.GetInstance(game).CardBack;
            CardFront = DataManager.GetInstance(game).CardFront;
        }

        /// <summary>
        /// Initialize the cards in the stack.
        /// </summary>
        private void InitializeCards()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If the top card is clicked, return it. Otherwise, return null.
        /// </summary>
        /// <returns>The clicked card or null.</returns>
        public Card GetSelectCard()
        {
            RefillIfEmpty();
            
            if (!_cards[^1].IsClicked())
            {
                return null;
            }
            
            // Draw card
            Card cardToDraw = _cards[^1];
            _cards.Remove(cardToDraw);
            return cardToDraw;
        }

        /// <summary>
        /// Refills the deck if empty.
        /// </summary>
        private void RefillIfEmpty()
        {
            if (_cards.Count > 1) 
                return;
            
            Console.WriteLine("Refilling deck...");
            List<Card> cardsToAdd = _state.PlayedCards.GetAllButLastCards();
            
            // Shuffle the cards
            Shuffle();
            
            // Add the cards to the deck
            _cards.AddRange(cardsToAdd);
            
            // Make sure all cards have the same position as the deck
            UpdatePosition(_game.ScreenWidth / 2.2f, _game.ScreenHeight / 2);
            Shuffle();
        }
        
        public Card GetCard()
        {
            RefillIfEmpty();
            Card cardToDraw = _cards[^1];
            _cards.Remove(cardToDraw);
            return cardToDraw;
        }
        
        /// <summary>
        /// Return seven cards from the stack.
        /// </summary>
        /// <returns>The seven cards.</returns>
        public CardStack GetSevenCards()
        {
            CardStack sevenCards = new CardStack(_game, _state);

            for (int i = 0; i < 7; i++)
            {
                sevenCards.AddToBottom(GetCard());
            }

            return sevenCards;
        }

        public override void Update(GameTime deltaTime)
        {
            // Initialize lists for hovered card positions
            List<Card> hoveredCards = new List<Card>();
            Dictionary<Card, float> cardPositions = new Dictionary<Card, float>();

            // Retrieve spacing and starting position of the cards
            int cardCount = _cards.Count;
            float spacing, startX;
            (spacing, startX) = CalculateCardPosition(cardCount);

            for (int i = 0; i < cardCount; i++)
            {
                // Calculate the position of the current card for centering
                Card currentCard = _cards[i];
                float cardX = startX + i * spacing;

                // Check if the current card is from the HumanPlayer's hand
                if (currentCard.IsHovered() && currentCard.Owner is HumanPlayer)
                {
                    hoveredCards.Add(currentCard);
                    cardPositions[currentCard] = cardX;
                }
                else
                {
                    UpdateNonHoveredCard(currentCard, cardX);
                    currentCard.Update(deltaTime);
                }
            }

            UpdateHoveredCard(hoveredCards, cardPositions, deltaTime);
        }

        /// <summary>
        /// Updates the non hovered card position.
        /// </summary>
        /// <param name="card">The non hovered card.</param>
        /// <param name="x">The new X positional value.</param>
        public virtual void UpdateNonHoveredCard(Card card, float x)
        {
            if (GetType().IsSubclassOf(typeof(CardStack)))
            {
                card.UpdatePosition(x, _position.Y, false);
            }
            else
            {
                card.UpdatePosition(x, _position.Y, true);
            }
        }

        /// <summary>
        /// Updates the hovered card position.
        /// </summary>
        /// <param name="hoveredCards">List of stored hovered cards.</param>
        /// <param name="cardPositions">Dictionary of cards and its X positional values.</param>
        /// <param name="deltaTime">The delta time.</param>
        private void UpdateHoveredCard(
            List<Card> hoveredCards,
            Dictionary<Card, float> cardPositions,
            GameTime deltaTime)
        {
            const int verticalMoveOffset = 20;
            Card _lastHoveredCard = null;

            // Draw hovered cards, except the last one, in their original positions
            for (int i = 0; i < hoveredCards.Count; i++)
            {
                Card hoveredCard = hoveredCards[i];
                float hoveredCardPosition = cardPositions[hoveredCard];

                // Draw all hovered cards except the last one first
                if (i < hoveredCards.Count - 1)
                {
                    hoveredCard.UpdatePosition(hoveredCardPosition, _position.Y, false);
                    hoveredCard.Update(deltaTime);
                }
            }

            // Draw the last hovered card (if any), move it slightly up, and store it
            if (hoveredCards.Count <= 0)
                return;

            _lastHoveredCard = hoveredCards[^1];
            float lastHoveredCardPosition = cardPositions[_lastHoveredCard];
            _lastHoveredCard.UpdatePosition(lastHoveredCardPosition, _position.Y - verticalMoveOffset, false);
            _lastHoveredCard.Update(deltaTime);
        }
    }
}
