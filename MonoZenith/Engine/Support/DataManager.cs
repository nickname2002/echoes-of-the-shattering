using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Engine.Support
{
    public class DataManager
    {
        private readonly Game _game;
        private static DataManager _instance;
        
        // Fonts
        public SpriteFont ComponentFont;
        public SpriteFont StartMenuFont;
        public SpriteFont PlayerFont;
        public SpriteFont CardFont;
        public SpriteFont IndicatorFont;
        public SpriteFont TransitionComponentFont;
        public SpriteFont GameOverTransitionComponentFont;
        
        // Textures
        public Texture2D MonoZenithLogo;
        public Texture2D MainMenuBackdrop;
        public Texture2D MainMenuHoverIndicator;
        public Texture2D Backdrop;
        public Texture2D LimgraveBackdrop;
        public Texture2D Player;
        public Texture2D Npc;
        public Texture2D PlayerCurrent;
        public Texture2D PlayerWaiting;
        public Texture2D DeckIndicator;
        public Texture2D ReserveIndicator;
        
        public Texture2D AshIndicatorDisabled;
        public Texture2D AshIndicatorEnabled;
        public Texture2D AshIndicatorHovered;
        
        public Texture2D MimicTearAsh;
        public Texture2D JellyfishAsh;
        public Texture2D WolvesAsh;
        
        public Texture2D EndTurnButtonIdleTexture;
        public Texture2D EndTurnButtonHoverTexture;
        public Texture2D EndTurnButtonDisabledTexture;

        public Texture2D CardBack;
        public Texture2D CardFront;
        public Texture2D CardHidden;
        public Texture2D CardCostStamina;
        public Texture2D CardCostFocus;

        public Texture2D CardLightAttack;
        public Texture2D CardHeavyAttack;
        public Texture2D CardUnsheathe;
        public Texture2D CardBloodhound;
        public Texture2D CardEndure;
        public Texture2D CardDoubleSlash;
        public Texture2D CardStormcaller;
        public Texture2D CardQuickstep;
        public Texture2D CardWarCry;
        public Texture2D CardRallyingStandard;
        public Texture2D CardCommandKneel;
        public Texture2D CardWaterfowlDance;

        public Texture2D CardGlintPebble;
        public Texture2D CardGlintPhalanx;
        public Texture2D CardCarianGSword;
        public Texture2D CardThopsBarrier;
        public Texture2D CardGreatShard;
        public Texture2D CardCometAzur;

        public Texture2D CardFlaskCrimson;
        public Texture2D CardFlaskCerulean;
        public Texture2D CardWondrousPhysick;
        public Texture2D CardBaldachinBless;
        public Texture2D CardLarvalTear;
        public Texture2D CardWarmingStone;
        public Texture2D CardPoisonPot;
        public Texture2D CardThrowingDagger;

        // Audio
        public SoundEffect MainMenuMusic;
        public SoundEffect LimgraveMusic;
        public SoundEffect StartButtonSound;
        public SoundEffect PlayerTurnSound;
        public SoundEffect EndPlayerTurnSound;
        public SoundEffect PlayerDeathSound;
        public SoundEffect EnemyDeathSound;
        public SoundEffect RetrieveCardsSound;
        public SoundEffect SpiritAshSummonSound;

        public SoundEffect DamageSound;
        public SoundEffect CardSound2;
        public SoundEffect HealingSound;

        public SoundEffect LightSwordSound;
        public SoundEffect HeavySwordSound;
        public SoundEffect UnsheatheSound;
        public SoundEffect BloodhoundSound;
        public SoundEffect EndureSound;
        public SoundEffect DoubleSlashSound;
        public SoundEffect StormcallerSound;
        public SoundEffect QuickstepSound;
        public SoundEffect WarCrySound;
        public SoundEffect RallyingSound;
        public SoundEffect CommandKneelSound;
        public SoundEffect WaterfowlDanceSound;

        public SoundEffect GlintPebbleSound;
        public SoundEffect GlintPhalanxSound;
        public SoundEffect CarianGSwordSound;
        public SoundEffect ThopsBarrierSound;
        public SoundEffect GreatShardSound;
        public SoundEffect CometAzurSound;

        public SoundEffect FlaskCrimsonSound;
        public SoundEffect FlaskCeruleanSound;
        public SoundEffect WondrousPhysickSound;
        public SoundEffect BaldachinBlessSound;
        public SoundEffect LarvalTearSound;
        public SoundEffect WarmingStoneSound;
        public SoundEffect PoisonPotSound;
        public SoundEffect ThrowingDaggerSound;

        private DataManager(Game game)
        {
            _game = game;
            LoadData();
        }

        public static DataManager GetInstance(Game game)
        {
            return _instance ??= new DataManager(game);
        }
        
        public void LoadData()
        {
            // Load fonts
            ComponentFont = _game.LoadFont("Fonts/pixel.ttf", 1 * AppSettings.Scaling.ScaleFactor);
            StartMenuFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.75f * AppSettings.Scaling.ScaleFactor);
            PlayerFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.5f * AppSettings.Scaling.ScaleFactor);
            CardFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1f * AppSettings.Scaling.ScaleFactor);
            IndicatorFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.25f * AppSettings.Scaling.ScaleFactor);
            TransitionComponentFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 3.5f * AppSettings.Scaling.ScaleFactor);
            GameOverTransitionComponentFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 5f * AppSettings.Scaling.ScaleFactor);
            
            // Load textures
            MonoZenithLogo = _game.LoadImage("Images/monozenith.png");
            MainMenuBackdrop = _game.LoadImage("Images/MainMenu/main-menu-backdrop.png");
            MainMenuHoverIndicator = _game.LoadImage("Images/MainMenu/menu-item-indicator.png");
            Player = _game.LoadImage("Images/Player/vargram.png");
            Npc = _game.LoadImage("Images/Player/varre.png");
            PlayerCurrent = _game.LoadImage("Images/Player/player-current.png");
            PlayerWaiting = _game.LoadImage("Images/Player/player-waiting.png");
            Backdrop = _game.LoadImage("Images/Backdrops/backdrop.png");
            LimgraveBackdrop = _game.LoadImage("Images/Backdrops/limgrave-backdrop.png");
            EndTurnButtonIdleTexture = _game.LoadImage("Images/EndTurnButton/end-turn-button-idle.png");
            EndTurnButtonHoverTexture = _game.LoadImage("Images/EndTurnButton/end-turn-button-hover.png");
            EndTurnButtonDisabledTexture = _game.LoadImage("Images/EndTurnButton/end-turn-button-disabled.png");
            DeckIndicator = _game.LoadImage("Images/Indicators/deck-indicator.png");
            ReserveIndicator = _game.LoadImage("Images/Indicators/reserve-indicator.png");
            
            AshIndicatorDisabled = _game.LoadImage("Images/Indicators/ash-indicator-disabled.png");
            AshIndicatorEnabled = _game.LoadImage("Images/Indicators/ash-indicator-enabled.png");
            AshIndicatorHovered = _game.LoadImage("Images/Indicators/ash-indicator-hovered.png");
            
            MimicTearAsh = _game.LoadImage("Images/Indicators/SpiritAshes/mimic-tear.png");
            JellyfishAsh = _game.LoadImage("Images/Indicators/SpiritAshes/jellyfish.png");
            WolvesAsh = _game.LoadImage("Images/Indicators/SpiritAshes/wolves.png");
            
            CardBack = _game.LoadImage("Images/Cards/back-card-design.png");
            CardFront = _game.LoadImage("Images/Cards/front-card-design.png");
            CardHidden = _game.LoadImage("Images/Cards/card-hidden.png");
            CardCostStamina = _game.LoadImage("Images/Cards/cost-stamina.png");
            CardCostFocus = _game.LoadImage("Images/Cards/cost-focus.png");

            CardLightAttack = _game.LoadImage("Images/Cards/card-light-attack.png");
            CardHeavyAttack = _game.LoadImage("Images/Cards/card-heavy-attack.png");
            CardUnsheathe = _game.LoadImage("Images/Cards/card-unsheathe.png");
            CardBloodhound = _game.LoadImage("Images/Cards/card-bloodhound.png");
            CardEndure = _game.LoadImage("Images/Cards/card-endure.png");
            CardDoubleSlash = _game.LoadImage("Images/Cards/card-double.png");
            CardStormcaller = _game.LoadImage("Images/Cards/card-stormcall.png");
            CardQuickstep = _game.LoadImage("Images/Cards/card-quickstep.png");
            CardWarCry = _game.LoadImage("Images/Cards/card-war-cry.png");
            CardRallyingStandard = _game.LoadImage("Images/Cards/card-rallying.png");
            CardCommandKneel = _game.LoadImage("Images/Cards/card-kneel.png");
            CardWaterfowlDance = _game.LoadImage("Images/Cards/card-waterfowl.png");

            CardGlintPebble = _game.LoadImage("Images/Cards/card-glint-peb.png");
            CardGlintPhalanx = _game.LoadImage("Images/Cards/card-phalanx.png");
            CardCarianGSword = _game.LoadImage("Images/Cards/card-carian-gsword.png");
            CardThopsBarrier = _game.LoadImage("Images/Cards/card-thops.png");
            CardGreatShard = _game.LoadImage("Images/Cards/card-great-glint.png");
            CardCometAzur = _game.LoadImage("Images/Cards/card-comet-azur.png");

            CardFlaskCrimson = _game.LoadImage("Images/Cards/card-crimson.png");
            CardFlaskCerulean = _game.LoadImage("Images/Cards/card-cerulean.png");
            CardWondrousPhysick = _game.LoadImage("Images/Cards/card-wondrous.png");
            CardBaldachinBless = _game.LoadImage("Images/Cards/card-baldachin.png");
            CardLarvalTear = _game.LoadImage("Images/Cards/card-larval.png");
            CardWarmingStone = _game.LoadImage("Images/Cards/card-warming.png");
            CardPoisonPot = _game.LoadImage("Images/Cards/card-poison-pot.png");
            CardThrowingDagger = _game.LoadImage("Images/Cards/card-throw-dagger.png");

            // Load audio
            MainMenuMusic = _game.LoadAudio("Audio/Music/main-menu-music.wav");
            PlayerTurnSound = _game.LoadAudio("Audio/SoundEffects/player-turn-sound.wav");
            LimgraveMusic = _game.LoadAudio("Audio/Music/limgrave-music.wav");
            StartButtonSound = _game.LoadAudio("Audio/SoundEffects/start-button-sound.wav");
            EndPlayerTurnSound = _game.LoadAudio("Audio/SoundEffects/end-turn-sound-effect.wav");
            PlayerDeathSound = _game.LoadAudio("Audio/SoundEffects/player-death.wav");
            EnemyDeathSound = _game.LoadAudio("Audio/SoundEffects/enemy-felled.wav");
            RetrieveCardsSound = _game.LoadAudio("Audio/SoundEffects/retrieve-cards.wav");
            SpiritAshSummonSound = _game.LoadAudio("Audio/SoundEffects/spirit-ash-summon.wav");

            DamageSound = _game.LoadAudio("Audio/SoundEffects/damage-sound.wav");
            HealingSound = _game.LoadAudio("Audio/SoundEffects/healing-sound.wav");
            CardSound2 = _game.LoadAudio("Audio/SoundEffects/card-sound2.wav");

            LightSwordSound = _game.LoadAudio("Audio/SoundEffects/light-sword-attack.wav");
            HeavySwordSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            UnsheatheSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            BloodhoundSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            EndureSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            DoubleSlashSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            StormcallerSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            QuickstepSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            WarCrySound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            RallyingSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            CommandKneelSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            WaterfowlDanceSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");

            GlintPebbleSound = _game.LoadAudio("Audio/SoundEffects/glintstone-pebble.wav");
            GlintPhalanxSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            CarianGSwordSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            ThopsBarrierSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            GreatShardSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            CometAzurSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");

            FlaskCrimsonSound = _game.LoadAudio("Audio/SoundEffects/flask-of-crimson-tears.wav");
            FlaskCeruleanSound = _game.LoadAudio("Audio/SoundEffects/flask-of-cerulean-tears.wav");
            WondrousPhysickSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            BaldachinBlessSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            LarvalTearSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            WarmingStoneSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            PoisonPotSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            ThrowingDaggerSound = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
        }

        // Methode om een SoundEffectInstance te maken en af te spelen
        public SoundEffectInstance PlaySound(SoundEffect soundEffect)
        {
            SoundEffectInstance instance = soundEffect.CreateInstance();
            instance.Play();
            return instance;
        }
    }
}