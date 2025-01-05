using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Card;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Items;

public abstract class SpiritAsh : Item
{
    public Player Owner { get; set; }
    protected float _scale;
    
    public Texture2D Texture { get; set; }
    
    protected SpiritAsh()
    {
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
    public void Update()
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
        
        DrawImage(
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
    public MimicTearAsh()
    {
        Texture = DataManager.GetInstance().MimicTearAsh;
    }

    public override string ToString()
    {
        return "Mimic Tear";
    }

    protected override void PerformEffect()
    {
        Owner.BuffManager.Buffs.Add(new CardTwiceAsStrongBuff(GetGameState(), Owner.BuffManager));
    }

    public override bool ShouldAIPlay(AiState aiState)
    {
        // TODO: Implement more advanced rules later
        return aiState == AiState.Aggressive;
    }
}

public class JellyfishAsh : SpiritAsh
{
    public JellyfishAsh()
    {
        Texture = DataManager.GetInstance().JellyfishAsh;
    }

    public override string ToString()
    {
        return "Jellyfish";
    }

    protected override void PerformEffect()
    {
        Owner.OpposingPlayer.BuffManager.Debuffs.Add(new PoisonEffectDebuff(
            GetGameState(),
            Owner.OpposingPlayer.BuffManager,
            3,
            10));
    }

    public override bool ShouldAIPlay(AiState aiState)
    {
        return Owner.OpposingPlayer.Health >= Owner.OpposingPlayer.OriginalHealth * 0.75f;
    }
}

public class WolvesAsh : SpiritAsh
{
    public WolvesAsh()
    {
        Texture = DataManager.GetInstance().WolvesAsh;
    }
    
    public override string ToString()
    {
        return "Wolves";
    }
    
    protected override void PerformEffect()
    {
        Owner.MoveSingleCardFromDeckToHand();
    }

    public override bool ShouldAIPlay(AiState aiState)
    {
        bool noHealthFlasksOnLowHealth = Owner.Health <= Owner.OriginalHealth * 0.5f 
                                         && !Owner.DeckStack.Cards.Any(c => c is FlaskOfCrimsonTearsCard);
        bool playerHasLowHealth = Owner.Health <= Owner.OriginalHealth * 0.5f;
        return noHealthFlasksOnLowHealth || playerHasLowHealth;
    }
}
    
    
    
    
    
    