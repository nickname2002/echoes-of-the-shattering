using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoZenith.Card
{
    internal class CardStack
    {
        protected Game _game;
        protected GameState _state;
        protected Vector2 _position;
        protected List<Card> _cards = new List<Card>();
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
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
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
        }

        /// <summary>
        /// Draw the stack.
        /// </summary>
        public void Draw()
        {
            if (_cards.Any())
            {
                // If the deck is a subclass of CardStack,
                // draw the cards face down
                if (this.GetType().IsSubclassOf(typeof(CardStack)))
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
        }
    }
}
