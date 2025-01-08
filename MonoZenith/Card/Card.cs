using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using System.Collections.Generic;
using MonoZenith.Items;
using static MonoZenith.Game;

namespace MonoZenith.Card
{
    public abstract class Card : Item
    {
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
        protected List<string> _description;
        protected Player _owner;
        protected SoundEffectInstance _soundOnPlay;
        protected float _buff;
        protected float _debuff;

        public string CardName { get; set; } = "Card";

        /// <summary>
        /// The position of the card.
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

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
        public float Buff
        {
            get => _buff;
            set
            {
                _buff = value;
                UpdateBuffsAndDebuffs();
            }
        }

        /// <summary>
        /// A debuff for the card.
        /// This value should be subtracted to a card's damage/healing.
        /// </summary>
        public float Debuff
        {
            get => _buff;
            set
            {
                _buff = value;
                UpdateBuffsAndDebuffs();
            }
        }

        protected Card()
        {
            _owner = null;
            _position = Vector2.Zero;
            _scale = 0.40f * AppSettings.Scaling.ScaleFactor;
            _frontTexture = DataManager.GetInstance().CardFront;
            _backTexture = DataManager.GetInstance().CardBack;
            _hiddenTexture = DataManager.GetInstance().CardHidden;
            _costStaminaTexture = DataManager.GetInstance().CardCostStamina;
            _width = (int)(_frontTexture.Width * _scale);
            _height = (int)(_frontTexture.Height * _scale);
            _name = "BaseCard";
            _soundOnPlay = null;
            _name = GetType().Name;
            _description = new List<string>();
            // Description includes max 3 strings of max 15 characters
            _buff = 0;
        }
        
        public virtual void SetOwner(Player owner)
        {
            _owner = owner;
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
            Point mousePosition = GetMousePosition();
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
            return IsHovered() && GetMouseButtonDown(MouseButtons.Left);
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
        /// Checks for enemy buffs and apply any effect if they exist
        /// </summary>
        public virtual void CheckEnemyBuffs()
        {
            return;
        }

        /// <summary>
        /// Updates the card with the new buffs and debuffs
        /// </summary>
        public abstract void UpdateBuffsAndDebuffs();

        /// <summary>
        /// Update the state of the card.
        /// </summary>
        /// <param name="deltaTime">The delta time.</param>
        public virtual void Update(GameTime deltaTime)
        {
            _textureInHand = _owner is HumanPlayer ? _frontTexture : _backTexture;
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
            if (GetGameState().TurnManager.SwitchingTurns)
                return 1250f * AppSettings.Scaling.ScaleFactor;
            
            return IsTransferringToExternalStack ? 
                1000f * AppSettings.Scaling.ScaleFactor 
                : 500f * AppSettings.Scaling.ScaleFactor;
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
            if (Stack != null && Stack == GetGameState().PlayedCards)
                active = true;
            
            Texture2D currentTexture = active ? _frontTexture : _textureInHand;
            DrawImage(currentTexture, _position, _scale, angle);
            
            if(!IsAffordable() && Stack is HandCardStack && _owner is HumanPlayer)
                DrawImage(_hiddenTexture, _position, _scale, angle);
            
            if (!active)
                return;
            
            if (active)
                DrawDescription();

            DrawMetaData();
        }

        /// <summary>
        /// Update the description of the card
        /// </summary>
        protected virtual void UpdateDescription()
        {
            return;
        }

        /// <summary>
        /// Draw the description of the card onto the front side of the card with scaling.
        /// </summary>
        protected virtual void DrawDescription()
        {
            // Set up position offsets based on card
            float offsetX = Width * 0.5f;
            float offsetY = Height * 0.72f;
            Vector2 cardOffset = new Vector2(offsetX, offsetY);

            // Retrieve font and calculate scaling factor
            SpriteFont cardFont = DataManager.GetInstance().CardFont;
            float baseTextHeight = cardFont.MeasureString("A").Y; // Reference height
            
            // Create scale based on Card Width with offset
            float scalingFactor = (Width - 35) / (cardFont.MeasureString(_description[0]).X + 10);

            // Adjust scaling so it remains reasonable for very small or large cards
            scalingFactor = MathHelper.Clamp(scalingFactor, 0.6f, 1.2f);

            int textCount = _description.Count;
            float textHeight = baseTextHeight * scalingFactor;

            // Calculate vertical offset to center the text dynamically
            float heightOffset = (textHeight - textCount * textHeight) * 0.5f;

            for (int i = 0; i < textCount; i++)
            {
                Vector2 textSize = cardFont.MeasureString(_description[i]) * scalingFactor;
                float textWidth = 0.5f * textSize.X;

                DrawText(
                    _description[i],
                    _position + cardOffset - new Vector2(textWidth, -textHeight * i - heightOffset),
                    DataManager.GetInstance().CardFont,
                    Color.Ivory,
                    scalingFactor
                );
            }
        }

        /// <summary>
        /// Draw the metadata of the card onto the front side of the card.
        /// </summary>
        protected virtual void DrawMetaData()
        {
            DrawText(
                _name,
                _position,
                DataManager.GetInstance().CardFont,
                Color.White
            );
        }
    }
}
