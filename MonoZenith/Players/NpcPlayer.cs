using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Engine.Support;

namespace MonoZenith.Players
{
    internal enum AiState
    {
        LowHealth,
        LowFocus,
        Aggressive
    };
    
    internal class NpcPlayer : Player
    {
        private readonly SoundEffectInstance _retrieveCardsSound;
        private readonly float _originalHealth;
        private readonly float _originalFocus;
        private float _currentMoveDelay;
        private const float MoveDelay = 1.5f;
        
        /* TODO: Add SpiritAsh here later. Npc players will use ashes directly, instead of through an indicator. */
        
        public NpcPlayer(Game game, GameState state, string name) : base(game, state, name)
        {
            _handPosY = 25 * AppSettings.Scaling.ScaleFactor;
            PlayerPosition = new Vector2(game.ScreenWidth * 0.05f, game.ScreenHeight * 0.085f);
            PlayerIcon = DataManager.GetInstance(game).Npc;
            _originalHealth = Health;
            _originalFocus = Focus;
            _retrieveCardsSound = DataManager.GetInstance(game).RetrieveCardsSound.CreateInstance();
        }

        /// <summary>
        /// Determine the state of the NpcPlayer.
        /// </summary>
        /// <returns>The state of the NpcPlayer.</returns>
        private AiState DetermineState()
        {
            if (HealthPercentage() <= 70f)
                return AiState.LowHealth;
            
            if (FocusPercentage() <= 70f)
                return AiState.LowFocus;
            
            return AiState.Aggressive;
        }

        /// <summary>
        /// Calculates the percentage of the NpcPlayer's current health relative to their original health.
        /// </summary>
        /// <returns>A float between 0 and 100 indicating the percentage of health remaining.</returns>
        private float HealthPercentage() => Health / _originalHealth * 100;
        
        /// <summary>
        /// Calculates the percentage of the NpcPlayer's current focus relative to their original focus.
        /// </summary>
        /// <returns>A float between 0 and 100 indicating the percentage of focus remaining.</returns>
        private float FocusPercentage() => Focus / _originalFocus * 100;
        
        /// <summary>
        /// Determines and plays the optimal card based on the current AI state.
        /// </summary>
        private void PlayStrategicCard()
        {
            var currentState = DetermineState();
            
            // Buffer actions with strategies 
            var strategies = new List<Func<bool>>
            {
                () => currentState == AiState.LowHealth && HealthRecoveryAttemptSuccessful(),
                () => currentState == AiState.LowFocus && FocusRecoveryAttemptSuccessful(),
                OffensiveAttackAttemptSuccessful
            };
            
            // Execute the first successful strategy based on state and exit
            foreach (var strategy in strategies)
            {
                if (strategy())
                    return;
            }
        }

        /// <summary>
        /// Attempt to play a health recovery card. If a FlaskOfCrimsonTearsCard is available in the hand
        /// and can be afforded, play it. Return true if successful, false otherwise.
        /// </summary>
        /// <returns>true if a health recovery card was played, false otherwise</returns>
        private bool HealthRecoveryAttemptSuccessful()
        {
            var healthCard = _handStack.Cards.Find(card => card is FlaskOfCrimsonTearsCard);

            if (healthCard == null || !healthCard.IsAffordable()) 
                return false;

            PlayCard(healthCard);
            return true;
        }

        /// <summary>
        /// Attempt to play a focus recovery card. If a FlaskOfCeruleanTearsCard is available in the hand
        /// and can be afforded, play it. Return true if successful, false otherwise.
        /// </summary>
        /// <returns>true if a focus recovery card was played, false otherwise</returns>
        private bool FocusRecoveryAttemptSuccessful()
        {
            var focusCard = _handStack.Cards.Find(card => card is FlaskOfCeruleanTearsCard);

            if (focusCard == null || !focusCard.IsAffordable()) 
                return false;
            
            PlayCard(focusCard);
            return true;
        }

        /// <summary>
        /// Attempt to play the most powerful attack card available in the hand.
        /// This method orders the attack cards by their attack power, 
        /// and then attempts to play the strongest card that can be afforded.
        /// Returns true if a card was successfully played; otherwise, false.
        /// </summary>
        /// <returns>true if an attack card was played; otherwise, false</returns>
        private bool OffensiveAttackAttemptSuccessful()
        {
            // Filter for attack cards in hand and order them by descending attack power
            var attackCards = _handStack.Cards
                .OfType<AttackCard>()              
                .OrderByDescending(card => card.Damage) 
                .ToList();

            // Attempt to play the most powerful affordable attack card
            foreach (var attackCard in attackCards)
            {
                if (!attackCard.IsAffordable()) 
                    continue;
                
                PlayCard(attackCard);   
                return true;            
            }

            return false;   
        }

        protected override void FillPlayerDeck()
        {
            var deck = new List<Card.Card>
            {
                // Flasks
                new FlaskOfCrimsonTearsCard(_game, _state, this),
                new FlaskOfCrimsonTearsCard(_game, _state, this),
                new FlaskOfCrimsonTearsCard(_game, _state, this),
                new FlaskOfCrimsonTearsCard(_game, _state, this),

                new FlaskOfCeruleanTearsCard(_game, _state, this),
                new FlaskOfCeruleanTearsCard(_game, _state, this),
                new FlaskOfCeruleanTearsCard(_game, _state, this),
                new FlaskOfCeruleanTearsCard(_game, _state, this),

                // Basic attacks
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),
                new LightSwordAttackCard(_game, _state, this),

                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),
                new HeavySwordAttackCard(_game, _state, this),

                // Magic attacks
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this),
                new GlintStonePebbleCard(_game, _state, this)
            };
            
            _deckStack.AddToFront(deck);
            
            // Set the starting position of the cards when moving from the deck to the hand
            _deckStack.UpdatePosition(
                _game.ScreenWidth / 2f,
                -Card.Card.Height);
            _reserveCardStack.UpdatePosition(
                _game.ScreenWidth / 2f,
                -Card.Card.Height);
            
            foreach (var card in _handStack.Cards)
                card.Stack = _deckStack;
        }

        public override void PerformTurn(GameState state)
        {
            base.PerformTurn(state);
            
            if (CardsAvailableToPlay())
            {
                // If any card is moving, return
                if (_handStack.Cards.Any(card => card.IsMoving) || _currentMoveDelay < MoveDelay)
                {
                    _currentMoveDelay += (float)state.GameTime.ElapsedGameTime.TotalSeconds;
                    return;
                }

                _currentMoveDelay = 0;
                PlayStrategicCard();
                return;
            }
            
            // If any card is moving, return
            if (_handStack.Cards.Any(card => card.IsMoving) || _currentMoveDelay < MoveDelay)
            {
                _currentMoveDelay += (float)state.GameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            
            _currentMoveDelay = 0;
            
            MoveCardsFromHandToReserve();
            MoveCardsFromPlayedToReserve();
            ResetPlayerStamina();
            _retrieveCardsSound.Play();
            _state.SwitchingTurns = true;
        }
        
        /// <summary>
        /// Determines if there are any cards in the player's hand that can be played.
        /// Only considers cards that are necessary based on the current AI state.
        /// </summary>
        /// <returns>True if there are cards in the player's hand that can be played,
        /// false otherwise.</returns>
        private bool CardsAvailableToPlay()
        {
            var currentState = DetermineState();

            switch (currentState)
            {
                case AiState.LowHealth when _handStack.Cards.Any(card => card is FlaskOfCrimsonTearsCard 
                                                                         && card.IsAffordable()):
                
                case AiState.LowFocus when _handStack.Cards.Any(card => card is FlaskOfCeruleanTearsCard 
                                                                        && card.IsAffordable()):
                    return true;
                
                case AiState.Aggressive:
                
                default:
                    return _handStack.Cards.OfType<AttackCard>().Any(card => card.IsAffordable());
            }
        }

        public override void Update(GameTime deltaTime)
        {
            _handStack.Update(deltaTime);
            _reserveCardStack.Update(deltaTime);
            _deckStack.Update(deltaTime);
        }

        protected override void DrawPlayerHealthAndName()
        {
            // Setup offsets and positions for name and health bar
            Vector2 playerOffset = GetOffset(PlayerCurrent, Scale);
            Vector2 namePosition = PlayerPosition + new Vector2(playerOffset.X * 1.2f, -playerOffset.Y * 0.875f);
            Vector2 shadowPosition = new(1.25f, 1.25f);
            int healthHeight = (int)(PlayerCurrent.Height * Scale * 0.05f);
            int healthWidth = (int)(_game.ScreenWidth * 0.9f);
            Vector2 healthPosition = PlayerPosition + new Vector2(0, -playerOffset.Y) + new Vector2(1, 1);
            Vector2 edgePosition = healthPosition - new Vector2(1, 1);

            // Draw name
            _game.DrawText(Name, namePosition + shadowPosition, PlayerFont, Color.DarkGray);
            _game.DrawText(Name, namePosition, PlayerFont, Color.White);

            // Draw Health bar with current health points
            _game.DrawRectangle(Color.Goldenrod, edgePosition, healthWidth + 2, healthHeight + 2);
            _game.DrawRectangle(Color.DarkGray, healthPosition, healthWidth, healthHeight);
            _game.DrawRectangle(Color.DarkRed, healthPosition, (int)(healthWidth * (Health / 100f)), healthHeight);
        }
    }
}
