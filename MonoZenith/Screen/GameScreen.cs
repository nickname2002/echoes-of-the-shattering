using Microsoft.Xna.Framework;

namespace MonoZenith.Screen
{
    internal class GameScreen : Screen
    {
        private GameState _gameState;

        public GameScreen(Game game) : base(game)
        {
            _gameState = new GameState(game);
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
