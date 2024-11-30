#nullable enable
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Card.CardStack;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith
{
    public class GameState
    {
        public readonly Game Game;
        public GameTime GameTime;
        
        private readonly TransitionComponent _gameOverTransitionComponent;
        private Player? _currentWinner;
        
        public readonly TurnManager TurnManager;
        
        public readonly HumanPlayer Player;
        public readonly NpcPlayer Npc;
        
        public readonly CardStack PlayedCards;
        
        private readonly SoundEffectInstance _playerDeathSound;
        private readonly SoundEffectInstance _enemyDeathSound;

        public GameState(Game game)
        {
            Game = game;
            GameTime = new GameTime();
         
            TurnManager = new TurnManager(Game, this);
            
            Player = new HumanPlayer(Game, this, "Player");
            Npc = new NpcPlayer(Game, this, "NPC");
            
            var gameOverTransitionComponentFont = DataManager.GetInstance(Game).GameOverTransitionComponentFont; 
            _gameOverTransitionComponent = new TransitionComponent(
                Game, "YOU DIED", new Color(255, 215, 0), gameOverTransitionComponentFont,
                1f, 3f, 1f,
                Game.BackToMainMenu);
            
            PlayedCards = new CardStack(Game, this, true);
            _playerDeathSound = DataManager.GetInstance(game).PlayerDeathSound.CreateInstance();
            _enemyDeathSound = DataManager.GetInstance(game).EnemyDeathSound.CreateInstance();
            InitializeState();
        }
        
        /// <summary>
        /// Initialize the game state.
        /// </summary>
        public void InitializeState()
        {
            _currentWinner = null;
            TurnManager.InitializeState(Player, Npc);
            PlayedCards.Clear();
            
            // Update the position of the played cards
            PlayedCards.UpdatePosition(
                Game.ScreenWidth / 2f, 
                Game.ScreenHeight / 2f - Card.Card.Height / 2f);

            _gameOverTransitionComponent.Reset();
            
            // Initialize players
            Player.InitializeState(Game, this);
            Npc.InitializeState(Game, this);
        }

        /// <summary>
        /// Check if there is a winner.
        /// </summary>
        /// <returns>The winning player, or null if there is no winner.</returns>
        public Player? HasWinner()
        {
            if (Npc.Health < 1)
            {
                if (_currentWinner == null)
                    _enemyDeathSound.Play();

                _currentWinner = Player;
                _gameOverTransitionComponent.Content = "ENEMY FELLED";
                _gameOverTransitionComponent.Color = new Color(255, 215, 0);
                return Player;
            }

            if (Player.Health > 0)
                return null;

            if (_currentWinner == null)
                _playerDeathSound.Play();

            _currentWinner = Npc;
            _gameOverTransitionComponent.Content = "YOU DIED";
            _gameOverTransitionComponent.Color = new Color(180, 30, 30);
            return Npc;
        }

        /// <summary>
        /// Displays the game over message.
        /// </summary>
        private void DisplayGameOverMessage()
        {
            _gameOverTransitionComponent.Draw();
        }
        
        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public void Update(GameTime deltaTime)
        {
            GameTime = deltaTime;
            
            // Make sure all cards are updated
            Player.Update(deltaTime);
            Npc.Update(deltaTime);
            PlayedCards.Update(deltaTime);
            
            // Update the turn manager
            TurnManager.Update(deltaTime, Player, Npc);
            if (TurnManager.SwitchingTurns) return;
            
            if (HasWinner() != null)
            {
                _gameOverTransitionComponent.Update(deltaTime);
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
                DataManager.GetInstance(Game).Backdrop, 
                Vector2.Zero, 
                AppSettings.Scaling.ScaleFactor);

            if (HasWinner() != null)
            {
                DisplayGameOverMessage();
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
