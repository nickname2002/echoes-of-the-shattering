using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZenith.Card
{
    internal class DrawableCardsStack : CardStack
    {
        public Texture2D CardBack;
        public DrawableCardsStack(Game game, GameState state) : base(game, state)
        {
            CardBack = DataManager.GetInstance(game).CardBack;
            // TODO: Ensure cards are able to show both back and front sides
            InitializeCards();
            Shuffle();
        }

        /// <summary>
        /// Initialize the cards in the stack.
        /// </summary>
        private void InitializeCards()
        {
            // Limgrave cards
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Wandering Noble", CardLabel.TWO, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Demi-Human", CardLabel.THREE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Godrick Soldier", CardLabel.FOUR, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Godrick Knight", CardLabel.FIVE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Warhawk", CardLabel.SIX, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Erdtree Avatar", CardLabel.SEVEN, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Tree Sentinel", CardLabel.EIGHT, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Margit the Fell Omen", CardLabel.NINE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Godrick the Grafted", CardLabel.A, Region.LIMGRAVE));

            // Liurnia cards
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Raya Lucaria Sorcerer", CardLabel.TWO, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Marionette Soldier", CardLabel.THREE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Raya Lucaria Scholar", CardLabel.FOUR, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Caria Knight", CardLabel.FIVE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Crystallian", CardLabel.SIX, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Red Wolf of Radagon", CardLabel.SEVEN, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Royal Revenant", CardLabel.EIGHT, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Bloodhound Knight", CardLabel.NINE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Rennala, Queen of the Full Moon", CardLabel.A, Region.LIURNIA));

            // Leyndell cards
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Golden Foot Soldier", CardLabel.TWO, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Crucible Knight", CardLabel.THREE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Oracle Envoy", CardLabel.FOUR, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Gargoyle", CardLabel.FIVE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Tree Sentinel", CardLabel.SIX, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Omenkiller", CardLabel.SEVEN, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Draconic Tree Sentinel", CardLabel.EIGHT, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Leyndell Knight", CardLabel.NINE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Morgott, the Omen King", CardLabel.A, Region.LEYNDELL));

            // Caelid cards
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Dogs of Caelid", CardLabel.TWO, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Kindred of Rot", CardLabel.THREE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Radahn Soldier", CardLabel.FOUR, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Radahn Beast", CardLabel.FIVE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Giant Crows", CardLabel.SIX, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Cleanrot Knight", CardLabel.SEVEN, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Putrid Tree Spirit", CardLabel.EIGHT, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Radahn’s Cavalry", CardLabel.NINE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, "Starscourge Radahn", CardLabel.A, Region.CAELID));
        
            // Joker cards
            AddToFront(new JokerCard(_game, _state, _position, CardBack, "Queen Marika the Eternal", Region.ALL, CardLabel.JOKER));
            AddToFront(new JokerCard(_game, _state, _position, CardBack, "Radagon of the Golden Order", Region.ALL, CardLabel.JOKER));

            // Grace cards
            AddToFront(new GraceCard(_game, _state, _position, CardBack, "Grace of Gold", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, CardBack, "Grace of Gold", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, CardBack, "Grace of Gold", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, CardBack, "Grace of Gold", Region.ALL, CardLabel.GRACE));

            // Power cards
            AddToFront(new LunarQueenRebirthCard(_game, _state, _position, CardBack, "Lunar Queen Rebirth", Region.LIURNIA, CardLabel.POWER));
            AddToFront(new ScarletBloomCard(_game, _state, _position, CardBack, "Scarlet Bloom", Region.CAELID, CardLabel.POWER));
            AddToFront(new GravityPullCard(_game, _state, _position, CardBack, "Gravity Pull", Region.CAELID, CardLabel.POWER));
            AddToFront(new ErdtreeBlessingCard(_game, _state, _position, CardBack, "Erdtree Blessing", Region.LEYNDELL, CardLabel.POWER));
            AddToFront(new MargitShacklesCard(_game, _state, _position, CardBack, "Margit Shackles", Region.LIMGRAVE, CardLabel.POWER));
            AddToFront(new RennalaFullMoonCard(_game, _state, _position, CardBack, "Rennala's Full Moon", Region.LIURNIA, CardLabel.POWER));
            AddToFront(new DeathRootDecayCard(_game, _state, _position, CardBack, "Godwyn's Deathroot Decay", Region.LEYNDELL, CardLabel.POWER));
            AddToFront(new WaterFlowDanceCard(_game, _state, _position, CardBack, "Malenia's Waterflow Dance", Region.CAELID, CardLabel.POWER));
            AddToFront(new MiquellaBlessingCard(_game, _state, _position, CardBack, "Miquella's Blessing", Region.LIMGRAVE, CardLabel.POWER));
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
