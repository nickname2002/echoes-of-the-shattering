using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Engine.Support;
using MonoZenith.Support.Managers;

namespace MonoZenith.Screen
{
    public class GameScreen : Screen
    {
        private SoundEffectInstance _backgroundMusic;

        public GameState GameState { get; }

        public GameScreen(Game game) : base(game)
        {
            GameState = new GameState(game);
            _backgroundMusic = DataManager.GetInstance(game).LimgraveMusic.CreateInstance();
            _backgroundMusic.IsLooped = true;
        }
        
        public void SetBackgroundMusic(SoundEffectInstance music) => _backgroundMusic = music;

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

        public override void Load()
        {
            GameState.InitializeState();
            _game.StartFadeIn();
            
            float musicFadeInSpeed = 0.015f;
            if (_backgroundMusic.Volume <= 1 - musicFadeInSpeed)
            {
                _backgroundMusic.Volume += musicFadeInSpeed;
            }
            else
            {
                _backgroundMusic.Volume = 1;
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
            GameState.Update(deltaTime);
        }

        /// <summary>
        /// Draw the game state.
        /// </summary>
        public override void Draw()
        {
            GameState.Draw();
        }
    }
}
