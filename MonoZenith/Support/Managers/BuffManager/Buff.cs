using Microsoft.Xna.Framework.Audio;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Support.Managers;

public abstract class Buff
{
    protected GameState _state;
    protected BuffManager _manager;
    protected Player _owner;
    
    protected Buff(GameState state, BuffManager manager)
    {
        _state = state;
        _manager = manager;
        _owner = _manager.Owner;
    }

    /// <summary>
    /// Perform the effect of the buff.
    /// </summary>
    public abstract void PerformEffect();
    
    /// <summary>
    /// Removes the buff under circumstances.
    /// </summary>
    /// <returns>True if the buff was removed; otherwise, false.</returns>
    public abstract bool BuffRemoved();
}

public class CardTwiceAsStrongBuff : Buff
{
    private readonly int _cardsPlayedOnActivation;
    
    public CardTwiceAsStrongBuff(GameState state, BuffManager manager) : 
        base(state, manager)
    {
        _cardsPlayedOnActivation = state.PlayedCards.Count;
    }

    public override void PerformEffect()
    {
        if (BuffRemoved())
            return;
        
        foreach (Card.Card card in _owner.HandStack.Cards)
        {
            card.Buff = card switch
            {
                AttackCard attackCard => attackCard.Damage,
                FlaskOfCeruleanTearsCard flaskCerTearsCard => flaskCerTearsCard.FocusBoost,
                FlaskOfCrimsonTearsCard flaskCrimTearsCard => flaskCrimTearsCard.HealthBoost,
                _ => card.Buff
            };
        }
    }

    public override bool BuffRemoved()
    {
        if (_state.PlayedCards.Count == _cardsPlayedOnActivation) return false;
        
        _manager.Buff = null;
        foreach (Card.Card card in _owner.HandStack.Cards)
            card.Buff = 0;
        
        return true;
    }
}

public class PoisonEffectDebuff : Buff
{
    private readonly int _damagePercentage;
    private readonly SoundEffect _damageSound;
    private int _roundsLeft;
    private int _currentRoundNumber;
    
    public PoisonEffectDebuff(GameState state, BuffManager manager, int damagePercentage, int rounds) : 
        base(state, manager)
    {
        _roundsLeft = rounds;
        _damagePercentage = damagePercentage;
        _currentRoundNumber = state.TurnManager.RoundNumber;
        
        // TODO: Add more fitting sound effect
        _damageSound = DataManager.GetInstance(state.Game).DamageSound;
    }
    
    private bool RoundSwitched()
    {
        return _state.TurnManager.RoundNumber != _currentRoundNumber;
    }
    
    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;
        
        _currentRoundNumber = _state.TurnManager.RoundNumber;
        _roundsLeft--;
        
        if (_owner == null) return;
        if (_state.TurnManager.CurrentPlayer != _owner) return;
        
        _owner.Health -= _owner.OriginalHealth * _damagePercentage / 100;
        _damageSound.Play();
    }

    public override bool BuffRemoved()
    {
        if (_roundsLeft > 0) return false;
        _manager.Buff = null;
        return true;
    }
}

