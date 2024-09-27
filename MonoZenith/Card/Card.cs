using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MonoZenith;
using MonoZenith.Engine.Support;
using static System.Formats.Asn1.AsnWriter;

namespace MonoZenith.Card
{
    internal abstract class Card
    {
        protected Game _game;
        protected GameState _state;
        protected Vector2 _position;
        protected Vector2 _originalPosition;
        protected int _width;
        protected int _height;
        protected float _scale;
        protected Texture2D _texture;
        protected Texture2D _activeTexture;
        protected string _name;
        
        public int Width => _width;
        public int Height => _height;
        public float Scale => _scale;

        protected Card(Game game, GameState state, Vector2 position, Texture2D texture, Texture2D activeTexture, string name)
        {
            _game = game;
            _state = state;
            _position = position;
            _originalPosition = position;
            _scale = 0.4f;
            _texture = texture;
            _activeTexture = activeTexture;
            _width = texture.Width;
            _height = texture.Height;
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Checks if the player is hovering over the card with the mouse pointer.
        /// </summary>
        /// <returns>Returns if the player is hovering over the card.</returns>
        public bool IsHovered()
        {
            // Clicked is using the _position value, while it should be using _currentPos.
            // TODO: Check how to refactor this properly.
            
            Point mousePosition = _game.GetMousePosition();
            bool inXRange = mousePosition.X >= _position.X && mousePosition.X <= _position.X + _width * _scale;
            bool inYRange = mousePosition.Y >= _position.Y && mousePosition.Y <= _position.Y + _height * _scale;
            return inXRange && inYRange;
        }

        /// <summary>
        /// Checks if the player clicked on the card with the left mouse button.
        /// </summary>
        /// <returns>If the card is clicked.</returns>
        public bool IsClicked()
        {
            return IsHovered() && _game.GetMouseButtonDown(MouseButtons.Left);
        }

        /// <summary>
        /// Checks if the card is valid card.
        /// </summary>
        /// <param name="previousCard">The card that was played before this one.</param>
        /// <returns>Whether the card is a valid next card.</returns>
        public abstract bool ValidNextCard(Card previousCard);

        /// <summary>
        /// Draw the metadata of the card onto the front side of the card.
        /// </summary>
        protected abstract void DrawMetaData();

        /// <summary>
        /// Changes the position of the card.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void ChangePosition(float x, float y)
        {
            _position = new Vector2(x, y);
        }
        
        /// <summary>
        /// Update the state of the card.
        /// </summary>
        /// <param name="deltaTime">The delta time.</param>
        public void Update(GameTime deltaTime)
        {
            
        }
        
        /// <summary>
        /// Draw the card.
        /// </summary>
        /// <param name="x">Positional x</param>
        /// <param name="y">Positional y</param>
        /// <param name="angle">Rotational Angle</param>
        /// <param name="offset">Offset Bool, determines whether the card is
        /// drawn starting at (0,0) or in the middle as offset.</param>
        /// <param name="active">Boolean to determine if active or back texture should be drawn</param>
        public void Draw(float x, float y, float angle = 0, bool offset = true, bool active = false)
        {
            float newX;
            float newY;

            if (offset)
            {
                newX = x - (_width * _scale / 2);
                newY = y - (_height * _scale / 2);
            }
            else
            {
                newX = x;
                newY = y;
            }
            
            _position = _originalPosition + new Vector2(newX, newY);
            Texture2D currentTexture = active ? _activeTexture : _texture;
            _game.DrawImage(currentTexture, _position, _scale, angle);

            if (!active)
                return; 
            
            DrawMetaData();
        }
    }
}
