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
        protected string _name;
        protected Player _owner;
        protected SoundEffectInstance _soundOnPlay;
        
        public Vector2 Position => _position;
        public static int Width => _width;
        public static int Height => _height;
        public float Scale => _scale;
        public Player Owner => _owner;
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
        /// Update the Card's position
        /// </summary>
        /// <param name="x">Positional x</param>
        /// <param name="y">Positional y</param>
        /// <param name="offset">Offset Bool, determines whether the card is
        /// drawn starting at (0,0) or in the middle as offset.</param>
        public void UpdatePosition(float x, float y, bool offset = true)
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

            _position = new Vector2(newX, newY);
        }
        
        /// <summary>
        /// Draw the card.
        /// </summary>
        /// <param name="angle">Rotational Angle</param>
        /// <param name="active">Boolean to determine if active or back texture should be drawn</param>
        public void Draw(float angle = 0, bool active = false)
        {
            Texture2D currentTexture = active ? _frontTexture : _textureInHand;
            _game.DrawImage(currentTexture, _position, _scale, angle);

            if(!IsAffordable() && Stack is HandCardStack)
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

        public override string ToString()
        {
            return _name;
        }
    }
}
