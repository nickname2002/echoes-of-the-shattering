using System;
using Microsoft.Xna.Framework;
using MonoZenith.Card;
using MonoZenith.Card.CardStack;
using MonoZenith.Support;

namespace MonoZenith.Players
{
    internal abstract class Player
    {
        private Game _game;
        protected GameState _state;
        public CardStack Hand;
        public string Name;
        public float width;
        public float height;

        protected Player(Game game, GameState state, string name)
        {
            _game = game;
            _state = state;
            Hand = new CardStack(_game, _state);
            Name = name;
        }

        public override string ToString()
        {
            return $"==== {Name} ====\n{Hand}\n";
        }

        /// <summary>
        /// Perform the player's turn.
        /// </summary>
        /// <param name="state">The current game state.</param>
        public abstract void PerformTurn(GameState state);

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
        public void DrawCombo(GameState state)
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
            RegionCard effectCard = card as RegionCard;
            effectCard.PerformEffect(_state);
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
    }
}
