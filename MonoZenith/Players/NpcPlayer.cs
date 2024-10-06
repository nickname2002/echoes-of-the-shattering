using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoZenith.Card;
using MonoZenith.Engine.Support;

namespace MonoZenith.Players
{
    internal class NpcPlayer : Player
    {
        public NpcPlayer(Game game, GameState state, string name) : base(game, state, name)
        {
            _handxPos = game.ScreenWidth / 2f;
            _handyPos = game.ScreenHeight / 3.45f;
            PlayerPosition = new Vector2(game.ScreenWidth * 0.06f, game.ScreenHeight * 0.1f);
            PlayerIcon = DataManager.GetInstance(game).Npc;
        }

        /// <summary>
        /// Draw all assets of the NpcPlayer.
        /// </summary>
        public override void Draw()
        {
            DrawPlayerHealthAndName();
            DrawPlayerUI();
            DrawHand();
        }

        public override void DrawPlayerHealthAndName()
        {
            // TODO: Refactor later
            // Setup offsets and positions for text and health bar
            Vector2 offset = GetOffset(PlayerCurrent, Scale);
            Vector2 textPosition = PlayerPosition + new Vector2(offset.X * 1.2f, -offset.Y * 0.75f);
            Vector2 shadowPosition = new(1.25f, 1.25f);
            Vector2 healthOffset = new(1, 1);
            int healthHeight = (int)(PlayerCurrent.Height * Scale * 0.05f);
            int healthWidth = (int)(_game.ScreenWidth * 0.9f);
            Vector2 healthPosition = PlayerPosition + new Vector2(0, -offset.Y) + healthOffset;
            int currentHealth = Math.Min(GetOpponentHandCount(), 7);

            // Draw text and health bar
            _game.DrawText(Name, textPosition + shadowPosition, PlayerFont, Color.DarkGray);
            _game.DrawText(Name, textPosition, PlayerFont, Color.White);

            _game.DrawRectangle(Color.Gold, healthPosition - healthOffset, healthWidth + 2, healthHeight + 2);
            _game.DrawRectangle(Color.DarkGray, healthPosition, healthWidth, healthHeight);

            // Draw current health based on opponent's hand count
            PlayHealthSound(currentHealth);
            _game.DrawRectangle(Color.Red, healthPosition, (int)(healthWidth / 7 * currentHealth), healthHeight);
        }

        /// <summary>
        /// Draw the Hand of the NpcPlayer.
        /// </summary>
        public override void DrawHand()
        {
            int count = Hand.Count;

            if (count == 0)
                return;

            float widthStep = _handxPos / count;
            
            foreach (Card.Card card in Hand.Cards)
            {
                float currentWidth = _handxPos - (_handxPos / 2) + (widthStep * count);

                card.Draw(currentWidth, _handyPos, 180, false, false);
                count--;
            }
        }

        /// <summary>
        /// Orders the hand so that:
        /// - All cards of type RegionCard are first in the hand.
        /// - All cards of type JokerCard and LunarQueenRebirthCard are last in the hand.
        /// </summary>
        private void OrderHand()
        {
            // Get all RegionCards (excluding offensive cards, to be placed first)
            var regionCards = Hand.Cards
                .OfType<RegionCard>()
                .Where(card => card is not JokerCard && card is not LunarQueenRebirthCard)
                .ToList();

            // Get all offensive cards (JokerCard and LunarQueenRebirthCard, to be placed last)
            var offensiveCards = Hand.Cards
                .OfType<RegionCard>()
                .Where(card => card is JokerCard or LunarQueenRebirthCard)
                .ToList();

            // Get all remaining cards that are not RegionCards (to be placed in the middle)
            var otherCards = Hand.Cards
                .Where(card => card is not RegionCard)
                .ToList();

            // Clear the current hand
            Hand.Cards.Clear();

            // Add RegionCards first, followed by other cards, and offensive cards last
            Hand.Cards.AddRange(regionCards);
            Hand.Cards.AddRange(otherCards);
            Hand.Cards.AddRange(offensiveCards);
        }
        
        /// <summary>
        /// Strategy to play when the opposing player has less than 4 cards.
        /// </summary>
        /// <returns>If strategy could be executed.</returns>
        private bool TryOffensiveStrategy()
        {
            List<RegionCard> offensiveCards = Hand.Cards.OfType<RegionCard>().Where(
                card => card is LunarQueenRebirthCard or JokerCard).ToList();
            
            // Play a switch card if available
            foreach (RegionCard card in offensiveCards)
            {
                if (card is not LunarQueenRebirthCard || IsValidPlay(card)) 
                    continue;
                
                PlayCard(card);
                return true;
            }
            
            // Else, play joker card if available
            foreach (RegionCard card in offensiveCards)
            {
                if (card is not JokerCard) 
                    continue;
                
                PlayCard(card);
                return true;
            }
            
            return false;
        }
            
        protected override bool TryPlayCard()
        {
            foreach (var card in Hand.Cards)
            {
                if (!IsValidPlay(card)) 
                    continue;
                
                PlayCard(card);
                return true;
            }

            return false;
        }

        protected override void TryDrawCard()
        {
            if (_state.Combo >= 1)
            {
                return;
            }
            
            var drawnCard = _state.DrawableCards.GetCard();
            Console.WriteLine($"{Name} drew: {drawnCard}");
            Hand.AddToFront(drawnCard);
            _state.SwitchTurn();    // TODO: may not be true in every situation
        }

        /// <summary>
        /// Checks if the given card can be played.
        /// </summary>
        private void PlayComboCard()
        {
            RegionCard comboCard = Hand.Cards.Cast<RegionCard>().
                FirstOrDefault(regionCard => regionCard.IsComboCard);

            if (comboCard == null)
                return;
            
            PlayCard(comboCard);
        }
        
        public override void PerformTurn(GameState state)
        {
            base.PerformTurn(_state);
            OrderHand();
            int cardsInOpponentHand = state.OpposingPlayer.Hand.Count;

            if (((RegionCard)state.PlayedCards.Cards.First()).IsComboCard)
            {
                PlayComboCard();
                return;
            }
            
            // If the opponent has less than 4 cards, try to play offensive cards.
            if (cardsInOpponentHand < 4)
            {
                if (TryOffensiveStrategy())
                {
                    return;
                }
            }

            // If the situation is stable, play the first card available.
            if (TryPlayCard())
                return;

            // If no card can be played, draw a card.
            TryDrawCard();
        }
        
        public override void Update(GameTime deltaTime)
        {
            foreach (var card in Hand.Cards)
            {
                card.Update(deltaTime);
            }
        }
    }
}
