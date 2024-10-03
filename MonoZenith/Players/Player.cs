using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card;
using MonoZenith.Card.CardStack;
using MonoZenith.Engine.Support;
using MonoZenith.Support;

namespace MonoZenith.Players
{
    public abstract class Player
    {
        protected Game _game;
        protected GameState _state;
        public CardStack Hand;
        public string Name;
        protected float _handxPos;
        protected float _handyPos;
        public Vector2 PlayerPosition;
        public Texture2D PlayerIcon;
        public readonly Texture2D PlayerCurrent;
        public readonly Texture2D PlayerWaiting;
        protected SpriteFont PlayerFont;
        protected float _scale;

        protected Player(Game game, GameState state, string name)
        {
            _game = game;
            _state = state;
            Hand = new CardStack(_game, _state);
            Name = name;
            _scale = 0.15f;
            PlayerCurrent = DataManager.GetInstance(game).PlayerCurrent;
            PlayerWaiting = DataManager.GetInstance(game).PlayerWaiting;
            PlayerFont = DataManager.GetInstance(game).PlayerFont;
        }

        public override string ToString()
        {
            return $"==== {Name} ====\n{Hand}\n";
        }

        /// <summary>
        /// Perform the player's turn.
        /// </summary>
        /// <param name="state">The current game state.</param>
        public virtual void PerformTurn(GameState state)
        {
            if (!NoValidCardsWithActiveCombo()) 
                return;
            
            DrawCombo(_state);
        }

        /// <summary>
        /// Draw a card from the drawable cards stack and add it to the player's hand.
        /// </summary>
        public void DrawCard()
        {
            Hand.AddToFront(_state.DrawableCards.GetCard());
        }

        /// <summary>
        /// Draw cards equal to the current card combo amount
        /// </summary>
        /// <param name="state">The current gamestate</param>
        protected void DrawCombo(GameState state)
        {
            // If there is a power effect in play,
            // draw cards equal to the current combo amount.
            if (state.Combo >= 1)
            {
                for (int i = 0; i < state.Combo; i++)
                {
                    DrawCard();
                }

                state.Combo = 0;
            }
            
            _state.SwitchTurn();
        }

        /// <summary>
        /// Attempt to play the selected card.
        /// </summary>
        /// <returns>Whether a valid card was played.</returns>
        protected abstract bool TryPlayCard();

        /// <summary>
        /// Returns whether the player has no valid cards, but there
        /// is an active combo.
        /// </summary>
        /// <returns></returns>
        protected bool NoValidCardsWithActiveCombo()
        {
            List<RegionCard> comboCards = new List<RegionCard>();
            comboCards.AddRange(Hand.Cards.OfType<RegionCard>().
                Where(card => card.IsComboCard));

            if (comboCards.Count != 0 || _state.Combo <= 0) 
                return false;
            
            DrawCombo(_state);
            return true;
        }
        
        /// <summary>
        /// If there is an active combo that cannot be blocked with the played card,
        /// the combo will be drawn by the player.
        /// </summary>
        /// <param name="card">The card chosen by the player to play.</param>
        /// <returns>Whether there was an unblockable active combo.</returns>
        protected bool UnblockableActiveCombo(RegionCard card)
        {
            if (_state.Combo < 1 ||
                card.IsComboCard) 
                return false;
         
            Console.WriteLine("Card is not powerful enough.");
            DrawCombo(_state);
            return true;
        }
        
        /// <summary>
        /// Play the selected card and update the game state.
        /// </summary>
        /// <param name="card">The card to play.</param>
        protected void PlayCard(Card.Card card)
        {
            if (UnblockableActiveCombo((RegionCard)card))
            {
                return;
            }
            
            Console.WriteLine($"{Name} played: {card}");

            // Update the current region if the card is a RegionCard and region is not "ALL"
            if (card is RegionCard regionCard && regionCard.Region != Region.ALL)
            {
                _state.CurrentRegion = regionCard.Region;
            }

            // Add the card to the played pile and remove it from the player's hand
            _state.PlayedCards.AddToBottom(card);
            Hand.Cards.Remove(card);

            // Perform the effect of the card
            RegionCard effectCard = (RegionCard)card;
            effectCard?.PerformEffect(_state);
        }
        
        /// <summary>
        /// Draw a card from the deck and add it to the player's hand.
        /// </summary>
        protected abstract void TryDrawCard();

        /// <summary>
        /// Check if the selected card is a valid play based on the last played card.
        /// </summary>
        /// <param name="card">The card to check.</param>
        /// <returns>True if the card can be played, false otherwise.</returns>
        protected bool IsValidPlay(Card.Card card)
        {
            var lastPlayedCard = _state.PlayedCards.Cards[^1]; 
            return card.ValidNextCard(lastPlayedCard);
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
        /// Draw the hand of the player.
        /// </summary>
        public abstract void DrawHand();

        /// <summary>
        /// Draw the Player UI Assets.
        /// </summary>
        public void DrawPlayerUI()
        {
            // Setup properties of UI assets
            Vector2 iconOffset = GetOffset(PlayerIcon, _scale);
            Vector2 borderOffset = GetOffset(PlayerCurrent, _scale);

            // Check if the player is the current playing player
            bool currentPlayer = _state.CurrentPlayer == this;
            Texture2D playerBorder = currentPlayer ? PlayerCurrent : PlayerWaiting;

            // Draw the assets
            _game.DrawImage(PlayerIcon, PlayerPosition - iconOffset, _scale, 0);
            _game.DrawImage(playerBorder, PlayerPosition - borderOffset, _scale, 0);
        }

        /// <summary>
        /// Draw the Player's name.
        /// </summary>
        public abstract void DrawPlayerName();

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
