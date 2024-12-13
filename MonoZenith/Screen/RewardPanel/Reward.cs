using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZenith.Screen.RewardPanel;

public class Reward
{
    public Texture2D RewardTexture { get; }
    public string RewardName { get; }
    public Type RewardItem { get; }
    
    public Reward(Texture2D texture, string name, Type item)
    {
        RewardTexture = texture;
        RewardName = name;
        RewardItem = item;
    }
}