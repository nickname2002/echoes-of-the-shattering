#nullable enable
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Screen;
using MonoZenith.Screen.RewardPanel;
using MonoZenith.Support.Managers;
using MonoZenith.Support;

namespace MonoZenith
{
    public class GameState
    {
        /// <summary>
        /// Game and properties
        /// </summary>
        public readonly Game Game;
        public GameTime GameTime;
        
        /// <summary>
        /// Managers
        /// </summary>
        public readonly TurnManager TurnManager;
        public readonly GameOverManager GameOverManager;
        public readonly VoiceLineManager VoiceLineManager;
        
        /// <summary>
        /// Players
        /// </summary>
        public readonly HumanPlayer Player;
        public NpcPlayer Npc;
        
        public Reward? Reward;
        public readonly CardStack PlayedCards;
        private Texture2D? _backdrop;
        
        /// <summary>
        /// Pause screen
        /// </summary>
        private readonly PauseScreen _pauseScreen;
        private const float PauseToggleCooldown = 100;
        private float _pauseToggleTimer;
        
        public Level? CurrentLevel { get; private set; }
        public GameStateType StateType { get; set; }
        public GameStateType StateBeforePause { get; set; }

        public GameState(Game game)
        {
            Game = game;
            GameTime = new GameTime();
            TurnManager = new TurnManager(Game, this);
            GameOverManager = new GameOverManager();
            VoiceLineManager = new VoiceLineManager();
            Player = new HumanPlayer(this, "Tarnished");
            Npc = new NpcPlayer(this, "NPC", DataManager.GetInstance().DefaultEnemyPortrait);
            PlayedCards = new CardStack(this, true);
            StateType = GameStateType.PlayingStartingVoiceLines;
            StateBeforePause = StateType;
            _pauseScreen = new PauseScreen();
            _pauseToggleTimer = 0;
            InitializeState();
        }
        
        /// <summary>
        /// Set the current level
        /// </summary>
        /// <param name="level">The level</param>
        public void SetLevel(Level level)
        {
            CurrentLevel = level;
            Npc = level.Enemy;
            _backdrop = level.Backdrop;
            Reward = level.LevelReward;

            VoiceLineManager.InitializeVoiceQueue(level.VoiceLinesBattleStart);
            StateType = GameStateType.PlayingStartingVoiceLines;
            StateBeforePause = StateType;

            InitializeState();
        }

        /// <summary>
        /// Initialize the game state.
        /// </summary>
        public void InitializeState()
        {
            TurnManager.InitializeState(Player, Npc);
            GameOverManager.InitializeState(Reward);
            PlayedCards.Clear();

            // Update the position of the played cards
            PlayedCards.UpdatePosition(
                Game.ScreenWidth / 2f,
                Game.ScreenHeight / 2f - Card.Card.Height / 2f);

            // Initialize players
            Player.InitializeState(this);
            Npc.InitializeState(this);
        }
        
        /// <summary>
        /// If the escape key is pressed, trigger the paused state.
        /// For the pause state to be triggered, additional conditions must be met.
        /// </summary>
        private void TryTriggerPausedState(GameTime deltaTime)
        {
            if (_pauseToggleTimer > 0)
            {
                _pauseToggleTimer -= deltaTime.ElapsedGameTime.Milliseconds;
                return;
            }
            
            if (!Game.GetKeyDown(Keys.Escape) || GameOverManager.HasWinner(Player, Npc) != null) return;
            _pauseToggleTimer = PauseToggleCooldown;
            
            if (StateType == GameStateType.Paused)
            {
                StateType = StateBeforePause;
                VoiceLineManager.ResumeVoiceLines();
                return;
            }
            
            StateBeforePause = StateType;
            StateType = GameStateType.Paused;
            VoiceLineManager.PauseVoiceLines();
        }
        
        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public void Update(GameTime deltaTime)
        {
            GameTime = deltaTime;
            TryTriggerPausedState(deltaTime);
            
            switch (StateType)
            {
                case GameStateType.PlayingStartingVoiceLines:
                    VoiceLineManager.UpdateStartingVoiceLines();
                    return;
                case GameStateType.InGame:
                    break;
                case GameStateType.PlayingDeathVoiceLines:
                    VoiceLineManager.UpdateDeathVoiceLines();
                    return;
                case GameStateType.PlayingVictoryVoiceLines:
                    VoiceLineManager.UpdateVictoryVoiceLines();
                    return;
                case GameStateType.EndGame:
                    break;
                case GameStateType.Paused:
                    _pauseScreen.Update(deltaTime);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Player.Update(deltaTime);
            Npc.Update(deltaTime);
            PlayedCards.Update(deltaTime);
            TurnManager.Update(deltaTime, Player, Npc);

            if (TurnManager.SwitchingTurns) return;

            var winner = GameOverManager.HasWinner(Player, Npc);
            if (winner != null)
            {
                if (StateType == GameStateType.InGame)
                {
                    if (winner is HumanPlayer)
                    {
                        StateType = GameStateType.PlayingVictoryVoiceLines;
                        VoiceLineManager.InitializeVoiceQueue(CurrentLevel?.VoiceLinesBattleVictory); 
                    }
                    else
                    {
                        StateType = GameStateType.PlayingDeathVoiceLines;
                        VoiceLineManager.InitializeVoiceQueue(CurrentLevel?.VoiceLinesBattleLoss); 
                    }

                    return;
                }

                if (GameOverManager is { Winner: HumanPlayer, TransitionComplete: true })
                {
                    GameOverManager.UpdateRewardPanel(deltaTime);
                    return;
                }
                
                GameOverManager.UpdateTransitionComponent(deltaTime);
                return;
            }

            TurnManager.CurrentPlayer?.PerformTurn(this);
        }

        /// <summary>
        /// Draw the game state.
        /// </summary>
        public void Draw()
        {
            // Draw backdrop
            Game.DrawImage(
                _backdrop,
                Vector2.Zero,
                AppSettings.Scaling.ScaleFactor,
                color: StateType == GameStateType.Paused ? new Color(85, 85, 85) : Color.White);
            
            // Draw player UI
            Player.DrawPlayerHealthAndName();
            Npc.DrawPlayerHealthAndName();
            Player.DrawPlayerUi();
            Npc.DrawPlayerUi();
            
            if (StateType == GameStateType.Paused)
            {
                _pauseScreen.Draw();
                return;
            }
            
            if (GameOverManager.HasWinner(Player, Npc) != null)
            {
                GameOverManager.DrawTransitionComponent();

                if (GameOverManager is not { Winner: HumanPlayer, TransitionComplete: true }
                    || Reward == null)
                    return;

                GameOverManager.DrawRewardPanel();
                return;
            }

            // Draw cards in play
            PlayedCards.Draw();

            // Draw player cards
            Player.Draw();
            Npc.Draw();

            TurnManager.Draw();
        }
    }
}