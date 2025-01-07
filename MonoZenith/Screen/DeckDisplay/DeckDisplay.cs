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
                100f * AppSettings.Scaling.ScaleFactor),
            scale: 0.15f * AppSettings.Scaling.ScaleFactor,
            spacing: 50 * AppSettings.Scaling.ScaleFactor
        );

        private HorizontalTabWidget _activeNumberTabWidget;
        
        // Horizontal tabwidget for browsing melee cards
        private readonly HorizontalTabWidget _meleeNumberTabWidget = new(
            new List<(Texture2D, Texture2D)>
            {
                (LoadImage("Images/Miscellaneous/Numbers/1-number-silver.png"), 
                    LoadImage("Images/Miscellaneous/Numbers/1-number-gold.png")),
                (LoadImage("Images/Miscellaneous/Numbers/2-number-silver.png"),
                    LoadImage("Images/Miscellaneous/Numbers/2-number-gold.png")),
                (LoadImage("Images/Miscellaneous/Numbers/3-number-silver.png"),
                    LoadImage("Images/Miscellaneous/Numbers/3-number-gold.png"))
            },
            new Vector2(
                ScreenWidth / 2f - 66.5f * AppSettings.Scaling.ScaleFactor,
                ScreenHeight - 100f * AppSettings.Scaling.ScaleFactor),
            scale: 0.10f * AppSettings.Scaling.ScaleFactor,
            spacing: 50 * AppSettings.Scaling.ScaleFactor
        );

        // Horizontal tabwidget for browsing magic cards
        private readonly HorizontalTabWidget _magicNumberTabWidget = new(
            new List<(Texture2D, Texture2D)>
            {
                (LoadImage("Images/Miscellaneous/Numbers/1-number-silver.png"), 
                    LoadImage("Images/Miscellaneous/Numbers/1-number-gold.png")),
            },
            new Vector2(
                ScreenWidth / 2f - 37.5f * AppSettings.Scaling.ScaleFactor,
                ScreenHeight - 100f * AppSettings.Scaling.ScaleFactor),
            scale: 0.10f * AppSettings.Scaling.ScaleFactor,
            spacing: 50 * AppSettings.Scaling.ScaleFactor
        );

        // Horizontal tabwidget for browsing item cards
        private readonly HorizontalTabWidget _itemNumberTabWidget = new(
            new List<(Texture2D, Texture2D)>
            {
                (LoadImage("Images/Miscellaneous/Numbers/1-number-silver.png"), 
                    LoadImage("Images/Miscellaneous/Numbers/1-number-gold.png")),
            },
            new Vector2(
                ScreenWidth / 2f,
                ScreenHeight - 100f * AppSettings.Scaling.ScaleFactor),
            scale: 0.10f * AppSettings.Scaling.ScaleFactor,
            spacing: 50 * AppSettings.Scaling.ScaleFactor
        );

        public static readonly List<CardAmountComponent> CardAmountComponents = new()
        {
            // Default deck
            new CardAmountComponent(new LightSwordAttackCard()),
            new CardAmountComponent(new HeavySwordAttackCard()),
            new CardAmountComponent(new FlaskOfCrimsonTearsCard()),
            new CardAmountComponent(new FlaskOfCeruleanTearsCard()),
            new CardAmountComponent(new GlintStonePebbleCard()),
            new CardAmountComponent(new ThrowingDaggerCard()),
            new CardAmountComponent(new EndureCard()),
            new CardAmountComponent(new DoubleSlashCard()),
            new CardAmountComponent(new WarCryCard()),
            new CardAmountComponent(new FlaskOfWondrousPhysickCard()),
            
            // Melee cards
            new CardAmountComponent(new UnsheatheCard(), "Bloody Finger Hunter Yura"),
            new CardAmountComponent(new BloodhoundStepCard(), "Blaidd, The Half-Wolf"),
            new CardAmountComponent(new QuickstepCard(), "White Mask Varré"),
            new CardAmountComponent(new StormcallerCard(), "Margit, The Fell Omen"),
            new CardAmountComponent(new RallyingStandardCard(), "Commander Niall"),
            new CardAmountComponent(new ICommandTheeKneelCard(), "Godrick the Grafted"),
            new CardAmountComponent(new WaterfowlDanceCard(), "Malenia, Blade of Miquella"),
            new CardAmountComponent(new StarcallerCryCard(), "Starscourge Radahn"),
            new CardAmountComponent(new CursedBloodSliceCard(), "Morgott, The Omen King"),
            new CardAmountComponent(new BloodboonRitualCard(), "Mohg, Lord of Blood"),
            new CardAmountComponent(new DestinedDeathCard(), "Maliketh, the Black Blade"),
            new CardAmountComponent(new RegalRoarCard(), "Godfrey, The First Elden Lord"),
            new CardAmountComponent(new WaveOfGoldCard(), "Radagon of the Golden Order"),
            new CardAmountComponent(new PoisonPotCard(), "Gatekeeper Gostoc"),
            
            // Magic cards
            new CardAmountComponent(new GlintbladePhalanxCard(), "Red Wolf of Radagon"),
            new CardAmountComponent(new ThopsBarrierCard(), "Thops"),
            new CardAmountComponent(new GreatGlintStoneCard(), "Sorceress Sellen"),
            new CardAmountComponent(new CarianGreatSwordCard(), "Royal Knight Loretta"),
            new CardAmountComponent(new CometAzurCard(), "Rennala, Queen of the Full Moon"),
            new CardAmountComponent(new MoonlightGreatswordCard(), "Tarnished, Consort of the Stars"),
            
            // Item cards
            new CardAmountComponent(new LarvalTearCard(), "Sir Gideon Ofnir, The All-Knowing"),
            new CardAmountComponent(new BaldachinBlessingCard(), "Fia, The Deathbed Companion"),
        };

        private const int CardsPerRow = 4;

        public DeckDisplay()
        {
            _activeNumberTabWidget = _meleeNumberTabWidget;
        }
        
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

        private List<CardAmountComponent> GetCardsInSelectedGroup(int groupIndex)
        {
            // Filter the cards based on the selected type (Melee, Magic, Item)
            var filteredCards = GetComponentsForSelectedType();
            filteredCards = GetUnlockedCards(filteredCards);

            // Divide cards into groups of 8
            const int cardsPerGroup = 8;
            int totalGroups = (int)Math.Ceiling((double)filteredCards.Count / cardsPerGroup); 

            // Ensure groupIndex is within bounds
            if (groupIndex < 0 || groupIndex >= totalGroups)
                return new List<CardAmountComponent>(); // Return an empty list if the index is out of range

            // Calculate start and end indexes for the selected group
            int startIndex = groupIndex * cardsPerGroup;
            int endIndex = Math.Min(startIndex + cardsPerGroup, filteredCards.Count);

            // Return the cards for the selected group
            return filteredCards.GetRange(startIndex, endIndex - startIndex);
        }
        
        private List<CardAmountComponent> GetUnlockedCards(List<CardAmountComponent> cards)
        {
            return cards.Where(card => card.IsUnlocked()).ToList();
        }
        
        public void DrawDeckContent()
        {
            int selectedGroup = _activeNumberTabWidget.SelectedOption;
            var cardsToDraw = GetCardsInSelectedGroup(selectedGroup);
            cardsToDraw = GetUnlockedCards(cardsToDraw);

            // Draw only the cards in the selected group
            foreach (var component in cardsToDraw)
            {
                component.Draw();
            }
        }
        
        // Calculates the positions of cards based on the selected card type
        private void CalculateCardPositions()
        {
            // Loop through each enum (cardtype)
            for (var i = 0; i < 3; i++) 
            {
                // Get amount from each type (filter cards based on the current CardType)
                var filteredCards = GetComponentForSpecificType((CardType)i);
                filteredCards = GetUnlockedCards(filteredCards);

                // Get each group of 8 (divide cards into groups of 8)
                for (var j = 0; j < filteredCards.Count; j += 8)
                {
                    // Calculate position for each card in the current group of 8
                    int groupSize = Math.Min(8, filteredCards.Count - j); 
                    
                    // Loop through the cards in the current group
                    for (int k = 0; k < groupSize; k++)
                    {
                        // Calculate position for the card based on its index in the group
                        float cardWidth = 620 * CardAmountComponents.First().Scale;
                        float cardHeight = 820 * CardAmountComponents.First().Scale;

                        float horizontalMargin = 100f * AppSettings.Scaling.ScaleFactor;
                        float verticalMargin = 160f * AppSettings.Scaling.ScaleFactor;

                        float startX = 
                            (ScreenWidth - (CardsPerRow * (cardWidth + horizontalMargin) - horizontalMargin)) / 2;
                        float startY = 
                            ScreenHeight / 2f - (2 * cardHeight + verticalMargin) / 2 - 60 * AppSettings.Scaling.ScaleFactor;

                        // Calculate row and column within the current group
                        int row = k / CardsPerRow;
                        int column = k % CardsPerRow;

                        // Calculate the X and Y position for this card
                        float x = startX + column * (cardWidth + horizontalMargin);
                        float y = startY + row * (cardHeight + verticalMargin);

                        // Set the card's position
                        filteredCards[j + k].Position = new Vector2(x, y);
                    }
                }
            }
        }
        
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
        
        private List<CardAmountComponent> GetComponentForSpecificType(CardType type)
        {
            return type switch
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

        private void UpdateCorrectNumberTabWidget(GameTime deltaTime)
        {
            switch (_selectedCardType)
            {
                case CardType.Melee:
                    _meleeNumberTabWidget.Update(deltaTime);
                    break;
                case CardType.Magic:
                    _magicNumberTabWidget.Update(deltaTime);
                    break;
                case CardType.Item:
                    _itemNumberTabWidget.Update(deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_selectedCardType), "Unexpected CardType encountered.");
            }
        }
        
        private void DrawCorrectNumberTabWidget()
        {
            switch (_selectedCardType)
            {
                case CardType.Melee:
                    _meleeNumberTabWidget.Draw();
                    break;
                case CardType.Magic:
                    _magicNumberTabWidget.Draw();
                    break;
                case CardType.Item:
                    _itemNumberTabWidget.Draw();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_selectedCardType), 
                        "Unexpected CardType encountered.");
            }
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
        
        public void Update(GameTime deltaTime)
        {
            _activeNumberTabWidget = _selectedCardType switch
            {
                CardType.Melee => _meleeNumberTabWidget,
                CardType.Magic => _magicNumberTabWidget,
                CardType.Item => _itemNumberTabWidget,
                _ => throw new ArgumentOutOfRangeException(nameof(_selectedCardType), 
                    "Unexpected CardType encountered.")
            };
            
            CalculateCardPositions();
            UpdateCorrectNumberTabWidget(deltaTime);
            
            // Filter and update cards
            int selectedGroup = _activeNumberTabWidget.SelectedOption;
            var filteredCards = GetCardsInSelectedGroup(selectedGroup);
            foreach (var component in filteredCards)
                component.Update(deltaTime);

            // Update the tab widget and selected card type
            _cardTypeTabWidget.Update(deltaTime);
            _selectedCardType = (CardType)_cardTypeTabWidget.SelectedOption;;
        }

        // Draws the deck display
        public void Draw()
        {
            DrawCorrectNumberTabWidget();
            DrawDeckContent();
            _cardTypeTabWidget.Draw();
            DrawSelectedCardsAmount();
        }
    }
}