using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Card
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

        public Card(Game game, GameState state, Vector2 position, int width, int height, Texture2D texture, string name)
        {
            _game = game;
            _state = state;
            _position = position;
            _width = width;
            _height = height;
            _texture = texture;
            _name = name;
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
        public void Draw()
        {
            _game.DrawImage(_texture, _position, _width, _height);
        }
    }
}
