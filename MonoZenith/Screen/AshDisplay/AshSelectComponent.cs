using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Items;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Screen.AshDisplay;

public class AshSelectComponent
{
    private readonly Texture2D _texture;
    private readonly string _beatenEnemyName;
 
    public float Scale { get; private set; }
    public SpiritAsh Ash { get; private set; }
    public bool Selected { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Dimensions => new(_texture.Width * Scale, _texture.Height * Scale);
    
    public AshSelectComponent(SpiritAsh ash, Texture2D texture, string beatenEnemyName)
    {
        Ash = ash;
        _texture = texture;
        _beatenEnemyName = beatenEnemyName;
        Selected = false;
    }
    
    public AshSelectComponent(SpiritAsh ash, Texture2D texture, string beatenEnemyName, bool selected)
    {
        Ash = ash;
        _texture = texture;
        _beatenEnemyName = beatenEnemyName;
        Selected = selected;
    }
    
    public bool IsUnlocked()
    {
        var level = OverworldScreen.LevelManager.GetLevelFromEnemy(_beatenEnemyName);
        var levelIndex = LevelManager.Levels.IndexOf(level);
        if (levelIndex == LevelManager.Levels.Count - 1) return true;
        return LevelManager.Levels[levelIndex + 1].Unlocked;
    }

    public bool IsHovered()
    {
        return IsUnlocked() && 
               GetMousePosition().X >= Position.X &&
               GetMousePosition().X <= Position.X + Dimensions.X &&
               GetMousePosition().Y >= Position.Y &&
               GetMousePosition().Y <= Position.Y + Dimensions.Y;
    }

    public void Update(GameTime deltaTime)
    {   
        Scale = 0.20f * AppSettings.Scaling.ScaleFactor;
    }
    
    public void Draw()
    {
        if (!IsUnlocked()) return;
        DrawImage(
            _texture,
            Position,
            Scale,
            alpha: Selected || IsHovered() ? 1f : 0.5f);
    }
}