using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Players
{
    public abstract class Player
    {
        private float _originalStamina;
        protected Game _game;
        protected GameState _state;
        protected float _handPosX;
        protected float _handPosY;
        protected float _scale;
        protected SpriteFont _playerFont;
        protected Vector2 _playerPosition;
        protected Texture2D _playerIcon;
        protected readonly Texture2D _playerCurrent;
        public bool CardsDrawn;
        
        public float Health;
        public float Stamina;
        public float Focus;
        public readonly Texture2D PlayerWaiting;
        public readonly string Name;
        
        // Card stacks
        protected CardStack _deckStack;
        protected CardStack _reserveCardStack;
        protected HandCardStack _handStack;

        public float OriginalHealth => 100f;
        public SpiritAsh SpiritAsh { get; set; }
        public BuffManager BuffManager { get; set; }
        
        /// <summary>
        /// The player's deck stack.
        /// </summary>
        public CardStack DeckStack => _deckStack;
        
        /// <summary>
        /// The player's reserve card stack.
        /// </summary>
        public CardStack ReserveCardStack => _reserveCardStack;
        
        /// <summary>
        /// The player's hand card stack.
        /// </summary>
        public HandCardStack HandStack => _handStack;
        
        /// <summary>
        /// The player's opponent.
        /// </summary>
        public Player OpposingPlayer { get; set; }
        
        /// <summary>
        /// Check if the player has any moving cards.
        /// </summary>
        public bool HasAnyMovingCards => _reserveCardStack.GetMovingCards().Count > 0;

        protected Player(GameState state, string name)
        {
            _state = state;
            Name = name;
            _scale = 0.15f * AppSettings.Scaling.ScaleFactor;
            _handPosX = ScreenWidth / 2f;
            
            // ReSharper disable once VirtualMemberCallInConstructor
            InitializeState(state);
            
            // Load textures and sound effects for player
            _playerCurrent = DataManager.GetInstance().PlayerCurrent;
            PlayerWaiting = DataManager.GetInstance().PlayerWaiting;
            _playerFont = DataManager.GetInstance().PlayerFont;
            
            // Ashes and buffs
            // TODO: Make sure players start without any ashes when game starts
            SpiritAsh = new WolvesAsh(state, this);
            BuffManager = new BuffManager(state, this);
        }

        /// <summary>
        /// Initialize the player's state.
        /// </summary>
        /// <param name="state">The game state.</param>
        /// <remarks>
        /// This method initializes the player properties and card stacks.
        /// </remarks>
        public virtual void InitializeState(GameState state)
        {
            // Initialize player properties
            Health = 100f;
            Stamina = 30f;
            Focus = 30f;
            _originalStamina = 30f;
            CardsDrawn = false;
            
            // Initialize card stacks
            _deckStack = new CardStack(state);
            _reserveCardStack = new CardStack(state);
            _handStack = new HandCardStack(state);
            BuffManager = new BuffManager(state, this);
            FillPlayerDeck();
            ChangeHandStackPosition();
        }
        public override string ToString()
        {
            return $"==== {Name} ====\n\n\n" +
                   $"HEALTH: {Health}\n\n" +
                   $"STAMINA: {Stamina}\n\n" +
                   $"FOCUS: {Focus}\n\n" +
                   $"DECK STACK: {_deckStack}\n\n" +
                   $"RESERVE CARD STACK: {_reserveCardStack}\n\n" +
                   $"HAND STACK: {_handStack}\n\n\n";
        }

        /// <summary>
        /// Fill the player deck with cards.
        /// </summary>
        /// <remarks>
        /// This method is used to fill the player deck with cards. It is called
        /// once when the player is created and is used to initialize the deck.
        /// </remarks>
        protected abstract void FillPlayerDeck();

        /// <summary>
        /// Changes the Player's hand position
        /// </summary>
        public void ChangeHandStackPosition()
        {
            _handStack.UpdatePosition(_handPosX, _handPosY);
        }

        /// <summary>
        /// Reset the player's stamina.
        /// </summary>
        public void ResetPlayerStamina() => Stamina = _originalStamina;

        /// <summary>
        /// Perform the player's turn.
        /// </summary>
        /// <param name="state">The current game state.</param>
        public virtual void PerformTurn(GameState state)
        {
            // Draw cards from hand only once
            if (_handStack.Count == 0 && !CardsDrawn)
            {
                DrawCardsFromDeck();
                CardsDrawn = true;
            }
            
            BuffManager.Update();
        }

        /// <summary>
        /// Refill the deck if it is empty.
        /// </summary>
        protected void RefillDeckIfEmpty()
        {
            if (_deckStack.Count != 0) return;
            if (_reserveCardStack.Count == 0)
                return;
                    
            MoveCardsFromReserveToDeck();
        }
        
        /// <summary>
        /// Draw 5 cards from the deck and add them to the player's hand,
        /// or draw as many cards as are available if there are fewer than 5
        /// cards in the deck.
        /// </summary>
        public void DrawCardsFromDeck()
        {
            for (int i = 0; i < 5; i++)
            {
                RefillDeckIfEmpty();
                MoveSingleCardFromDeckToHand();
            }
        }
        
        /// <summary>
        /// Move a single card from the deck to the hand.
        /// </summary>
        public void MoveSingleCardFromDeckToHand()
        {
            RefillDeckIfEmpty();
            _handStack.AddToFront(_deckStack.PopRandomCard());
        }

        /// <summary>
        /// Move the cards from the hand to the reserve pile.
        /// </summary>
        public void MoveCardsFromHandToReserve()
        {
            List<Card.Card> cardsFromHand = _handStack.Cards;
            _reserveCardStack.AddToFront(cardsFromHand); 
            _handStack.Clear();
            CardsDrawn = false;
        }
        
        /// <summary>
        /// Move all the cards from the reserve pile to the deck,
        /// in the same order. Then, clear the reserve pile.
        /// </summary>
        protected void MoveCardsFromReserveToDeck()
        {
            List<Card.Card> cardsFromReserve = _reserveCardStack.Cards;
            _deckStack.AddToFront(cardsFromReserve); 
            _reserveCardStack.Clear();
            _deckStack.Shuffle();
        }

        /// <summary>
        /// Move the cards from the played stack to the reserve stack.
        /// </summary>
        public void MoveCardsFromPlayedToReserve()
        {
            List<Card.Card> cardsFromPlayed = _state.PlayedCards.Cards;
            _reserveCardStack.AddToFront(cardsFromPlayed); 
            _state.PlayedCards.Clear();
        }
        
        /// <summary>
        /// Move the cards from the played stack to the reserve stack.
        /// </summary>
        public void MoveCardsOutOfScreen()
        {
            var movingCards = new List<Card.Card>();
            movingCards.AddRange(_reserveCardStack.GetMovingCards());
            
            if (movingCards.Count > 0)
            {
                foreach (var card in movingCards)
                    card.Update(_state.GameTime);
                
                // Filter out cards that are no longer moving
                movingCards = movingCards.Where(card => card.IsMoving).ToList();
            }
        }
        
        /// <summary>
        /// Play the selected card and update the game state.
        /// </summary>
        /// <param name="card">The card to play.</param>
        protected void PlayCard(Card.Card card)
        {
            _state.PlayedCards.AddToBottom(card);
            _handStack.Remove(card);
            card.PerformEffect();
            DebugLog(this.Name + " playing card: " + card);
        }

        /// <summary>
        /// Update the player.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public abstract void Update(GameTime deltaTime);

        /// <summary>
        /// Draw all the assets of the player.
        /// </summary>
        public virtual void Draw()
        {
            DrawPlayerHealthAndName();
            DrawPlayerUi();
            _handStack.Draw();
            _reserveCardStack.Draw();
        }

        /// <summary>
        /// Draw the Player's name.
        /// </summary>
        protected abstract void DrawPlayerHealthAndName();

        /// <summary>
        /// Draw the Player UI Assets.
        /// </summary>
        protected virtual void DrawPlayerUi()
        {
            // Setup properties of UI assets
            Vector2 iconOffset = GetOffset(_playerIcon, _scale);
            Vector2 borderOffset = GetOffset(_playerCurrent, _scale);

            // Check if the player is the current playing player
            bool currentPlayer = _state.TurnManager.CurrentPlayer == this;
            Texture2D playerBorder = currentPlayer ? _playerCurrent : PlayerWaiting;

            // Draw the assets
            DrawImage(_playerIcon, _playerPosition - iconOffset, _scale, 0);
            DrawImage(playerBorder, _playerPosition - borderOffset, _scale, 0);
        }

        /// <summary>
        /// Gets the positional offset of the texture in order to
        /// draw the texture in the middle instead of (0,0).
        /// </summary>
        /// <param name="texture">The given texture.</param>
        /// <param name="scale">The scale in which the texture will be drawn.</param>
        protected Vector2 GetOffset(Texture2D texture, float scale)
        {
            var widthOffset = texture.Width * scale * 0.5f;
            var heightOffset = texture.Height * scale * 0.5f;
            return new Vector2(widthOffset, heightOffset);
        }
    }
}
