using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Support;

namespace MonoZenith.Card
{
    internal class JokerCard : RegionCard
    {
        public JokerCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            return true;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to draw five cards unless
            // they can play another joker card.
            state.Combo += 5;
            state.SwitchTurn();
        }
    }

    internal class GraceCard : RegionCard
    {
        public GraceCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }
    }

    internal class LunarQueenRebirthCard : RegionCard
    {
        public LunarQueenRebirthCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region.
            return (prevCard == null
                    || prevCard.Label == Label
                    || Region == _state.CurrentRegion
                    || prevCard.Region == Region.ALL) &&
                   _state.Combo < 1;
        }


        public override void PerformEffect(GameState state)
        {
            // Switches hands between the players.
            (state.CurrentPlayer.Hand, state.OpposingPlayer.Hand) = 
                (state.OpposingPlayer.Hand, state.CurrentPlayer.Hand);
            state.SwitchTurn();
        }
    }

    internal class ScarletBloomCard : RegionCard
    {
        public ScarletBloomCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region
            return (prevCard == null
                    || prevCard.Label == Label
                    || Region == _state.CurrentRegion
                    || prevCard.Region == Region.ALL) &&
                   _state.Combo < 1;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to skip two turns.
            state.Skip += 2;
            state.SwitchTurn();
        }
    }

    internal class GravityPullCard : RegionCard
    {
        public GravityPullCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region.
            return (prevCard == null
                    || prevCard.Label == Label
                    || Region == _state.CurrentRegion
                    || prevCard.Region == Region.ALL) &&
                   _state.Combo < 1;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to skip two turns.
            state.Skip += 2;
            state.SwitchTurn();
        }
    }

    internal class ErdtreeBlessingCard : RegionCard
    {
        public ErdtreeBlessingCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region.
            return (prevCard == null
                    || prevCard.Label == Label
                    || Region == _state.CurrentRegion
                    || prevCard.Region == Region.ALL) &&
                   _state.Combo < 1;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to skip one turn.
            state.Skip++;
            state.SwitchTurn();
        }
    }

    internal class MargitShacklesCard : RegionCard
    {
        public MargitShacklesCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region.
            return (prevCard == null
                    || prevCard.Label == Label
                    || Region == _state.CurrentRegion
                    || prevCard.Region == Region.ALL) &&
                   _state.Combo < 1;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to skip one turn.
            state.Skip++;
            state.SwitchTurn();
        }
    }

    internal class RennalaFullMoonCard : RegionCard
    {
        public RennalaFullMoonCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region.
            return prevCard == null
                   || prevCard.Label == Label
                   || Region == _state.CurrentRegion
                   || prevCard.Region == Region.ALL
                   || prevCard is JokerCard;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to draw two cards unless
            // they can play another power card.
            state.Combo += 2;
            state.SwitchTurn();
        }
    }

    internal class DeathRootDecayCard : RegionCard
    {
        public DeathRootDecayCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region.
            return prevCard == null
                   || prevCard.Label == Label
                   || Region == _state.CurrentRegion
                   || prevCard.Region == Region.ALL
                   || prevCard is JokerCard;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to draw two cards unless
            // they can play another power card.
            state.Combo += 2;
            state.SwitchTurn();
        }
    }

    internal class WaterFlowDanceCard : RegionCard
    {
        public WaterFlowDanceCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region.
            return prevCard == null
                   || prevCard.Label == Label
                   || Region == _state.CurrentRegion
                   || prevCard.Region == Region.ALL
                   || prevCard is JokerCard;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to draw two cards unless
            // they can play another power card.
            state.Combo += 2;
            state.SwitchTurn();
        }
    }

    internal class MiquellaBlessingCard : RegionCard
    {
        public MiquellaBlessingCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            Texture2D activeTexture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, activeTexture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            var prevCard = previousCard as RegionCard;

            // Power cards matches on the same label or region.
            return prevCard == null
                   || prevCard.Label == Label
                   || Region == _state.CurrentRegion
                   || prevCard.Region == Region.ALL
                   || prevCard is JokerCard;
        }

        public override void PerformEffect(GameState state)
        {
            // Opposing player needs to draw two cards unless
            // they can play another power card.
            state.Combo += 2;
            state.SwitchTurn();
        }
    }
}