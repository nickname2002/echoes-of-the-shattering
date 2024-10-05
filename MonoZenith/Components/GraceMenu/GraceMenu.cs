using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Support;

namespace MonoZenith.Components
{
    public class GraceMenu : Component
    {
        private GameState _state;
        private readonly GraceMenuButton _limgraveButton;
        private readonly GraceMenuButton _caelidButton;
        private readonly GraceMenuButton _liurniaButton;
        private readonly GraceMenuButton _leyndellButton;
        private readonly Texture2D _graceMenuBackdrop;
        public bool Hidden;

        private RegionIndicator _activeRegionIndicator; // To track the currently active region indicator
        private RegionIndicator _limgraveRegionIndicator;
        private RegionIndicator _caelidRegionIndicator;
        private RegionIndicator _liurniaRegionIndicator;
        private RegionIndicator _leyndellRegionIndicator;

        public GraceMenu(Game g, GameState state) : base(g, Vector2.Zero, 275, 400)
        {
            _state = state;
            Position = new Vector2(50, Game.ScreenHeight / 2 - Height / 2);
            Hidden = true;

            _graceMenuBackdrop = DataManager.GetInstance(g).GraceMenuBackdrop;

            // Initialize region buttons and set their on-click actions
            _limgraveButton = new GraceMenuButton(g, new Vector2(Position.X + 30.7f, Position.Y + 100.4f), 
                DataManager.GetInstance(g).LimgraveButtonTexture, 
                DataManager.GetInstance(g).LimgraveButtonHoverTexture);
            _limgraveButton.SetOnClickAction(() => SetRegionActive(state.GameTime, Region.LIMGRAVE, _limgraveRegionIndicator));

            _caelidButton = new GraceMenuButton(g, new Vector2(Position.X + 141.6f, Position.Y + 100.4f), 
                DataManager.GetInstance(g).CaelidButtonTexture, 
                DataManager.GetInstance(g).CaelidButtonHoverTexture);
            _caelidButton.SetOnClickAction(() => SetRegionActive(state.GameTime, Region.CAELID, _caelidRegionIndicator));

            _liurniaButton = new GraceMenuButton(g, new Vector2(Position.X + 30.7f, Position.Y + 230.8f), 
                DataManager.GetInstance(g).LiurniaButtonTexture, 
                DataManager.GetInstance(g).LiurniaButtonHoverTexture);
            _liurniaButton.SetOnClickAction(() => SetRegionActive(state.GameTime, Region.LIURNIA, _liurniaRegionIndicator));

            _leyndellButton = new GraceMenuButton(g, new Vector2(Position.X + 141, Position.Y + 230.8f), 
                DataManager.GetInstance(g).LeyndellTexture2D, 
                DataManager.GetInstance(g).LeyndellButtonHoverTexture);
            _leyndellButton.SetOnClickAction(() => SetRegionActive(state.GameTime, Region.LEYNDELL, _leyndellRegionIndicator));

            // Initialize region indicators
            _limgraveRegionIndicator = new RegionIndicator(g, "Limgrave");
            _caelidRegionIndicator = new RegionIndicator(g, "Caelid");
            _liurniaRegionIndicator = new RegionIndicator(g, "Liurnia");
            _leyndellRegionIndicator = new RegionIndicator(g, "Leyndell");

            _activeRegionIndicator = null; // No active region at the start
        }

        /// <summary>
        /// Sets a new region as active and activates the corresponding region indicator.
        /// </summary>
        /// <param name="deltaTime">The game time.</param>
        /// <param name="activeRegion">New active region.</param>
        /// <param name="indicator">The region indicator to activate.</param>
        private void SetRegionActive(GameTime deltaTime, Region activeRegion, RegionIndicator indicator)
        {
            _state.CurrentRegion = activeRegion;
            Console.WriteLine($"Player selected region: {activeRegion}");
            Hidden = true;
            _state.SwitchTurn();

            // Activate the selected region's indicator
            _activeRegionIndicator = indicator;
            _activeRegionIndicator?.Express(deltaTime); // Start the fade-in process
        }

        public override void Update(GameTime deltaTime)
        {
            // Update the active region indicator (if any)
            _activeRegionIndicator?.Update(deltaTime);
            
            if (Hidden)
                return;

            // Update the grace menu buttons
            _limgraveButton.Update(deltaTime);
            _caelidButton.Update(deltaTime);
            _liurniaButton.Update(deltaTime);
            _leyndellButton.Update(deltaTime);
        }

        public override void Draw()
        {
            // Draw the active region indicator (if any)
            _activeRegionIndicator?.Draw();
            
            if (Hidden)
                return;

            // Draw the menu backdrop and buttons
            Game.DrawImage(_graceMenuBackdrop, Position);
            _limgraveButton.Draw();
            _caelidButton.Draw();
            _liurniaButton.Draw();
            _leyndellButton.Draw();
        }
    }
}