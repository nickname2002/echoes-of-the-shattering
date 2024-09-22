using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoZenith.Classes.Players
{
    internal class NpcPlayer : Player
    {
        public NpcPlayer(Game game, GameState state, string name) : base(game, state, name)
        {
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void PerformTurn(GameState state)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
