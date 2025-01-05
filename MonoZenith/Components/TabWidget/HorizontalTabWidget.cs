using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
// ReSharper disable InconsistentNaming

namespace MonoZenith.Components.TabWidget;

public class HorizontalTabWidget
{
    protected readonly List<SelectableImageButton> _optionButtons;
    protected Vector2 _position;
    protected float _scale;
    protected float _spacing;
    
    public int SelectedOption { get; set; }
    
    public HorizontalTabWidget(
        List<(Texture2D, Texture2D)> optionTextures, 
        Vector2 position,
        float scale = 1f,
        float spacing = 20)
    {   
        SelectedOption = 0;
        _position = position;
        _scale = scale;
        _spacing = spacing;
    
        _optionButtons = new List<SelectableImageButton>();
        float currentX = position.X; // Use this variable to correctly manage X positioning
    
        for (var i = 0; i < optionTextures.Count; i++)
        {
            var (normalTexture, selectedTexture) = optionTextures[i];
            var buttonIndex = i;
        
            var buttonWidth = normalTexture.Width * scale; // Properly account for scaled width
            var button = new SelectableImageButton(
                new Vector2(currentX, position.Y),
                normalTexture,
                selectedTexture,
                scale: scale,
                onClickAction: () => SelectedOption = buttonIndex
            );
        
            _optionButtons.Add(button);
        
            // Increment X position for the next button
            currentX += buttonWidth + _spacing;
        }
    }
    
    public void Update(GameTime deltaTime)
    {
        // Update selected button
        for (var i = 0; i < _optionButtons.Count; i++)
        {
            var button = _optionButtons[i];
            
            // TODO: Possibly use another sound effect later (?)
            if (button.Selected && i != SelectedOption)
                DataManager.GetInstance().EndPlayerTurnSound.CreateInstance().Play();
            
            button.Selected = i == SelectedOption;
        }
        
        foreach (var button in _optionButtons)
        {
            button.Update(deltaTime);
        }
    }
    
    public void Draw()
    {
        foreach (var button in _optionButtons)
        {
            button.Draw();
        }
    }
}