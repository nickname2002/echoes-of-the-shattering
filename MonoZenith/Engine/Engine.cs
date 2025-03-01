using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Engine
{
    public class Engine : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameFacade _gameFacade;
        private float _splashScreenTimer = 3000;

        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initialize the game.
        /// </summary>
        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameFacade = new GameFacade(_spriteBatch, _graphics, Content);
            Game.Initialize(_gameFacade);
            base.Initialize();
        }

        /// <summary>
        /// Load game content.
        /// </summary>
        protected override void LoadContent()
        {
            Init();
            Window.AllowUserResizing = ScreenResizable;

            int targetWidth = 1600;
            int targetHeight = 900;

            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            if (ScreenFullScreen)
            {
                // Bereken de schaalfactor om 16:9 aspect ratio te behouden
                float scaleX = (float)screenWidth / targetWidth;
                float scaleY = (float)screenHeight / targetHeight;
                float scale = MathF.Min(scaleX, scaleY);  // Kies de kleinste schaal om afsnijding te voorkomen

                // Pas de geschaalde resolutie toe
                int scaledWidth = (int)(targetWidth * scale);
                int scaledHeight = (int)(targetHeight * scale);

                // Centreer de viewport
                int viewportX = (screenWidth - scaledWidth) / 2;
                int viewportY = (screenHeight - scaledHeight) / 2;

                // Stel de viewport in
                GraphicsDevice.Viewport = new Viewport(viewportX, viewportY, scaledWidth, scaledHeight);

                // Pas de schermgrootte aan
                SetScreenSize(scaledWidth, scaledHeight);
            }
            else
            {
                // Gebruik de standaardgrootte in venstermodus
                SetScreenSize(targetWidth, targetHeight);
            }

            // Toepassen van instellingen
            _graphics.IsFullScreen = ScreenFullScreen;
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();

            Window.Title = WindowTitle;

            // Update schaalfactor
            AppSettings.Scaling.UpdateScaleFactor(ScreenWidth, ScreenHeight);

            // Content en schermen laden
            DataManager.GetInstance();
            InitializeScreens();
        }

        /// <summary>
        /// Apply scaling for the viewport based on the new screen dimensions and adjust for top/bottom bars.
        /// </summary>
        private void ApplyViewportScaling(int screenWidth, int screenHeight)
        {
            int adjustedHeight = screenHeight;

            float scaleX = (float)screenWidth / ScreenWidth;
            float scaleY = (float)adjustedHeight / ScreenHeight;

            _graphics.GraphicsDevice.Viewport = new Viewport(0, 0, screenWidth, adjustedHeight);
            _graphics.ApplyChanges();

            // Update schaalfactoren
            AppSettings.Scaling.UpdateScaleFactor(
                (int)(screenWidth * scaleX), 
                (int)(adjustedHeight * scaleY));
        }

        /// <summary>
        /// Handle controller support.
        /// </summary>
        private void HandleControllerSupport()
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

            if (!capabilities.IsConnected)
                return;

            _gameFacade.ControllerConnected = true;

            Dictionary<Func<GamePadCapabilities, bool>, Action> capabilityMappings = 
                new Dictionary<Func<GamePadCapabilities, bool>, Action>
            {
                { cap => cap.HasLeftXThumbStick, () => _gameFacade.HasLeftStick = true },
                { cap => cap.HasRightXThumbStick, () => _gameFacade.HasRightStick = true },
                { cap => cap.HasDPadRightButton, () => _gameFacade.HasDPad = true },
                { cap => cap.HasRightTrigger, () => _gameFacade.HasRightTrigger = true },
                { cap => cap.HasLeftTrigger, () => _gameFacade.HasLeftTrigger = true },
                { cap => cap.HasLeftShoulderButton, () => _gameFacade.HasLeftBumper = true },
                { cap => cap.HasRightShoulderButton, () => _gameFacade.HasRightBumper = true },
                { cap => cap.HasAButton, () => _gameFacade.HasAButton = true },
                { cap => cap.HasBButton, () => _gameFacade.HasBButton = true },
                { cap => cap.HasXButton, () => _gameFacade.HasXButton = true },
                { cap => cap.HasYButton, () => _gameFacade.HasYButton = true },
                { cap => cap.HasStartButton, () => _gameFacade.HasStartButton = true },
                { cap => cap.HasBackButton, () => _gameFacade.HasBackButton = true }
            };

            foreach (var mapping in capabilityMappings.
                         Where(mapping => mapping.Key(capabilities)))
            {
                mapping.Value();
            }
        }

        /// <summary>
        /// Show splash screen.
        /// </summary>
        private void ShowSplashScreen()
        {
            Texture2D splashScreen = DataManager.GetInstance().MonoZenithLogo;
            float scale = (float)ScreenWidth / splashScreen.Width * 0.6f;
            scale += (1 - _splashScreenTimer / 3000) / 10;
            
            // Fade in slowly
            float alpha = 1;
            if (_splashScreenTimer > 2000)
            {
                alpha = 1 - (_splashScreenTimer - 2000) / 1000;
            }
            
            // Calculate splash screen position
            float x = ScreenWidth / 2f;
            float y = ScreenHeight / 2f;
            
            // Draw splash screen
            _spriteBatch.Draw(
                splashScreen, 
                new Vector2(x, y), 
                null, 
                new Color(Color.White, alpha), 
                0, 
                new Vector2(
                    splashScreen.Width / 2f, 
                    splashScreen.Height / 2f),
                scale,
                SpriteEffects.None,
                0);
        }

        /// <summary>
        /// Update the game.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (QuitToDesktop && !IsFadingOut) Exit();

            // Update mouse click cooldown timer
            if (_gameFacade.CurrentClickCooldown > 0.0f)
                _gameFacade.CurrentClickCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            
            HandleControllerSupport();
            MonoZenith.Game.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the game.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (QuitToDesktop && !IsFadingOut) return;
            GraphicsDevice.Clear(BackgroundColor);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // TODO: Show splash screen when development is done.
            Game.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}