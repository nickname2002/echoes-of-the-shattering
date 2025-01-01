using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using MonoZenith.Screen;
using MonoZenith.Support.Managers.Models;

namespace MonoZenith.Support.Managers;

public class SaveManager
{
    private const string SavePath = "SaveFiles/";
    private readonly int _selectedSlot = 0;

    public bool HasSaveFile => File.Exists(SavePath + $"levels_{_selectedSlot}.json");
    
    public void SaveGame()
    {
        SaveLevels();
    }

    public void LoadGame()
    {
        LoadLevels();
    }
    
    public void RemoveSaveFile()
    {
        if (File.Exists(SavePath + $"levels_{_selectedSlot}.json"))
            File.Delete(SavePath + $"levels_{_selectedSlot}.json");
        
        ResetLevels();
    }

    private void ResetDeck()
    {
        throw new NotImplementedException();
    }
    
    private void ResetLevels()
    {
        foreach (var level in LevelManager.Levels.Where(
                     level => LevelManager.Levels.IndexOf(level) != 0))
        {
            level.Unlocked = false;
            
            if (level.RewardCollected)
                level.RewardCollected = false;
            
            if (level.SecondPhase != null)
                level.SecondPhase.RewardCollected = false;
        }
    }

    private void SaveLevels()
    {
        var levels = LevelManager.Levels;
        var json = JsonSerializer.Serialize(levels.Select(level => new LevelModel
        {
            EnemyName = level.EnemyName,
            Unlocked = level.Unlocked,
            RewardCollected = level.RewardCollected
        }));
        File.WriteAllText(SavePath + $"levels_{_selectedSlot}.json", json);
    }

    private void SaveDeck()
    {
        throw new NotImplementedException();
    }

    private void LoadLevels()
    {
        if (!File.Exists(SavePath + $"levels_{_selectedSlot}.json"))
            return;
        
        var json = File.ReadAllText(SavePath + $"levels_{_selectedSlot}.json");
        var levels = JsonSerializer.Deserialize<LevelModel[]>(json);
        
        foreach (var level in levels)
        {
            if (!level.Unlocked) continue;
            var levelToUnlock = OverworldScreen.LevelManager.GetLevelFromEnemy(level.EnemyName);
            levelToUnlock.Unlocked = true;
            levelToUnlock.RewardCollected = level.RewardCollected;
        }
    }
    
    public void LoadDeck()
    {
        throw new NotImplementedException();
    }
}