using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
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

