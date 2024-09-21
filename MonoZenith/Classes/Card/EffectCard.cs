using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Card
{
    internal abstract class EffectCard : RegionCard
    {
        public abstract void PerformEffect(Game g);
    }

    internal class Joker : EffectCard
    {
        public override void ValidNextCard(Card previousCard)
        {
            throw new NotImplementedException();
        }
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class GraceCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class LunarQueenRebirthCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class ScarletBloomCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class GravityPullCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class ErdtreeBlessingCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class MargitShacklesCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class RennalaFullMoonCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class DeathRootDecayMoonCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class WaterFlowDanceCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }

    internal class MiquellaBlessingCard : EffectCard
    {
        public override void PerformEffect(Game g)
        {
            throw new NotImplementedException();
        }
    }
}
