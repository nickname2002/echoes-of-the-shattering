using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoZenith;
using static System.Formats.Asn1.AsnWriter;

namespace MonoZenith.Card
{
    internal abstract class Card
    {
        protected Game _game;
        protected GameState _state;
        protected Vector2 _position;
        protected int _width;
        protected int _height;
        protected Texture2D _texture;
        protected string _name;
        // TODO: Ensure cards are able to show both back and front sides

        public Card(Game game, GameState state, Vector2 position, Texture2D texture, string name)
        {
            _game = game;
            _state = state;
            _position = position;
            _texture = texture;
            _width = texture.Width;
            _height = texture.Height;
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Draw the card.
        /// </summary>
        /// <param name="previousCard">The card that was played before this one.</param>
        /// <returns>Whether the card is a valid next card.</returns>
        public abstract bool ValidNextCard(Card previousCard);

        /// <summary>
        /// Draw the Card
        /// </summary>
        /// <param name="width">Width Position</param>
        /// <param name="height">Height Position</param>
        /// <param name="angle">Rotational Angle</param>
        public void Draw(float width, float height, float angle = 0, bool offset = true)
        {
            float scale = 0.2f;
            float newWidth = 0;
            float newHeight = 0;
            if (offset)
            {
                newWidth = width - (_width * scale / 2);
                newHeight = height - (_height * scale / 2);
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }
            
            Vector2 currentPos = _position + new Vector2(newWidth, newHeight);
            _game.DrawImage(_texture, currentPos, scale, angle);
        }
    }
}
