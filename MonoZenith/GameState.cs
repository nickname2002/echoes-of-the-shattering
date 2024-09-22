#nullable enable
using Microsoft.Xna.Framework;
using MonoZenith.Classes.Card;
using MonoZenith.Classes.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes
{
    internal class GameState
    {
        Game _game;
        private HumanPlayer _player;
        private NpcPlayer _npc;
        private CardStack _drawableCards;
        private CardStack _playedCards;
        private Region _currentRegion;

        public GameState(Game game)
        {
            _game = game;
            _player = new HumanPlayer(game, this, "Player");
            _npc = new NpcPlayer(game, this, "NPC");
            _drawableCards = new CardStack();
            _playedCards = new CardStack();
            _currentRegion = Region.LIMGRAVE;   // TODO: Set to random region.
        }

        /// <summary>
        /// Check if there is a winner.
        /// </summary>
        /// <returns>The winning player, or null if there is no winner.</returns>
        public Player? HasWinner()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the game state.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public void Update(GameTime deltaTime)
        {
            // _player.Update(deltaTime);
            // _npc.Update(deltaTime);
        }

        /// <summary>
        /// Draw the game state.
        /// </summary>
        public void Draw()
        {
            // _player.Draw();
            // _npc.Draw();
        }
    }
}
