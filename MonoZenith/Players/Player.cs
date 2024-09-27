using Microsoft.Xna.Framework;
using MonoZenith.Card;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Players
{
    internal abstract class Player
    {
        private Game _game;
        protected GameState _state;
        public CardStack Hand;
        public string Name;
        public float width;
        public float height;

        protected Player(Game game, GameState state, string name)
        {
            _game = game;
            _state = state;
            Hand = new CardStack(_game, _state);
            Name = name;
        }

        public override string ToString()
        {
            return $"==== {Name} ====\n{Hand}\n";
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

        /// <summary>
        /// Draw tha hand of the player.
        /// </summary>
        public abstract void DrawHand();
    }
}
