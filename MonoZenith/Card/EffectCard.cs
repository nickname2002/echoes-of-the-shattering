using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Players;
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
            // they can play another power card.
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

        /// <summary>
        /// Check which region occurs most frequently in the player's hand.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>Most frequently occuring region in the player's hand.</returns>
        private Region MostFrequentRegion(Player player)
        {
            var regionCounts = new int[Enum.GetNames(typeof(Region)).Length];
            
            foreach (var card in player.Hand.Cards)
            {
                if (card is RegionCard regionCard)
                {
                    regionCounts[(int)regionCard.Region]++;
                }
            }

            return (Region)Array.IndexOf(regionCounts, regionCounts.Max());
        }

        /// <summary>
        /// Return a random region from the available regions.
        /// </summary>
        /// <returns>A random region from the available regions.</returns>
        private Region ChooseRandomRegion()
        {
            Random rand = new Random();
            return (Region)rand.Next(Enum.GetNames(typeof(Region)).Length);
        }
        
        public override void PerformEffect(GameState state)
        {
            Player player = state.CurrentPlayer;
            
            // If the player is an NPC, choose a region that occurs most frequently in their hand.
            if (player is NpcPlayer npc)
            {
                Region mostOccuringRegion = MostFrequentRegion(npc);

                state.CurrentRegion = mostOccuringRegion == Region.ALL ? 
                    ChooseRandomRegion() : 
                    mostOccuringRegion;
                
                Console.WriteLine($"NPC chose region: {mostOccuringRegion}");
                state.SwitchTurn();
                return;
            }
            
            // If the player is a human player, choose a random region.
            state.SwitchTurn();
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