using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Components;
using MonoZenith.Components.TabWidget;
using MonoZenith.Engine.Support;
using MonoZenith.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen.LoadoutDisplay;

public class LoadoutDisplay
{
    private readonly ImageButton _backToOverworldButton;
    private readonly VerticalTabWidget _loadoutTypeTabWidget;
    private LoadoutType _selectedLoadoutType;
    private DeckDisplay.DeckDisplay _deckDisplay;
    private AshDisplay.AshDisplay _ashDisplay;
    
    // ReSharper disable once ConvertConstructorToMemberInitializers
    public LoadoutDisplay()
    {
        _backToOverworldButton = new ImageButton(
            new Vector2(30 * AppSettings.Scaling.ScaleFactor, 30 * AppSettings.Scaling.ScaleFactor),
            LoadImage("Images/LoadoutDisplay/Buttons/to-overworld.png"),
            scale: 0.15f * AppSettings.Scaling.ScaleFactor,
            onClickAction: () =>
            {
                DataManager.GetInstance().EndPlayerTurnSound.CreateInstance().Play();
                ShowLoadoutDisplay(false);
            }
        );
        
        _loadoutTypeTabWidget = new VerticalTabWidget(
            new List<(Texture2D, Texture2D)>
            {
                // Deck
                (LoadImage("Images/LoadoutDisplay/Buttons/deck.png"), 
                    LoadImage("Images/LoadoutDisplay/Buttons/deck-selected.png")),
                
                // Ashes
                (LoadImage("Images/LoadoutDisplay/Buttons/ash.png"), 
                    LoadImage("Images/LoadoutDisplay/Buttons/ash-selected.png")),
            },
            new Vector2(
                80 * AppSettings.Scaling.ScaleFactor,
                ScreenHeight / 2f - 100 * AppSettings.Scaling.ScaleFactor
            ),
            scale: 0.15f * AppSettings.Scaling.ScaleFactor
        );
        
        _selectedLoadoutType = LoadoutType.Deck;
        _deckDisplay = new DeckDisplay.DeckDisplay();
        _ashDisplay = new AshDisplay.AshDisplay();
    }
        
    public void Update(GameTime deltaTime)
    {
        _backToOverworldButton.Update(deltaTime);
        _loadoutTypeTabWidget.Update(deltaTime);
        _selectedLoadoutType = (LoadoutType)_loadoutTypeTabWidget.SelectedOption;
        
        switch (_selectedLoadoutType)
        {
            case LoadoutType.Deck:
                _deckDisplay.Update(deltaTime);
                break;
            case LoadoutType.Ashes:
                _ashDisplay.Update(deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Draw()
    {
        _backToOverworldButton.Draw();
        _loadoutTypeTabWidget.Draw();
        
        switch (_selectedLoadoutType)
        {
            case LoadoutType.Deck:
                _deckDisplay.Draw();
                break;
            case LoadoutType.Ashes:
                _ashDisplay.Draw();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}