using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Card
{
    internal class CardStack
    {
        public List<Card> cards = new List<Card>();

        public void AddToFront(Card card)
        {
            cards.Insert(0, card);
        }

        public void AddToFront(List<Card> cardList)
        {
            cards.InsertRange(0, cardList);
        }
        public void AddToBottom(Card card)
        {
            cards.Add(card);
        }

        public void AddToBottom(List<Card> cardList)
        {
            cards.AddRange(cardList);
        }

        public Card Pop()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public void Shuffle()
        {
            return;
        }

    }
}
