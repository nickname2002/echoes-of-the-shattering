#nullable enable
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonoZenith.Screen
{
    public class GameScreen : Screen
    {
        private SoundEffectInstance? _backgroundMusic;
        public GameState GameState { get; } = new(Game.Instance);

        public SoundEffectInstance? BackgroundMusic => _backgroundMusic;

        public void SetBackgroundMusic(SoundEffectInstance music) => _backgroundMusic = music;

        public override void Unload(float fadeSpeed = 0.015f, Action? onUnloadComplete = null)
        {
            float musicFadeOutSpeed = fadeSpeed;

            if (_backgroundMusic != null && _backgroundMusic.Volume >= musicFadeOutSpeed)
            {
                _backgroundMusic.Volume -= musicFadeOutSpeed;
            }
            else
            {
                if (_backgroundMusic != null) _backgroundMusic.Stop();
                onUnloadComplete?.Invoke();
            }
        }

        public override void Load()
        {
            GameState.InitializeState();
            Game.StartFadeIn();

            float musicFadeInSpeed = 0.015f;
            if (_backgroundMusic != null && _backgroundMusic.Volume <= 1 - musicFadeInSpeed)
            {
                _backgroundMusic.Volume += musicFadeInSpeed;
            }
            else
            {
                if (_backgroundMusic != null) _backgroundMusic.Volume = 1;
            }
        }

        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public override void Update(GameTime deltaTime)
        {
            if (_backgroundMusic != null && 
                !(Game.IsFadingOut || Game.IsFadingIn))
            {
                _backgroundMusic.Volume = 0.5f;
                _backgroundMusic.Play();
            }

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