
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
        
        // Textures
        public Texture2D MonoZenithLogo;
        public Texture2D MainMenuBackdrop;
        public Texture2D MainMenuHoverIndicator;
        public Texture2D Backdrop;
        public Texture2D CardBack;
        public Texture2D CardFront;
        public Texture2D GraceMenuBackdrop;
        public Texture2D LimgraveButtonTexture;
        public Texture2D LimgraveButtonHoverTexture;
        public Texture2D CaelidButtonTexture;
        public Texture2D CaelidButtonHoverTexture;
        public Texture2D LiurniaButtonTexture;
        public Texture2D LiurniaButtonHoverTexture;
        public Texture2D LeyndellTexture2D;
        public Texture2D LeyndellButtonHoverTexture;
        
        // Audio
        
        /// <summary>
        /// Credits: Arcane Bard Audio - https://www.youtube.com/watch?v=WNm0TaVTGWo
        /// </summary>
        public SoundEffectInstance MainMenuMusic;

        /// <summary>
        /// Credits: Gooley's Tunes - https://www.youtube.com/watch?v=2FZKge-T3oA
        /// </summary>
        public SoundEffectInstance LimgraveMusic;

        public SoundEffectInstance StartButtonSound;

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
            
            // Load textures
            MonoZenithLogo = _game.LoadImage("Images/monozenith.png");
            MainMenuBackdrop = _game.LoadImage("Images/MainMenu/main-menu-backdrop.png");
            MainMenuHoverIndicator = _game.LoadImage("Images/MainMenu/menu-item-indicator.png");
            Backdrop = _game.LoadImage("Images/Backdrops/backdrop.png");
            CardBack = _game.LoadImage("Images/Cards/back-card-design.png");
            CardFront = _game.LoadImage("Images/Cards/front-card-design.png");
            GraceMenuBackdrop = _game.LoadImage("Images/GraceMenu/grace-nav-card.png");
            LimgraveButtonTexture = _game.LoadImage("Images/GraceMenu/Buttons/limgrave-button.png");
            LimgraveButtonHoverTexture = _game.LoadImage("Images/GraceMenu/Buttons/limgrave-button-border.png");
            CaelidButtonTexture = _game.LoadImage("Images/GraceMenu/Buttons/caelid-button.png");
            CaelidButtonHoverTexture = _game.LoadImage("Images/GraceMenu/Buttons/caelid-button-border.png");
            LiurniaButtonTexture = _game.LoadImage("Images/GraceMenu/Buttons/liurnia-button.png");
            LiurniaButtonHoverTexture = _game.LoadImage("Images/GraceMenu/Buttons/liurnia-button-border.png");
            LeyndellTexture2D = _game.LoadImage("Images/GraceMenu/buttons/leyndell-button.png");
            LeyndellButtonHoverTexture = _game.LoadImage("Images/GraceMenu/buttons/leyndell-button-border.png");
            
            // Load audio
            MainMenuMusic = _game.LoadAudio("Audio/Music/main-menu-music.wav");
            LimgraveMusic = _game.LoadAudio("Audio/Music/limgrave-music.wav");
            StartButtonSound = _game.LoadAudio("Audio/SoundEffects/start-button-sound.wav");
        }
    }
}