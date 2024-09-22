using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Card
{
    internal class CardStack
    {
        private List<Card> _cards = new List<Card>();
        public List<Card> Cards => _cards;

        public CardStack()
        {
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
            throw new NotImplementedException();
        }
    }
}
