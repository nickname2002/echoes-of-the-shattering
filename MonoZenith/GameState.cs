#nullable enable
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using MonoZenith.Players;
using MonoZenith.Screen.RewardPanel;
using MonoZenith.Support.Managers;

namespace MonoZenith
{
    public class GameState
    {
        public readonly Game Game;
        public GameTime GameTime;
        public readonly TurnManager TurnManager;
        public readonly GameOverManager GameOverManager;
        public HumanPlayer Player;
        public NpcPlayer Npc;
        public Reward Reward;
        public readonly CardStack PlayedCards;
        private Texture2D? _backdrop;

        public GameState(Game game)
        {
            Game = game;
            GameTime = new GameTime();
            TurnManager = new TurnManager(Game, this);
            GameOverManager = new GameOverManager(Game);
            Player = new HumanPlayer(this, "Player");
            Npc = new NpcPlayer(this, "NPC");
            Reward = new Reward(
                DataManager.GetInstance().WolvesAsh,
                "Wolves Spirit Ash",
                typeof(WolvesAsh));
            PlayedCards = new CardStack(this, true);
            InitializeState();
        }

        /// <summary>
        /// Set the current level
        /// </summary>
        /// <param name="level">The level</param>
        public void SetLevel(Level level)
        {
            Npc = level.Enemy;
            _backdrop = level.Backdrop;
            Reward = level.Reward;
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
            
            if (GameOverManager.HasWinner(Player, Npc) != null)
            {
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
            
            if (GameOverManager.HasWinner(Player, Npc) != null)
            {
                GameOverManager.DrawTransitionComponent();

                if (GameOverManager is not { Winner: HumanPlayer, TransitionComplete: true }) 
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
