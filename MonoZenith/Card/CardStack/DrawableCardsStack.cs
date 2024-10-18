using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Support;

namespace MonoZenith.Card.CardStack
{
    /// <summary>
    /// Index ^1: First card to draw
    /// Index 0: Last card to draw
    /// </summary>
    public class DrawableCardsStack : CardStack
    {
        public readonly Texture2D CardBack;
        public readonly Texture2D CardFront;
        
        public DrawableCardsStack(Game game, GameState state) : base(game, state)
        {
            // Load in Textures
            CardBack = DataManager.GetInstance(game).CardBack;
            CardFront = DataManager.GetInstance(game).CardFront;

            // InitializeCards();
            Shuffle();
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
            ChangePosition(_game.ScreenWidth / 2.2f, _game.ScreenHeight / 2);
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
    }
}
