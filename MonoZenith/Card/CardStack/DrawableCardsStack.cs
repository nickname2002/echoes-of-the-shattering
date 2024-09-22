using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Card
{
    internal class DrawableCardsStack : CardStack
    {
        public DrawableCardsStack(Game game, GameState state) : base(game, state)
        {
            InitializeCards();
            Shuffle();
        }

        /// <summary>
        /// Initialize the cards in the stack.
        /// </summary>
        private void InitializeCards()
        {
            // Limgrave cards
            AddToFront(new RegionCard(_game, _state, _position, null, "Wandering Noble", CardLabel.TWO, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, null, "Demi-Human", CardLabel.THREE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, null, "Godrick Soldier", CardLabel.FOUR, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, null, "Godrick Knight", CardLabel.FIVE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, null, "Warhawk", CardLabel.SIX, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, null, "Erdtree Avatar", CardLabel.SEVEN, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, null, "Tree Sentinel", CardLabel.EIGHT, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, null, "Margit the Fell Omen", CardLabel.NINE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, null, "Godrick the Grafted", CardLabel.A, Region.LIMGRAVE));

            // Liurnia cards
            AddToFront(new RegionCard(_game, _state, _position, null, "Raya Lucaria Sorcerer", CardLabel.TWO, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, null, "Marionette Soldier", CardLabel.THREE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, null, "Raya Lucaria Scholar", CardLabel.FOUR, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, null, "Caria Knight", CardLabel.FIVE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, null, "Crystallian", CardLabel.SIX, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, null, "Red Wolf of Radagon", CardLabel.SEVEN, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, null, "Royal Revenant", CardLabel.EIGHT, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, null, "Bloodhound Knight", CardLabel.NINE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, null, "Rennala, Queen of the Full Moon", CardLabel.A, Region.LIURNIA));

            // Leyndell cards
            AddToFront(new RegionCard(_game, _state, _position, null, "Golden Foot Soldier", CardLabel.TWO, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, null, "Crucible Knight", CardLabel.THREE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, null, "Oracle Envoy", CardLabel.FOUR, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, null, "Gargoyle", CardLabel.FIVE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, null, "Tree Sentinel", CardLabel.SIX, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, null, "Omenkiller", CardLabel.SEVEN, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, null, "Draconic Tree Sentinel", CardLabel.EIGHT, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, null, "Leyndell Knight", CardLabel.NINE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, null, "Morgott, the Omen King", CardLabel.A, Region.LEYNDELL));

            // Caelid cards
            AddToFront(new RegionCard(_game, _state, _position, null, "Dogs of Caelid", CardLabel.TWO, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, null, "Kindred of Rot", CardLabel.THREE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, null, "Radahn Soldier", CardLabel.FOUR, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, null, "Radahn Beast", CardLabel.FIVE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, null, "Giant Crows", CardLabel.SIX, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, null, "Cleanrot Knight", CardLabel.SEVEN, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, null, "Putrid Tree Spirit", CardLabel.EIGHT, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, null, "Radahn’s Cavalry", CardLabel.NINE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, null, "Starscourge Radahn", CardLabel.A, Region.CAELID));
        
            // Joker cards
            AddToFront(new JokerCard(_game, _state, _position, null, "Queen Marika the Eternal", Region.ALL, CardLabel.JOKER));
            AddToFront(new JokerCard(_game, _state, _position, null, "Radagon of the Golden Order", Region.ALL, CardLabel.JOKER));

            // Grace cards
            AddToFront(new GraceCard(_game, _state, _position, null, "Grace of Gold", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, null, "Grace of Silver", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, null, "Grace of Steel", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, null, "Grace of Light", Region.ALL, CardLabel.GRACE));

            // Power cards
            AddToFront(new LunarQueenRebirthCard(_game, _state, _position, null, "Lunar Queen Rebirth", Region.LIURNIA, CardLabel.POWER));
            AddToFront(new ScarletBloomCard(_game, _state, _position, null, "Scarlet Bloom", Region.CAELID, CardLabel.POWER));
            AddToFront(new GravityPullCard(_game, _state, _position, null, "Gravity Pull", Region.CAELID, CardLabel.POWER));
            AddToFront(new ErdtreeBlessingCard(_game, _state, _position, null, "Erdtree Blessing", Region.LEYNDELL, CardLabel.POWER));
            AddToFront(new MargitShacklesCard(_game, _state, _position, null, "Margit Shackles", Region.LIMGRAVE, CardLabel.POWER));
            AddToFront(new RennalaFullMoonCard(_game, _state, _position, null, "Rennala's Full Moon", Region.LIURNIA, CardLabel.POWER));
            AddToFront(new DeathRootDecayCard(_game, _state, _position, null, "Godwyn's Deathroot Decay", Region.LEYNDELL, CardLabel.POWER));
            AddToFront(new WaterFlowDanceCard(_game, _state, _position, null, "Malenia's Waterflow Dance", Region.CAELID, CardLabel.POWER));
            AddToFront(new MiquellaBlessingCard(_game, _state, _position, null, "Miquella's Blessing", Region.LIMGRAVE, CardLabel.POWER));
        }

        /// <summary>
        /// Return seven cards from the stack.
        /// </summary>
        /// <returns>The seven cards.</returns>
        public CardStack GetSevenCards()
        {
            CardStack sevenCards = new CardStack(_game, _state);

            for (int i = 0; i < 7; i++)
            {
                sevenCards.AddToBottom(Pop());
            }
            
            return sevenCards;
        }
    }
}
