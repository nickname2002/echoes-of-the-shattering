#nullable enable
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Screen.RewardPanel;
using MonoZenith.Support.Managers;
using System.Collections.Generic;

namespace MonoZenith
{
    public class GameState
    {
        public readonly Game Game;
        public GameTime GameTime;
        public readonly TurnManager TurnManager;
        public readonly GameOverManager GameOverManager;
        public readonly HumanPlayer Player;
        public NpcPlayer Npc;
        public Reward? Reward;
        public readonly CardStack PlayedCards;
        private Texture2D? _backdrop;
        private GameStateType _stateType;
        private Queue<SoundEffectInstance>? _voiceQueue;
        private SoundEffectInstance? _currentPlayingVoiceLine;
        
        public Level? CurrentLevel { get; private set; }
        public GameStateType StateType => _stateType;

        public GameState(Game game)
        {
            Game = game;
            GameTime = new GameTime();
            TurnManager = new TurnManager(Game, this);
            GameOverManager = new GameOverManager(Game);
            Player = new HumanPlayer(this, "Player");
            Npc = new NpcPlayer(this, "NPC", DataManager.GetInstance().DefaultEnemyPortrait);
            PlayedCards = new CardStack(this, true);
            _stateType = GameStateType.PlayingStartingVoiceLines;
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
            Reward = level.Reward;

            InitializeVoiceQueue(level.VoiceLinesBattleStart);
            PlayNextVoiceLine();
            _stateType = GameStateType.PlayingStartingVoiceLines;

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
        
        public bool PlayingVoiceLines => _stateType 
                is GameStateType.PlayingStartingVoiceLines 
                or GameStateType.PlayingDeathVoiceLines 
                or GameStateType.PlayingVictoryVoiceLines;

        private void UpdateStartingVoiceLines()
        {
            if (_currentPlayingVoiceLine?.State == SoundState.Playing) return;

            if (_voiceQueue is { Count: > 0 })
            {
                PlayNextVoiceLine();
                return;
            }

            _stateType = GameStateType.InGame;
        }

        private void UpdateDeathVoiceLines()
        {
            if (_currentPlayingVoiceLine?.State == SoundState.Playing) return;

            if (_voiceQueue is { Count: > 0 })
            {
                PlayNextVoiceLine();
                return;
            }

            _stateType = GameStateType.EndGame;
        }

        private void UpdateVictoryVoiceLines()
        {
            if (_currentPlayingVoiceLine?.State == SoundState.Playing) return;

            if (_voiceQueue is { Count: > 0 })
            {
                PlayNextVoiceLine();
                return;
            }

            _stateType = GameStateType.EndGame;
        }

        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public void Update(GameTime deltaTime)
        {
            GameTime = deltaTime;
            
            switch (_stateType)
            {
                case GameStateType.PlayingStartingVoiceLines:
                    UpdateStartingVoiceLines();
                    return;
                case GameStateType.InGame:
                    break;
                case GameStateType.PlayingDeathVoiceLines:
                    UpdateDeathVoiceLines();
                    return;
                case GameStateType.PlayingVictoryVoiceLines:
                    UpdateVictoryVoiceLines();
                    return;
                case GameStateType.EndGame:
                    break;
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
                if (_stateType == GameStateType.InGame)
                {
                    if (winner is HumanPlayer)
                    {
                        _stateType = GameStateType.PlayingVictoryVoiceLines;
                        InitializeVoiceQueue(LevelManager.CurrentLevel.VoiceLinesBattleVictory); 
                    }
                    else
                    {
                        _stateType = GameStateType.PlayingDeathVoiceLines;
                        InitializeVoiceQueue(LevelManager.CurrentLevel.VoiceLinesBattleLoss); 
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
                AppSettings.Scaling.ScaleFactor);

            // Draw player UI
            Player.DrawPlayerHealthAndName();
            Npc.DrawPlayerHealthAndName();
            Player.DrawPlayerUi();
            Npc.DrawPlayerUi();
            
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

        /// <summary>
        /// Initialize a new voice queue.
        /// </summary>
        /// <param name="voiceLines">The list of voice lines to initialize the queue with.</param>
        private void InitializeVoiceQueue(List<SoundEffectInstance>? voiceLines)
        {
            if (voiceLines == null || voiceLines.Count == 0)
            {
                _voiceQueue = null; 
                return;
            }

            _voiceQueue = new Queue<SoundEffectInstance>(voiceLines);
            _currentPlayingVoiceLine = null; 
        }

        /// <summary>
        /// Play the next voice line in the queue.
        /// </summary>
        private void PlayNextVoiceLine()
        {
            if (_voiceQueue == null || _voiceQueue.Count == 0)
            {
                _currentPlayingVoiceLine = null; 
                return;
            }

            _currentPlayingVoiceLine = _voiceQueue.Dequeue();
            _currentPlayingVoiceLine.Play();
        }
    }

    public enum GameStateType
    {
        PlayingStartingVoiceLines,
        InGame,
        PlayingDeathVoiceLines,
        PlayingVictoryVoiceLines,
        EndGame
    }
}