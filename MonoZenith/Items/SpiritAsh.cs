using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith.Items;

public abstract class SpiritAsh
{
    protected Game _game;
    protected GameState _state;
    protected Player _owner;
    protected float _scale;
    
    public Texture2D Texture { get; set; }
    
    protected SpiritAsh(Game g, GameState state, Player owner)
    {
        _game = g;
        _state = state;
        _owner = owner;
        _scale = 0.085f * AppSettings.Scaling.ScaleFactor;
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
    
    /// <summary>
    /// Draw the spirit ash at the specified position.
    /// </summary>
    /// <param name="position">The position to draw the spirit ash.</param>
    /// <param name="alpha">The alpha value of the spirit ash.</param>
    public void Draw(Vector2 position, float alpha = 1f)
    {
        var correctedPosition = new Vector2(
            position.X - 6 * AppSettings.Scaling.ScaleFactor,
            position.Y + 6 * AppSettings.Scaling.ScaleFactor);
        
        _game.DrawImage(
            Texture, 
            correctedPosition,
            _scale,
            0,
            false,
            alpha);
    }
}

public class MimicTearAsh : SpiritAsh
{
    public MimicTearAsh(Game g, GameState state, Player owner) : 
        base(g, state, owner)
    {
        Texture = DataManager.GetInstance(_state.Game).MimicTearAsh;
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