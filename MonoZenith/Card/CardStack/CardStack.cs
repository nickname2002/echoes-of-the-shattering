using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Card.CardStack
{
    public class CardStack
    {
        protected Game _game;
        protected GameState _state;
        protected Vector2 _position;
        protected List<Card> _cards = new();
        public List<Card> Cards => _cards;
        public int Count => _cards.Count;

        public CardStack(Game game, GameState state)
        {
            _game = game;
            _state = state;
        }

        public override string ToString()
        {
            return string.Join(",\n", _cards);
        }

        /// <summary>
        /// Check if the stack is empty.
        /// </summary>
        /// <returns>Whether the stack is empty.</returns>
        public bool IsEmpty() => _cards.Count == 0;
        
        /// <summary>
        /// Determine if there are any affordable cards in the stack.
        /// </summary>
        /// <returns>True if there is at least one card that is affordable; otherwise, false.</returns>
        public bool ContainsAffordableCards() => _cards.Any(card => card.IsAffordable());
        
        /// <summary>
        /// Clear the stack of all cards.
        /// </summary>
        public void Clear()
        {
            _cards.Clear();
        }

        /// <summary>
        /// Remove card from the stack of cards.
        /// </summary>
        /// <param name="card">The card to remove.</param>
        public void Remove(Card card)
        {
            _cards.Remove(card);
        }
        
        /// <summary>
        /// Add a card to the top of the stack.
        /// </summary>
        /// <param name="card">The card to add.</param>
        public void AddToFront(Card card)
        {
            _cards.Insert(0, card);
        }

        /// <summary>
        /// Add a list of cards to the top of the stack.
        /// </summary>
        /// <param name="cardList">The list of cards to add.</param>
        public void AddToFront(List<Card> cardList)
        {
            _cards.InsertRange(0, cardList);
        }

        /// <summary>
        /// Add a card to the bottom of the stack.
        /// </summary>
        /// <param name="card">The card to add.</param>
        public void AddToBottom(Card card)
        {
            _cards.Add(card);
        }

        /// <summary>
        /// Add a list of cards to the bottom of the stack.
        /// </summary>
        /// <param name="cardList">The list of cards to add.</param>
        public void AddToBottom(List<Card> cardList)
        {
            _cards.AddRange(cardList);
        }

        /// <summary>
        /// Pop a card from the top of the stack.
        /// </summary>
        /// <returns>The card that was popped.</returns>
        public Card Pop()
        {
            Card card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// Pop a random card from the stack.
        /// </summary>
        /// <returns>The randomly popped card.</returns>
        public Card PopRandomCard()
        {
            Random rng = new Random();
            int index = rng.Next(_cards.Count);
            Card card = _cards[index];
            _cards.RemoveAt(index);
            return card;
        }
        
        /// <summary>
        /// Shuffle the stack.
        /// </summary>
        public void Shuffle()
        {
            Random rng = new Random();
            int n = _cards.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
            }
        }

        /// <summary>
        /// Changes the position of the stack.
        /// </summary>
        /// <param name="x">Positional X</param>
        /// <param name="y">Positional Y</param>
        public void ChangePosition(float x, float y)
        {
            _position = new Vector2(x, y);
            
            // Update the position of the contained cards
            foreach (var card in _cards)
            {
                card.ChangePosition(
                    _position.X - Card.Width / 2 * _cards[0].Scale * AppSettings.Scaling.ScaleFactor, 
                    _position.Y - Card.Height / 2 * _cards[0].Scale * AppSettings.Scaling.ScaleFactor);
            }
        }

        /// <summary>
        /// Get all but the last card in the stack.
        /// </summary>
        /// <returns>A list of all but the last cards in the stack.</returns>
        public List<Card> GetAllButLastCards()
        {
            return _cards.Count < 2 ? new List<Card>() : _cards.Take(_cards.Count - 1).ToList();
        }

        /// <summary>
        /// Get all cards in the stack.
        /// </summary>
        /// <returns>A list of all but the last cards in the stack.</returns>
        public List<Card> GetAllCards()
        {
            return _cards.Take(_cards.Count).ToList();
        }

        /// <summary>
        /// Draw the stack in a straight line, centered on the stack's position.
        /// </summary>
        public void Draw()
        {
            if (!_cards.Any()) 
                return;

            foreach (var card in _cards)
            {
                if (card.Owner is NpcPlayer)
                {
                    card.Draw(180);
                }
                else
                {
                    card.Draw(0, true);
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            // Calculate the position step for card spacing
            List<Card> hoveredCards = new List<Card>();
            Dictionary<Card, float> cardPositions = new Dictionary<Card, float>();

            int cardCount = _cards.Count;
            float cardWidth = Card.Width;

            // Define the spacing between cards
            int offset = 20;
            float spacing = cardWidth + offset * AppSettings.Scaling.ScaleFactor;

            // Calculate the total width occupied by all cards including spacing
            float totalWidth = cardCount * cardWidth + (cardCount - 1) * offset * AppSettings.Scaling.ScaleFactor;

            // Calculate the starting position to center the cards
            float startX = _position.X - totalWidth / 2 + cardWidth / 2;

            for (int i = 0; i < cardCount; i++)
            {
                Card currentCard = _cards[i];

                float cardX = startX + i * spacing;
                if (currentCard.IsHovered() && currentCard.Owner is HumanPlayer)
                {
                    hoveredCards.Add(currentCard);
                    cardPositions[currentCard] = cardX;
                }
                else
                {
                    UpdateCard(currentCard, cardX);
                }
                currentCard.Update(gameTime);
            }

            UpdateHoveredCard(hoveredCards, cardPositions);
        }

        public virtual void UpdateCard(Card card, float x)
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

        private void UpdateHoveredCard(List<Card> hoveredCards, Dictionary<Card, float> cardPositions)
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
                }
            }

            // Draw the last hovered card (if any), move it slightly up, and store it
            if (hoveredCards.Count <= 0)
                return;

            _lastHoveredCard = hoveredCards[^1];
            float lastHoveredCardPosition = cardPositions[_lastHoveredCard];
            _lastHoveredCard.UpdatePosition(lastHoveredCardPosition, _position.Y - verticalMoveOffset, false);
        }
    }
}
