
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Engine.Support
{
    public class DataManager
    {
        private readonly Game _game;
        private static DataManager _instance;
        
        // Fonts
        public SpriteFont ComponentFont;
        
        // Textures
        public Texture2D MonoZenithLogo;
        public Texture2D Backdrop;
        public Texture2D CardBack;
        public Texture2D CardFront;
        
        // Audio

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
            
            // Load textures
            MonoZenithLogo = _game.LoadImage("Images/monozenith.png");
            Backdrop = _game.LoadImage("Images/Backdrops/backdrop.png");
            CardBack = _game.LoadImage("Images/Cards/back-card-design.png");
            CardFront = _game.LoadImage("Images/Cards/front-card-design.png");
            
            // Load audio
        }
    }
}