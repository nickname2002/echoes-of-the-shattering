using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Components.OverworldScreen;
using MonoZenith.Engine.Support;
using MonoZenith.Support;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Screen;

public class OverworldScreen : Screen
{
    private static readonly LevelManager LevelManager = new();
    
    /// <summary>
    /// NOTE: To view the region map, change the enum value by hand for now.
    /// TODO: Map this value to button in ECHOE-92
    /// </summary>
    private Region SelectedRegion = Region.AltusPlateau; 

    /// <summary>
    /// Region backdrops
    /// </summary>
    private readonly Texture2D _limgraveBackdrop = LoadImage("Images/OverworldScreen/Backdrops/limgrave-map.png");
    private readonly Texture2D _liurniaBackdrop = LoadImage("Images/OverworldScreen/Backdrops/liurnia-map.png");
    private readonly Texture2D _altusPlateauBackdrop = LoadImage("Images/OverworldScreen/Backdrops/altus-plateau-map.png");
    
    // Soundtrack and properties
    private readonly SoundEffectInstance _soundtrack = 
        LoadAudio("Audio/Music/OverworldScreen/overworld-background-music.wav").CreateInstance();
    private readonly SoundEffectInstance _enterBattleSound = 
        LoadAudio("Audio/SoundEffects/enter-battle.wav").CreateInstance();
    
    // TODO: Add battle enter sound on click grace button
    
    /// <summary>
    /// Site of grace containers
    /// </summary>
    private readonly List<SiteOfGraceButton> _limgraveGraceButtons = new List<SiteOfGraceButton>()
    {
        // White Mask Varré
        new()
        {
            Position = new Vector2(989, 745),
            Level = LevelManager.GetLevelFromEnemy("White Mask Varré")
        },

        // Tree Sentinel
        new()
        {
            Position = new Vector2(1030, 602),
            Level = LevelManager.GetLevelFromEnemy("Tree Sentinel")
        },

        // Roderika
        new()
        {
            Position = new Vector2(989, 460),
            Level = LevelManager.GetLevelFromEnemy("Roderika")
        },

        // Renna
        new()
        {
            Position = new Vector2(964, 343),
            Level = LevelManager.GetLevelFromEnemy("Renna")
        },

        // Blaidd, The Half-Wolf
        new()
        {
            Position = new Vector2(1077, 295),
            Level = LevelManager.GetLevelFromEnemy("Blaidd, The Half-Wolf")
        },

        // Sorceress Sellen
        new()
        {
            Position = new Vector2(983, 189),
            Level = LevelManager.GetLevelFromEnemy("Sorceress Sellen")
        },

        // Thops
        new()
        {
            Position = new Vector2(811, 231),
            Level = LevelManager.GetLevelFromEnemy("Thops")
        },

        // Fia, The Deathbed Companion
        new()
        {
            Position = new Vector2(871, 101),
            Level = LevelManager.GetLevelFromEnemy("Fia, The Deathbed Companion")
        },

        // Gatekeeper Gostoc
        new()
        {
            Position = new Vector2(740, 131),
            Level = LevelManager.GetLevelFromEnemy("Gatekeeper Gostoc")
        },

        // Margit the Fell Omen
        new()
        {
            Position = new Vector2(588, 173),
            Level = LevelManager.GetLevelFromEnemy("Margit, The Fell Omen")
        },

        // Godrick the Grafted
        new()
        {
            Position = new Vector2(527, 60),
            Level = LevelManager.GetLevelFromEnemy("Godrick the Grafted")
        }
    };

    private readonly List<SiteOfGraceButton> _liurniaGraceButtons = new List<SiteOfGraceButton>
    {
        // Mimic tear
        new()
        {
            Position = new Vector2(835, 657),
            Level = LevelManager.GetLevelFromEnemy("Mimic Tear")
        },
        
        // Royal Knight Loretta
        new()
        {
            Position = new Vector2(734, 527),
            Level = LevelManager.GetLevelFromEnemy("Royal Knight Loretta")
        },
        
        // Red Wolf of Radagon
        new()
        {
            Position = new Vector2(663, 362),
            Level = LevelManager.GetLevelFromEnemy("Red Wolf of Radagon")
        },
        
        // Starscourge Radahn
        new()
        {
            Position = new Vector2(439, 362),
            Level = LevelManager.GetLevelFromEnemy("Starscourge Radahn")
        },
        
        // Rennala, Queen of the Full Moon
        new()
        {
            Position = new Vector2(495, 198),
            Level = LevelManager.GetLevelFromEnemy("Rennala, Queen of the Full Moon")
        }
    };

    private readonly List<SiteOfGraceButton> _altusPlateauGraceButtons = new List<SiteOfGraceButton>()
    {
        // Bloody Finger Hunter Yura
        new()
        {
            Position = new Vector2(353, 781),
            Level = LevelManager.GetLevelFromEnemy("Bloody Finger Hunter Yura")
        },
        
        // Sir Gideon Ofnir
        new()
        {
            Position = new Vector2(267, 694),
            Level = LevelManager.GetLevelFromEnemy("Sir Gideon Ofnir, The All-Knowing")
        },
        
        // Dung Eater
        new()
        {
            Position = new Vector2(273, 557),
            Level = LevelManager.GetLevelFromEnemy("Dung Eater")
        },
        
        // Commander Niall
        new()
        {
            Position = new Vector2(376, 469),
            Level = LevelManager.GetLevelFromEnemy("Commander Niall")
        },
        
        // Mohg, Lord of Blood
        new()
        {
            Position = new Vector2(529, 444),
            Level = LevelManager.GetLevelFromEnemy("Mohg, Lord of Blood")
        },

        // Malenia, Blade of Miquella
        new()
        {
            Position = new Vector2(649, 391),
            Level = LevelManager.GetLevelFromEnemy("Malenia, Blade of Miquella")
        },

        // Morgott, The Omen King
        new()
        {
            Position = new Vector2(673, 518),
            Level = LevelManager.GetLevelFromEnemy("Morgott, The Omen King")
        },
        
        // Godfrey, The First Elden Lord
        new()
        {
            Position = new Vector2(888, 596),
            Level = LevelManager.GetLevelFromEnemy("Godfrey, The First Elden Lord")
        },
        
        // Radagon of the Golden Order
        new()
        {
            Position = new Vector2(1022, 685),
            Level = LevelManager.GetLevelFromEnemy("Radagon of the Golden Order")
        },
        
        // Tarnished, Consort of the Stars
        new()
        {
            Position = new Vector2(1119, 773),
            Level = LevelManager.GetLevelFromEnemy("Tarnished, Consort of the Stars")
        }
    };

    public override void Unload(float fadeSpeed = 0.015f, Action unOnloadComplete = null)
    {
        float musicFadeOutSpeed = fadeSpeed;
        if (_soundtrack != null && _soundtrack.Volume >= musicFadeOutSpeed)
        {
            _soundtrack.Volume -= musicFadeOutSpeed;
        }
        else
        {
            _soundtrack?.Stop();
            unOnloadComplete?.Invoke();
        }
    }

    public override void Load()
    {
        StartFadeIn();
        
        float musicFadeInSpeed = 0.015f;
        if (_soundtrack.Volume <= 0.5f - musicFadeInSpeed)
        {
            _soundtrack.Volume += musicFadeInSpeed;
        }
        else
        {
            _soundtrack.Volume = 0.5f;
        }
    }

    /// <summary>
    /// Activate the selected level
    /// </summary>
    private void ActivateSelectedLevel()
    {
        LevelManager.CurrentLevel.Initialize(GetGameState());
        GetGameScreen().SetBackgroundMusic(LevelManager.CurrentLevel.SoundTrack);
        GetGameState().SetLevel(LevelManager.CurrentLevel);
        ToGameScreen();
    }

    private void TrackRegion(List<SiteOfGraceButton> buttonsFromRegion)
    {
        if (!GetMouseButtonDown(MouseButtons.Left)) return;
        foreach (var grace in buttonsFromRegion.Where(grace => grace.IsClicked()))
        {
            _enterBattleSound.Play();
            LevelManager.CurrentLevel = grace.Level;
            ActivateSelectedLevel();
            return;
        }
    }
    
    public override void Update(GameTime deltaTime)
    {
        if (_soundtrack != null && 
            !(IsFadingOut || IsFadingIn))
        {
            _soundtrack.Volume = 0.5f;
            if (_soundtrack.State != SoundState.Playing)
                _soundtrack.Play();
        }
        
        switch (SelectedRegion)
        {
            case Region.Limgrave:
                TrackRegion(_limgraveGraceButtons);
                break;
            case Region.Liurnia:
                TrackRegion(_liurniaGraceButtons);
                break;
            case Region.AltusPlateau:
                TrackRegion(_altusPlateauGraceButtons);
                break;
            case Region.None:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public override void Draw()
    {
        switch (SelectedRegion)
        {
            case Region.Limgrave:
                DrawImage(_limgraveBackdrop, Vector2.Zero, AppSettings.Scaling.ScaleFactor);
                foreach (var grace in _limgraveGraceButtons) { grace.Draw(); }
                break;
            case Region.Liurnia:
                DrawImage(_liurniaBackdrop, Vector2.Zero, AppSettings.Scaling.ScaleFactor);
                foreach (var grace in _liurniaGraceButtons) { grace.Draw(); }
                break;
            case Region.AltusPlateau:
                DrawImage(_altusPlateauBackdrop, Vector2.Zero, AppSettings.Scaling.ScaleFactor);
                foreach (var grace in _altusPlateauGraceButtons) { grace.Draw(); }
                break;
            case Region.None:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}