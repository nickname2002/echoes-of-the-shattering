#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Components.Indicator;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using static MonoZenith.Game;

namespace MonoZenith.Players
{
    public enum AiState
    {
        LowHealth,
        LowFocus,
        Aggressive
    };

    public sealed class NpcPlayer : Player
    {
        private readonly SoundEffectInstance _retrieveCardsSound;
        private SpiritAshIndicator? _spiritAshIndicator;
        private readonly float _originalHealth;
        private readonly float _originalFocus;
        private float _currentMoveDelay;
        private const float MoveDelay = 1.5f;

        public List<Card.Card> CardsInDeck { get; set; } = new();

        public NpcPlayer(GameState state, string name, Texture2D playerIcon) : base(state, name)
        {
            _handPosY = 25 * AppSettings.Scaling.ScaleFactor;
            _playerPosition = new Vector2(ScreenWidth * 0.05f, ScreenHeight * 0.085f);
            _playerIcon = playerIcon;
            _originalHealth = Health;
            _originalFocus = Focus;
            _retrieveCardsSound = DataManager.GetInstance().RetrieveCardsSound.CreateInstance();
        }
        
        public override void InitializeState(GameState state)
        {
            base.InitializeState(state);
            OpposingPlayer = state.Player;
        }
        
        /// <summary>
        /// Set the spirit ash for the NpcPlayer.
        /// </summary>
        /// <param name="ashType">The type of the spirit ash to set.</param>
        public void SetSpiritAsh(Type ashType)
        {
            // ReSharper disable once NullableWarningSuppressionIsUsed
            var spiritAsh = (SpiritAsh)Activator.CreateInstance(ashType)!;
            SpiritAsh = spiritAsh;
            var disabledIndicatorTexture = DataManager.GetInstance().AshIndicatorDisabled;

            var position = new Vector2(
                ScreenWidth - 100 * AppSettings.Scaling.ScaleFactor, 
                25 * AppSettings.Scaling.ScaleFactor
            );
            
            _spiritAshIndicator = new SpiritAshIndicator(
                GetGameState(),
                position,
                disabledIndicatorTexture,
                spiritAsh,
                false
            );
        }
        
        /// <summary>
        /// Determine the state of the NpcPlayer.
        /// </summary>
        /// <returns>The state of the NpcPlayer.</returns>
        private AiState DetermineState()
        {
            if (HealthPercentage() < 90)
                return AiState.LowHealth;
            
            if (FocusPercentage() <= 60f)
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
            if (SpiritAsh != null && SpiritAsh.ShouldAIPlay(currentState)
                && _spiritAshIndicator is { IsActive: true })
            {
                _spiritAshIndicator.InvokeClickEvent(_state.GameTime);
                _currentMoveDelay = 0;
                return;
            }

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
            _deckStack.Clear();
            _handStack.Clear();
            
            // Add cards to the deck
            _deckStack.AddToFront(CardsInDeck);
            
           _deckStack.UpdatePosition(
                ScreenWidth / 2f,
                -Card.Card.Height);
            _reserveCardStack.UpdatePosition(
                ScreenWidth / 2f,
                -Card.Card.Height);
            
            foreach (var card in _handStack.Cards)
                card.Stack = _deckStack;
        }
        
        /// <summary>
        /// Check if the player is currently pausing.
        /// </summary>
        /// <returns>True if the player is pausing; otherwise, false.</returns>
        private bool IsPausing()
        {
            if (!(_currentMoveDelay < MoveDelay)) return false;
            _currentMoveDelay += (float)_state.GameTime.ElapsedGameTime.TotalSeconds;
            return true;
        }
        
        public override void PerformTurn(GameState state)
        {
            base.PerformTurn(state);
            _spiritAshIndicator?.Update(state.GameTime);
            
            if (IsPausing())
                return;
            
            if (CardsAvailableToPlay())
            {
                // If any card is moving, return
                if (_handStack.Cards.Any(card => card.IsMoving) || IsPausing())
                {
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
            _state.TurnManager.SwitchingTurns = true;
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

        public override void DrawPlayerHealthAndName()
        {
            // Setup offsets and positions for name and health bar
            Vector2 playerOffset = GetOffset(_playerCurrent, _scale);
            Vector2 namePosition = _playerPosition + new Vector2(playerOffset.X * 1.2f, -playerOffset.Y * 0.875f);
            Vector2 shadowPosition = new(1.25f, 1.25f);
            int healthHeight = (int)(_playerCurrent.Height * _scale * 0.05f);
            int healthWidth = (int)(ScreenWidth * 0.9f);
            Vector2 healthPosition = _playerPosition + new Vector2(0, -playerOffset.Y) + new Vector2(1, 1);
            Vector2 edgePosition = healthPosition - new Vector2(1, 1);

            // Draw name
            DrawText(Name, namePosition + shadowPosition, _playerFont, Color.DarkGray);
            DrawText(Name, namePosition, _playerFont, Color.White);

            // Draw Health bar with current health points
            DrawRectangle(Color.Goldenrod, edgePosition, healthWidth + 2, healthHeight + 2);
            DrawRectangle(Color.DarkGray, healthPosition, healthWidth, healthHeight);
            DrawRectangle(Color.DarkRed, healthPosition, (int)(healthWidth * (Health / 100f)), healthHeight);
            
            _spiritAshIndicator?.Draw();            
        }
    }
}
