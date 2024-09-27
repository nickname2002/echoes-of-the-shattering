using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoZenith.Card;

namespace MonoZenith.Classes.Players
{
    internal class HumanPlayer : Player
    {
        public HumanPlayer(Game game, GameState state, string name) : base(game, state, name)
        {
            width = game.ScreenWidth / 2;
            height = game.ScreenHeight / 1.25f;
        }
    
        /// <summary>
        /// Draw all assets of the HumanPlayer.
        /// </summary>
        public override void Draw()
        {
            DrawHand();
        }

        /// <summary>
        /// Draw the Hand of the HumanPlayer.
        /// </summary>
        public override void DrawHand()
        {
            // Checks the current count of cards in the hand in order
            // to determine the drawing placement of the cards
            // TODO: Later, add max widthStep when count gets too high
            // In that case increase the hand width instead of reducing the distance
            // between the cards
            int count = Hand.Count;
            if (count != 0)
            {
                float widthStep = width / count;
                foreach (Card.Card card in Hand.Cards)
                {
                    float currentWidth = width + (width / 2) - (widthStep * count);
                    card.Draw(currentWidth, height, 0, false, true);
                    count--;
                }
            }
        }

        public override void PerformTurn(GameState state)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime deltaTime)
        {
            // Update every card in hand
            foreach (var card in Hand.Cards)
            {
                card.Update(deltaTime);
            }
        }
    }
}
