#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using static MonoZenith.Game;

namespace MonoZenith.Screen.AshDisplay;

public class AshDisplay
{
    public static SpiritAsh? SelectedAsh { get; private set; }
    public static readonly List<AshSelectComponent> AshSelectComponents = new()
    { 
        new AshSelectComponent(
            new JellyfishAsh(),
            LoadImage("Images/LoadoutDisplay/AshSelectComponents/jellyfish-ash-component.png"),
            "Roderika"),
        new AshSelectComponent(
            new WolvesAsh(),
            LoadImage("Images/LoadoutDisplay/AshSelectComponents/wolves-ash-component.png"),
            "Renna"),
        new AshSelectComponent(
            new MimicTearAsh(),
            LoadImage("Images/LoadoutDisplay/AshSelectComponents/mimic-tear-ash-component.png"),
            "Mimic Tear"),
    };

    private void CalculateAshPositions()
    { 
        int amountOfAshes = AshSelectComponents.Count(ash => ash.IsUnlocked());
        if (amountOfAshes == 0) return; 

        float ashScale = AshSelectComponents.First().Scale;
        float ashWidth = 620 * ashScale;
        float ashHeight = 820 * ashScale;
        float margin = 60 * AppSettings.Scaling.ScaleFactor;
        float totalWidth = amountOfAshes * ashWidth + (amountOfAshes - 1) * margin;
        float startX = (ScreenWidth - totalWidth) / 2;
        float startY = (ScreenHeight - ashHeight) / 2;

        for (int i = 0; i < amountOfAshes; i++)
        {
            float currentMargin = margin;
            if (i == 1) currentMargin = 40 * AppSettings.Scaling.ScaleFactor;

            AshSelectComponents[i].Position = new Vector2(
                startX + i * (ashWidth + currentMargin),
                startY
            );
        }
    }

    public static void SelectAsh(AshSelectComponent ash)
    {
        if (ash.Selected) return;
        foreach (var otherAsh in AshSelectComponents)
            otherAsh.Selected = false;
        
        SelectedAsh = ash.Ash;
        ash.Selected = true;
    }
    
    public static void SetAllAshesUnselected()
    {
        SelectedAsh = null;
        foreach (var ash in AshSelectComponents)
            ash.Selected = false;
    }
    
    public void Update(GameTime deltaTime)
    {
        CalculateAshPositions();
        foreach (var ash in AshSelectComponents)
        {
            ash.Update(deltaTime);
            if (ash.IsHovered() && GetMouseButtonDown(MouseButtons.Left))
            {
                SelectAsh(ash);
            }
        }
    }
    
    private void DrawSelectedAshAmount()
    {
        int amountOfSelectedAshes = AshSelectComponents.Count(ash => ash.Selected);
        var stringToDraw = $"{Math.Min(amountOfSelectedAshes, 1)}/1";
        DrawText(stringToDraw,
            new Vector2(
                ScreenWidth - DataManager.GetInstance().ComponentFont.MeasureString(stringToDraw).X 
                            - 50 * AppSettings.Scaling.ScaleFactor, 
                ScreenHeight / 2f - DataManager.GetInstance().ComponentFont.MeasureString(stringToDraw).Y 
                * AppSettings.Scaling.ScaleFactor),
            DataManager.GetInstance().IndicatorFont,
            new Color(147, 137, 111),
            AppSettings.Scaling.ScaleFactor);
    }
    
    public void Draw()
    {   
        DrawSelectedAshAmount();
        foreach (var ash in AshSelectComponents)
            ash.Draw();
    }
}