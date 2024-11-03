﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Card
{
    public abstract class Card
    {
        protected Game _game;
        protected GameState _state;
        protected Vector2 _position;
        protected static int _width;
        protected static int _height;
        protected float _scale;
        protected Texture2D _textureInHand;
        protected Texture2D _frontTexture;
        protected Texture2D _backTexture;
        protected Texture2D _hiddenTexture;
        protected string _name;
        protected Player _owner;
        protected SoundEffectInstance _soundOnPlay;
        
        /// <summary>
        /// The position of the card.
        /// </summary>
        public Vector2 Position => _position;
        
        /// <summary>
        /// The target position of the card.
        /// </summary>
        public Vector2 TargetPosition { get; set; }
        
        /// <summary>
        /// Boolean to determine if the card is being transferred to an external stack.
        /// </summary>
        public bool IsTransferringToExternalStack { get; set; }
        
        /// <summary>
        /// Boolean to determine if the card is moving.
        /// </summary>
        public bool IsMoving => _position != TargetPosition ;
        
        /// <summary>
        /// The width and height of the card.
        /// </summary>
        public static int Width => _width;
        public static int Height => _height;
        
        /// <summary>
        /// The scale of the card.
        /// </summary>
        public float Scale => _scale;
        
        /// <summary>
        /// The owner of the card.
        /// </summary>
        public Player Owner => _owner;
        
        /// <summary>
        /// The stack the card is in.
        /// </summary>
        public CardStack.CardStack Stack { get; set; }

        protected Card(Game game, GameState state, Player owner)
        {
            _game = game;
            _state = state;
            _owner = owner;
            _position = Vector2.Zero;
            _scale = 0.35f * AppSettings.Scaling.ScaleFactor;
            _frontTexture = DataManager.GetInstance(_game).CardFront;
            _backTexture = DataManager.GetInstance(_game).CardBack;
            _hiddenTexture = DataManager.GetInstance(_game).CardHidden;
            _textureInHand = owner is HumanPlayer ? _frontTexture : _backTexture;
            _width = (int)(_frontTexture.Width * _scale);
            _height = (int)(_frontTexture.Height * _scale);
            _name = "BaseCard";
            _soundOnPlay = null;
            _name = GetType().Name;
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
            Point mousePosition = _game.GetMousePosition();
            bool inXRange = mousePosition.X >= _position.X && mousePosition.X <= _position.X + _width;
            bool inYRange = mousePosition.Y >= _position.Y && mousePosition.Y <= _position.Y + _height;
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
        /// Checks if the card is affordable.
        /// </summary>
        /// <returns>If the card is affordable.</returns>
        public abstract bool IsAffordable();

        /// <summary>
        /// Perform the effect of the card.
        /// </summary>
        public abstract void PerformEffect();
        
        /// <summary>
        /// Update the state of the card.
        /// </summary>
        /// <param name="deltaTime">The delta time.</param>
        public void Update(GameTime deltaTime)
        {
            MoveTowardsTargetPosition(deltaTime);

            // Controleer of de kaart de doelpositie heeft bereikt
            if (IsTransferringToExternalStack && TargetPosition == _position)
            {
                IsTransferringToExternalStack = false;
                TargetPosition = Vector2.Zero;
            }
        }

        /// <summary>
        /// Move the card towards the target position.
        /// </summary>
        /// <param name="deltaTime">The delta time.</param>
        private void MoveTowardsTargetPosition(GameTime deltaTime)
        {
            if (TargetPosition == Vector2.Zero)
                return;

            Vector2 direction = TargetPosition - _position;
            float distance = direction.Length();
            float speed = 500 * (float)deltaTime.ElapsedGameTime.TotalSeconds;

            if (distance <= speed)
            {
                _position = TargetPosition;
            }
            else
            {
                Vector2 velocity = direction * speed / distance;
                _position += velocity;
            }
        }
        
        /// <summary>
        /// Set the position of the card.
        /// </summary>
        /// <param name="position">The new position of the card.</param>
        public void SetPosition(Vector2 position)
        {
            _position = position;
        }
        
        /// <summary>
        /// Update the Card's position.
        /// <paramref name="offset"/> determines if the Card's position
        /// starts on its relative origin (0, 0) or centralised.
        /// </summary>
        /// <param name="x">Positional x</param>
        /// <param name="y">Positional y</param>
        /// <param name="offset">Boolean to determine whether the card
        /// should be centralised.</param>
        public void UpdatePosition(float x, float y, bool offset = true)
        {
            float newX;
            float newY;

            if (offset)
            {
                // Centralise the position of the card
                newX = x - _width * _scale / 2 * AppSettings.Scaling.ScaleFactor;
                newY = y - _height * _scale / 2 * AppSettings.Scaling.ScaleFactor;
            }
            else
            {
                newX = x;
                newY = y;
            }

            TargetPosition = new Vector2(newX, newY);
        }
        
        /// <summary>
        /// Draw the card.
        /// </summary>
        /// <param name="angle">Rotational Angle</param>
        /// <param name="active">Boolean to determine if active or back texture should be drawn</param>
        public void Draw(float angle = 0, bool active = false)
        {
            if (Stack == _state.PlayedCards)
                active = true;
            
            Texture2D currentTexture = active ? _frontTexture : _textureInHand;
            _game.DrawImage(currentTexture, _position, _scale, angle);
            
            if(!IsAffordable() && Stack is HandCardStack && _owner is HumanPlayer)
                _game.DrawImage(_hiddenTexture, _position, _scale, angle);

            if (!active)
                return; 
            
            DrawMetaData();
        }

        /// <summary>
        /// Draw the metadata of the card onto the front side of the card.
        /// </summary>
        protected void DrawMetaData()
        {
            _game.DrawText(
                _name,
                _position,
                DataManager.GetInstance(_game).CardFont,
                Color.Black
            );
        }
    }
}
