using System;
using System.Collections.Generic;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using MonoZenith.Players;
using MonoZenith.Screen.RewardPanel;

namespace MonoZenith.Support.Managers;

public class LevelManager
{
    private Dictionary<string, NpcPlayer> _enemies;
    private Dictionary<string, Reward> _rewards;
    private Dictionary<string, List<(Type, int)>> _decks;
    private string[] _enemyNames =
    {
        "White Mask Varré",
        "Tree Sentinel",
        "Renna",
        "Roderika",
        "Margit, The Fell Omen",
        "Fia, The Deathbed Companion",
        "Blaidd, The Half-Wolf",
        "Sorceress Sellen",
        "Gatekeeper Gostoc",
        "Godrick the Grafted",
        "Godrick the Grafted (2nd phase)",
        "Thops",
        "Red Wolf of Radagon",
        "Rennala, Queen of the Full Moon",
        "Rennala, Queen of the Full Moon (2nd phase)",
        "Royal Knight Loretta",
        "Mimic Tear",
        "Starscourge Radahn",
        "Bloody Finger Hunter Yura",
        "Morgott, The Omen King",
        "Dung Eater",
        "Mohg, Lord of Blood",
        "Commander Niall",
        "Malenia, Blade of Miquella",
        "Malenia, Goddess of Rot",
        "Sir Gideon Ofnir, The All-Knowing",
        "Godfrey, The First Elden Lord",
        "Hoarah Loux, Warrior",
        "Radagon of the Golden Order",
        "Elden Beast",
        "Tarnished, Consort of the Stars",
        "Ranni, Queen of the Dark Moon"
    };
    
    public List<Level> Levels { get; set; }
    public Level CurrentLevel { get; set; }
    
    public LevelManager()
    {
        _enemies = new Dictionary<string, NpcPlayer>();
        _decks = new Dictionary<string, List<(Type, int)>>();
        
        ConfigureEnemyObjects();
        ConfigureSpiritAshes();
        ConfigureRewards();
        ConfigureDecks();
        ConfigureLevels();
        
        CurrentLevel = Levels[31];
    }
    
    private List<Card.Card> GenerateDeck(GameState state, NpcPlayer enemy, List<(Type, int)> cards)
    {
        var deck = new List<Card.Card>();
        
        foreach (var (cardType, count) in cards)
        {
            for (var i = 0; i < count; i++)
            {
                var card = (Card.Card) Activator.CreateInstance(cardType, state, enemy);
                deck.Add(card);
            }
        }
        
        return deck;
    }

    private void ConfigureEnemyObjects()
    {
        foreach (var name in _enemyNames)
            _enemies[name] = new NpcPlayer(Game.GetGameState(), name);
    }

    private void ConfigureSpiritAshes()
    {
        _enemies["Renna"].SetSpiritAsh(typeof(WolvesAsh));
        _enemies["Roderika"].SetSpiritAsh(typeof(JellyfishAsh));
        _enemies["Blaidd, The Half-Wolf"].SetSpiritAsh(typeof(WolvesAsh));
        _enemies["Sorceress Sellen"].SetSpiritAsh(typeof(MimicTearAsh));
        _enemies["Gatekeeper Gostoc"].SetSpiritAsh(typeof(JellyfishAsh));
        _enemies["Red Wolf of Radagon"].SetSpiritAsh(typeof(WolvesAsh));
        _enemies["Rennala, Queen of the Full Moon"].SetSpiritAsh(typeof(MimicTearAsh));
        // TODO: _enemies["Dung Eater"].SetSpiritAsh(typeof(DungEaterPuppet));
        _enemies["Commander Niall"].SetSpiritAsh(typeof(WolvesAsh));
        _enemies["Malenia, Goddess of Rot"].SetSpiritAsh(typeof(MimicTearAsh));
        _enemies["Sir Gideon Ofnir, The All-Knowing"].SetSpiritAsh(typeof(JellyfishAsh));
        _enemies["Tarnished, Consort of the Stars"].SetSpiritAsh(typeof(MimicTearAsh));
        _enemies["Ranni, Queen of the Dark Moon"].SetSpiritAsh(typeof(MimicTearAsh));
    }

    private void ConfigureRewards()
    {
        _rewards = new Dictionary<string, Reward>
        {
            ["White Mask Varré"] = new(DataManager.GetInstance().CardQuickstep, "Quickstep", typeof(QuickstepCard)),
            ["Tree Sentinel"] = new(DataManager.GetInstance().CardWarmingStone, "Warming Stone", typeof(WarmingStoneCard)),
            ["Renna"] = new(DataManager.GetInstance().WolvesAsh, "Wolves", typeof(WolvesAsh)),
            ["Roderika"] = new(DataManager.GetInstance().JellyfishAsh, "Jelly Fish", typeof(JellyfishAsh)),
            ["Margit, The Fell Omen"] = new(DataManager.GetInstance().CardStormcaller, "Stormcaller", typeof(StormcallerCard)),
            ["Fia, The Deathbed Companion"] = new(DataManager.GetInstance().CardBaldachinBless, "Baldachin's Blessing", typeof(BaldachinBlessingCard)),
            ["Blaidd, The Half-Wolf"] = new(DataManager.GetInstance().CardBloodhound, "Bloodhound's Step", typeof(BloodhoundStepCard)),
            ["Sorceress Sellen"] = new(DataManager.GetInstance().CardGreatShard, "Great Glintstone Shard", typeof(GreatGlintStoneCard)),
            ["Gatekeeper Gostoc"] = new(DataManager.GetInstance().CardPoisonPot, "Poison Pot", typeof(PoisonPotCard)),
            ["Godrick the Grafted"] = null,
            ["Godrick the Grafted (2nd phase)"] = new(DataManager.GetInstance().CardCommandKneel, "I Command Thee, Kneel!", typeof(ICommandTheeKneelCard)),
            ["Thops"] = new(DataManager.GetInstance().CardWarmingStone, "Warming Stone", typeof(WarmingStoneCard)),
            ["Red Wolf of Radagon"] = new(DataManager.GetInstance().CardGlintPhalanx, "Glintblade Phalanx", typeof(GlintbladePhalanxCard)),
            ["Rennala, Queen of the Full Moon"] = null,
            ["Rennala, Queen of the Full Moon (2nd phase)"] = new(DataManager.GetInstance().CardCometAzur, "Comet Azur", typeof(CometAzurCard)),
            ["Royal Knight Loretta"] = new(DataManager.GetInstance().CardCarianGSword, "Carian Greatsword", typeof(CarianGreatSwordCard)),
            ["Mimic Tear"] = new(DataManager.GetInstance().MimicTearAsh, "Mimic Tear", typeof(MimicTearAsh)),
            ["Starscourge Radahn"] = null,  // TODO: Add reward
            ["Bloody Finger Hunter Yura"] = new(DataManager.GetInstance().CardUnsheathe, "Unsheathe", typeof(UnsheatheCard)),
            ["Morgott, The Omen King"] = new(DataManager.GetInstance().CardWarmingStone, "Warming Stone", typeof(WarmingStoneCard)),
            ["Dung Eater"] = null,  // TODO: Add reward
            ["Mohg, Lord of Blood"] = null,     // TODO: Add reward
            ["Commander Niall"] = new(DataManager.GetInstance().CardRallyingStandard, "Rallying Standard", typeof(RallyingStandardCard)),
            ["Malenia, Blade of Miquella"] = null,
            ["Malenia, Goddess of Rot"] = new(DataManager.GetInstance().CardWaterfowlDance, "Waterfowl Dance", typeof(WaterfowlDanceCard)),
            ["Sir Gideon Ofnir, The All-Knowing"] = new(DataManager.GetInstance().CardLarvalTear, "Larval Tear", typeof(LarvalTearCard)),
            ["Godfrey, The First Elden Lord"] = null,
            ["Hoarah Loux, Warrior"] = null,    // TODO: Add reward
            ["Radagon of the Golden Order"] = null,
            ["Elden Beast"] = null,     // TODO: Add reward
            ["Tarnished, Consort of the Stars"] = null,
            ["Ranni, Queen of the Dark Moon"] = null    // TODO: Add reward
        };
    }

    private void ConfigureDecks()
    {
        _decks = new Dictionary<string, List<(Type, int)>>
        {
            ["White Mask Varré"] = new()
            {
                (typeof(LightSwordAttackCard), 8),
                (typeof(HeavySwordAttackCard), 6),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(ThrowingDaggerCard), 6),
                (typeof(EndureCard), 4),
                (typeof(QuickstepCard), 4)
            },
            ["Tree Sentinel"] = new()
            {
                (typeof(LightSwordAttackCard), 6),
                (typeof(HeavySwordAttackCard), 10),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(WarmingStoneCard), 4),
                (typeof(EndureCard), 4),
                (typeof(QuickstepCard), 4)
            },
            ["Renna"] = new()
            {
                (typeof(GreatGlintStoneCard), 5),
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(CarianGreatSwordCard), 5),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(LightSwordAttackCard), 5),
                (typeof(GlintStonePebbleCard), 6)
            },
            ["Roderika"] = new()
            {
                (typeof(LightSwordAttackCard), 5),
                (typeof(HeavySwordAttackCard), 3),
                (typeof(FlaskOfCrimsonTearsCard), 3),
                (typeof(ThrowingDaggerCard), 10),
                (typeof(PoisonPotCard), 5),
                (typeof(WarmingStoneCard), 4)
            },
            ["Margit, The Fell Omen"] = new()
            {
                (typeof(LightSwordAttackCard), 3),
                (typeof(HeavySwordAttackCard), 7),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(StormcallerCard), 5),
                (typeof(EndureCard), 5),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(WarCryCard), 4)
            },
            ["Fia, The Deathbed Companion"] = new()
            {
                (typeof(BaldachinBlessingCard), 3),
                (typeof(LightSwordAttackCard), 6),
                (typeof(FlaskOfCrimsonTearsCard), 3),
                (typeof(EndureCard), 4),
                (typeof(PoisonPotCard), 3),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(HeavySwordAttackCard), 3),
                (typeof(WarmingStoneCard), 4)
            },
            ["Blaidd, The Half-Wolf"] = new()
            {
                (typeof(BloodhoundStepCard), 4),
                (typeof(DoubleSlashCard), 6),
                (typeof(HeavySwordAttackCard), 8),
                (typeof(WarCryCard), 4),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(EndureCard), 3),
            },
            ["Sorceress Sellen"] = new()
            {
                (typeof(GreatGlintStoneCard), 5),
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(CarianGreatSwordCard), 5),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(EndureCard), 3),
                (typeof(GlintStonePebbleCard), 5),
                (typeof(ThrowingDaggerCard), 4),
            },
            ["Gatekeeper Gostoc"] = new()
            {
                (typeof(PoisonPotCard), 10),
                (typeof(LightSwordAttackCard), 8),
                (typeof(FlaskOfCrimsonTearsCard), 3),
                (typeof(ThrowingDaggerCard), 9),
            },
            ["Godrick the Grafted"] = new()
            {
                (typeof(ICommandTheeKneelCard), 2),
                (typeof(HeavySwordAttackCard), 10),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(RallyingStandardCard), 3),
                (typeof(EndureCard), 5),
                (typeof(WarCryCard), 5),
                (typeof(ThrowingDaggerCard), 5)
            },
            ["Godrick the Grafted (2nd phase)"] = new()
            {
                (typeof(ICommandTheeKneelCard), 2),
                (typeof(HeavySwordAttackCard), 10),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(RallyingStandardCard), 3),
                (typeof(EndureCard), 5),
                (typeof(WarCryCard), 5),
                (typeof(ThrowingDaggerCard), 5)
            },
            ["Thops"] = new()
            {
                // (typeof(BarrierCard), 3),    // TODO: Add card
                (typeof(LightSwordAttackCard), 3),
                
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(GreatGlintStoneCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 3),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(PoisonPotCard), 5),
                (typeof(ThrowingDaggerCard), 7)
            },
            ["Red Wolf of Radagon"] = new()
            {
                (typeof(GlintbladePhalanxCard), 6),
                (typeof(GreatGlintStoneCard), 6),
                (typeof(LightSwordAttackCard), 5),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(EndureCard), 3),
                (typeof(WarCryCard), 3)
            },
            ["Rennala, Queen of the Full Moon"] = new()
            {
                (typeof(GreatGlintStoneCard), 7),
                (typeof(GlintbladePhalanxCard), 7),
                (typeof(LightSwordAttackCard), 3),
                (typeof(FlaskOfCeruleanTearsCard), 3),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(CarianGreatSwordCard), 3),
                (typeof(EndureCard), 3)
            },
            ["Rennala, Queen of the Full Moon (2nd phase)"] = new()
            {
                (typeof(CometAzurCard), 1),
                (typeof(GreatGlintStoneCard), 7),
                (typeof(GlintbladePhalanxCard), 7),
                (typeof(LightSwordAttackCard), 3),
                (typeof(FlaskOfCeruleanTearsCard), 3),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(CarianGreatSwordCard), 3),
                (typeof(EndureCard), 3)
            },
            ["Royal Knight Loretta"] = new()
            {
                (typeof(CarianGreatSwordCard), 5),
                (typeof(GreatGlintStoneCard), 5),
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(LightSwordAttackCard), 3),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(EndureCard), 3),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(WarCryCard), 2)
            },
            ["Mimic Tear"] = new()
            {
                // TODO: Make sure to copy the deck of the player
            },
            ["Starscourge Radahn"] = new()
            {
                // TODO: Add stormcaller cry card
                (typeof(StormcallerCard), 3),
                
                (typeof(HeavySwordAttackCard), 10),
                (typeof(RallyingStandardCard), 3),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(EndureCard), 3),
                (typeof(WarCryCard), 5),
                (typeof(ThrowingDaggerCard), 4)
            },
            ["Bloody Finger Hunter Yura"] = new()
            {
                (typeof(UnsheatheCard), 5),
                (typeof(QuickstepCard), 4),
                (typeof(LightSwordAttackCard), 5),
                (typeof(HeavySwordAttackCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(EndureCard), 4),
                (typeof(PoisonPotCard), 2)
            },
            ["Morgott, The Omen King"] = new()
            {
                // TODO: Add Special Boss Card
                (typeof(LightSwordAttackCard), 3),
                
                (typeof(HeavySwordAttackCard), 8),
                (typeof(EndureCard), 4),
                (typeof(WarCryCard), 4),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(PoisonPotCard), 2),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(RallyingStandardCard), 3)
            },
            ["Dung Eater"] = new()
            {
                (typeof(HeavySwordAttackCard), 8),
                (typeof(PoisonPotCard), 6),
                (typeof(EndureCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(ThrowingDaggerCard), 6),
                (typeof(WarCryCard), 3)
            },
            ["Mohg, Lord of Blood"] = new()
            {
                // TODO: (typeof(BloodboonRitualCard), 1),
                (typeof(LightSwordAttackCard), 5),
                
                (typeof(BloodhoundStepCard), 5),
                (typeof(HeavySwordAttackCard), 10),
                (typeof(RallyingStandardCard), 3),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(ThrowingDaggerCard), 5),
                (typeof(WarCryCard), 3)
            },
            ["Commander Niall"] = new()
            {
                (typeof(RallyingStandardCard), 3),
                (typeof(HeavySwordAttackCard), 7),
                (typeof(WarCryCard), 5),
                (typeof(LightSwordAttackCard), 5),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(PoisonPotCard), 3)
            },
            ["Malenia, Blade of Miquella"] = new()
            {
                (typeof(LightSwordAttackCard), 6),
                (typeof(HeavySwordAttackCard), 5),
                (typeof(PoisonPotCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(EndureCard), 5),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(RallyingStandardCard), 3)
            },
            ["Malenia, Goddess of Rot"] = new()
            {
                (typeof(WaterfowlDanceCard), 3),
                (typeof(LightSwordAttackCard), 8),
                (typeof(PoisonPotCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(EndureCard), 5),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(RallyingStandardCard), 3)
            },
            ["Sir Gideon Ofnir, The All-Knowing"] = new()
            {
                (typeof(GreatGlintStoneCard), 6),
                (typeof(GlintbladePhalanxCard), 6),
                (typeof(CarianGreatSwordCard), 6),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(PoisonPotCard), 4),
                (typeof(EndureCard), 3),
                (typeof(LarvalTearCard), 1)
            },
            ["Godfrey, The First Elden Lord"] = new()
            {
                (typeof(HeavySwordAttackCard), 10),
                (typeof(RallyingStandardCard), 4),
                (typeof(EndureCard), 5),
                (typeof(WarCryCard), 5),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(FlaskOfCrimsonTearsCard), 2)
            },
            ["Hoarah Loux, Warrior"] = new()
            {
                // TODO: Add Special Boss Card
                (typeof(LightSwordAttackCard), 3),
                
                (typeof(HeavySwordAttackCard), 10),
                (typeof(RallyingStandardCard), 3),
                (typeof(EndureCard), 4),
                (typeof(WarCryCard), 4),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(FlaskOfCrimsonTearsCard), 2)
            },
            ["Radagon of the Golden Order"] = new()
            {
                (typeof(GreatGlintStoneCard), 5),
                (typeof(CarianGreatSwordCard), 5),
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(EndureCard), 3),
                (typeof(LightSwordAttackCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(WarCryCard), 3)
            },
            ["Elden Beast"] = new()
            {
                // TODO: Add Special Boss Card
                (typeof(ThrowingDaggerCard), 3),
                
                (typeof(CometAzurCard), 2),
                (typeof(GreatGlintStoneCard), 6),
                (typeof(LightSwordAttackCard), 5),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(EndureCard), 4),
            },
            ["Tarnished, Consort of the Stars"] = new()
            {
                // TODO: (typeof(DarkMoonGreatSwordCard), 3),
                (typeof(ThrowingDaggerCard), 3),
                
                (typeof(HeavySwordAttackCard), 7),
                (typeof(LightSwordAttackCard), 6),
                (typeof(EndureCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(GlintStonePebbleCard), 3)
            },
            ["Ranni, Queen of the Dark Moon"] = new()
            {
                // TODO: (typeof(StarcallerCryCard), 3),
                (typeof(LightSwordAttackCard), 3),
                
                (typeof(CometAzurCard), 2),
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(GreatGlintStoneCard), 5),
                (typeof(HeavySwordAttackCard), 5),
                (typeof(LightSwordAttackCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfCeruleanTearsCard), 3)
            }
        };
    }

    private void ConfigureLevels()
    {
        Levels = new()
        {
            // White mask Varré
            new Level
            {
                Enemy = _enemies["White Mask Varré"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                Reward = _rewards["White Mask Varré"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["White Mask Varré"], _decks["White Mask Varré"])
            },
            
            // Tree sentinel
            new Level
            {
                Enemy = _enemies["Tree Sentinel"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                Reward = _rewards["Tree Sentinel"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Tree Sentinel"], _decks["Tree Sentinel"])
            },
            
            // Renna
            new Level
            {
                Enemy = _enemies["Renna"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                Reward = _rewards["Renna"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Renna"], _decks["Renna"])
            },

            // Roderika
            new Level
            {
                Enemy = _enemies["Roderika"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                Reward = _rewards["Roderika"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Roderika"], _decks["Roderika"])
            },
            
            // Margit the Fell Omen
            new Level
            {
                Enemy = _enemies["Margit, The Fell Omen"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Stormveil Castle backdrop
                Reward = _rewards["Margit, The Fell Omen"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Stormveil Castle music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Margit, The Fell Omen"], _decks["Margit, The Fell Omen"])
            },
            
            // Fia, The Deathbed Companion
            new Level
            {
                Enemy = _enemies["Fia, The Deathbed Companion"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Roundtable hold backdrop
                Reward = _rewards["Fia, The Deathbed Companion"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Roundtable hold music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Fia, The Deathbed Companion"], _decks["Fia, The Deathbed Companion"])
            },
            
            // Blaidd, The Half-Wolf
            new Level
            {
                Enemy = _enemies["Blaidd, The Half-Wolf"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                Reward = _rewards["Blaidd, The Half-Wolf"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Blaidd, The Half-Wolf"], _decks["Blaidd, The Half-Wolf"])
            },
            
            // Sorceress Sellen
            new Level
            {
                Enemy = _enemies["Sorceress Sellen"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                Reward = _rewards["Sorceress Sellen"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Sorceress Sellen"], _decks["Sorceress Sellen"])
            },
            
            // Gatekeeper Gostoc
            new Level
            {
                Enemy = _enemies["Gatekeeper Gostoc"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                Reward = _rewards["Gatekeeper Gostoc"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Gatekeeper Gostoc"], _decks["Gatekeeper Gostoc"])
            },
            
            // Godrick the Grafted
            new Level
            {
                Enemy = _enemies["Godrick the Grafted"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Stormveil castle backdrop
                Reward = _rewards["Godrick the Grafted"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Stormveil castle music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Godrick the Grafted"], _decks["Godrick the Grafted"])
            },
            
            // Godrick the Grafted (2nd phase)
            new Level
            {
                Enemy = _enemies["Godrick the Grafted (2nd phase)"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Stormveil castle backdrop
                Reward = _rewards["Godrick the Grafted (2nd phase)"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Stormveil castle music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Godrick the Grafted (2nd phase)"], _decks["Godrick the Grafted (2nd phase)"])
            },
            
            // Thops
            new Level
            {
                Enemy = _enemies["Thops"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                Reward = _rewards["Thops"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Thops"], _decks["Thops"])
            },
            
            // Red Wolf of Radagon
            new Level
            {
                Enemy = _enemies["Red Wolf of Radagon"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Raya Lucaria backdrop
                Reward = _rewards["Red Wolf of Radagon"],    
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Red wolf of Radagon boss music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Red Wolf of Radagon"], _decks["Red Wolf of Radagon"])
            },
            
            // Rennala, Queen of the Full Moon
            new Level
            {
                Enemy = _enemies["Rennala, Queen of the Full Moon"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Raya Lucaria backdrop
                Reward = _rewards["Rennala, Queen of the Full Moon"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Rennala boss music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Rennala, Queen of the Full Moon"], _decks["Rennala, Queen of the Full Moon"])
            },
            
            // Rennala, Queen of the Full Moon (2nd phase)
            new Level
            {
                Enemy = _enemies["Rennala, Queen of the Full Moon (2nd phase)"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Raya Lucaria backdrop
                Reward = _rewards["Rennala, Queen of the Full Moon (2nd phase)"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Rennala boss music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Rennala, Queen of the Full Moon (2nd phase)"], _decks["Rennala, Queen of the Full Moon (2nd phase)"])
            },
            
            // Royal Knight Loretta
            new Level
            {
                Enemy = _enemies["Royal Knight Loretta"],
                Backdrop = DataManager.GetInstance().LiurniaBackdrop,  
                Reward = _rewards["Royal Knight Loretta"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Liurnia music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Royal Knight Loretta"], _decks["Royal Knight Loretta"])
            },
            
            // Mimic Tear
            new Level
            {
                Enemy = _enemies["Mimic Tear"],
                Backdrop = DataManager.GetInstance().LiurniaBackdrop,   // TODO: Nokron backdrop  
                Reward = _rewards["Mimic Tear"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Nokron music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Mimic Tear"], _decks["Mimic Tear"])
            },
            
            // Starscourge Radahn
            new Level
            {
                Enemy = _enemies["Starscourge Radahn"],
                Backdrop = DataManager.GetInstance().CaelidBackdrop,
                Reward = _rewards["Starscourge Radahn"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Starscourge Radahn"], _decks["Starscourge Radahn"])
            },
            
            // Bloody Finger Hunter Yura
            new Level
            {
                Enemy = _enemies["Bloody Finger Hunter Yura"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Leyndell backdrop
                Reward = _rewards["Bloody Finger Hunter Yura"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Leyndell music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Bloody Finger Hunter Yura"], _decks["Bloody Finger Hunter Yura"])
            },
            
            // Morgott, The Omen King
            new Level
            {
                Enemy = _enemies["Morgott, The Omen King"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Morgott arena backdrop
                Reward = _rewards["Morgott, The Omen King"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Morgott fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Morgott, The Omen King"], _decks["Morgott, The Omen King"])
            },
            
            // Dung Eater
            new Level
            {
                Enemy = _enemies["Dung Eater"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Leyndell backdrop
                Reward = _rewards["Dung Eater"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Leyndell music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Dung Eater"], _decks["Dung Eater"])
            },
            
            // Mohg, Lord of Blood
            new Level
            {
                Enemy = _enemies["Mohg, Lord of Blood"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Mohg arena backdrop
                Reward = _rewards["Mohg, Lord of Blood"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Mohg fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Mohg, Lord of Blood"], _decks["Mohg, Lord of Blood"])
            },
            
            // Commander Niall
            new Level
            {
                Enemy = _enemies["Commander Niall"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Niall arena backdrop
                Reward = _rewards["Commander Niall"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Niall fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Commander Niall"], _decks["Commander Niall"])
            },
            
            // Malenia, Blade of Miquella
            new Level
            {
                Enemy = _enemies["Malenia, Blade of Miquella"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Malenia arena backdrop
                Reward = _rewards["Malenia, Blade of Miquella"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Malenia fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Malenia, Blade of Miquella"], _decks["Malenia, Blade of Miquella"])
            },
            
            // Malenia, Goddess of Rot
            new Level
            {
                Enemy = _enemies["Malenia, Goddess of Rot"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Malenia arena backdrop
                Reward = _rewards["Malenia, Goddess of Rot"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Malenia fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Malenia, Goddess of Rot"], _decks["Malenia, Goddess of Rot"])
            },
            
            // Sir Gideon Ofnir, The All-Knowing
            new Level
            {
                Enemy = _enemies["Sir Gideon Ofnir, The All-Knowing"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Leyndell backdrop
                Reward = _rewards["Sir Gideon Ofnir, The All-Knowing"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Leyndell music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Sir Gideon Ofnir, The All-Knowing"], _decks["Sir Gideon Ofnir, The All-Knowing"])
            },
            
            // Godfrey, The First Elden Lord
            new Level
            {
                Enemy = _enemies["Godfrey, The First Elden Lord"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Godfrey arena backdrop
                Reward = _rewards["Godfrey, The First Elden Lord"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Godfrey fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Godfrey, The First Elden Lord"], _decks["Godfrey, The First Elden Lord"])
            },
            
            // Hoarah Loux, Warrior
            new Level
            {
                Enemy = _enemies["Hoarah Loux, Warrior"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Hoarah arena backdrop
                Reward = _rewards["Hoarah Loux, Warrior"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Hoarah fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Hoarah Loux, Warrior"], _decks["Hoarah Loux, Warrior"])
            },
            
            // Radagon of the Golden Order
            new Level
            {
                Enemy = _enemies["Radagon of the Golden Order"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Radagon arena backdrop
                Reward = _rewards["Radagon of the Golden Order"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Radagon fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Radagon of the Golden Order"], _decks["Radagon of the Golden Order"])
            },
            
            // Elden Beast
            new Level
            {
                Enemy = _enemies["Elden Beast"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Radagon arena backdrop
                Reward = _rewards["Elden Beast"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Elden beast fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Elden Beast"], _decks["Elden Beast"])
            },
            
            // Tarnished, Consort of the Stars
            new Level
            {
                Enemy = _enemies["Tarnished, Consort of the Stars"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Dark moon backdrop
                Reward = _rewards["Tarnished, Consort of the Stars"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Come up with proper music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Tarnished, Consort of the Stars"], _decks["Tarnished, Consort of the Stars"])
            },
            
            // Ranni, Queen of the Dark Moon
            new Level
            {
                Enemy = _enemies["Ranni, Queen of the Dark Moon"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,  // TODO: Radagon arena backdrop
                Reward = _rewards["Ranni, Queen of the Dark Moon"],
                SoundTrack = DataManager.GetInstance().LimgraveMusic.CreateInstance(),  // TODO: Come up with proper music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Ranni, Queen of the Dark Moon"], _decks["Ranni, Queen of the Dark Moon"])
            }
        };
    }
}