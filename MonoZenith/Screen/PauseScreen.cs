using System;
using Microsoft.Xna.Framework;
using MonoZenith.Components.MainMenuScreen;
using MonoZenith.Engine.Support;
using MonoZenith.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen
{
    internal class PauseScreen : Screen
    {
        private readonly MainMenuOptionButton _resumeButton = new(
            Instance,
            ScreenHeight / 2f,
            "Continue",
            ContinueGameState,
            DataManager.GetInstance().EndPlayerTurnSound.CreateInstance());
        private readonly MainMenuOptionButton _overworldButton = new(
            Instance,
            ScreenHeight / 2f + 75 * AppSettings.Scaling.ScaleFactor,
            "Overworld",
            BackToOverworld,
            DataManager.GetInstance().StartButtonSound.CreateInstance());

        private static void ContinueGameState()
        {
            var stateBeforePause = GetGameScreen().GameState.StateBeforePause;
            GetGameScreen().GameState.StateType = stateBeforePause;
            
            if (stateBeforePause 
                is GameStateType.PlayingStartingVoiceLines 
                or GameStateType.PlayingDeathVoiceLines 
                or GameStateType.PlayingVictoryVoiceLines)
            {
                GetGameScreen().GameState.VoiceLineManager.ResumeVoiceLines();
            }
        }
        
        public override void Unload(float fadeSpeed = 0.015f, Action onUnloadComplete = null) { }
        public override void Load() { }

        public override void Update(GameTime deltaTime)
        {
            _resumeButton.Update(deltaTime);
            _overworldButton.Update(deltaTime);
        }

        public override void Draw()
        {
            DrawText(
                "Paused",
                new Vector2(
                    ScreenWidth / 2f - DataManager.GetInstance().HeaderFont.MeasureString("Paused").X / 2f, 
                    ScreenHeight / 2f - 150 * AppSettings.Scaling.ScaleFactor),
                DataManager.GetInstance().HeaderFont,
                Color.White);
            
            _resumeButton.Draw();
            _overworldButton.Draw();
        }
    }
}
