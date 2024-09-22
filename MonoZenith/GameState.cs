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
        private HumanPlayer _player;
        private NpcPlayer _npc;
        private CardStack _drawableCards;
        private CardStack _playedCards;
        private Region _currentRegion;

        /// <summary>
        /// Check if there is a winner.
        /// </summary>
        /// <returns>The winning player, or null if there is no winner.</returns>
        public Player? HasWinner()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            _player = new HumanPlayer("player1");
            _npc = new NpcPlayer("cpu");
            _drawableCards = new CardStack();
            _playedCards = new CardStack();
            _currentRegion = Region.LIMGRAVE;   // TODO: Set to random region.
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
