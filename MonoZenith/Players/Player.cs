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
        
        public float Health;
        public float Stamina;
        public float Focus;
        public Vector2 PlayerPosition;
        public Texture2D PlayerIcon;
        public readonly Texture2D PlayerCurrent;
        public readonly Texture2D PlayerWaiting;
        public readonly string Name;
        
        // Card stacks
        protected CardStack _deckStack;
        protected CardStack _reserveCardStack;
        protected CardStack _handStack;

        protected Player(Game game, GameState state, string name)
        {
            _game = game;
            _state = state;
            Name = name;
            Scale = 0.15f;
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
        public void InitializeState(Game game, GameState state)
        {
            // Initialize player properties
            Health = 100f;
            Stamina = 30f;
            Focus = 30f;
            
            // Initialize card stacks
            _deckStack = new CardStack(game, state);
            _reserveCardStack = new CardStack(game, state);
            _handStack = new CardStack(game, state);
            FillPlayerDeck();
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
        }

        public override string ToString()
        {
            return $"==== {Name} ====\n\n\n" +
                   $"DECK STACK: {_deckStack}\n\n" +
                   $"RESERVE CARD STACK: {_reserveCardStack}\n\n" +
                   $"HAND STACK: {_handStack}\n\n\n";
        }

        /// <summary>
        /// Perform the player's turn.
        /// </summary>
        /// <param name="state">The current game state.</param>
        public virtual void PerformTurn(GameState state)
        {
            // Draw cards from hand only once
            if (_handStack.Count == 0)
                DrawCardsFromDeck();
            
            Console.WriteLine($"{Name}'s Turn\n\n");
            Console.WriteLine($"Hand: {_handStack}\n" +
                              $"Reserve: {_reserveCardStack.Count}\n" +
                              $"Deck: {_deckStack.Count}");
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

        public void MoveCardsFromHandToReserve()
        {
            List<Card.Card> cardsFromHand = _handStack.Cards;
            _reserveCardStack.AddToFront(cardsFromHand); 
            _handStack.Clear();
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
        }

        /// <summary>
        /// Attempt to play the selected card.
        /// </summary>
        /// <returns>Whether a valid card was played.</returns>
        protected abstract bool TryPlayCard();
        
        /// <summary>
        /// Play the selected card and update the game state.
        /// </summary>
        /// <param name="card">The card to play.</param>
        protected void PlayCard(Card.Card card)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draw a card from the deck and add it to the player's hand.
        /// </summary>
        protected abstract void TryDrawCard();

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
        /// Draw the hand of the player.
        /// </summary>
        public abstract void DrawHand();

        /// <summary>
        /// Draw the Player UI Assets.
        /// </summary>
        public void DrawPlayerUI()
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
        /// Draw the Player's name.
        /// </summary>
        public abstract void DrawPlayerHealthAndName();

        /// <summary>
        /// Gets the positional offset of the texture in order to
        /// draw the texture in the middle instead of (0,0).
        /// </summary>
        /// <param name="texture">The given texture.</param>
        /// <param name="scale">The scale in which the texture will be drawn.</param>
        public Vector2 GetOffset(Texture2D texture, float scale)
        {
            float widthOffset = texture.Width * scale * 0.5f;
            float heightOffset = texture.Height * scale * 0.5f;
            return new Vector2(widthOffset, heightOffset);
        }
    }
}
