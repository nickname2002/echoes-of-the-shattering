using System;
using Microsoft.Xna.Framework;

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

        public override void PerformTurn(GameState state)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
