using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoZenith.Engine.Support;

namespace MonoZenith.Players
{
    internal class HumanPlayer : Player
    {
        private Card.Card _lastHoveredCard = null;  
        
        public HumanPlayer(Game game, GameState state, string name) : base(game, state, name)
        {
            _handxPos = game.ScreenWidth / 2f;
            _handyPos = game.ScreenHeight / 1.41f;
            PlayerPosition = new Vector2(game.ScreenWidth * 0.06f, game.ScreenHeight * 0.9f);
            PlayerIcon = DataManager.GetInstance(game).Player;
        }
    
        /// <summary>
        /// Draw all assets of the HumanPlayer.
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
            Vector2 textPosition = PlayerPosition + new Vector2(offset.X * 1.2f, offset.Y * 0.25f);
            Vector2 shadowPosition = new(1.25f, 1.25f);
            Vector2 healthOffset = new(1, 1);
            int healthHeight = (int)(PlayerCurrent.Height * Scale * 0.05f);
            int healthWidth = (int)(_game.ScreenWidth * 0.9f);
            Vector2 healthPosition = PlayerPosition + new Vector2(0, offset.Y - healthHeight) - healthOffset;
            int currentHealth = Math.Min(GetOpponentHandCount(), 7);

            // Draw text and health bar
            _game.DrawText(Name, textPosition + shadowPosition, PlayerFont, Color.DarkGray);
            _game.DrawText(Name, textPosition, PlayerFont, Color.White);

            _game.DrawRectangle(Color.Gold, healthPosition - healthOffset, healthWidth + 2, healthHeight + 2);
            _game.DrawRectangle(Color.DarkGray, healthPosition, healthWidth, healthHeight);

            // Draw current health based on opponent's hand count
            // PlayHealthSound(currentHealth);
            _game.DrawRectangle(Color.Red, healthPosition, (int)(healthWidth / 7 * currentHealth), healthHeight);
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
            float widthStep = _handxPos / count;
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
        /// <param name="xPosStep">Step for card positioning.</param>
        private void DrawNonHoveredCards(List<Card.Card> cards, List<Card.Card> hoveredCards, 
                                         Dictionary<Card.Card, float> cardPositions, float xPosStep)
        {
            int count = cards.Count();
            int currentIndex = 0;
            
            foreach (Card.Card card in cards)
            {
                float currentWidth = _handxPos + (_handxPos / 2) - (xPosStep * count) + (xPosStep * currentIndex);

                if (card.IsHovered())
                {
                    hoveredCards.Add(card);
                    cardPositions[card] = currentWidth;
                }
                else
                {
                    card.Draw(currentWidth, _handyPos, 0, false, true);
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
                    hoveredCard.Draw(hoveredCardPosition, _handyPos, 0, false, true);
                }
            }

            // Draw the last hovered card (if any), move it slightly up, and store it
            if (hoveredCards.Count <= 0) 
                return;
            
            _lastHoveredCard = hoveredCards[^1];  
            float lastHoveredCardPosition = cardPositions[_lastHoveredCard];
            _lastHoveredCard.Draw(lastHoveredCardPosition, _handyPos - verticalMoveOffset, 0, false, true);
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
            base.PerformTurn(state);
            if (!_state.GraceMenu.Hidden)
            {
                return;
            }
            
            if (TryPlayCard()) 
                return;

            // If no card can be played, draw a card.
            TryDrawCard();
        }

        /// <summary>
        /// Attempt to play the selected card.
        /// </summary>
        /// <returns>Whether a valid card was played.</returns>
        protected override bool TryPlayCard()
        {
            var selectedCard = GetSelectedCard();

            // If no card is selected or the card is not valid, return false.
            if (selectedCard == null || !IsValidPlay(selectedCard)) 
                return false;

            PlayCard(selectedCard);
            return true;
        }

        /// <summary>
        /// Draw a card from the deck and add it to the player's hand.
        /// </summary>
        protected override void TryDrawCard()
        {
            if (_state.Combo >= 1)
            {
                return;
            }
            
            var drawnCard = _state.DrawableCards.GetSelectCard();

            // If no card was drawn, return
            if (drawnCard == null) 
                return;

            Console.WriteLine($"{Name} drew: {drawnCard}");
            Hand.AddToFront(drawnCard);
            _state.SwitchTurn();    // TODO: may not be true in every situation
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
