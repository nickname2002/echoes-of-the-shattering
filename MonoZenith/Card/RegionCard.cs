using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Support;

namespace MonoZenith.Card
{
    public class RegionCard : Card
    {
        public readonly CardLabel Label; 
        public readonly Region Region;
        protected bool _isComboCard;
        public bool IsComboCard => _isComboCard;

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
            _isComboCard = false;
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
            var prevCard = previousCard as RegionCard;

            // RegionCards matches on the same label or region.
            return (prevCard == null
                    || prevCard.Label == Label
                    || Region == _state.CurrentRegion
                    || Region == Region.ALL);
        }
        
        /// <summary>
        /// Perform the effect of the card.
        /// </summary>
        /// <param name="state">The current game state.</param>
        public virtual void PerformEffect(GameState state)
        {
            state.SwitchTurn();
        }
        
        // TODO: Add card for changing region for power card when in combo mode.
        
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
