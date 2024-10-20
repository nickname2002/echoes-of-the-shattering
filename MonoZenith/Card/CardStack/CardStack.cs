using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

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
        /// Clear the stack of all cards.
        /// </summary>
        public void Clear()
        {
            _cards.Clear();
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
            this._position = new Vector2(x, y);
            
            // Update the position of the contained cards
            foreach (var card in _cards)
            {
                card.ChangePosition(
                    _position.X - _cards[0].Width / 2 * _cards[0].Scale, 
                    _position.Y - _cards[0].Height / 2 * _cards[0].Scale);
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
        /// Draw the stack.
        /// </summary>
        public void Draw()
        {
            if (!_cards.Any()) 
                return;
            
            // If the deck is a subclass of CardStack,
            // draw the cards face down
            if (GetType().IsSubclassOf(typeof(CardStack)))
            {
                Card currentCard = _cards[0];
                currentCard.Draw(_position.X, _position.Y);
            }
            else
            {
                Card currentCard = _cards.Last();
                currentCard.Draw(_position.X, _position.Y, 0, true, true);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var card in _cards)
            {
                card.Update(gameTime);
            }
        }
    }
}
