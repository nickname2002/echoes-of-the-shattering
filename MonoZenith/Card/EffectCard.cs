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
            for (int i = 0; i < 5; i++)
                state.OpposingPlayer.DrawCard();

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
    }
}