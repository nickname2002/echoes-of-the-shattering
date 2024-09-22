using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoZenith.Classes.Screen
{
    internal class GameScreen : Screen
    {
        private GameState _gameState;

        public GameScreen()
        {
            _gameState = new GameState();
        }

        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public override void Update(GameTime deltaTime)
        {
            _gameState.Update(deltaTime);
        }

        /// <summary>
        /// Draw the game state.
        /// </summary>
        public override void Draw()
        {
            _gameState.Draw();
        }
    }
}
