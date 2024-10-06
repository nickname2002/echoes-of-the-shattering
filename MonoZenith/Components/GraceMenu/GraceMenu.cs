using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Support;

namespace MonoZenith.Components
{
    public class GraceMenu : Component
    {
        private readonly GameState _state;
        private readonly GraceMenuButton _limgraveButton;
        private readonly GraceMenuButton _caelidButton;
        private readonly GraceMenuButton _liurniaButton;
        private readonly GraceMenuButton _leyndellButton;
        private readonly Texture2D _graceMenuBackdrop;
        private RegionIndicator _activeRegionIndicator;
        private readonly Dictionary<Region, RegionIndicator> _buttonToIndicatorMap;
        private readonly SoundEffectInstance _newRegionSound;
        public bool Hidden;

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
            _limgraveButton.SetOnClickAction(() => SetRegionActive(state.GameTime, Region.LIMGRAVE));

            _caelidButton = new GraceMenuButton(g, new Vector2(Position.X + 141.6f, Position.Y + 100.4f), 
                DataManager.GetInstance(g).CaelidButtonTexture, 
                DataManager.GetInstance(g).CaelidButtonHoverTexture);
            _caelidButton.SetOnClickAction(() => SetRegionActive(state.GameTime, Region.CAELID));

            _liurniaButton = new GraceMenuButton(g, new Vector2(Position.X + 30.7f, Position.Y + 230.8f), 
                DataManager.GetInstance(g).LiurniaButtonTexture, 
                DataManager.GetInstance(g).LiurniaButtonHoverTexture);
            _liurniaButton.SetOnClickAction(() => SetRegionActive(state.GameTime, Region.LIURNIA));

            _leyndellButton = new GraceMenuButton(g, new Vector2(Position.X + 141, Position.Y + 230.8f), 
                DataManager.GetInstance(g).LeyndellTexture2D, 
                DataManager.GetInstance(g).LeyndellButtonHoverTexture);
            _leyndellButton.SetOnClickAction(() => SetRegionActive(state.GameTime, Region.LEYNDELL));

            // Initialize region indicators
            var limgraveRegionIndicator = new RegionIndicator(g, "Limgrave");
            var caelidRegionIndicator = new RegionIndicator(g, "Caelid");
            var liurniaRegionIndicator = new RegionIndicator(g, "Liurnia");
            var leyndellRegionIndicator = new RegionIndicator(g, "Leyndell");
            _activeRegionIndicator = null; // No active region at the start
            
            _buttonToIndicatorMap = new Dictionary<Region, RegionIndicator>
            {
                { Region.LIMGRAVE, limgraveRegionIndicator },
                { Region.CAELID, caelidRegionIndicator },
                { Region.LIURNIA, liurniaRegionIndicator },
                { Region.LEYNDELL, leyndellRegionIndicator }
            };
            
            // Load sound effect for new region
            _newRegionSound = DataManager.GetInstance(Game).NewLocationSound;
            _newRegionSound.Volume = 0.6f;
        }

        /// <summary>
        /// Sets a new region as active and activates the corresponding region indicator.
        /// </summary>
        /// <param name="deltaTime">The game time.</param>
        /// <param name="activeRegion">New active region.</param>
        public void SetRegionActive(GameTime deltaTime, Region activeRegion)
        {
            _newRegionSound.Play();
            _state.CurrentRegion = activeRegion;
            Console.WriteLine($"New region: {activeRegion}");
            Hidden = true;
            _state.SwitchTurn();

            // Activate the selected region's indicator
            _activeRegionIndicator = _buttonToIndicatorMap[activeRegion];
            _activeRegionIndicator.ResetIndicator();
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