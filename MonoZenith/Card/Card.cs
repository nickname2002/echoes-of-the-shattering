using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoZenith;

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

        public Card(Game game, GameState state, Vector2 position, Texture2D texture, string name)
        {
            _game = game;
            _state = state;
            _position = position;
            _width = 0;
            _height = 0;
            _texture = texture;
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
        /// Draw the card.
        /// </summary>
        public void Draw(float width, float height, float angle = 0)
        {
            Vector2 currentPos = _position + new Vector2(width, height);
            float scale = 0.2f;
            _game.DrawImage(_texture, currentPos, scale, angle);
        }
    }
}
