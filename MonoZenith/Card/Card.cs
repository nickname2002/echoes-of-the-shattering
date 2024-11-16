using Microsoft.Xna.Framework;
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
        protected Texture2D _costStaminaTexture;
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

        /// <summary>
        /// A buff for the card.
        /// This value should be added to a card's damage/healing.
        /// </summary>
        public float Buff { get; set; } = 0;

        protected Card(Game game, GameState state, Player owner)
        {
            _game = game;
            _state = state;
            _owner = owner;
            _position = Vector2.Zero;
            _scale = 0.40f * AppSettings.Scaling.ScaleFactor;
            _frontTexture = DataManager.GetInstance(_game).CardFront;
            _backTexture = DataManager.GetInstance(_game).CardBack;
            _hiddenTexture = DataManager.GetInstance(_game).CardHidden;
            _costStaminaTexture = DataManager.GetInstance(_game).CardCostStamina;
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
            if (IsTransferringToExternalStack && TargetPosition == _position)
            {
                IsTransferringToExternalStack = false;
            }
        }

        /// <summary>
        /// Move the card towards the target position.
        /// </summary>
        /// <param name="deltaTime">The delta time.</param>
        private void MoveTowardsTargetPosition(GameTime deltaTime)
        {
            Vector2 direction = TargetPosition - _position;
            float distance = direction.Length();
            float speed = MovementSpeed() * 
                          (float)deltaTime.ElapsedGameTime.TotalSeconds;

            if (distance <= speed)
            {
                _position = TargetPosition;
                IsTransferringToExternalStack = false;
            }
            else
            {
                Vector2 velocity = direction * speed / distance;
                _position += velocity;
            }
        }

        /// <summary>
        /// Calculate the movement speed of the card.
        /// </summary>
        /// <returns>The movement speed of the card.</returns>
        private float MovementSpeed()
        {
            if (_state.SwitchingTurns)
                return 1250f;
            
            return IsTransferringToExternalStack ? 1000f : 500f;
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
        /// starts on its relative origin (0, 0) or centralised.
        /// </summary>
        /// <param name="x">Positional x</param>
        /// <param name="y">Positional y</param>
        public void UpdatePosition(float x, float y)
        {
            TargetPosition = new Vector2(x, y);
        }
        
        /// <summary>
        /// Draw the card.
        /// </summary>
        /// <param name="angle">Rotational Angle</param>
        /// <param name="active">Boolean to determine if metadata should be drawn</param>
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
        protected virtual void DrawMetaData()
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
