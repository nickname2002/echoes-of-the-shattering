using Microsoft.Xna.Framework;
using MonoZenith.Classes.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Players
{
    internal abstract class Player
    {
        public CardStack Hand;
        public string Name;

        public Player(string name)
        {
            Hand = new CardStack();
            Name = name;
        }

        /// <summary>
        /// Perform the player's turn.
        /// </summary>
        /// <param name="state">The current game state.</param>
        public abstract void PerformTurn(GameState state);

        /// <summary>
        /// Update the player.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public abstract void Update(GameTime deltaTime);

        /// <summary>
        /// Draw the player.
        /// </summary>
        public abstract void Draw();
    }
}
