using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;

namespace MonoZenith.Players
{
    public abstract class Player
    {
        protected Game _game;
        protected GameState _state;
        protected float _handxPos;
        protected float _handyPos;
        protected float Scale;
        protected SpriteFont PlayerFont;
        private float _originalStamina;
        
        public float Health;
        public float Stamina;
        public float Focus;
        public Vector2 PlayerPosition;
        public Texture2D PlayerIcon;
        public readonly Texture2D PlayerCurrent;
        public readonly Texture2D PlayerWaiting;
        public readonly string Name;
        protected bool _cardsDrawn;
        
        // Card stacks
        protected CardStack _deckStack;
        protected CardStack _reserveCardStack;
        protected HandCardStack _handStack;

        protected Player(Game game, GameState state, string name)
        {
            _game = game;
            _state = state;
            Name = name;
            Scale = 0.15f * AppSettings.Scaling.ScaleFactor;
            
            // ReSharper disable once VirtualMemberCallInConstructor
            InitializeState(game, state);
            
            // Load textures and sound effects for player
            PlayerCurrent = DataManager.GetInstance(game).PlayerCurrent;
            PlayerWaiting = DataManager.GetInstance(game).PlayerWaiting;
            PlayerFont = DataManager.GetInstance(game).PlayerFont;
        }
        
        /// <summary>
        /// Initialize the player's state.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="state">The game state.</param>
        /// <remarks>
        /// This method initializes the player properties and card stacks.
        /// </remarks>
        public virtual void InitializeState(Game game, GameState state)
        {
            // Initialize player properties
            Health = 100f;
            Stamina = 30f;
            Focus = 30f;
            _originalStamina = 30f;
            _cardsDrawn = false;
            
            // Initialize card stacks
            _deckStack = new CardStack(game, state);
            _reserveCardStack = new CardStack(game, state);
            _handStack = new HandCardStack(game, state);
            FillPlayerDeck();
            ChangeHandStackPosition();
        }

        /// <summary>
        /// Fill the player deck with cards.
        /// </summary>
        /// <remarks>
        /// This method is used to fill the player deck with cards. It is called
        /// once when the player is created and is used to initialize the deck.
        /// </remarks>
        protected void FillPlayerDeck()
        {
            var deck = new List<Card.Card>
            {
                // Flasks
                new FlaskOfCrimsonTearsCard(_game, _state, this),
                new FlaskOfCrimsonTearsCard(_game, _state, this),
                new FlaskOfCrimsonTearsCard(_game, _state, this),
                new FlaskOfCrimsonTearsCard(_game, _state, this),

                new FlaskOfCeruleanTearsCard(_game, _state, this),
                new FlaskOfCeruleanTearsCard(_game, _state, this),
                new FlaskOfCeruleanTearsCard(_game, _state, this),
                new FlaskOfCeruleanTearsCard(_game, _state, this),

                // Basic attacks
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),

                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),

                // Magic attacks
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this)
            };
            
            _deckStack.AddToFront(deck);
            foreach (var card in _handStack.Cards)
                card.Stack = _handStack;
        }

        /// <summary>
        /// Changes the Player's hand position
        /// </summary>
        public void ChangeHandStackPosition()
        {
            _handStack.ChangePosition(_handxPos, _handyPos);
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
            if (_handStack.Count == 0 && !_cardsDrawn)
            {
                DrawCardsFromDeck();
                _cardsDrawn = true;
            }
        }
        
        /// <summary>
        /// Draw 5 cards from the deck and add them to the player's hand,
        /// or draw as many cards as are available if there are fewer than 5
        /// cards in the deck.
        /// </summary>
        protected void DrawCardsFromDeck()
        {
            for (int i = 0; i < 5; i++)
            {
                if (_deckStack.Count == 0)
                {
                    if (_reserveCardStack.Count == 0)
                        break;
                    
                    MoveCardsFromReserveToDeck();
                }
                
                Card.Card cardToAdd = _deckStack.PopRandomCard();
                _handStack.AddToFront(cardToAdd);
            }
        }

        /// <summary>
        /// Move the cards from the hand to the reserve pile.
        /// </summary>
        public void MoveCardsFromHandToReserve()
        {
            List<Card.Card> cardsFromHand = _handStack.Cards;
            _reserveCardStack.AddToFront(cardsFromHand); 
            _handStack.Clear();
            _cardsDrawn = false;
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
        /// Play the selected card and update the game state.
        /// </summary>
        /// <param name="card">The card to play.</param>
        protected void PlayCard(Card.Card card)
        {
            _state.PlayedCards.AddToBottom(card);
            card.PerformEffect();
            _handStack.Remove(card);
            _game.DebugLog(this.Name + " playing card: " + card);
        }

        /// <summary>
        /// Update the player.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        public abstract void Update(GameTime deltaTime);

        /// <summary>
        /// Draw the player.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Draw the Player's name.
        /// </summary>
        public abstract void DrawPlayerHealthAndName();

        /// <summary>
        /// Draw the Player UI Assets.
        /// </summary>
        protected virtual void DrawPlayerUi()
        {
            // Setup properties of UI assets
            Vector2 iconOffset = GetOffset(PlayerIcon, Scale);
            Vector2 borderOffset = GetOffset(PlayerCurrent, Scale);

            // Check if the player is the current playing player
            bool currentPlayer = _state.CurrentPlayer == this;
            Texture2D playerBorder = currentPlayer ? PlayerCurrent : PlayerWaiting;

            // Draw the assets
            _game.DrawImage(PlayerIcon, PlayerPosition - iconOffset, Scale, 0);
            _game.DrawImage(playerBorder, PlayerPosition - borderOffset, Scale, 0);
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
    }
}
