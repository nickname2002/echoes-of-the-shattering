using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith.Items;

public abstract class SpiritAsh : Item
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
        PerformEffect();
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
        _owner.BuffManager.Buffs.Add(new CardTwiceAsStrongBuff(_state, _owner.BuffManager));
    }

    public override bool ShouldAIPlay(AiState aiState)
    {
        // TODO: Implement more advanced rules later
        return aiState == AiState.Aggressive;
    }
}

public class JellyfishAsh : SpiritAsh
{
    public JellyfishAsh(Game g, GameState state, Player owner) :
        base(g, state, owner)
    {
        Texture = DataManager.GetInstance(_state.Game).JellyfishAsh;
    }
    
    protected override void PerformEffect()
    {
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new PoisonEffectDebuff(
            _state,
            _owner.OpposingPlayer.BuffManager,
            3,
            10));
    }

    public override bool ShouldAIPlay(AiState aiState)
    {
        return _owner.OpposingPlayer.Health >= _owner.OpposingPlayer.OriginalHealth * 0.75f;
    }
}

public class WolvesAsh : SpiritAsh
{
    public WolvesAsh(Game g, GameState state, Player owner) : 
        base(g, state, owner)
    {
        Texture = DataManager.GetInstance(_state.Game).WolvesAsh;
    }

    protected override void PerformEffect()
    {
        _owner.MoveSingleCardFromDeckToHand();
    }

    public override bool ShouldAIPlay(AiState aiState)
    {
        bool noHealthFlasksOnLowHealth = _owner.Health <= _owner.OriginalHealth * 0.5f 
                            && !_owner.DeckStack.Cards.Any(c => c is FlaskOfCrimsonTearsCard);
        bool playerHasLowHealth = _owner.Health <= _owner.OriginalHealth * 0.5f;
        return noHealthFlasksOnLowHealth || playerHasLowHealth;
    }
}
    
    
    
    
    
    