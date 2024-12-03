#nullable enable
using Microsoft.Xna.Framework;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith
{
    public class GameState
    {
        public readonly Game Game;
        public GameTime GameTime;
        public readonly TurnManager TurnManager;
        public readonly GameOverManager GameOverManager;
        public readonly HumanPlayer Player;
        public readonly NpcPlayer Npc;
        public readonly CardStack PlayedCards;

        public GameState(Game game)
        {
            Game = game;
            GameTime = new GameTime();
            TurnManager = new TurnManager(Game, this);
            GameOverManager = new GameOverManager(Game);
            Player = new HumanPlayer(Game, this, "Player");
            Npc = new NpcPlayer(Game, this, "NPC");
            PlayedCards = new CardStack(Game, this, true);
            InitializeState();
        }
        
        /// <summary>
        /// Initialize the game state.
        /// </summary>
        public void InitializeState()
        {
            TurnManager.InitializeState(Player, Npc);
            GameOverManager.InitializeState();
            PlayedCards.Clear();
            
            // Update the position of the played cards
            PlayedCards.UpdatePosition(
                Game.ScreenWidth / 2f, 
                Game.ScreenHeight / 2f - Card.Card.Height / 2f);
            
            // Initialize players
            Player.InitializeState(Game, this);
            Npc.InitializeState(Game, this);
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
                GameOverManager.UpdateGameOverTransition(deltaTime);
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
            
            if (GameOverManager.HasWinner(Player, Npc) != null)
            {
                GameOverManager.DisplayGameOverMessage();
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
