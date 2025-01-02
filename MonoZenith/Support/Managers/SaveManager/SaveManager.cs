using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using MonoZenith.Screen;
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
    }

    public void LoadGame()
    {
        LoadLevels();
    }

    public void RemoveSaveFile()
    {
        var saveFilePath = GetSaveFilePath("levels");
        if (File.Exists(saveFilePath))
            File.Delete(saveFilePath);

        ResetLevels();
    }

    private void ResetDeck()
    {
        throw new NotImplementedException();
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

    private void SaveDeck()
    {
        throw new NotImplementedException();
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

    public void LoadDeck()
    {
        throw new NotImplementedException();
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