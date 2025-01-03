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
        // SetUnlockedUpUntil("Godrick the Grafted");
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
            level.SecondPhase.RewardCollected = true;
    }
    
    private void SetAllLevelsUnlocked()
    {
        foreach (var level in Levels)
        {
            level.Unlocked = true;
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
        }
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
        // TODO: _enemies["Dung Eater"].SetSpiritAsh(typeof(DungEaterPuppet));
        _enemies["Commander Niall"].SetSpiritAsh(typeof(WolvesAsh));
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
            ["Thops"] = new(DataManager.GetInstance().CardWarmingStone, "Warming Stone", typeof(WarmingStoneCard)),
            ["Red Wolf of Radagon"] = new(DataManager.GetInstance().CardGlintPhalanx, "Glintblade Phalanx", typeof(GlintbladePhalanxCard)),
            ["Rennala, Queen of the Full Moon"] = null,
            ["Rennala, Queen of the Full Moon (2nd phase)"] = new(DataManager.GetInstance().CardCometAzur, "Comet Azur", typeof(CometAzurCard)),
            ["Royal Knight Loretta"] = new(DataManager.GetInstance().CardCarianGSword, "Carian Greatsword", typeof(CarianGreatSwordCard)),
            ["Mimic Tear"] = new(DataManager.GetInstance().MimicTearAsh, "Mimic Tear", typeof(MimicTearAsh)),
            ["Starscourge Radahn"] = null,
            ["Starscourge Radahn (2nd phase)"] = new(DataManager.GetInstance().CardStarcallerCry, "Starcaller Cry", typeof(StarcallerCryCard)),
            ["Bloody Finger Hunter Yura"] = new(DataManager.GetInstance().CardUnsheathe, "Unsheathe", typeof(UnsheatheCard)),
            ["Morgott, The Omen King"] = new(DataManager.GetInstance().CardWarmingStone, "Warming Stone", typeof(WarmingStoneCard)),
            ["Dung Eater"] = null,  // TODO: Add reward
            ["Mohg, Lord of Blood"] = new(DataManager.GetInstance().CardBloodboon, "Bloodboon Ritual", typeof(BloodboonRitualCard)),
            ["Commander Niall"] = new(DataManager.GetInstance().CardRallyingStandard, "Rallying Standard", typeof(RallyingStandardCard)),
            ["Malenia, Blade of Miquella"] = null,
            ["Malenia, Goddess of Rot"] = new(DataManager.GetInstance().CardWaterfowlDance, "Waterfowl Dance", typeof(WaterfowlDanceCard)),
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
                // TODO: Make sure to copy the deck of the player
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
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["White Mask Varré"], _decks["White Mask Varré"]),
                Unlocked = true
            },
            
            // Tree sentinel
            new Level
            {
                Enemy = _enemies["Tree Sentinel"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Tree Sentinel"],
                SoundTrack = DataManager.GetInstance().TreeSentinelSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Tree Sentinel"], _decks["Tree Sentinel"])
            },
            
            // Roderika
            new Level
            {
                Enemy = _enemies["Roderika"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Roderika"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Roderika"], _decks["Roderika"])
            },
            
            // Renna
            new Level
            {
                Enemy = _enemies["Renna"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Renna"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Renna"], _decks["Renna"])
            },
            
            // Blaidd, The Half-Wolf
            new Level
            {
                Enemy = _enemies["Blaidd, The Half-Wolf"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Blaidd, The Half-Wolf"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Blaidd, The Half-Wolf"], _decks["Blaidd, The Half-Wolf"])
            },
            
            // Sorceress Sellen
            new Level
            {
                Enemy = _enemies["Sorceress Sellen"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Sorceress Sellen"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Sorceress Sellen"], _decks["Sorceress Sellen"])
            },
            
            // Thops
            new Level
            {
                Enemy = _enemies["Thops"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Thops"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Thops"], _decks["Thops"])
            },
            
            // Fia, The Deathbed Companion
            new Level
            {
                Enemy = _enemies["Fia, The Deathbed Companion"],
                Backdrop = DataManager.GetInstance().RoundtableHoldBackdrop,
                LevelReward = _rewards["Fia, The Deathbed Companion"],
                SoundTrack = DataManager.GetInstance().RoundtableSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Fia, The Deathbed Companion"], _decks["Fia, The Deathbed Companion"])
            },
            
            // Gatekeeper Gostoc
            new Level
            {
                Enemy = _enemies["Gatekeeper Gostoc"],
                Backdrop = DataManager.GetInstance().LimgraveBackdrop,
                LevelReward = _rewards["Gatekeeper Gostoc"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Gatekeeper Gostoc"], _decks["Gatekeeper Gostoc"])
            },
            
            // Margit the Fell Omen
            new Level
            {
                Enemy = _enemies["Margit, The Fell Omen"],
                Backdrop = DataManager.GetInstance().StormveilBackdrop,
                LevelReward = _rewards["Margit, The Fell Omen"],
                SoundTrack = DataManager.GetInstance().MargitSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Margit, The Fell Omen"], _decks["Margit, The Fell Omen"])
            },
            
            // Godrick the Grafted
            new Level
            {
                Enemy = _enemies["Godrick the Grafted"],
                Backdrop = DataManager.GetInstance().StormveilBackdrop,
                LevelReward = _rewards["Godrick the Grafted"],
                SoundTrack = DataManager.GetInstance().GodrickP1Soundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Godrick the Grafted"], _decks["Godrick the Grafted"]),
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
                    EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Godrick the Grafted "], _decks["Godrick the Grafted (2nd phase)"]),
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
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),  // TODO: Nokron music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Mimic Tear"], _decks["Mimic Tear"])
            },
            
            // Royal Knight Loretta
            new Level
            {
                Enemy = _enemies["Royal Knight Loretta"],
                Backdrop = DataManager.GetInstance().LiurniaBackdrop,
                LevelReward = _rewards["Royal Knight Loretta"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),  // TODO: Liurnia music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Royal Knight Loretta"], _decks["Royal Knight Loretta"])
            },
            
            // Red Wolf of Radagon
            new Level
            {
                Enemy = _enemies["Red Wolf of Radagon"],
                Backdrop = DataManager.GetInstance().RayaLucariaBackdrop,
                LevelReward = _rewards["Red Wolf of Radagon"],
                SoundTrack = DataManager.GetInstance().RedWolfSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Red Wolf of Radagon"], _decks["Red Wolf of Radagon"])
            },
            
            // Starscourge Radahn
            new Level
            {
                Enemy = _enemies["Starscourge Radahn"],
                Backdrop = DataManager.GetInstance().RadahnBattlefieldBackdrop,
                LevelReward = _rewards["Starscourge Radahn"],
                SoundTrack = DataManager.GetInstance().StarscourgeRadahnP1Soundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Starscourge Radahn"], _decks["Starscourge Radahn"]),
                SecondPhase = new Level
                {
                    Enemy = _enemies["Starscourge Radahn "],
                    Backdrop = DataManager.GetInstance().RadahnBattlefieldPhase2Backdrop,
                    LevelReward = _rewards["Starscourge Radahn (2nd phase)"],
                    SoundTrack = DataManager.GetInstance().StarscourgeRadahnP2Soundtrack.CreateInstance(),
                    EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Starscourge Radahn "], _decks["Starscourge Radahn (2nd phase)"])
                }
            },
            
            // Rennala, Queen of the Full Moon
            new Level
            {
                Enemy = _enemies["Rennala, Queen of the Full Moon"],
                Backdrop = DataManager.GetInstance().RayaLucariaBackdrop,
                LevelReward = _rewards["Rennala, Queen of the Full Moon"],
                SoundTrack = DataManager.GetInstance().RennalaP1Soundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Rennala, Queen of the Full Moon"], _decks["Rennala, Queen of the Full Moon"]),
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
                    EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Rennala, Queen of the Full Moon "], _decks["Rennala, Queen of the Full Moon (2nd phase)"]),
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
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),  // TODO: Leyndell music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Bloody Finger Hunter Yura"], _decks["Bloody Finger Hunter Yura"])
            },
            
            // Sir Gideon Ofnir, The All-Knowing
            new Level
            {
                Enemy = _enemies["Sir Gideon Ofnir, The All-Knowing"],
                Backdrop = DataManager.GetInstance().LeyndellFireBackdrop,
                LevelReward = _rewards["Sir Gideon Ofnir, The All-Knowing"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),  // TODO: Leyndell music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Sir Gideon Ofnir, The All-Knowing"], _decks["Sir Gideon Ofnir, The All-Knowing"])
            },
            
            // Dung Eater
            new Level
            {
                Enemy = _enemies["Dung Eater"],
                Backdrop = DataManager.GetInstance().AltusPlateauBackdrop,
                LevelReward = _rewards["Dung Eater"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),  // TODO: Leyndell music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Dung Eater"], _decks["Dung Eater"])
            },
            
            // Commander Niall
            new Level
            {
                Enemy = _enemies["Commander Niall"],
                Backdrop = DataManager.GetInstance().CastleSolBackdrop,
                LevelReward = _rewards["Commander Niall"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),  // TODO: Niall fight music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Commander Niall"], _decks["Commander Niall"])
            },
            
            // Mohg, Lord of Blood
            new Level
            {
                Enemy = _enemies["Mohg, Lord of Blood"],
                Backdrop = DataManager.GetInstance().MohgBackdrop,
                LevelReward = _rewards["Mohg, Lord of Blood"],
                SoundTrack = DataManager.GetInstance().MohgSoundtrack.CreateInstance(),
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Mohg, Lord of Blood"], _decks["Mohg, Lord of Blood"]),
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
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Malenia, Blade of Miquella"], _decks["Malenia, Blade of Miquella"]),
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
                    EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Malenia, Goddess of Rot"], _decks["Malenia, Goddess of Rot"]),
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
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Morgott, The Omen King"], _decks["Morgott, The Omen King"])
            },
            
            // Maliketh, the Black Blade
            new Level
            {
                Enemy = _enemies["Maliketh, the Black Blade"],
                Backdrop = DataManager.GetInstance().FarumAzulaBackdrop, 
                LevelReward = _rewards["Maliketh, the Black Blade"],
                SoundTrack = DataManager.GetInstance().MalikethSoundtrack.CreateInstance(),  
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Maliketh, the Black Blade"], _decks["Maliketh, the Black Blade"]),
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
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Godfrey, The First Elden Lord"], _decks["Godfrey, The First Elden Lord"]),
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
                    EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Hoarah Loux, Warrior"], _decks["Hoarah Loux, Warrior"]),
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
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Radagon of the Golden Order"], _decks["Radagon of the Golden Order"]),
                SecondPhase = new Level     // Elden Beast
                {
                    Enemy = _enemies["Elden Beast"],
                    Backdrop = DataManager.GetInstance().InsideErdtreeBackdrop, 
                    LevelReward = _rewards["Elden Beast"],
                    SoundTrack = DataManager.GetInstance().EldenBeastSoundtrack.CreateInstance(),
                    EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Elden Beast"], _decks["Elden Beast"])
                }
            },
            
            // Tarnished, Consort of the Stars
            new Level
            {
                Enemy = _enemies["Tarnished, Consort of the Stars"],
                Backdrop = DataManager.GetInstance().DarkMoonBackdrop, 
                LevelReward = _rewards["Tarnished, Consort of the Stars"],
                SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),  // TODO: Come up with proper music
                EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Tarnished, Consort of the Stars"], _decks["Tarnished, Consort of the Stars"]),
                SecondPhase = new Level     // Ranni, Queen of the Dark Moon
                {
                    Enemy = _enemies["Ranni, Queen of the Dark Moon"],
                    Backdrop = DataManager.GetInstance().DarkMoonBackdrop, 
                    LevelReward = _rewards["Ranni, Queen of the Dark Moon"],
                    SoundTrack = DataManager.GetInstance().LimgraveSoundtrack.CreateInstance(),  // TODO: Come up with proper music
                    EnemyDeck = GenerateDeck(Game.GetGameState(), _enemies["Ranni, Queen of the Dark Moon"], _decks["Ranni, Queen of the Dark Moon"])
                }
            },
        };
    }
}