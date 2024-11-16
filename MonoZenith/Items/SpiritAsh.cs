using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith.Items;

public abstract class SpiritAsh
{
    protected GameState _state;
    protected Player _owner;
    
    public Texture2D TextureEnabled { get; set; }
    public Texture2D TextureDisabled { get; set; }
    
    protected SpiritAsh(GameState state, Player owner)
    {
        _state = state;
        _owner = owner;
    }
    
    /// <summary>
    /// Perform the effect of the spirit ash.
    /// </summary>
    protected abstract void PerformEffect();
    
    /// <summary>
    /// Check if the spirit ash can be invoked.
    /// </summary>
    /// <returns>True if the spirit ash can be invoked; otherwise, false.</returns>
    protected abstract bool Invocable();

    /// <summary>
    /// Check if the AI should play the spirit ash.
    /// </summary>
    /// <returns>True if the AI should play the spirit ash; otherwise, false.</returns>
    public abstract bool ShouldAIPlay(AiState aiState);

    /// <summary>
    /// Update the state of the spirit ash.
    /// </summary>
    /// <param name="deltaTime">The delta time.</param>
    public void Update(GameTime deltaTime)
    {   
        if (Invocable())
        {
            PerformEffect();
        }
    }
}

public class MimicTearAsh : SpiritAsh
{
    public MimicTearAsh(GameState state, Player owner) : 
        base(state, owner)
    {
        TextureDisabled = DataManager.GetInstance(state.Game).MimicTearIndicatorDisabled;
        TextureEnabled = DataManager.GetInstance(state.Game).MimicTearIndicatorEnabled;
    }

    protected override void PerformEffect()
    {
        _owner.BuffManager.Buff = new CardTwiceAsStrongBuff(_state, _owner.BuffManager);
    }

    protected override bool Invocable()
    {
        return true;
    }

    public override bool ShouldAIPlay(AiState aiState)
    {
        // TODO: Implement more advanced rules later
        return aiState == AiState.Aggressive;
    }
}