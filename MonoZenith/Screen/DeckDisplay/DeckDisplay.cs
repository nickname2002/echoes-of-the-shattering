using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Components.TabWidget;
using MonoZenith.Engine.Support;
using MonoZenith.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen.DeckDisplay
{
    public class DeckDisplay
    {
        // Selected card type (default to Melee)
        private CardType _selectedCardType = CardType.Melee;

        // Horizontal tab widget for selecting card types
        private readonly HorizontalTabWidget _cardTypeTabWidget = new(
            new List<(Texture2D, Texture2D)>
            {
                (LoadImage("Images/LoadoutDisplay/CardAmountComponents/melee-selected.png"), 
                    LoadImage("Images/LoadoutDisplay/CardAmountComponents/melee.png")),
                (LoadImage("Images/LoadoutDisplay/CardAmountComponents/magic-selected.png"),
                    LoadImage("Images/LoadoutDisplay/CardAmountComponents/magic.png")),
                (LoadImage("Images/LoadoutDisplay/CardAmountComponents/item-selected.png"),
                    LoadImage("Images/LoadoutDisplay/CardAmountComponents/item.png")),
            },
            new Vector2(
                ScreenWidth / 2f - 170 * AppSettings.Scaling.ScaleFactor,
                100 * AppSettings.Scaling.ScaleFactor),
            scale: 0.15f * AppSettings.Scaling.ScaleFactor,
            spacing: 50 * AppSettings.Scaling.ScaleFactor
        );

        // Card amount components (sample deck)
        public static readonly List<CardAmountComponent> CardAmountComponents = new()
        {
            new CardAmountComponent(new LightSwordAttackCard()),
            new CardAmountComponent(new HeavySwordAttackCard()),
            new CardAmountComponent(new FlaskOfCrimsonTearsCard()),
            new CardAmountComponent(new FlaskOfCeruleanTearsCard()),
            new CardAmountComponent(new GlintStonePebbleCard()),
            new CardAmountComponent(new ThrowingDaggerCard()),
        };

        private const int CardsPerRow = 4;

        // Flag to track if the card positions need to be recalculated
        private bool _cardPositionsDirty = true;

        public static List<Card.Card> GeneratePlayerDeck()
        {
            var deck = new List<Card.Card>();
            foreach (var cardAmountComponent in CardAmountComponents)
            {
                for (var i = 0; i < cardAmountComponent.Amount; i++)
                {
                    var cardType = cardAmountComponent.Card.GetType();
                    var card = (Card.Card)Activator.CreateInstance(cardType);
                    deck.Add(card);
                }
            }

            return deck;
        }

        public static int GetAmountOfSelectedCards() => CardAmountComponents.Sum(card => card.Amount);

        public void DrawDeckContent()
        {
            throw new NotImplementedException();
        }

        // Calculates the positions of cards based on the selected card type
        private void CalculateCardPositions()
        {
            var filteredCards = GetComponentsForSelectedType();

            int totalCards = filteredCards.Count;
            float cardWidth = 620 * CardAmountComponents.First().Scale;
            float cardHeight = 820 * CardAmountComponents.First().Scale;

            float horizontalMargin = 100f * AppSettings.Scaling.ScaleFactor;
            float verticalMargin = 160f * AppSettings.Scaling.ScaleFactor;

            float startX = (ScreenWidth - (CardsPerRow * (cardWidth + horizontalMargin) - horizontalMargin)) / 2;
            float startY = ScreenHeight / 2f - (2 * cardHeight + verticalMargin) / 2 - 60 * AppSettings.Scaling.ScaleFactor;

            for (int i = 0; i < totalCards; i++)
            {
                int row = i / CardsPerRow;
                int column = i % CardsPerRow;

                float x = startX + column * (cardWidth + horizontalMargin);
                float y = startY + row * (cardHeight + verticalMargin);

                filteredCards[i].Position = new Vector2(x, y);
            }

            // Set the flag to false since positions are now calculated
            _cardPositionsDirty = false;
        }

        // Filters card components based on the selected card type
        private List<CardAmountComponent> GetComponentsForSelectedType()
        {
            return _selectedCardType switch
            {
                CardType.Melee => CardAmountComponents
                    .Where(card => card.Card is AttackCard && !(card.Card is MagicCard)) 
                    .ToList(),
                CardType.Magic => CardAmountComponents
                    .Where(card => card.Card is MagicCard)
                    .ToList(),
                CardType.Item => CardAmountComponents
                    .Where(card => card.Card is ItemCard)
                    .ToList(),
                _ => throw new ArgumentOutOfRangeException(nameof(_selectedCardType), "Unexpected CardType encountered.")
            };
        }

        // Updates the deck display state
        public void Update(GameTime deltaTime)
        {
            // Recalculate card positions only if they are marked as dirty
            if (_cardPositionsDirty)
            {
                CalculateCardPositions();
                _cardPositionsDirty = false;  
            }

            // Update the filtered cards
            var filteredCards = GetComponentsForSelectedType();
            foreach (var component in filteredCards)
                component.Update(deltaTime);

            // Update the tab widget and selected card type
            _cardTypeTabWidget.Update(deltaTime);
            var newSelectedCardType = (CardType)_cardTypeTabWidget.SelectedOption;
    
            // Only change selected type if it has actually changed
            if (_selectedCardType == newSelectedCardType) return;
            _selectedCardType = newSelectedCardType;
            _cardPositionsDirty = true;
        }
        
        private void DrawSelectedCardsAmount()
        {
            int amount = GetAmountOfSelectedCards();
            var stringToDraw = $"{amount}/30";
            _ = DataManager.GetInstance().CardAmountFont.MeasureString(stringToDraw).ToPoint().X;
            DrawText(
                stringToDraw, 
                new Vector2(
                    ScreenWidth - DataManager.GetInstance().ComponentFont.MeasureString(stringToDraw).X 
                                - 50 * AppSettings.Scaling.ScaleFactor, 
                    ScreenHeight / 2f - DataManager.GetInstance().ComponentFont.MeasureString(stringToDraw).Y 
                    * AppSettings.Scaling.ScaleFactor),
                DataManager.GetInstance().IndicatorFont,
                new Color(147, 137, 111),
                AppSettings.Scaling.ScaleFactor);
        }

        // Draws the deck display
        public void Draw()
        {
            // Draw the cards for the selected type
            var filteredCards = GetComponentsForSelectedType();
            foreach (var component in filteredCards)
                component.Draw();

            _cardTypeTabWidget.Draw();
            DrawSelectedCardsAmount();
        }
    }
}