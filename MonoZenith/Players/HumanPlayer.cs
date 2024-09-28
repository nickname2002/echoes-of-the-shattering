using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoZenith.Card;

namespace MonoZenith.Classes.Players
{
    internal class HumanPlayer : Player
    {
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

        /// <summary>
        /// Draws all hovered cards, with the last hovered card drawn on top.
        /// </summary>
        /// <param name="hoveredCards">List of hovered cards.</param>
        /// <param name="cardPositions">Dictionary storing the positions of each card.</param>
        private void DrawHoveredCards(List<Card.Card> hoveredCards, Dictionary<Card.Card, float> cardPositions)
        {
            const int verticalMoveOffset = 20;
            
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

            // Draw the last hovered card (if any), move it slightly up, and draw it last to ensure it is on top
            if (hoveredCards.Count <= 0) 
                return;
            
            Card.Card lastHoveredCard = hoveredCards[^1];
            float lastHoveredCardPosition = cardPositions[lastHoveredCard];
            lastHoveredCard.Draw(lastHoveredCardPosition, height - verticalMoveOffset, 0, false, true);
        }

        /// <summary>
        /// Get the selected card from the hand.
        /// </summary>
        /// <returns>Selected card, or null if no card is hovered.</returns>
        public Card.Card GetSelectedCard()
        {
            List<Card.Card> clickedCards = Hand.Cards.Where(c => c.IsClicked()).ToList();
            return clickedCards.Count switch
            {
                0 => null,
                > 1 => clickedCards[^1],
                _ => clickedCards[0]
            };
        }
        
        public override void PerformTurn(GameState state)
        {
            Card.Card selectedCard = GetSelectedCard();
            Card.Card drawnCard = _state.DrawableCards.GetSelectCard();
            
            // Playing a card
            if (selectedCard != null)
            {
                if (selectedCard.ValidNextCard(_state.PlayedCards.Cards[^1]))
                {
                    Console.WriteLine($"Human player played: {selectedCard}");
                    
                    if (selectedCard is RegionCard regionCard)
                    {
                        if (regionCard.Region != Region.ALL)
                        {
                            _state.CurrentRegion = regionCard.Region;
                        }
                    }
                    
                    _state.PlayedCards.AddToBottom(selectedCard);
                    Hand.Cards.Remove(selectedCard);   
                    return;
                }
                
                PerformTurn(_state);
            }

            // Drawing cards
            if (drawnCard == null) 
                return;
            
            Console.WriteLine($"Human player drew: {drawnCard}");
            Hand.AddToFront(drawnCard);
        }

        public override void Update(GameTime deltaTime)
        {
            // Update every card in hand
            foreach (var card in Hand.Cards)
            {
                card.Update(deltaTime);
            }
        }
    }
}
