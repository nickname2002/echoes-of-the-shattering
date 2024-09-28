using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoZenith.Card;
using MonoZenith.Support;

namespace MonoZenith.Players
{
    internal class HumanPlayer : Player
    {
        private Card.Card _lastHoveredCard = null;  
        
        public HumanPlayer(Game game, GameState state, string name) : base(game, state, name)
        {
            width = game.ScreenWidth / 2;
            height = game.ScreenHeight / 1.25f;
        }
    
        /// <summary>
        /// Draw all assets of the HumanPlayer.
        /// </summary>
        public override void Draw()
        {
            DrawHand();
        }

        /// <summary>
        /// Draw the Hand of the HumanPlayer.
        /// </summary>
        public override void DrawHand()
        {
            int count = Hand.Count;
            if (count == 0) 
                return;
            
            // Variables and buffers
            float widthStep = width / count;
            List<Card.Card> hoveredCards = new List<Card.Card>();  
            Dictionary<Card.Card, float> cardPositions = new Dictionary<Card.Card, float>(); 

            // Draw cards
            DrawNonHoveredCards(Hand.Cards, hoveredCards, cardPositions, widthStep);
            DrawHoveredCards(hoveredCards, cardPositions);
        }

        /// <summary>
        /// Draws all non-hovered cards and stores hovered cards with their positions.
        /// </summary>
        /// <param name="cards">List of cards to process.</param>
        /// <param name="hoveredCards">List to store hovered cards.</param>
        /// <param name="cardPositions">Dictionary to store card positions.</param>
        /// <param name="widthStep">Step for card positioning.</param>
        private void DrawNonHoveredCards(List<Card.Card> cards, List<Card.Card> hoveredCards, 
                                         Dictionary<Card.Card, float> cardPositions, float widthStep)
        {
            int count = cards.Count();
            int currentIndex = 0;
            
            foreach (Card.Card card in cards)
            {
                float currentWidth = width + (width / 2) - (widthStep * count) + (widthStep * currentIndex);

                if (card.IsHovered())
                {
                    hoveredCards.Add(card);
                    cardPositions[card] = currentWidth;
                }
                else
                {
                    card.Draw(currentWidth, height, 0, false, true);
                }
                
                currentIndex++;
            }
        }
        
        private void DrawHoveredCards(List<Card.Card> hoveredCards, Dictionary<Card.Card, float> cardPositions)
        {
            const int verticalMoveOffset = 20;
            _lastHoveredCard = null;

            // Draw hovered cards, except the last one, in their original positions
            for (int i = 0; i < hoveredCards.Count; i++)
            {
                Card.Card hoveredCard = hoveredCards[i];
                float hoveredCardPosition = cardPositions[hoveredCard];

                // Draw all hovered cards except the last one first
                if (i < hoveredCards.Count - 1)
                {
                    hoveredCard.Draw(hoveredCardPosition, height, 0, false, true);
                }
            }

            // Draw the last hovered card (if any), move it slightly up, and store it
            if (hoveredCards.Count <= 0) 
                return;
            
            _lastHoveredCard = hoveredCards[^1];  
            float lastHoveredCardPosition = cardPositions[_lastHoveredCard];
            _lastHoveredCard.Draw(lastHoveredCardPosition, height - verticalMoveOffset, 0, false, true);
        }

        /// <summary>
        /// Get the selected card from the hand.
        /// </summary>
        /// <returns>Selected card, or null if no card is hovered or clicked.</returns>
        public Card.Card GetSelectedCard()
        {
            // Check if the last hovered card was clicked and return it if true
            if (_lastHoveredCard != null && _lastHoveredCard.IsClicked())
            {
                Console.WriteLine($"Hovered card selected: {_lastHoveredCard}");
                return _lastHoveredCard;
            }

            // If no hovered card was clicked, check for any other clicked cards
            List<Card.Card> clickedCards = Hand.Cards.Where(c => c.IsClicked()).ToList();
    
            // Print names of all clicked cards
            Console.WriteLine("Clicked cards:");
            foreach (var card in clickedCards)
            {
                Console.WriteLine($"- {card}");
            }

            // Return the first clicked card, or null if no cards were clicked
            return clickedCards.Count switch
            {
                0 => null,
                >= 1 => clickedCards[0],
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void PerformTurn(GameState state)
        {
            if (TryPlayCard()) 
                return;

            DrawCard();
        }

        /// <summary>
        /// Attempt to play the selected card.
        /// </summary>
        /// <returns>Whether a valid card was played.</returns>
        private bool TryPlayCard()
        {
            var selectedCard = GetSelectedCard();

            // If no card is selected or the card is not valid, return false.
            if (selectedCard == null || !IsValidPlay(selectedCard)) 
                return false;

            PlayCard(selectedCard);
            return true;
        }

        /// <summary>
        /// Check if the selected card is a valid play based on the last played card.
        /// </summary>
        /// <param name="card">The card to check.</param>
        /// <returns>True if the card can be played, false otherwise.</returns>
        private bool IsValidPlay(Card.Card card)
        {
            var lastPlayedCard = _state.PlayedCards.Cards[^1]; 
            return card.ValidNextCard(lastPlayedCard);
        }

        /// <summary>
        /// Play the selected card and update the game state.
        /// </summary>
        /// <param name="card">The card to play.</param>
        private void PlayCard(Card.Card card)
        {
            Console.WriteLine($"Human player played: {card}");

            // Update the current region if the card is a RegionCard and region is not "ALL"
            if (card is RegionCard regionCard && regionCard.Region != Region.ALL)
            {
                _state.CurrentRegion = regionCard.Region;
            }

            // Add the card to the played pile and remove it from the player's hand
            _state.PlayedCards.AddToBottom(card);
            Hand.Cards.Remove(card);
        }

        /// <summary>
        /// Draw a card from the deck and add it to the player's hand.
        /// </summary>
        private void DrawCard()
        {
            var drawnCard = _state.DrawableCards.GetSelectCard();

            // If no card was drawn, return
            if (drawnCard == null) 
                return;

            Console.WriteLine($"Human player drew: {drawnCard}");
            Hand.AddToFront(drawnCard);
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
