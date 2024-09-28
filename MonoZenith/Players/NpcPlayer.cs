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
        /// Strategy to play when the opposing player has less than 4 cards.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OffensiveStrategy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Strategy to play when the opposing player has more than 3 cards.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void NormalStrategy()
        {
            throw new NotImplementedException();
        }
            
        public override void PerformTurn(GameState state)
        {
            if (Hand.Cards.Any(card => TryPlayCard()))
            {
                return;
            }

            TryDrawCard();
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
            var drawnCard = _state.DrawableCards.GetCard();
            Console.WriteLine($"{Name} drew: {drawnCard}");
            Hand.AddToFront(drawnCard);
            _state.SwitchTurn();    // TODO: may not be true in every situation
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
