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

        /// <summary>
        /// Checks if the card is valid card.
        /// </summary>
        /// <param name="previousCard">The card that was played before this one.</param>
        /// <returns>Whether the card is a valid next card.</returns>
        public override bool ValidNextCard(Card previousCard)
        {
            // Checks the typing in order to determine what rule it needs to apply.
            Type cardType = previousCard.GetType();

            if (cardType.IsSubclassOf(typeof(EffectCard)))
            {
                EffectCard effectCard = (EffectCard)previousCard;

                // If there is an ongoing effect, only power cards can be played.
                // Else, it matches on the same region, as no label can be matched.
                if (this._state.Combo != 0)
                {
                    return false;
                }
                else if (effectCard.Region == this.Region)
                {
                    return true;
                }
            }
            else if (previousCard is RegionCard regionCard)
            {
                // RegionCards matches on the same label or region.
                if (regionCard.Label == this.Label || regionCard.Region == this.Region)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Draw the card metadata.
        /// </summary>
        protected override void DrawMetaData()
        {
            float cardOffset = 15;
            
            // Draw card label
            _game.DrawText(
                $"{Label.ToString()}", 
                _position, 
                DataManager.GetInstance(_game).ComponentFont, 
                Color.Black, 
                0.5f);
            
            // Draw card region
            _game.DrawText(
                $"{Region.ToString()}",
                _position + new Vector2(0, cardOffset * 1),
                DataManager.GetInstance(_game).ComponentFont,
                Color.Black, 
                0.5f);
            
            // Draw card name
            _game.DrawText(
                $"{_name}",
                _position + new Vector2(0, cardOffset * 2),
                DataManager.GetInstance(_game).ComponentFont,
                Color.Black, 
                0.5f);
        }
    }
}
