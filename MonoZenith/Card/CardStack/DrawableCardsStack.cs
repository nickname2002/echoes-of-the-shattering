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
        public Texture2D CardFront;

        public DrawableCardsStack(Game game, GameState state) : base(game, state)
        {
            // Load in Textures
            CardBack = DataManager.GetInstance(game).CardBack;
            CardFront = DataManager.GetInstance(game).CardFront;

            InitializeCards();
            Shuffle();
        }

        /// <summary>
        /// Initialize the cards in the stack.
        /// </summary>
        private void InitializeCards()
        {
            // Limgrave cards
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Wandering Noble", CardLabel.TWO, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Demi-Human", CardLabel.THREE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Godrick Soldier", CardLabel.FOUR, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Godrick Knight", CardLabel.FIVE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Warhawk", CardLabel.SIX, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Erdtree Avatar", CardLabel.SEVEN, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Tree Sentinel", CardLabel.EIGHT, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Margit the Fell Omen", CardLabel.NINE, Region.LIMGRAVE));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Godrick the Grafted", CardLabel.A, Region.LIMGRAVE));

            // Liurnia cards
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Raya Lucaria Sorcerer", CardLabel.TWO, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Marionette Soldier", CardLabel.THREE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Raya Lucaria Scholar", CardLabel.FOUR, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Caria Knight", CardLabel.FIVE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Crystallian", CardLabel.SIX, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Red Wolf of Radagon", CardLabel.SEVEN, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Royal Revenant", CardLabel.EIGHT, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Bloodhound Knight", CardLabel.NINE, Region.LIURNIA));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Rennala, Queen of the Full Moon", CardLabel.A, Region.LIURNIA));

            // Leyndell cards
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Golden Foot Soldier", CardLabel.TWO, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Crucible Knight", CardLabel.THREE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Oracle Envoy", CardLabel.FOUR, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Gargoyle", CardLabel.FIVE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Tree Sentinel", CardLabel.SIX, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Omenkiller", CardLabel.SEVEN, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Draconic Tree Sentinel", CardLabel.EIGHT, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Leyndell Knight", CardLabel.NINE, Region.LEYNDELL));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Morgott, the Omen King", CardLabel.A, Region.LEYNDELL));

            // Caelid cards
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Dogs of Caelid", CardLabel.TWO, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Kindred of Rot", CardLabel.THREE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Radahn Soldier", CardLabel.FOUR, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Radahn Beast", CardLabel.FIVE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Giant Crows", CardLabel.SIX, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Cleanrot Knight", CardLabel.SEVEN, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Putrid Tree Spirit", CardLabel.EIGHT, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Radahn’s Cavalry", CardLabel.NINE, Region.CAELID));
            AddToFront(new RegionCard(_game, _state, _position, CardBack, CardFront, "Starscourge Radahn", CardLabel.A, Region.CAELID));
        
            // Joker cards
            AddToFront(new JokerCard(_game, _state, _position, CardBack, CardFront, "Queen Marika the Eternal", Region.ALL, CardLabel.JOKER));
            AddToFront(new JokerCard(_game, _state, _position, CardBack, CardFront, "Radagon of the Golden Order", Region.ALL, CardLabel.JOKER));

            // Grace cards
            AddToFront(new GraceCard(_game, _state, _position, CardBack, CardFront, "Grace of Gold", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, CardBack, CardFront, "Grace of Gold", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, CardBack, CardFront, "Grace of Gold", Region.ALL, CardLabel.GRACE));
            AddToFront(new GraceCard(_game, _state, _position, CardBack, CardFront, "Grace of Gold", Region.ALL, CardLabel.GRACE));

            // Power cards
            AddToFront(new LunarQueenRebirthCard(_game, _state, _position, CardBack, CardFront, "Lunar Queen Rebirth", Region.LIURNIA, CardLabel.POWER));
            AddToFront(new ScarletBloomCard(_game, _state, _position, CardBack, CardFront, "Scarlet Bloom", Region.CAELID, CardLabel.POWER));
            AddToFront(new GravityPullCard(_game, _state, _position, CardBack, CardFront, "Gravity Pull", Region.CAELID, CardLabel.POWER));
            AddToFront(new ErdtreeBlessingCard(_game, _state, _position, CardBack, CardFront, "Erdtree Blessing", Region.LEYNDELL, CardLabel.POWER));
            AddToFront(new MargitShacklesCard(_game, _state, _position, CardBack, CardFront, "Margit Shackles", Region.LIMGRAVE, CardLabel.POWER));
            AddToFront(new RennalaFullMoonCard(_game, _state, _position, CardBack, CardFront, "Rennala's Full Moon", Region.LIURNIA, CardLabel.POWER));
            AddToFront(new DeathRootDecayCard(_game, _state, _position, CardBack, CardFront, "Godwyn's Deathroot Decay", Region.LEYNDELL, CardLabel.POWER));
            AddToFront(new WaterFlowDanceCard(_game, _state, _position, CardBack, CardFront, "Malenia's Waterflow Dance", Region.CAELID, CardLabel.POWER));
            AddToFront(new MiquellaBlessingCard(_game, _state, _position, CardBack, CardFront, "Miquella's Blessing", Region.LIMGRAVE, CardLabel.POWER));

            // DEBUG REMOVE LATER
            RegionCard test = new RegionCard(_game, _state, _position, CardBack, CardFront, "Dogs of Caelid", CardLabel.TWO, Region.CAELID);
            Console.WriteLine(test.ValidNextCard(new RegionCard(_game, _state, _position, CardBack, CardFront, "Kindred of Rot", CardLabel.TWO, Region.LIURNIA)));
            Console.WriteLine(test.ValidNextCard(new RegionCard(_game, _state, _position, CardBack, CardFront, "Rennala, Queen of the Full Moon", CardLabel.A, Region.LIURNIA)));
            Console.WriteLine(test.ValidNextCard(new GraceCard(_game, _state, _position, CardBack, CardFront, "Grace of Gold", Region.LIURNIA, CardLabel.GRACE)));
            Console.WriteLine(test.ValidNextCard(new JokerCard(_game, _state, _position, CardBack, CardFront, "Queen Marika the Eternal", Region.CAELID, CardLabel.JOKER)));
            Console.WriteLine(test.ValidNextCard(new GravityPullCard(_game, _state, _position, CardBack, CardFront, "Gravity Pull", Region.LIURNIA, CardLabel.POWER)));
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
