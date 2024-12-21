using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Components.Indicator;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using static MonoZenith.Game;

namespace MonoZenith.Players
{
    public sealed class HumanPlayer : Player
    {
        private Card.Card _lastHoveredCard;
        private CardStackIndicator _deckIndicator;
        private CardStackIndicator _reserveIndicator;
        private SpiritAshIndicator _spiritAshIndicator;
        
        public HumanPlayer(GameState state, string name) : base(state, name)
        {
            _handPosY = ScreenHeight / 1.45f;
            _playerPosition = new Vector2(
                ScreenWidth * 0.05f, 
                ScreenHeight * 0.915f);
            _playerIcon = DataManager.GetInstance().Player;
        }

        public override void InitializeState(GameState state)
        {
            base.InitializeState(state);
            OpposingPlayer = state.Npc;
            
            // Initialize indicators
            _deckIndicator = new CardStackIndicator(state, 
                new Vector2(
                    ScreenWidth - 185 * AppSettings.Scaling.ScaleFactor, 
                    ScreenHeight - 200 * AppSettings.Scaling.ScaleFactor), 
                DataManager.GetInstance().DeckIndicator,
                _deckStack);
            _reserveIndicator = new CardStackIndicator(state, 
                new Vector2(
                    ScreenWidth - 100 * AppSettings.Scaling.ScaleFactor, 
                    ScreenHeight - 253 * AppSettings.Scaling.ScaleFactor), 
                DataManager.GetInstance().ReserveIndicator,
                _reserveCardStack);
            _spiritAshIndicator = new SpiritAshIndicator(state, 
                new Vector2(
                    ScreenWidth - 100 * AppSettings.Scaling.ScaleFactor, 
                    ScreenHeight - 147 * AppSettings.Scaling.ScaleFactor), 
                DataManager.GetInstance().AshIndicatorDisabled, 
                new JellyfishAsh(_game, _state, this));
        }

        protected override void FillPlayerDeck()
        {
            var deck = new List<Card.Card>
            {
                // Flasks
                new FlaskOfCrimsonTearsCard(_game, _state, this),
                new FlaskOfCrimsonTearsCard(_game, _state, this),

                new FlaskOfCeruleanTearsCard(_game, _state, this),
                new FlaskOfCeruleanTearsCard(_game, _state, this),

                new FlaskOfWondrousPhysickCard(_game, _state, this),
                new FlaskOfWondrousPhysickCard(_game, _state, this),

                new WarmingStoneCard(_game, _state, this),
                new BaldachinBlessingCard(_game, _state, this),
                new ThrowingDaggerCard(_game, _state, this),
                new PoisonPotCard(_game, _state, this),
                new LarvalTearCard(_game, _state, this),

                // Basic attacks
                new LightSwordAttackCard(_state, this),
                new LightSwordAttackCard(_state, this),
                new LightSwordAttackCard(_state, this),
                new UnsheatheCard(_game, _state, this),
                new WarCryCard(_game, _state, this),
                new QuickstepCard(_game, _state, this),
                new EndureCard(_game, _state, this),

                new HeavySwordAttackCard(_game, _state, this),
                new BloodhoundStepCard(_game, _state, this),
                new DoubleSlashCard(_game, _state, this),
                new RallyingStandardCard(_game, _state, this),
                new WaterfowlDanceCard(_game, _state, this),
                new ICommandTheeKneelCard(_state, this),

                // Magic attacks
                new GlintStonePebbleCard(_game, _state, this),
                new GreatGlintStoneCard(_game, _state, this),
                new GlintbladePhalanxCard(_game, _state, this),
                new ThopsBarrierCard(_game, _state, this),
                new CarianGreatSwordCard(_game, _state, this),
                new CometAzurCard(_game, _state, this)
            };
            
            _deckStack.AddToFront(deck);
            
            // Set the starting position of the cards when moving from the deck to the hand
            _deckStack.SetPosition(new Vector2(
                ScreenWidth / 2f,
                ScreenHeight + Card.Card.Height / 2f));
            _reserveCardStack.SetPosition(new Vector2(
                ScreenWidth / 2f,
                ScreenHeight + Card.Card.Height / 2f));
            
            foreach (var card in _handStack.Cards)
                card.Stack = _deckStack;
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
            List<Card.Card> clickedCards = _handStack.Cards.Where(c => c.IsClicked()).ToList();
    
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
            _spiritAshIndicator.Update(state.GameTime);
            TryPlayCard();
        }

        /// <summary>
        /// Attempt to play the selected card.
        /// </summary>
        /// <returns>Whether a valid card was played.</returns>
        public bool TryPlayCard()
        {
            var selectedCard = GetSelectedCard();

            // If no card is selected or the card is not valid, return false.
            if (selectedCard == null) 
                return false;

            // If the card conditions are not met, return false.
            if (!selectedCard.IsAffordable())
            {
                return false;
            }
                
            PlayCard(selectedCard);
            return true;
        }

        public override void Update(GameTime deltaTime)
        {
            _handStack.Update(deltaTime);
            _deckStack.Update(deltaTime);
            _reserveCardStack.Update(deltaTime);
            _deckIndicator.Update(deltaTime);
            _reserveIndicator.Update(deltaTime);
        }

        protected override void DrawPlayerHealthAndName()
        {
            // Setup offsets and positions for name and player bars
            Vector2 playerOffset = GetOffset(_playerCurrent, _scale);
            Vector2 namePosition = _playerPosition + new Vector2(playerOffset.X * 1.2f, 0);
            Vector2 shadowPosition = new(1.25f, 1.25f);
            int healthHeight = (int)(_playerCurrent.Height * _scale * 0.05f);
            int healthWidth = (int)(ScreenWidth * 0.9f);
            int barWidth = (int)(ScreenWidth * 0.3f);
            Vector2 barOffset = new Vector2(0, healthHeight + 4);
            Vector2 healthPosition = _playerPosition + new Vector2(0, playerOffset.Y - healthHeight * 4.5f) - new Vector2(1, 1);
            Vector2 edgePosition = healthPosition - new Vector2(1, 1);

            // Draw name
            DrawText(Name, namePosition + shadowPosition, _playerFont, Color.DarkGray);
            DrawText(Name, namePosition, _playerFont, Color.White);

            // Draw Health bar with current health points
            DrawRectangle(Color.Goldenrod, edgePosition, healthWidth + 2, healthHeight + 2);
            DrawRectangle(Color.DarkGray, healthPosition, healthWidth, healthHeight);
            DrawRectangle(Color.DarkRed, healthPosition, (int)(healthWidth * (Health / 100f)), healthHeight);

            // Draw Focus bar with current focus points
            DrawRectangle(Color.Goldenrod, edgePosition + barOffset, barWidth + 2, healthHeight + 2);
            DrawRectangle(Color.DarkGray, healthPosition + barOffset, barWidth, healthHeight);
            DrawRectangle(Color.MediumBlue, healthPosition + barOffset, (int)(barWidth * (Focus / 30f)), healthHeight);

            // Draw Stamina bar with current stamina points
            DrawRectangle(Color.Goldenrod, edgePosition + barOffset * 2, barWidth + 2, healthHeight + 2);
            DrawRectangle(Color.DarkGray, healthPosition + barOffset * 2, barWidth, healthHeight);
            DrawRectangle(Color.ForestGreen, healthPosition + barOffset * 2, (int)(barWidth * (Stamina / 30f)),
                healthHeight);
        }

        protected override void DrawPlayerUi()
        {
            base.DrawPlayerUi();
            _deckIndicator.Draw();
            _reserveIndicator.Draw();
            _spiritAshIndicator.Draw();
        }
    }
}
