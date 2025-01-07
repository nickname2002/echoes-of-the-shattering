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
        protected GameState _state;
        protected Vector2 _position;
        protected List<Card> _cards = new();
        public List<Card> Cards => _cards;
        public int Count => _cards.Count;
        private bool _horizontalStack;
        
        public CardStack(GameState state, bool horizontalStack = false)
        {
            _state = state;
            _horizontalStack = horizontalStack;
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
        /// Determine if there are any cards in the stack that can be played.
        /// </summary>
        /// <returns>True if there is at least one card that can be played; otherwise, false.</returns>
        public List<Card> GetMovingCards() => _cards.Where(card => card.IsMoving).ToList();
        
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
            card.Stack = this;
            card.IsTransferringToExternalStack = true;
            card.UpdatePosition(_position.X, _position.Y); 
        }

        /// <summary>
        /// Add a list of cards to the top of the stack.
        /// </summary>
        /// <param name="cardList">The list of cards to add.</param>
        public void AddToFront(List<Card> cardList)
        {
            _cards.InsertRange(0, cardList);
            foreach (Card card in cardList)
            {
                card.Stack = this;
                card.IsTransferringToExternalStack = true;
                card.UpdatePosition(_position.X, _position.Y);
            }
        }

        /// <summary>
        /// Add a card to the bottom of the stack.
        /// </summary>
        /// <param name="card">The card to add.</param>
        public void AddToBottom(Card card)
        {
            _cards.Add(card);
            card.IsTransferringToExternalStack = true;
            card.Stack = this;
            card.UpdatePosition(_position.X, _position.Y); 
        }

        /// <summary>
        /// Add a list of cards to the bottom of the stack.
        /// </summary>
        /// <param name="cardList">The list of cards to add.</param>
        public void AddToBottom(List<Card> cardList)
        {
            _cards.AddRange(cardList);
            foreach (Card card in cardList)
            {
                card.Stack = this;
                card.IsTransferringToExternalStack = true;
                card.UpdatePosition(_position.X, _position.Y); 
            }
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
            if (_cards.Count == 0) return null;
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

        public virtual void Update(GameTime deltaTime)
        {
            // Retrieve spacing and starting position of the cards
            int cardCount = _cards.Count;
            float spacing, startX;
            (spacing, startX) = CalculateCardPosition(cardCount);
            
            for (int i = 0; i < cardCount; i++)
            {
                // Calculate the target position of the current card for centering
                Card currentCard = _cards[i];
                float cardX = startX + i * spacing;
                
                // Set the target position for the card
                currentCard.UpdatePosition(_horizontalStack ? cardX : _position.X, _position.Y);

                // Update the card's position towards the target position
                currentCard.Update(deltaTime);
            }
        }

        protected (float, float) CalculateCardPosition(int cardCount)
        {
            float cardWidth = Card.Width;

            // Define the spacing between cards
            float offset = 30 * AppSettings.Scaling.ScaleFactor;
            float spacing = cardWidth + offset;

            // Calculate the total width occupied by all cards including spacing
            float totalWidth = cardCount * spacing - offset;

            // Calculate the starting position to center the cards
            float startX = _position.X - totalWidth / 2;

            return (spacing, startX);
        }

        /// <summary>
        /// Update the position of the stack.
        /// </summary>
        /// <param name="position">The new position of the stack.</param>
        public void SetPosition(Vector2 position)
        {
            _position = position;
            
            foreach (var card in _cards)
            {
                card.SetPosition(position);
            }
        }

        /// <summary>
        /// Updates the position of the stack.
        /// </summary>
        /// <param name="x">Positional X</param>
        /// <param name="y">Positional Y</param>
        public void UpdatePosition(float x, float y)
        {
            _position = new Vector2(x, y);

            // Update the position of the contained cards
            foreach (var card in _cards)
            {
                card.SetPosition(new Vector2(_position.X, _position.Y));
            }
        }

        /// <summary>
        /// Draw the stack with all of its cards, centered on the stack's position.
        /// </summary>
        public void Draw()
        {
            if (!_cards.Any())
                return;

            foreach (var card in _cards)
            {
                if (card.Owner is NpcPlayer)
                {
                    card.Draw();
                }
                else
                {
                    card.Draw(0, true);
                }
            }
        }
    }
}
