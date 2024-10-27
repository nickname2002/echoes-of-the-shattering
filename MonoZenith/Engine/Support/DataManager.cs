
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
        
        // Textures
        // Textures: MonoZenith
        public Texture2D MonoZenithLogo;

        // Textures: Main Menu
        public Texture2D MainMenuBackdrop;
        public Texture2D MainMenuHoverIndicator;

        // Textures: Game UI
        public Texture2D Backdrop;
        public Texture2D Player;
        public Texture2D Npc;
        public Texture2D PlayerCurrent;
        public Texture2D PlayerWaiting;
        public Texture2D DeckIndicator;
        public Texture2D ReserveIndicator;
        public Texture2D MimicTearIndicatorDisabled;

        // Textures: Card 
        public Texture2D CardBack;
        public Texture2D CardFront;
        public Texture2D CardHidden;
        
        // Textures: EndTurnButton
        public Texture2D EndTurnButtonIdleTexture;
        public Texture2D EndTurnButtonHoverTexture;
        public Texture2D EndTurnButtonDisabledTexture;
        
        // Audio
        
        /// <summary>
        /// Credits: Arcane Bard Audio - https://www.youtube.com/watch?v=WNm0TaVTGWo
        /// </summary>
        public SoundEffectInstance MainMenuMusic;

        /// <summary>
        /// Credits: Gooley's Tunes - https://www.youtube.com/watch?v=2FZKge-T3oA
        /// </summary>
        public SoundEffectInstance LimgraveMusic;

        // Audio: Main Menu
        public SoundEffectInstance StartButtonSound;

        // Audio: Game
        public SoundEffectInstance NewLocationSound;
        public SoundEffectInstance DamageSound;
        public SoundEffectInstance HealingSound;
        public SoundEffectInstance LightSwordAttack;
        public SoundEffectInstance CardSound2;
        public SoundEffectInstance HeavySwordAttack;
        public SoundEffectInstance GlintStonePebble;
        public SoundEffectInstance FlaskOfCrimsonTears;
        public SoundEffectInstance FlaskOfCeruleanTears;
        public SoundEffectInstance EndTurnSound;
        public SoundEffectInstance PlayerDeathSound;
        public SoundEffectInstance EnemyDeathSound;
        
        private DataManager(Game game)
        {
            _game = game;
            LoadData();
        }

        /// <summary>
        /// Get the instance of the DataManager.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <returns>The DataManager instance.</returns>
        public static DataManager GetInstance(Game game)
        {
            return _instance ??= new DataManager(game);
        }
        
        /// <summary>
        /// Load all data.
        /// </summary>
        private void LoadData()
        {
            // Load fonts
            ComponentFont = _game.LoadFont("Fonts/pixel.ttf", 1);
            StartMenuFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.75f);
            PlayerFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.5f);
            CardFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1f);
            IndicatorFont = _game.LoadFont("Fonts/Garamond/EBGaramond-Regular.ttf", 1.25f);
            
            // Load textures
            MonoZenithLogo = _game.LoadImage("Images/monozenith.png");
            MainMenuBackdrop = _game.LoadImage("Images/MainMenu/main-menu-backdrop.png");
            MainMenuHoverIndicator = _game.LoadImage("Images/MainMenu/menu-item-indicator.png");
            Player = _game.LoadImage("Images/Player/vargram.png");
            Npc = _game.LoadImage("Images/Player/varre.png");
            PlayerCurrent = _game.LoadImage("Images/Player/player-current.png");
            PlayerWaiting = _game.LoadImage("Images/Player/player-waiting.png");
            Backdrop = _game.LoadImage("Images/Backdrops/backdrop.png");
            CardBack = _game.LoadImage("Images/Cards/back-card-design.png");
            CardFront = _game.LoadImage("Images/Cards/front-card-design.png");
            CardHidden = _game.LoadImage("Images/Cards/card-hidden.png");
            EndTurnButtonIdleTexture = _game.LoadImage("Images/EndTurnButton/end-turn-button-idle.png");
            EndTurnButtonHoverTexture = _game.LoadImage("Images/EndTurnButton/end-turn-button-hover.png");
            EndTurnButtonDisabledTexture = _game.LoadImage("Images/EndTurnButton/end-turn-button-disabled.png");
            DeckIndicator = _game.LoadImage("Images/Indicators/deck-indicator.png");
            ReserveIndicator = _game.LoadImage("Images/Indicators/reserve-indicator.png");
            MimicTearIndicatorDisabled = _game.LoadImage("Images/Indicators/mimic-tear-indicator-disabled.png");
            
            // Load audio
            MainMenuMusic = _game.LoadAudio("Audio/Music/main-menu-music.wav");
            NewLocationSound = _game.LoadAudio("Audio/SoundEffects/new-location-sound.wav");
            LimgraveMusic = _game.LoadAudio("Audio/Music/limgrave-music.wav");
            StartButtonSound = _game.LoadAudio("Audio/SoundEffects/start-button-sound.wav");
            DamageSound = _game.LoadAudio("Audio/SoundEffects/damage-sound.wav");
            HealingSound = _game.LoadAudio("Audio/SoundEffects/healing-sound.wav");
            LightSwordAttack = _game.LoadAudio("Audio/SoundEffects/light-sword-attack.wav");
            CardSound2 = _game.LoadAudio("Audio/SoundEffects/card-sound2.wav");
            HeavySwordAttack = _game.LoadAudio("Audio/SoundEffects/heavy-sword-attack.wav");
            GlintStonePebble = _game.LoadAudio("Audio/SoundEffects/glintstone-pebble.wav");
            FlaskOfCrimsonTears = _game.LoadAudio("Audio/SoundEffects/flask-of-crimson-tears.wav");
            FlaskOfCeruleanTears = _game.LoadAudio("Audio/SoundEffects/flask-of-cerulean-tears.wav");
            EndTurnSound = _game.LoadAudio("Audio/SoundEffects/end-turn-sound-effect.wav");
            PlayerDeathSound = _game.LoadAudio("Audio/SoundEffects/player-death.wav");
            EnemyDeathSound = _game.LoadAudio("Audio/SoundEffects/enemy-felled.wav");
        }
    }
}