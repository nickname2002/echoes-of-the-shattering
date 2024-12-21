using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Engine.Support;

namespace MonoZenith.Screen
{
    public class GameScreen : Screen
    {
        private SoundEffectInstance _backgroundMusic;

        public GameState GameState { get; }

        public GameScreen()
        {
            GameState = new GameState(Game.Instance);
            _backgroundMusic = DataManager.GetInstance().LimgraveMusic.CreateInstance();
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
            Game.StartFadeIn();
            
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
