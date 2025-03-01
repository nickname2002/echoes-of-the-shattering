using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using MonoZenith.Players;
using MonoZenith.Screen.RewardPanel;

namespace MonoZenith.Support.Managers;

public class LevelManager
{
    private readonly Dictionary<string, NpcPlayer> _enemies;
    private Dictionary<string, Reward> _rewards;
    private Dictionary<string, List<(Type, int)>> _decks;
    private readonly Dictionary<string, Texture2D> _enemyPortraits;
    private readonly string[] _enemyNames =
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
        "Godrick the Grafted ",  // Second phase
        "Thops",
        "Red Wolf of Radagon",
        "Rennala, Queen of the Full Moon",
        "Rennala, Queen of the Full Moon ",  // Second phase
        "Royal Knight Loretta",
        "Mimic Tear",
        "Starscourge Radahn",
        "Starscourge Radahn ",  // Second phase
        "Bloody Finger Hunter Yura",
        "Morgott, The Omen King",
        "Dung Eater",
        "Rykard, Lord of Blasphemy",
        "Mohg, Lord of Blood",
        "Commander Niall",
        "Malenia, Blade of Miquella",
        "Malenia, Goddess of Rot",  // Second phase
        "Maliketh, the Black Blade",
        "Sir Gideon Ofnir, The All-Knowing",
        "Godfrey, The First Elden Lord",
        "Hoarah Loux, Warrior",  // Second phase
        "Radagon of the Golden Order",
        "Elden Beast",  // Second phase
        "Tarnished, Consort of the Stars",
        "Ranni, Queen of the Dark Moon"
    };
    
    public static List<Level> Levels { get; set; }
    public static Level CurrentLevel { get; set; }
    
    public LevelManager()
    {
        _enemies = new Dictionary<string, NpcPlayer>();
        _decks = new Dictionary<string, List<(Type, int)>>();
        _enemyPortraits = new Dictionary<string, Texture2D>();
        
        ConfigureEnemyPortraits();
        ConfigureEnemyObjects();
        ConfigureSpiritAshes();
        ConfigureRewards();
        ConfigureDecks();
        ConfigureLevels();
        
        // TODO: Unlock needed levels for testing purposes using following helper methods:
        // SetAllLevelsUnlocked();
        // SetUnlockedUpUntil("Radagon of the Golden Order");
    }
    
    public Level GetLevelFromEnemy(string enemyName)
    {
        return Levels.Find(level => level.Enemy.Name == enemyName);
    }
    
    public static void SetNextLevelUnlocked(Level level)
    {
        var currentLevelIndex = Levels.IndexOf(level);
        if (currentLevelIndex == Levels.Count - 1)
            return;
        
        Levels[currentLevelIndex + 1].Unlocked = true;
    }
    
    public static void SetRewardCollected(Level level)
    {
        Console.WriteLine("Reward collected from: " + level.Enemy.Name);
        level.RewardCollected = true;
        if (level.SecondPhase != null)
        {
            level.SecondPhase.RewardCollected = true;
        }
    }
    
    private void SetAllLevelsUnlocked()
    {
        foreach (var level in Levels)
        {
            level.Unlocked = true;
            SetRewardCollected(level);
        }
    }

    public bool RegionActive(Region region)
    {
        return region switch
        {
            Region.Limgrave => true,
            Region.Liurnia => GetLevelFromEnemy("Mimic Tear").Unlocked,
            Region.AltusPlateau => GetLevelFromEnemy("Bloody Finger Hunter Yura").Unlocked,
            _ => false
        };
    }
    
    private void SetUnlockedUpUntil(string enemyName)
    {
        var level = GetLevelFromEnemy(enemyName);
        var levelIndex = Levels.IndexOf(level);
        for (var i = 0; i <= levelIndex; i++)
        {
            Levels[i].Unlocked = true;
            if (i + 1 > levelIndex) return;
            SetRewardCollected(Levels[i]);
        }
    }
    
    private List<Card.Card> GenerateDeck(NpcPlayer enemy, List<(Type, int)> cards)
    {
        var deck = new List<Card.Card>();
        
        foreach (var (cardType, count) in cards)
        {
            for (var i = 0; i < count; i++)
            {
                var card = (Card.Card) Activator.CreateInstance(cardType);
                if (card == null) continue;
                card.SetOwner(enemy);
                deck.Add(card);
            }
        }
        
        return deck;
    }

    private void ConfigureEnemyPortraits()
    {
        foreach (var name in _enemyNames)
        {
            try
            {
                _enemyPortraits[name] = Game.LoadImage($"Images/Player/{name}.png");
            }
            catch (Exception)
            {
                _enemyPortraits[name] = DataManager.GetInstance().DefaultEnemyPortrait;
            }
        }
    }

    private void ConfigureEnemyObjects()
    {
        foreach (var name in _enemyNames)
        {
            _enemies[name] = new NpcPlayer(
                Game.GetGameState(),
                name,
                _enemyPortraits.GetValueOrDefault(name, DataManager.GetInstance().DefaultEnemyPortrait));
        }
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
        _enemies["Commander Niall"].SetSpiritAsh(typeof(WolvesAsh));
        _enemies["Rykard, Lord of Blasphemy"].SetSpiritAsh(typeof(JellyfishAsh));
        _enemies["Malenia, Goddess of Rot"].SetSpiritAsh(typeof(MimicTearAsh));
        _enemies["Maliketh, the Black Blade"].SetSpiritAsh(typeof(MimicTearAsh));
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
            ["Thops"] = new(DataManager.GetInstance().CardThopsBarrier, "Thops' Barrier", typeof(ThopsBarrierCard)),
            ["Red Wolf of Radagon"] = new(DataManager.GetInstance().CardGlintPhalanx, "Glintblade Phalanx", typeof(GlintbladePhalanxCard)),
            ["Rennala, Queen of the Full Moon"] = null,
            ["Rennala, Queen of the Full Moon (2nd phase)"] = new(DataManager.GetInstance().CardCometAzur, "Comet Azur", typeof(CometAzurCard)),
            ["Royal Knight Loretta"] = new(DataManager.GetInstance().CardCarianGSword, "Carian Greatsword", typeof(CarianGreatSwordCard)),
            ["Mimic Tear"] = new(DataManager.GetInstance().MimicTearAsh, "Mimic Tear", typeof(MimicTearAsh)),
            ["Starscourge Radahn"] = null,
            ["Starscourge Radahn (2nd phase)"] = new(DataManager.GetInstance().CardStarcallerCry, "Starcaller Cry", typeof(StarcallerCryCard)),
            ["Bloody Finger Hunter Yura"] = new(DataManager.GetInstance().CardUnsheathe, "Unsheathe", typeof(UnsheatheCard)),
            ["Dung Eater"] = null, 
            ["Rykard, Lord of Blasphemy"] = null,
            ["Mohg, Lord of Blood"] = new(DataManager.GetInstance().CardBloodboon, "Bloodboon Ritual", typeof(BloodboonRitualCard)),
            ["Commander Niall"] = new(DataManager.GetInstance().CardRallyingStandard, "Rallying Standard", typeof(RallyingStandardCard)),
            ["Malenia, Blade of Miquella"] = null,
            ["Malenia, Goddess of Rot"] = new(DataManager.GetInstance().CardWaterfowlDance, "Waterfowl Dance", typeof(WaterfowlDanceCard)),
            ["Morgott, The Omen King"] = new(DataManager.GetInstance().CardCursedSlice, "Cursed Blood Slice", typeof(CursedBloodSliceCard)),
            ["Maliketh, the Black Blade"] = new(DataManager.GetInstance().CardDestinedDeath, "Destined Death", typeof(DestinedDeathCard)),
            ["Sir Gideon Ofnir, The All-Knowing"] = new(DataManager.GetInstance().CardLarvalTear, "Larval Tear", typeof(LarvalTearCard)),
            ["Godfrey, The First Elden Lord"] = null,
            ["Hoarah Loux, Warrior"] = new(DataManager.GetInstance().CardRegalRoar, "Regal Roar", typeof(RegalRoarCard)),
            ["Radagon of the Golden Order"] = null,
            ["Elden Beast"] = new(DataManager.GetInstance().CardWaveOfGold, "Wave of Gold", typeof(WaveOfGoldCard)),
            ["Tarnished, Consort of the Stars"] = null,
            ["Ranni, Queen of the Dark Moon"] = new(DataManager.GetInstance().CardMoonlight, "Moonlight Greatsword", typeof(MoonlightGreatswordCard))
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
                (typeof(HeavySwordAttackCard), 10),
                (typeof(FlaskOfCrimsonTearsCard), 3),
                (typeof(RallyingStandardCard), 3),
                (typeof(EndureCard), 5),
                (typeof(WarCryCard), 4),
                (typeof(ThrowingDaggerCard), 5)
            },
            ["Godrick the Grafted (2nd phase)"] = new()
            {
                (typeof(ICommandTheeKneelCard), 2),
                (typeof(HeavySwordAttackCard), 10),
                (typeof(FlaskOfCrimsonTearsCard), 3),
                (typeof(RallyingStandardCard), 3),
                (typeof(EndureCard), 5),
                (typeof(WarCryCard), 4),
                (typeof(ThrowingDaggerCard), 5)
            },
            ["Thops"] = new()
            {
                (typeof(LightSwordAttackCard), 3),                
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(GreatGlintStoneCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 3),
                (typeof(FlaskOfCeruleanTearsCard), 3),
                (typeof(ThopsBarrierCard), 4),
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
                (typeof(GlintbladePhalanxCard), 6),
                (typeof(LightSwordAttackCard), 3),
                (typeof(FlaskOfCeruleanTearsCard), 3),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(CarianGreatSwordCard), 3),
                (typeof(EndureCard), 3),
                (typeof(LarvalTearCard), 1)
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
                // TODO: Will copy the deck of the player on load
            },
            ["Starscourge Radahn"] = new()
            {
                (typeof(HeavySwordAttackCard), 10),
                (typeof(RallyingStandardCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(EndureCard), 2),
                (typeof(WarCryCard), 2),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(GreatGlintStoneCard), 6)
            },
            ["Starscourge Radahn (2nd phase)"] = new()
            {
                (typeof(StarcallerCryCard), 4),   
                (typeof(HeavySwordAttackCard), 10),
                (typeof(RallyingStandardCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(EndureCard), 2),
                (typeof(WarCryCard), 2),
                (typeof(GreatGlintStoneCard), 6)
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
                (typeof(CursedBloodSliceCard), 2),
                (typeof(LightSwordAttackCard), 3),
                (typeof(HeavySwordAttackCard), 6),
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
            ["Rykard, Lord of Blasphemy"] = new()
            {
                (typeof(HeavySwordAttackCard), 6),
                (typeof(LightSwordAttackCard), 8),
                (typeof(EndureCard), 2),
                (typeof(WarCryCard), 2),
                (typeof(ThrowingDaggerCard), 5),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(PoisonPotCard), 4),
                (typeof(StormcallerCard), 2),
            },
            ["Mohg, Lord of Blood"] = new()
            {
                (typeof(BloodboonRitualCard), 2),
                (typeof(LightSwordAttackCard), 4),
                (typeof(HeavySwordAttackCard), 8),
                (typeof(BloodhoundStepCard), 5),
                (typeof(RallyingStandardCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 4),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(WarCryCard), 2)
            },
            ["Commander Niall"] = new()
            {
                (typeof(RallyingStandardCard), 3),
                (typeof(HeavySwordAttackCard), 7),
                (typeof(WarCryCard), 5),
                (typeof(LightSwordAttackCard), 5),
                (typeof(ThrowingDaggerCard), 4),
                (typeof(FlaskOfCrimsonTearsCard), 3),
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
            ["Maliketh, the Black Blade"] = new()
            {
                (typeof(DestinedDeathCard), 3),
                (typeof(LightSwordAttackCard), 6),
                (typeof(HeavySwordAttackCard), 5),      
                (typeof(ThrowingDaggerCard), 4),       
                (typeof(FlaskOfCrimsonTearsCard), 2),  
                (typeof(EndureCard), 2),               
                (typeof(WarCryCard), 2),               
                (typeof(StormcallerCard), 2),         
                (typeof(PoisonPotCard), 2),           
                (typeof(WarmingStoneCard), 2)         
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
                (typeof(RegalRoarCard), 2),
                (typeof(LightSwordAttackCard), 3),
                (typeof(HeavySwordAttackCard), 8),
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
                (typeof(WaveOfGoldCard), 2),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(CometAzurCard), 2),
                (typeof(GreatGlintStoneCard), 6),
                (typeof(LightSwordAttackCard), 5),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfWondrousPhysickCard), 2),
                (typeof(GlintbladePhalanxCard), 3),
                (typeof(EndureCard), 3)
            },
            ["Tarnished, Consort of the Stars"] = new()
            {
                (typeof(MoonlightGreatswordCard), 2),
                (typeof(ThrowingDaggerCard), 3),
                (typeof(HeavySwordAttackCard), 6),
                (typeof(LightSwordAttackCard), 6),
                (typeof(EndureCard), 4),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfCeruleanTearsCard), 2),
                (typeof(FlaskOfWondrousPhysickCard), 2),
                (typeof(GlintStonePebbleCard), 3)
            },
            ["Ranni, Queen of the Dark Moon"] = new()
            {
                (typeof(StarcallerCryCard), 3),
                (typeof(CometAzurCard), 2),
                (typeof(GlintbladePhalanxCard), 5),
                (typeof(GreatGlintStoneCard), 5),
                (typeof(HeavySwordAttackCard), 4),
                (typeof(LightSwordAttackCard), 4),
                (typeof(FlaskOfCrimsonTearsCard), 2),
                (typeof(FlaskOfCeruleanTearsCard), 3),
                (typeof(FlaskOfWondrousPhysickCard), 2),
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
                LevelReward = _rewards["White Mask Varré"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["White Mask Varré"], _decks["White Mask Varré"]),
                Unlocked = true,
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Varre/varre-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Varre/varre-start-voiceline-2.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Varre/varre-start-voiceline-3.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Varre/varre-start-voiceline-4.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Varre/varre-start-voiceline-5.wav").CreateInstance()
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Varre/varre-loss-voiceline-1.wav").CreateInstance()
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Varre/varre-win-voiceline-1.wav").CreateInstance()
                }
            },
            
            // Tree sentinel
            new Level
            {
                Enemy = _enemies["Tree Sentinel"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Tree Sentinel"],
                SoundTrack = DataManager.GetInstance().TreeSentinelSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Tree Sentinel"], _decks["Tree Sentinel"])
            },
            
            // Roderika
            new Level
            {
                Enemy = _enemies["Roderika"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Roderika"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Roderika"], _decks["Roderika"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Roderika/roderika-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Roderika/roderika-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Roderika/roderika-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Roderika/roderika-win-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Roderika/roderika-win-voiceline-2.wav").CreateInstance(),
                }
            },
            
            // Renna
            new Level
            {
                Enemy = _enemies["Renna"],
                Backdrop = DataManager.GetInstance().ChurchOfEllehBackdrop,
                LevelReward = _rewards["Renna"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Renna"], _decks["Renna"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Renna/renna-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Renna/renna-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Renna/renna-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Renna/renna-win-voiceline-1.wav").CreateInstance(),
                }
            },
            
            // Blaidd, The Half-Wolf
            new Level
            {
                Enemy = _enemies["Blaidd, The Half-Wolf"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Blaidd, The Half-Wolf"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Blaidd, The Half-Wolf"], _decks["Blaidd, The Half-Wolf"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Blaidd/blaidd-start-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Blaidd/blaidd-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Blaidd/blaidd-win-voiceline-1.wav").CreateInstance(),
                }
            },
            
            // Sorceress Sellen
            new Level
            {
                Enemy = _enemies["Sorceress Sellen"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Sorceress Sellen"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Sorceress Sellen"], _decks["Sorceress Sellen"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Sellen/sellen-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Sellen/sellen-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Sellen/sellen-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Sellen/sellen-win-voiceline-1.wav").CreateInstance(),
                }
            },
            
            // Thops
            new Level
            {
                Enemy = _enemies["Thops"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Thops"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Thops"], _decks["Thops"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Thops/thops-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Thops/thops-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Thops/thops-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Thops/thops-win-voiceline-1.wav").CreateInstance(),
                }
            },
            
            // Fia, The Deathbed Companion
            new Level
            {
                Enemy = _enemies["Fia, The Deathbed Companion"],
                Backdrop = DataManager.GetInstance().RoundtableHoldBackdrop,
                LevelReward = _rewards["Fia, The Deathbed Companion"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Fia, The Deathbed Companion"], _decks["Fia, The Deathbed Companion"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Fia/fia-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Fia/fia-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Fia/fia-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Fia/fia-win-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Fia/fia-win-voiceline-2.wav").CreateInstance(),
                }
            },
            
            // Gatekeeper Gostoc
            new Level
            {
                Enemy = _enemies["Gatekeeper Gostoc"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Gatekeeper Gostoc"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Gatekeeper Gostoc"], _decks["Gatekeeper Gostoc"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Gostoc/gostoc-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Gostoc/gostoc-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Gostoc/gostoc-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Gostoc/gostoc-win-voiceline-1.wav").CreateInstance(),
                }
            },
            
            // Margit the Fell Omen
            new Level
            {
                Enemy = _enemies["Margit, The Fell Omen"],
                Backdrop = DataManager.GetInstance().StormveilBackdrop,
                LevelReward = _rewards["Margit, The Fell Omen"],
                SoundTrack = DataManager.GetInstance().MargitSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Margit, The Fell Omen"], _decks["Margit, The Fell Omen"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-start-voiceline-2.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-start-voiceline-3.wav").CreateInstance(),
                    Game.LoadAudio("Audio/SoundEffects/cursed-blood-slice.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-start-voiceline-4.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-win-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-win-voiceline-2.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-win-voiceline-3.wav").CreateInstance(),
                }
            },
            
            // Godrick the Grafted
            new Level
            {
                Enemy = _enemies["Godrick the Grafted"],
                Backdrop = DataManager.GetInstance().StormveilBackdrop,
                LevelReward = _rewards["Godrick the Grafted"],
                SoundTrack = DataManager.GetInstance().GodrickP1Soundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Godrick the Grafted"], _decks["Godrick the Grafted"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p1-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p1-start-voiceline-2.wav").CreateInstance(),
                    Game.LoadAudio("Audio/SoundEffects/command-kneel.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p1-start-voiceline-3.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p1-loss-voiceline-1.wav").CreateInstance(),
                },
                SecondPhase = new Level     // Godrick the Grafted (2nd phase)
                {
                    Enemy = _enemies["Godrick the Grafted "],
                    Backdrop = DataManager.GetInstance().StormveilBackdrop,
                    LevelReward = _rewards["Godrick the Grafted (2nd phase)"],
                    SoundTrack = DataManager.GetInstance().GodrickP2Soundtrack.CreateInstance(),
                    EnemyDeck = GenerateDeck(_enemies["Godrick the Grafted "], _decks["Godrick the Grafted (2nd phase)"]),
                    VoiceLinesBattleStart = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p2-start-voiceline-1.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p2-start-voiceline-2.wav").CreateInstance(),
                    },
                    VoiceLinesBattleLoss = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p2-loss-voiceline-1.wav").CreateInstance(),
                    },
                    VoiceLinesBattleVictory = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p2-win-voiceline-1.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p2-win-voiceline-2.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Godrick/godrick-p2-win-voiceline-3.wav").CreateInstance(),
                    }
                }
            },
            
            // Mimic Tear
            new Level
            {
                Enemy = _enemies["Mimic Tear"],
                Backdrop = DataManager.GetInstance().NokronBackdrop,
                LevelReward = _rewards["Mimic Tear"],
                SoundTrack = DataManager.GetInstance().GideonSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Mimic Tear"], _decks["Mimic Tear"])
            },
            
            // Royal Knight Loretta
            new Level
            {
                Enemy = _enemies["Royal Knight Loretta"],
                Backdrop = DataManager.GetInstance().LiurniaBackdrop,
                LevelReward = _rewards["Royal Knight Loretta"],
                SoundTrack = DataManager.GetInstance().TreeSentinelSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Royal Knight Loretta"], _decks["Royal Knight Loretta"])
            },
            
            // Red Wolf of Radagon
            new Level
            {
                Enemy = _enemies["Red Wolf of Radagon"],
                Backdrop = DataManager.GetInstance().RayaLucariaBackdrop,
                LevelReward = _rewards["Red Wolf of Radagon"],
                SoundTrack = DataManager.GetInstance().RedWolfSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Red Wolf of Radagon"], _decks["Red Wolf of Radagon"])
            },
            
            // Starscourge Radahn
            new Level
            {
                Enemy = _enemies["Starscourge Radahn"],
                Backdrop = DataManager.GetInstance().RadahnBattlefieldBackdrop,
                LevelReward = _rewards["Starscourge Radahn"],
                SoundTrack = DataManager.GetInstance().StarscourgeRadahnP1Soundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Starscourge Radahn"], _decks["Starscourge Radahn"]),
                SecondPhase = new Level
                {
                    Enemy = _enemies["Starscourge Radahn "],
                    Backdrop = DataManager.GetInstance().RadahnBattlefieldPhase2Backdrop,
                    LevelReward = _rewards["Starscourge Radahn (2nd phase)"],
                    SoundTrack = DataManager.GetInstance().StarscourgeRadahnP2Soundtrack.CreateInstance(),
                    EnemyDeck = GenerateDeck(_enemies["Starscourge Radahn "], _decks["Starscourge Radahn (2nd phase)"])
                }
            },
            
            // Rennala, Queen of the Full Moon
            new Level
            {
                Enemy = _enemies["Rennala, Queen of the Full Moon"],
                Backdrop = DataManager.GetInstance().RayaLucariaBackdrop,
                LevelReward = _rewards["Rennala, Queen of the Full Moon"],
                SoundTrack = DataManager.GetInstance().RennalaP1Soundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Rennala, Queen of the Full Moon"], _decks["Rennala, Queen of the Full Moon"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p1-start-voicelines-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p1-start-voicelines-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p1-loss-voicelines-1.wav").CreateInstance(),
                },
                SecondPhase = new Level     // Rennala, Queen of the Full Moon (2nd phase)
                {
                    Enemy = _enemies["Rennala, Queen of the Full Moon "],
                    Backdrop = DataManager.GetInstance().RayaLucariaBackdrop,
                    LevelReward = _rewards["Rennala, Queen of the Full Moon (2nd phase)"],
                    SoundTrack = DataManager.GetInstance().RennalaP2Soundtrack.CreateInstance(),
                    EnemyDeck = GenerateDeck(_enemies["Rennala, Queen of the Full Moon "], _decks["Rennala, Queen of the Full Moon (2nd phase)"]),
                    VoiceLinesBattleStart = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p2-start-voicelines-1.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p2-start-voicelines-2.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p2-start-voicelines-3.wav").CreateInstance(),
                    },
                    VoiceLinesBattleLoss = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p2-loss-voicelines-1.wav").CreateInstance(),
                    },
                    VoiceLinesBattleVictory = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p2-win-voicelines-1.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Rennala/rennala-p2-win-voicelines-2.wav").CreateInstance(),
                    }
                }
            },
            
            // Bloody Finger Hunter Yura
            new Level
            {
                Enemy = _enemies["Bloody Finger Hunter Yura"],
                Backdrop = DataManager.GetInstance().AltusPlateauBackdrop,
                LevelReward = _rewards["Bloody Finger Hunter Yura"],
                SoundTrack = DataManager.GetInstance().LeyndellSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Bloody Finger Hunter Yura"], _decks["Bloody Finger Hunter Yura"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Yura/yura-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Yura/yura-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Yura/yura-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Yura/yura-win-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Yura/yura-win-voiceline-2.wav").CreateInstance(),
                }
            },
            
            // Sir Gideon Ofnir, The All-Knowing
            new Level
            {
                Enemy = _enemies["Sir Gideon Ofnir, The All-Knowing"],
                Backdrop = DataManager.GetInstance().LeyndellFireBackdrop,
                LevelReward = _rewards["Sir Gideon Ofnir, The All-Knowing"],
                SoundTrack = DataManager.GetInstance().GideonSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Sir Gideon Ofnir, The All-Knowing"], _decks["Sir Gideon Ofnir, The All-Knowing"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-start-voiceline-2.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-start-voiceline-3.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-start-voiceline-4.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-start-voiceline-5.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-start-voiceline-6.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-win-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Gideon/gideon-win-voiceline-2.wav").CreateInstance(),
                }
            },
            
            // Dung Eater
            new Level
            {
                Enemy = _enemies["Dung Eater"],
                Backdrop = DataManager.GetInstance().AltusPlateauBackdrop,
                LevelReward = _rewards["Dung Eater"],
                SoundTrack = DataManager.GetInstance().LeyndellSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Dung Eater"], _decks["Dung Eater"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Dung/dung-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Dung/dung-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Dung/dung-loss-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Dung/dung-loss-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Dung/dung-win-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Dung/dung-win-voiceline-2.wav").CreateInstance(),
                }
            },
            
            // Rykard, Lord of Blasphemy
            new Level
            {
                Enemy = _enemies["Rykard, Lord of Blasphemy"],
                Backdrop = DataManager.GetInstance().RykardBackdrop,
                LevelReward = _rewards["Rykard, Lord of Blasphemy"],
                SoundTrack = DataManager.GetInstance().RykardSoundtrack.CreateInstance(),  
                EnemyDeck = GenerateDeck(_enemies["Rykard, Lord of Blasphemy"], _decks["Rykard, Lord of Blasphemy"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Rykard/rykard-start-voicelines-1.wav").CreateInstance()
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Rykard/rykard-loss-voicelines-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Rykard/rykard-win-voicelines-1.wav").CreateInstance(),
                }
            },
            
            // Commander Niall
            new Level
            {
                Enemy = _enemies["Commander Niall"],
                Backdrop = DataManager.GetInstance().CastleSolBackdrop,
                LevelReward = _rewards["Commander Niall"],
                SoundTrack = DataManager.GetInstance().NiallSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Commander Niall"], _decks["Commander Niall"])
            },
            
            // Mohg, Lord of Blood
            new Level
            {
                Enemy = _enemies["Mohg, Lord of Blood"],
                Backdrop = DataManager.GetInstance().MohgBackdrop,
                LevelReward = _rewards["Mohg, Lord of Blood"],
                SoundTrack = DataManager.GetInstance().MohgSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Mohg, Lord of Blood"], _decks["Mohg, Lord of Blood"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Mohg/mohg-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Mohg/mohg-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Mohg/mohg-loss-voiceline.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Mohg/mohg-win-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Mohg/mohg-win-voiceline-2.wav").CreateInstance(),
                }
            },
            
            // Malenia, Blade of Miquella
            new Level
            {
                Enemy = _enemies["Malenia, Blade of Miquella"],
                Backdrop = DataManager.GetInstance().MaleniaRestplaceBackdrop,
                LevelReward = _rewards["Malenia, Blade of Miquella"],
                SoundTrack = DataManager.GetInstance().MaleniaSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Malenia, Blade of Miquella"], _decks["Malenia, Blade of Miquella"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p1-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p1-start-voiceline-2.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p1-start-voiceline-3.wav").CreateInstance()
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p1-loss-voiceline-1.wav").CreateInstance()
                },
                SecondPhase = new Level     // Malenia, Goddess of Rot
                {
                    Enemy = _enemies["Malenia, Goddess of Rot"],
                    Backdrop = DataManager.GetInstance().MaleniaRestplaceBackdrop,
                    LevelReward = _rewards["Malenia, Goddess of Rot"],
                    SoundTrack = DataManager.GetInstance().GoddessOfRotSoundtrack.CreateInstance(),
                    EnemyDeck = GenerateDeck(_enemies["Malenia, Goddess of Rot"], _decks["Malenia, Goddess of Rot"]),
                    VoiceLinesBattleStart = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p2-start-voiceline-1.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p2-start-voiceline-2.wav").CreateInstance()
                    },
                    VoiceLinesBattleLoss = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p2-loss-voiceline-2.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p2-loss-voiceline-1.wav").CreateInstance()
                    },
                    VoiceLinesBattleVictory = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p2-win-voiceline-1.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Malenia/malenia-p2-win-voiceline-2.wav").CreateInstance()
                    }
                }
            },
            
            // Morgott, The Omen King
            new Level
            {
                Enemy = _enemies["Morgott, The Omen King"],
                Backdrop = DataManager.GetInstance().EldenThroneBackdrop,
                LevelReward = _rewards["Morgott, The Omen King"],
                SoundTrack = DataManager.GetInstance().MorgottSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Morgott, The Omen King"], _decks["Morgott, The Omen King"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Morgott/morgott-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Morgott/morgott-start-voiceline-2.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Morgott/morgott-start-voiceline-3.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Morgott/morgott-start-voiceline-4.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Margit/margit-loss-voiceline-1.wav").CreateInstance(),
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Morgott/morgott-win-voiceline-1.wav").CreateInstance(),
                }
            },
            
            // Maliketh, the Black Blade
            new Level
            {
                Enemy = _enemies["Maliketh, the Black Blade"],
                Backdrop = DataManager.GetInstance().FarumAzulaBackdrop, 
                LevelReward = _rewards["Maliketh, the Black Blade"],
                SoundTrack = DataManager.GetInstance().MalikethSoundtrack.CreateInstance(),  
                EnemyDeck = GenerateDeck(_enemies["Maliketh, the Black Blade"], _decks["Maliketh, the Black Blade"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Maliketh/maliketh-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Maliketh/maliketh-start-voiceline-2.wav").CreateInstance(),
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Maliketh/maliketh-loss-voiceline-1.wav").CreateInstance()
                },
                VoiceLinesBattleVictory = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Maliketh/maliketh-win-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Maliketh/maliketh-win-voiceline-2.wav").CreateInstance()
                }  
            },
            
            // Godfrey, The First Elden Lord
            new Level
            {
                Enemy = _enemies["Godfrey, The First Elden Lord"],
                Backdrop = DataManager.GetInstance().LeyndellFireBackdrop,
                LevelReward = _rewards["Godfrey, The First Elden Lord"],
                SoundTrack = DataManager.GetInstance().GodfreySoundtrack.CreateInstance(),  
                EnemyDeck = GenerateDeck(_enemies["Godfrey, The First Elden Lord"], _decks["Godfrey, The First Elden Lord"]),
                VoiceLinesBattleStart = new List<SoundEffectInstance>
                {   
                    Game.LoadAudio("Audio/VoiceLines/Godfrey/godfrey-start-voiceline-1.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Godfrey/godfrey-start-voiceline-2.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Godfrey/godfrey-start-voiceline-3.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Godfrey/godfrey-start-voiceline-4.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Godfrey/godfrey-start-voiceline-5.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Godfrey/godfrey-start-voiceline-6.wav").CreateInstance(),
                    Game.LoadAudio("Audio/VoiceLines/Godfrey/godfrey-start-voiceline-7.wav").CreateInstance()
                },
                VoiceLinesBattleLoss = new List<SoundEffectInstance>
                {
                    Game.LoadAudio("Audio/VoiceLines/Godfrey/godfrey-loss-voiceline-1.wav").CreateInstance(),
                },
                SecondPhase = new Level     // Hoarah Loux, Warrior
                {
                    Enemy = _enemies["Hoarah Loux, Warrior"],
                    Backdrop = DataManager.GetInstance().LeyndellFireBackdrop, 
                    LevelReward = _rewards["Hoarah Loux, Warrior"],
                    SoundTrack = DataManager.GetInstance().HoarahLouxSoundtrack.CreateInstance(),  
                    EnemyDeck = GenerateDeck(_enemies["Hoarah Loux, Warrior"], _decks["Hoarah Loux, Warrior"]),
                    VoiceLinesBattleStart = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Godfrey/hoarah-loux-start-voiceline-1.wav").CreateInstance(),
                    },
                    VoiceLinesBattleLoss = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Godfrey/hoarah-loux-loss-voiceline-1.wav").CreateInstance(),
                    },
                    VoiceLinesBattleVictory = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Godfrey/hoarah-loux-win-voiceline-2.wav").CreateInstance(),
                    }
                }
            },
            
            // Radagon of the Golden Order
            new Level
            {
                Enemy = _enemies["Radagon of the Golden Order"],
                Backdrop = DataManager.GetInstance().InsideErdtreeBackdrop,  
                LevelReward = _rewards["Radagon of the Golden Order"],
                SoundTrack = DataManager.GetInstance().RadagonSoundtrack.CreateInstance(), 
                EnemyDeck = GenerateDeck(_enemies["Radagon of the Golden Order"], _decks["Radagon of the Golden Order"]),
                SecondPhase = new Level     // Elden Beast
                {
                    Enemy = _enemies["Elden Beast"],
                    Backdrop = DataManager.GetInstance().InsideErdtreeBackdrop, 
                    LevelReward = _rewards["Elden Beast"],
                    SoundTrack = DataManager.GetInstance().EldenBeastSoundtrack.CreateInstance(),
                    EnemyDeck = GenerateDeck(_enemies["Elden Beast"], _decks["Elden Beast"])
                }
            },
            
            // Tarnished, Consort of the Stars
            new Level
            {
                // Source Portrait:https://www.bilibili.com/opus/948092070459867140?spm_id_from=333.999.0.0
                Enemy = _enemies["Tarnished, Consort of the Stars"], 
                Backdrop = DataManager.GetInstance().DarkMoonBackdrop, 
                LevelReward = _rewards["Tarnished, Consort of the Stars"],
                SoundTrack = DataManager.GetInstance().ConsortMoonP1Soundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(_enemies["Tarnished, Consort of the Stars"], _decks["Tarnished, Consort of the Stars"]),
                SecondPhase = new Level     // Ranni, Queen of the Dark Moon
                {
                    Enemy = _enemies["Ranni, Queen of the Dark Moon"],
                    Backdrop = DataManager.GetInstance().DarkMoonBackdrop, 
                    LevelReward = _rewards["Ranni, Queen of the Dark Moon"],
                    SoundTrack = DataManager.GetInstance().ConsortMoonP2Soundtrack.CreateInstance(),
                    EnemyDeck = GenerateDeck(_enemies["Ranni, Queen of the Dark Moon"], _decks["Ranni, Queen of the Dark Moon"]),
                    VoiceLinesBattleStart = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Ranni/ranni-start-voiceline-1.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Ranni/ranni-start-voiceline-2.wav").CreateInstance(),
                        Game.LoadAudio("Audio/VoiceLines/Ranni/ranni-start-voiceline-3.wav").CreateInstance(),
                    },
                    VoiceLinesBattleLoss = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Ranni/ranni-loss-voiceline-1.wav").CreateInstance(),
                    },
                    VoiceLinesBattleVictory = new List<SoundEffectInstance>
                    {
                        Game.LoadAudio("Audio/VoiceLines/Ranni/ranni-win-voiceline-1.wav").CreateInstance(),
                    }
                }
            },
        };
    }
}