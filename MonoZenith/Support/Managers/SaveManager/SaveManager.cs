using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MonoZenith.Items;
using MonoZenith.Screen;
using MonoZenith.Screen.AshDisplay;
using MonoZenith.Screen.DeckDisplay;
using MonoZenith.Support.Managers.Models;

namespace MonoZenith.Support.Managers;

public class SaveManager
{
    private static readonly string SaveDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveFiles");
    
    public SaveManager()
    {
        EnsureSaveDirectoryExists();
    }

    public bool HasSaveFile()
    {
        return File.Exists(GetSaveFilePath("levels"));
    }

    public void SaveGame()
    {
        SaveLevels();
        SaveAsh();
        SaveDeck();
    }

    public void LoadGame()
    {
        LoadLevels();
        LoadAsh();
        LoadDeck();
    }

    public void RemoveSaveFile()
    {
        var levelSave = GetSaveFilePath("levels");
        var ashSave = GetSaveFilePath("ash");
        var deckSave = GetSaveFilePath("deck");
        
        foreach (var filepath in new[] { levelSave, ashSave, deckSave })
        {
            if (File.Exists(filepath))
                File.Delete(filepath);
        }

        ResetLevels();
        ResetAsh();
        ResetDeck();
    }

    private void ResetDeck()
    {
        List<(int, int)> defaultDeck = new()
        {
            (0, 10),
            (1, 5),
            (2, 3),
            (3, 3),
            (4, 5),
            (5, 4)
        };
        
        foreach (var (cardId, amount) in defaultDeck)
            DeckDisplay.CardAmountComponents[cardId].Amount = amount;
    }

    private void ResetLevels()
    {
        foreach (var level in LevelManager.Levels)
        {
            if (LevelManager.Levels.IndexOf(level) != 0)
                level.Unlocked = false;

            if (level.RewardCollected)
                level.RewardCollected = false;

            if (level.SecondPhase != null)
                level.SecondPhase.RewardCollected = false;
        }
    }

    private void ResetAsh() => AshDisplay.SetAllAshesUnselected();
    
    private void SaveLevels()
    {
        var levels = LevelManager.Levels;
        var json = JsonSerializer.Serialize(levels.Select(level => new LevelModel
        {
            EnemyName = level.EnemyName,
            Unlocked = level.Unlocked,
            RewardCollected = level.RewardCollected
        }));
        File.WriteAllText(GetSaveFilePath("levels"), json);
    }

    private void SaveAsh()
    {
        string selectedAsh = AshDisplay.SelectedAsh?.ToString();
        if (selectedAsh == null) return;
        var json = JsonSerializer.Serialize(selectedAsh);
        File.WriteAllText(GetSaveFilePath("ash"), json);
    }
    
    private void SaveDeck()
    {
        List<DeckCardModel> amountsForIndex = DeckDisplay.CardAmountComponents
            .Select((t, i) => new DeckCardModel
            {
                CardId = i,
                Amount = t.Amount
            })
            .ToList();

        var json = JsonSerializer.Serialize(amountsForIndex);
        File.WriteAllText(GetSaveFilePath("deck"), json);
    }

    private void LoadLevels()
    {
        var saveFilePath = GetSaveFilePath("levels");
        if (!File.Exists(saveFilePath))
            return;

        var json = File.ReadAllText(saveFilePath);
        var levels = JsonSerializer.Deserialize<LevelModel[]>(json);

        foreach (var level in levels)
        {
            if (!level.Unlocked) continue;
            var levelToUnlock = OverworldScreen.LevelManager.GetLevelFromEnemy(level.EnemyName);
            levelToUnlock.Unlocked = true;
            levelToUnlock.RewardCollected = level.RewardCollected;
        }
    }

    public void LoadAsh()
    {
        var saveFilePath = GetSaveFilePath("ash");
        if (!File.Exists(saveFilePath))
            return;

        // Parse from JSON
        var json = File.ReadAllText(saveFilePath);
        var selectedAsh = JsonSerializer.Deserialize<string>(json);
        
        // Find the AshSelectComponent that matches the selectedAsh
        var ash = AshDisplay.AshSelectComponents.FirstOrDefault(
            ash => ash.Ash.ToString() == selectedAsh);
        
        // Select the Ash
        if (ash == null) return;
        AshDisplay.SelectAsh(ash);
    }

    public void LoadDeck()
    {
        var saveFilePath = GetSaveFilePath("deck");
        if (!File.Exists(saveFilePath))
            return;

        // Parse from JSON
        var json = File.ReadAllText(saveFilePath);
        var deckCardModels = JsonSerializer.Deserialize<List<DeckCardModel>>(json);

        // Set the amount of cards for each CardAmountComponent
        foreach (var deckCardModel in deckCardModels)
        {
            DeckDisplay.CardAmountComponents[deckCardModel.CardId].Amount = deckCardModel.Amount;
        }
    }

    private void EnsureSaveDirectoryExists()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
    }

    private string GetSaveFilePath(string fileName)
    {
        return Path.Combine(SaveDirectory, $"{fileName}.json");
    }
}