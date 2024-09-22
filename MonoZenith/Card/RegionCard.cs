using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Classes.Card
{
    internal class RegionCard : Card
    {
        public readonly CardLabel Label; 
        public readonly Region Region;
        private Vector2 position;
        private int width;
        private int height;
        private string name;

        public RegionCard(Game game, GameState state, Vector2 position, int width, int height, Texture2D texture, string name, CardLabel label, Region region)
            : base(game, state, position, width, height, texture, name)
        {
            Label = label;
            Region = region; 
        }

        public override bool ValidNextCard(Card previousCard)
        {
            throw new NotImplementedException();
        }
    }
}
