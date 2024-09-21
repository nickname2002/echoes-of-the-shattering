using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Classes.Card
{
    internal class RegionCard : Card
    {
        public readonly Region region;
        public readonly string label; //(2,3,4...A)
        //Suggestion: Enum for the label?
        public override void ValidNextCard(Card previousCard)
        {
            throw new NotImplementedException();
        }
    }
}
