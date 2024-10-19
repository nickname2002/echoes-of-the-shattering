using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public CardStack Hand;
        public string Name;
        
        // TODO: Remove (some) sounds once they are built into the cards themselves.
        private readonly SoundEffectInstance _damageSound;
        private readonly SoundEffectInstance _healingSound;
        private readonly SoundEffectInstance _cardSound1;
        private readonly SoundEffectInstance _cardSound2;

        protected Player(Game game, GameState state, string name)
        {
            _game = game;
            _state = state;
            Hand = new CardStack(_game, _state);
            Name = name;
            Scale = 0.15f;
            Health = 100f;
            Stamina = 30f;
            Focus = 30f;
            
            // Load textures and sound effects for player
            PlayerCurrent = DataManager.GetInstance(game).PlayerCurrent;
            PlayerWaiting = DataManager.GetInstance(game).PlayerWaiting;
            PlayerFont = DataManager.GetInstance(game).PlayerFont;
            _damageSound = DataManager.GetInstance(game).DamageSound;
            _healingSound = DataManager.GetInstance(game).HealingSound;
            _cardSound1 = DataManager.GetInstance(game).LightSwordAttack;
            _cardSound2 = DataManager.GetInstance(game).CardSound2;
            _damageSound.Volume = 0.2f;
            _healingSound.Volume = 0.3f;
            _cardSound1.Volume = 0.3f;
            _cardSound2.Volume = 0.3f;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draw a card from the drawable cards stack and add it to the player's hand.
        /// </summary>
        public void DrawCard()
        {
            throw new NotImplementedException();
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
