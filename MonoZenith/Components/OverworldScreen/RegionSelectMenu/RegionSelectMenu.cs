using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Engine.Support;
using MonoZenith.Support;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Components.OverworldScreen.RegionSelectMenu
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
            Region = Region.Limgrave
        };
        
        private readonly RegionSelectButton _liurniaButton = new()
        {
            SelectedTexture = LoadImage("Images/OverworldScreen/Buttons/Liurnia/liurnia-button-selected.png"),
            HoveredTexture = LoadImage("Images/OverworldScreen/Buttons/Liurnia/liurnia-button-hovered.png"),
            InactiveTexture = LoadImage("Images/OverworldScreen/Buttons/Liurnia/liurnia-button-inactive.png"),
            Region = Region.Liurnia
        };
        
        private readonly RegionSelectButton _altusButton = new()
        {
            SelectedTexture = LoadImage("Images/OverworldScreen/Buttons/Altus/altus-button-selected.png"),
            HoveredTexture = LoadImage("Images/OverworldScreen/Buttons/Altus/altus-button-hovered.png"),
            InactiveTexture = LoadImage("Images/OverworldScreen/Buttons/Altus/altus-button-inactive.png"),
            Region = Region.AltusPlateau
        };

        public RegionSelectMenu()
        {
            UpdateButtonPositions();
        }

        private void UpdateButtonPositions()
        {
            var regionSelectButton = new[] { _altusButton, _liurniaButton, _limgraveButton };
            var longestButton = regionSelectButton.OrderByDescending(button => button.Dimensions.X).First();
            var xPosition = ScreenWidth - longestButton.Dimensions.X + 40f * AppSettings.Scaling.ScaleFactor; 
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
                if (_limgraveButton.IsHovered()
                    && Screen.OverworldScreen.LevelManager.RegionActive(Region.Limgrave))
                {
                    if (SelectedRegion != Region.Limgrave)
                        _limgraveSelectedSoundEffect.Play();
                    SelectedRegion = Region.Limgrave;
                }
                else if (_liurniaButton.IsHovered() 
                         && Screen.OverworldScreen.LevelManager.RegionActive(Region.Liurnia))
                {
                    if (SelectedRegion != Region.Liurnia)
                        _liurniaSelectedSoundEffect.Play();
                    SelectedRegion = Region.Liurnia;
                }
                else if (_altusButton.IsHovered() 
                         && Screen.OverworldScreen.LevelManager.RegionActive(Region.AltusPlateau))
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
            RegionSelectButton[] regionSelectButton = { _altusButton, _liurniaButton, _limgraveButton };
            foreach (RegionSelectButton button in regionSelectButton)
            { 
                button.Draw();
            }
        }
    }
}