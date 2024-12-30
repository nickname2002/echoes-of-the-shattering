using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Engine.Support;
using MonoZenith.Support;
using static MonoZenith.Game;

namespace MonoZenith.Components.OverworldScreen
{
    public class RegionSelectMenu
    {
        public Region SelectedRegion { get; set; } = Region.Limgrave;
        private readonly SoundEffect _limgraveSelectedSoundEffect = DataManager.GetInstance().EndPlayerTurnSound;
        private readonly SoundEffect _liurniaSelectedSoundEffect = DataManager.GetInstance().EndPlayerTurnSound;
        private readonly SoundEffect _altusSelectedSoundEffect = DataManager.GetInstance().EndPlayerTurnSound;

        private readonly RegionSelectButton _limgraveButton = new()
        {
            SelectedTexture = LoadImage("Images/OverworldScreen/Buttons/Limgrave/limgrave-button-selected.png"),
            HoveredTexture = LoadImage("Images/OverworldScreen/Buttons/Limgrave/limgrave-button-hovered.png"),
            InactiveTexture = LoadImage("Images/OverworldScreen/Buttons/Limgrave/limgrave-button-inactive.png"),
        };
        
        private readonly RegionSelectButton _liurniaButton = new()
        {
            SelectedTexture = LoadImage("Images/OverworldScreen/Buttons/Liurnia/liurnia-button-selected.png"),
            HoveredTexture = LoadImage("Images/OverworldScreen/Buttons/Liurnia/liurnia-button-hovered.png"),
            InactiveTexture = LoadImage("Images/OverworldScreen/Buttons/Liurnia/liurnia-button-inactive.png"),
        };
        
        private readonly RegionSelectButton _altusButton = new()
        {
            SelectedTexture = LoadImage("Images/OverworldScreen/Buttons/Altus/altus-button-selected.png"),
            HoveredTexture = LoadImage("Images/OverworldScreen/Buttons/Altus/altus-button-hovered.png"),
            InactiveTexture = LoadImage("Images/OverworldScreen/Buttons/Altus/altus-button-inactive.png"),
        };

        public RegionSelectMenu()
        {
            UpdateButtonPositions();
        }

        private void UpdateButtonPositions()
        {
            var regionSelectButton = new[] { _limgraveButton, _liurniaButton, _altusButton };
        
            var xPosition = ScreenWidth - _altusButton.Dimensions.X + 50f * AppSettings.Scaling.ScaleFactor; 
            var totalHeight = regionSelectButton.Sum(button => button.Dimensions.Y * 2);
            var startY = (ScreenHeight - totalHeight) / 2;
            var offset = _altusButton.Dimensions.Y;
        
            for (var i = 0; i < regionSelectButton.Length; i++)
            {
                var button = regionSelectButton[i];
                button.Update(new Vector2(
                    xPosition - button.Dimensions.X / 2, 
                    startY + i * button.Dimensions.Y + offset * i));
            }
        }

        private void UpdateSelected()
        {
            if (GetMouseButtonDown(MouseButtons.Left))
            {
                if (_limgraveButton.IsHovered())
                {
                    if (SelectedRegion != Region.Limgrave)
                        _limgraveSelectedSoundEffect.Play();
                    SelectedRegion = Region.Limgrave;
                }
                else if (_liurniaButton.IsHovered())
                {
                    if (SelectedRegion != Region.Liurnia)
                        _liurniaSelectedSoundEffect.Play();
                    SelectedRegion = Region.Liurnia;
                }
                else if (_altusButton.IsHovered())
                {
                    if (SelectedRegion != Region.AltusPlateau)
                        _altusSelectedSoundEffect.Play();
                    SelectedRegion = Region.AltusPlateau;
                }
            }
            
            _limgraveButton.Selected = SelectedRegion == Region.Limgrave;
            _liurniaButton.Selected = SelectedRegion == Region.Liurnia;
            _altusButton.Selected = SelectedRegion == Region.AltusPlateau;
        }
        
        public void Update()
        {
            UpdateButtonPositions();
            UpdateSelected();
        }

        public void Draw()
        {
            var regionSelectButton = new[] { _limgraveButton, _liurniaButton, _altusButton };
            foreach (var button in regionSelectButton)
            {
                button.Draw();
            }
        }
    }
}