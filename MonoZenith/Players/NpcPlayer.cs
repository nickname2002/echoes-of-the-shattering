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
            _handyPos = game.ScreenHeight / 3.9f;
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
            Vector2 textPosition = PlayerPosition + new Vector2(offset.X * 1.2f, -offset.Y * 0.875f);
            Vector2 shadowPosition = new(1.25f, 1.25f);
            Vector2 healthOffset = new(1, 1);
            int healthHeight = (int)(PlayerCurrent.Height * Scale * 0.05f);
            int healthWidth = (int)(_game.ScreenWidth * 0.9f);
            Vector2 healthPosition = PlayerPosition + new Vector2(0, -offset.Y) + healthOffset;
            int currentHealth = Math.Min(GetOpponentHandCount(), 7);

            // Draw text and health bar
            _game.DrawText(Name, textPosition + shadowPosition, PlayerFont, Color.DarkGray);
            _game.DrawText(Name, textPosition, PlayerFont, Color.White);

            _game.DrawRectangle(Color.Goldenrod, healthPosition - healthOffset, healthWidth + 2, healthHeight + 2);
            _game.DrawRectangle(Color.DarkGray, healthPosition, healthWidth, healthHeight);

            // Draw current health based on opponent's hand count
            _game.DrawRectangle(Color.DarkRed, healthPosition, (int)(healthWidth / 7f * currentHealth), healthHeight);
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

        protected override bool TryPlayCard()
        {
            throw new NotImplementedException();
        }

        protected override void TryDrawCard()
        {
            throw new NotImplementedException();
        }
        
        public override void PerformTurn(GameState state)
        {
            throw new NotImplementedException();
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
