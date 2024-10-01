using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoZenith.Card;

namespace MonoZenith.Players
{
    internal class NpcPlayer : Player
    {
        public NpcPlayer(Game game, GameState state, string name) : base(game, state, name)
        {
            width = game.ScreenWidth / 2;
            height = game.ScreenHeight / 5f;
        }

        /// <summary>
        /// Draw all assets of the NpcPlayer.
        /// </summary>
        public override void Draw()
        {
            DrawHand();
        }

        /// <summary>
        /// Draw the Hand of the NpcPlayer.
        /// </summary>
        public override void DrawHand()
        {
            int count = Hand.Count;

            if (count == 0)
                return;

            float widthStep = width / count;

            foreach (Card.Card card in Hand.Cards)
            {
                float currentWidth = width - (width / 2) + (widthStep * count);

                card.Draw(currentWidth, height, 180, false, false);
                count--;
            }
        }

        /// <summary>
        /// Orders the hand so that:
        /// - All cards of type RegionCard are first in the hand.
        /// - All cards of type JokerCard and LunarQueenRebirthCard are last in the hand.
        /// </summary>
        private void OrderHand()
        {
            // Get all RegionCards (excluding offensive cards, to be placed first)
            var regionCards = Hand.Cards
                .OfType<RegionCard>()
                .Where(card => card is not JokerCard && card is not LunarQueenRebirthCard)
                .ToList();

            // Get all offensive cards (JokerCard and LunarQueenRebirthCard, to be placed last)
            var offensiveCards = Hand.Cards
                .OfType<RegionCard>()
                .Where(card => card is JokerCard or LunarQueenRebirthCard)
                .ToList();

            // Get all remaining cards that are not RegionCards (to be placed in the middle)
            var otherCards = Hand.Cards
                .Where(card => card is not RegionCard)
                .ToList();

            // Clear the current hand
            Hand.Cards.Clear();

            // Add RegionCards first, followed by other cards, and offensive cards last
            Hand.Cards.AddRange(regionCards);
            Hand.Cards.AddRange(otherCards);
            Hand.Cards.AddRange(offensiveCards);
        }
        
        /// <summary>
        /// Strategy to play when the opposing player has less than 4 cards.
        /// </summary>
        /// <returns>If strategy could be executed.</returns>
        private bool TryOffensiveStrategy()
        {
            List<RegionCard> offensiveCards = Hand.Cards.OfType<RegionCard>().Where(
                card => card is LunarQueenRebirthCard or JokerCard).ToList();
            
            // Play a switch card if available
            foreach (RegionCard card in offensiveCards)
            {
                if (card is not LunarQueenRebirthCard || IsValidPlay(card)) 
                    continue;
                
                PlayCard(card);
                return true;
            }
            
            // Else, play joker card if available
            foreach (RegionCard card in offensiveCards)
            {
                if (card is not JokerCard) 
                    continue;
                
                PlayCard(card);
                return true;
            }
            
            return false;
        }
            
        protected override bool TryPlayCard()
        {
            foreach (var card in Hand.Cards)
            {
                if (!IsValidPlay(card)) 
                    continue;
                
                PlayCard(card);
                return true;
            }

            return false;
        }

        protected override void TryDrawCard()
        {
            if (_state.Combo >= 1)
            {
                return;
            }
            
            var drawnCard = _state.DrawableCards.GetCard();
            Console.WriteLine($"{Name} drew: {drawnCard}");
            Hand.AddToFront(drawnCard);
            _state.SwitchTurn();    // TODO: may not be true in every situation
        }
        
        public override void PerformTurn(GameState state)
        {
            base.PerformTurn(_state);
            OrderHand();
            int cardsInOpponentHand = state.OpposingPlayer.Hand.Count;

            // If the opponent has less than 4 cards, try to play offensive cards.
            if (cardsInOpponentHand < 4)
            {
                if (TryOffensiveStrategy())
                {
                    return;
                }
            }

            // If the situation is stable, play the first card available.
            if (TryPlayCard())
                return;

            // If no card can be played, draw a card.
            TryDrawCard();
        }
        
        public override void Update(GameTime deltaTime)
        {
            foreach (var card in Hand.Cards)
            {
                card.Update(deltaTime);
            }
        }
    }
}
