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

        public Player HasWinner()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

    }
}
