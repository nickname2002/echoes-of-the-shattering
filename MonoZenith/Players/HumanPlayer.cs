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
            height = game.ScreenHeight / 1.3f;
        }
    
        /// <summary>
        /// Draw the hand of the Human Player
        /// </summary>
        public override void Draw()
        {
            int count = Hand.Count;
            float widthStep = width / count;
            foreach(Card.Card card in Hand.Cards)
            {
                float currentWidth = width + (width/2) - (widthStep * count); 
                card.Draw(currentWidth, height);
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
