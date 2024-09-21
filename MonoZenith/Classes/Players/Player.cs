using MonoZenith.Classes.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Players
{
    internal abstract class Player
    {
        public CardStack hand;
        public string name;

        public abstract void PerformTurn(GameState state);

    }
}
