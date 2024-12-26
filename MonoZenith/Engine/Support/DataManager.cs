using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using static MonoZenith.Game;

namespace MonoZenith.Engine.Support
{
    public class DataManager
    {
        private static DataManager _instance;
        
        // Fonts
        public SpriteFont ComponentFont;
        public SpriteFont StartMenuFont;
        public SpriteFont PlayerFont;
        public SpriteFont CardFont;
        public SpriteFont IndicatorFont;
        public SpriteFont TransitionComponentFont;
        public SpriteFont GameOverTransitionComponentFont;
        public SpriteFont RewardFont;
        
        // Textures
        public Texture2D MonoZenithLogo;
        public Texture2D MainMenuBackdrop;
        public Texture2D MainMenuHoverIndicator;
        public Texture2D PlayerCurrent;
        public Texture2D PlayerWaiting;
        public Texture2D DeckIndicator;
        public Texture2D ReserveIndicator;
        
        // Player icons
        public Texture2D Player;
        public Texture2D DefaultEnemyPortrait;
        
        // Backdrops
        public Texture2D LiurniaBackdrop;
        public Texture2D LimgraveBackdrop;
        public Texture2D CaelidBackdrop;
        public Texture2D CastleSolBackdrop;
        public Texture2D DarkMoonBackdrop;
        public Texture2D InsideErdtreeBackdrop;
        public Texture2D LeyndellBackdrop;
        public Texture2D LeyndellFireBackdrop;
        public Texture2D MaleniaRestplaceBackdrop;
        public Texture2D MohgBackdrop;
        public Texture2D NokronBackdrop;
        public Texture2D RayaLucariaBackdrop;
        public Texture2D RoundtableHoldBackdrop;
        public Texture2D StormveilBackdrop;
        public Texture2D AltusPlateauBackdrop;
        
        // Remembrances
        public Texture2D RemembranceOfTheStarscourge;
        
        // Indicators
        public Texture2D AshIndicatorDisabled;
        public Texture2D AshIndicatorEnabled;
        public Texture2D AshIndicatorHovered;
        
        // Spirit ashes
        public Texture2D MimicTearAsh;
        public Texture2D JellyfishAsh;
        public Texture2D WolvesAsh;
        
        // Reward panel
        public Texture2D RewardContainer;
        public Texture2D CollectRewardButton;
        public Texture2D CollectRewardButtonHover;
        
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
        public SoundEffect StartButtonSound;
        public SoundEffect PlayerTurnSound;
        public SoundEffect EndPlayerTurnSound;
        public SoundEffect PlayerDeathSound;
        public SoundEffect EnemyDeathSound;
        public SoundEffect RetrieveCardsSound;
        public SoundEffect SpiritAshSummonSound;
        public SoundEffect ThrowingDaggerSound;
        public SoundEffect NewItemSound; 
        
        // Sound tracks
        public SoundEffect LimgraveSoundtrack;
        public SoundEffect MohgSoundtrack;
        public SoundEffect MaleniaSoundtrack;
        public SoundEffect GoddessOfRotSoundtrack;

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

        private DataManager()
        {
            LoadData();
        }

        public static DataManager GetInstance()
        {
            return _instance ??= new DataManager();
        }

        private void LoadFonts()
        {
            ComponentFont = LoadFont("Fonts/pixel.ttf", 1 * AppSettings.Scaling.ScaleFactor);
            StartMenuFont = LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.75f * AppSettings.Scaling.ScaleFactor);
            PlayerFont = LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.1f * AppSettings.Scaling.ScaleFactor);
            CardFont = LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1f * AppSettings.Scaling.ScaleFactor);
            IndicatorFont = LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.25f * AppSettings.Scaling.ScaleFactor);
            TransitionComponentFont = LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 3.5f * AppSettings.Scaling.ScaleFactor);
            GameOverTransitionComponentFont = LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 5f * AppSettings.Scaling.ScaleFactor);
            RewardFont = LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.3f * AppSettings.Scaling.ScaleFactor);
        }

        private void LoadTextures()
        {
            MonoZenithLogo = LoadImage("Images/monozenith.png");
            MainMenuBackdrop = LoadImage("Images/MainMenu/main-menu-backdrop.png");
            MainMenuHoverIndicator = LoadImage("Images/MainMenu/menu-item-indicator.png");
            PlayerCurrent = LoadImage("Images/Player/player-current.png");
            PlayerWaiting = LoadImage("Images/Player/player-waiting.png");
            EndTurnButtonIdleTexture = LoadImage("Images/EndTurnButton/end-turn-button-idle.png");
            EndTurnButtonHoverTexture = LoadImage("Images/EndTurnButton/end-turn-button-hover.png");
            EndTurnButtonDisabledTexture = LoadImage("Images/EndTurnButton/end-turn-button-disabled.png");
            DeckIndicator = LoadImage("Images/Indicators/deck-indicator.png");
            ReserveIndicator = LoadImage("Images/Indicators/reserve-indicator.png");
            
            // Player icons
            Player = LoadImage("Images/Player/Tarnished.png");
            DefaultEnemyPortrait = LoadImage("Images/Player/White Mask Varré.png");
            
            // Backdrops
            LiurniaBackdrop = LoadImage("Images/Backdrops/backdrop.png");
            LimgraveBackdrop = LoadImage("Images/Backdrops/limgrave-backdrop.png");
            CaelidBackdrop = LoadImage("Images/Backdrops/caelid-backdrop.png");
            CastleSolBackdrop = LoadImage("Images/Backdrops/castle-sol-backdrop.png");
            DarkMoonBackdrop = LoadImage("Images/Backdrops/dark-moon-backdrop.png");
            InsideErdtreeBackdrop = LoadImage("Images/Backdrops/inside-erdtree-backdrop.png");
            LeyndellBackdrop = LoadImage("Images/Backdrops/leyndell-backdrop.png");
            LeyndellFireBackdrop = LoadImage("Images/Backdrops/leyndell-fire-backdrop.png");
            MaleniaRestplaceBackdrop = LoadImage("Images/Backdrops/malenia-restplace-backdrop.png");
            MohgBackdrop = LoadImage("Images/Backdrops/mohg-backdrop.png");
            NokronBackdrop = LoadImage("Images/Backdrops/nokron-backdrop.png");
            RayaLucariaBackdrop = LoadImage("Images/Backdrops/raya-lucaria-backdrop.png");
            RoundtableHoldBackdrop = LoadImage("Images/Backdrops/roundtable-hold-backdrop.png");
            StormveilBackdrop = LoadImage("Images/Backdrops/stormveil-backdrop.png");
            AltusPlateauBackdrop = LoadImage("Images/Backdrops/altus-plateau-backdrop.png");
            
            // Remembrances
            RemembranceOfTheStarscourge = LoadImage("Images/Remembrances/remembrance-of-starscourge.png");
            
            // Ash indicators
            AshIndicatorDisabled = LoadImage("Images/Indicators/ash-indicator-disabled.png");
            AshIndicatorEnabled = LoadImage("Images/Indicators/ash-indicator-enabled.png");
            AshIndicatorHovered = LoadImage("Images/Indicators/ash-indicator-hovered.png");
            
            // Spirit ashes
            MimicTearAsh = LoadImage("Images/Indicators/SpiritAshes/mimic-tear.png");
            JellyfishAsh = LoadImage("Images/Indicators/SpiritAshes/jellyfish.png");
            WolvesAsh = LoadImage("Images/Indicators/SpiritAshes/wolves.png");
            
            // Reward panel
            RewardContainer = LoadImage("Images/RewardPanel/reward-container.png");
            CollectRewardButton = LoadImage("Images/RewardPanel/collect-button.png");
            CollectRewardButtonHover = LoadImage("Images/RewardPanel/collect-button-hover.png");
            
            CardBack = LoadImage("Images/Cards/back-card-design.png");
            CardFront = LoadImage("Images/Cards/front-card-design.png");
            CardHidden = LoadImage("Images/Cards/card-hidden.png");
            CardCostStamina = LoadImage("Images/Cards/cost-stamina.png");
            CardCostFocus = LoadImage("Images/Cards/cost-focus.png");

            CardLightAttack = LoadImage("Images/Cards/card-light-attack.png");
            CardHeavyAttack = LoadImage("Images/Cards/card-heavy-attack.png");
            CardUnsheathe = LoadImage("Images/Cards/card-unsheathe.png");
            CardBloodhound = LoadImage("Images/Cards/card-bloodhound.png");
            CardEndure = LoadImage("Images/Cards/card-endure.png");
            CardDoubleSlash = LoadImage("Images/Cards/card-double.png");
            CardStormcaller = LoadImage("Images/Cards/card-stormcall.png");
            CardQuickstep = LoadImage("Images/Cards/card-quickstep.png");
            CardWarCry = LoadImage("Images/Cards/card-war-cry.png");
            CardRallyingStandard = LoadImage("Images/Cards/card-rallying.png");
            CardCommandKneel = LoadImage("Images/Cards/card-kneel.png");
            CardWaterfowlDance = LoadImage("Images/Cards/card-waterfowl.png");

            CardGlintPebble = LoadImage("Images/Cards/card-glint-peb.png");
            CardGlintPhalanx = LoadImage("Images/Cards/card-phalanx.png");
            CardCarianGSword = LoadImage("Images/Cards/card-carian-gsword.png");
            CardThopsBarrier = LoadImage("Images/Cards/card-thops.png");
            CardGreatShard = LoadImage("Images/Cards/card-great-glint.png");
            CardCometAzur = LoadImage("Images/Cards/card-comet-azur.png");

            CardFlaskCrimson = LoadImage("Images/Cards/card-crimson.png");
            CardFlaskCerulean = LoadImage("Images/Cards/card-cerulean.png");
            CardWondrousPhysick = LoadImage("Images/Cards/card-wondrous.png");
            CardBaldachinBless = LoadImage("Images/Cards/card-baldachin.png");
            CardLarvalTear = LoadImage("Images/Cards/card-larval.png");
            CardWarmingStone = LoadImage("Images/Cards/card-warming.png");
            CardPoisonPot = LoadImage("Images/Cards/card-poison-pot.png");
            CardThrowingDagger = LoadImage("Images/Cards/card-throw-dagger.png");
        }
        
        private void LoadSoundEffects()
        {
            MainMenuMusic = LoadAudio("Audio/Music/main-menu-music.wav");
            PlayerTurnSound = LoadAudio("Audio/SoundEffects/player-turn-sound.wav");
            StartButtonSound = LoadAudio("Audio/SoundEffects/start-button-sound.wav");
            EndPlayerTurnSound = LoadAudio("Audio/SoundEffects/end-turn-sound-effect.wav");
            PlayerDeathSound = LoadAudio("Audio/SoundEffects/player-death.wav");
            EnemyDeathSound = LoadAudio("Audio/SoundEffects/enemy-felled.wav");
            RetrieveCardsSound = LoadAudio("Audio/SoundEffects/retrieve-cards.wav");
            SpiritAshSummonSound = LoadAudio("Audio/SoundEffects/spirit-ash-summon.wav");
            NewItemSound = LoadAudio("Audio/SoundEffects/new-item.wav");
            
            LimgraveSoundtrack = LoadAudio("Audio/Music/limgrave-music.wav");
            MohgSoundtrack = LoadAudio("Audio/Music/mohg-battle-soundtrack.wav");
            MaleniaSoundtrack = LoadAudio("Audio/Music/malenia-battle-soundtrack.wav");
            GoddessOfRotSoundtrack = LoadAudio("Audio/Music/goddess-of-rot-battle-soundtrack.wav");

            DamageSound = LoadAudio("Audio/SoundEffects/damage-sound.wav");
            HealingSound = LoadAudio("Audio/SoundEffects/healing-sound.wav");
            CardSound2 = LoadAudio("Audio/SoundEffects/card-sound2.wav");

            LightSwordSound = LoadAudio("Audio/SoundEffects/light-sword-attack.wav");
            HeavySwordSound = LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            UnsheatheSound = LoadAudio("Audio/SoundEffects/unsheathe.wav");
            BloodhoundSound = LoadAudio("Audio/SoundEffects/bloodhound.wav");
            EndureSound = LoadAudio("Audio/SoundEffects/endure.wav");
            DoubleSlashSound = LoadAudio("Audio/SoundEffects/double-slash.wav");
            StormcallerSound = LoadAudio("Audio/SoundEffects/stormcaller.wav");
            QuickstepSound = LoadAudio("Audio/SoundEffects/quickstep.wav");
            WarCrySound = LoadAudio("Audio/SoundEffects/warcry.wav");
            RallyingSound = LoadAudio("Audio/SoundEffects/rallying.wav");
            CommandKneelSound = LoadAudio("Audio/SoundEffects/command-kneel.wav");
            WaterfowlDanceSound = LoadAudio("Audio/SoundEffects/waterfowl.wav");

            GlintPebbleSound = LoadAudio("Audio/SoundEffects/glintstone-pebble.wav");
            GlintPhalanxSound = LoadAudio("Audio/SoundEffects/glint-phalanx.wav");
            CarianGSwordSound = LoadAudio("Audio/SoundEffects/cariang-sword.wav");
            ThopsBarrierSound = LoadAudio("Audio/SoundEffects/thops-barrier.wav");
            GreatShardSound = LoadAudio("Audio/SoundEffects/great-shard.wav");
            CometAzurSound = LoadAudio("Audio/SoundEffects/comet-azur.wav");

            FlaskCrimsonSound = LoadAudio("Audio/SoundEffects/flask-of-crimson-tears.wav");
            FlaskCeruleanSound = LoadAudio("Audio/SoundEffects/flask-of-cerulean-tears.wav");
            WondrousPhysickSound = LoadAudio("Audio/SoundEffects/wondrous-physick.wav");
            BaldachinBlessSound = LoadAudio("Audio/SoundEffects/baldachin-bless.wav");
            LarvalTearSound = LoadAudio("Audio/SoundEffects/larval-tear.wav");
            WarmingStoneSound = LoadAudio("Audio/SoundEffects/warming-stone.wav");
            PoisonPotSound = LoadAudio("Audio/SoundEffects/poison-pot.wav");
            ThrowingDaggerSound = LoadAudio("Audio/SoundEffects/throwing-dagger.wav");
        }
        
        public void LoadData()
        {
            LoadFonts();
            LoadTextures();
            LoadSoundEffects();
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