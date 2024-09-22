using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Card
{
    internal abstract class EffectCard : RegionCard
    {
        public EffectCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            CardLabel label,
            Region region)
            : base(game, state, position, texture, name, label, region)
        {
        }

        /// <summary>
        /// Perform the effect of the card.
        /// </summary>
        /// <param name="state">The current game state.</param>
        public abstract void PerformEffect(GameState state);
    }

    internal class JokerCard : EffectCard
    {
        public JokerCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override bool ValidNextCard(Card previousCard)
        {
            return true;
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class GraceCard : EffectCard
    {
        public GraceCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class LunarQueenRebirthCard : EffectCard
    {
        public LunarQueenRebirthCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class ScarletBloomCard : EffectCard
    {
        public ScarletBloomCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class GravityPullCard : EffectCard
    {
        public GravityPullCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class ErdtreeBlessingCard : EffectCard
    {
        public ErdtreeBlessingCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class MargitShacklesCard : EffectCard
    {
        public MargitShacklesCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class RennalaFullMoonCard : EffectCard
    {
        public RennalaFullMoonCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class DeathRootDecayCard : EffectCard
    {
        public DeathRootDecayCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class WaterFlowDanceCard : EffectCard
    {
        public WaterFlowDanceCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }

    internal class MiquellaBlessingCard : EffectCard
    {
        public MiquellaBlessingCard(
            Game game,
            GameState state,
            Vector2 position,
            Texture2D texture,
            string name,
            Region region,
            CardLabel label)
            : base(game, state, position, texture, name, label, region)
        {
        }

        public override void PerformEffect(GameState state)
        {
            throw new NotImplementedException();
        }
    }
}