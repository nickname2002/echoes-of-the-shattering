using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Engine.Support;

namespace MonoZenith.Screen
{
    internal class GameScreen : Screen
    {
        private readonly GameState _gameState;
        private readonly SoundEffectInstance _backgroundMusic;

        public GameScreen(Game game) : base(game)
        {
            _gameState = new GameState(game);
            _backgroundMusic = DataManager.GetInstance(game).LimgraveMusic.CreateInstance();
            _backgroundMusic.IsLooped = true;
        }

        public override void Unload()
        {
            float musicFadeOutSpeed = 0.015f;

            if (_backgroundMusic.Volume >= musicFadeOutSpeed)
            {
                _backgroundMusic.Volume -= musicFadeOutSpeed;
            }
            else
            {
                _backgroundMusic.Stop();
            }
        }
        
        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public override void Update(GameTime deltaTime)
        {
            _backgroundMusic.Play();
            _backgroundMusic.Volume = 1;
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
