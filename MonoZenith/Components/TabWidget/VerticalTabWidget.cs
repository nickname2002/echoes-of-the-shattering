using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Components.TabWidget;

public class VerticalTabWidget : HorizontalTabWidget
{
    public VerticalTabWidget(
        List<(Texture2D, Texture2D)> optionTextures, 
        Vector2 position, 
        float scale = 1, 
        float spacing = 20) : base(optionTextures, position, scale, spacing)
    {
        _optionButtons.Clear();
        for (var i = 0; i < optionTextures.Count; i++)
        {
            var (normalTexture, selectedTexture) = optionTextures[i];
            var buttonIndex = i;
            var button = new SelectableImageButton(
                new Vector2(position.X, position.Y + i * (normalTexture.Height * scale + spacing)),
                normalTexture,
                selectedTexture,
                scale: scale,
                onClickAction: () => SelectedOption = buttonIndex
            );
            _optionButtons.Add(button);
        }
    }
}