using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Card
{
    internal class RegionCard : Card
    {
        public readonly CardLabel Label; 
        public readonly Region Region;

        public RegionCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            CardLabel label,
            Region region)
            : base(game, state, position, texture, activeTexture, name)
        {
            Label = label;
            Region = region; 
        }

        public override string ToString()
        {
            return $"{_name} ({Region}) - {Label}";
        }

        public override bool ValidNextCard(Card previousCard)
        {
            throw new NotImplementedException();
        }
    }
}
