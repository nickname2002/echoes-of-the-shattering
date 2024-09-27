using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;

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
            Type type = previousCard.GetType();

            if (previousCard is RegionCard regionCard)
            {
                if (regionCard.Label == this.Label || regionCard.Region == this.Region)
                {
                    return true;
                }
            }
            else if (previousCard is GraceCard graceCard)
            {
                if (graceCard.Region == this.Region)
                {
                    return true;
                }
            }
            else if (type.IsSubclassOf(typeof(EffectCard)))
            {
                EffectCard effectCard = (EffectCard)previousCard;

                if (this._state.Combo == 0)
                {
                    return false;
                }
                else if (effectCard.Region == this.Region)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void DrawMetaData(Vector2 position)
        {
            float cardOffset = 15;
            
            // Draw card label
            _game.DrawText(
                $"{Label.ToString()}", 
                position, 
                DataManager.GetInstance(_game).ComponentFont, 
                Color.Black, 
                0.5f);
            
            // Draw card region
            _game.DrawText(
                $"{Region.ToString()}",
                position + new Vector2(0, cardOffset * 1),
                DataManager.GetInstance(_game).ComponentFont,
                Color.Black, 
                0.5f);
            
            // Draw card name
            _game.DrawText(
                $"{_name}",
                position + new Vector2(0, cardOffset * 2),
                DataManager.GetInstance(_game).ComponentFont,
                Color.Black, 
                0.5f);
        }
    }
}
