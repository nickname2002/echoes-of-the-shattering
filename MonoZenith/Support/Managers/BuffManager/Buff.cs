using Microsoft.Xna.Framework.Audio;
using MonoZenith.Card;
using MonoZenith.Card.AttackCard;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using System;
using System.Collections.Generic;
using System.Linq;

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
    protected readonly int _cardsPlayedOnActivation;
    
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

        _manager.Buffs.Remove(this);
        foreach (Card.Card card in _owner.HandStack.Cards)
            card.Buff = 0;
        
        return true;
    }
}

public class MeleeCardTwiceAsStrongBuff : CardTwiceAsStrongBuff
{
    public MeleeCardTwiceAsStrongBuff(GameState state, BuffManager manager) :
    base(state, manager)
    {
        
    }

    public override void PerformEffect()
    {
        if (BuffRemoved())
            return;

        foreach (Card.Card card in _owner.HandStack.Cards)
        {
            card.Buff = card switch
            {
                MagicCard magicCard => card.Buff,
                AttackCard attackCard => attackCard.Damage,
                _ => card.Buff
            };
        }
    }
}

public class TurnBuff : Buff
{
    public int RoundsLeft;
    protected int _currentRoundNumber;

    public TurnBuff(GameState state, BuffManager manager, int rounds) :
        base(state, manager)
    {
        RoundsLeft = rounds;
        _currentRoundNumber = state.TurnManager.RoundNumber;
    }

    protected bool RoundSwitched()
    {
        return _state.TurnManager.RoundNumber != _currentRoundNumber;
    }

    public override void PerformEffect()
    {
    }

    public override bool BuffRemoved()
    {
        if (RoundsLeft > 0) return false;
        _manager.Buffs.Remove(this);
        return true;
    }
}

public class PoisonEffectDebuff : TurnBuff
{
    private readonly int _damagePercentage;
    private readonly SoundEffect _damageSound;
    
    public PoisonEffectDebuff(GameState state, BuffManager manager, int rounds, int damagePercentage) : 
        base(state, manager, rounds)
    {
        _damagePercentage = damagePercentage;
        
        // TODO: Add more fitting sound effect
        _damageSound = DataManager.GetInstance().DamageSound;
    }
    
    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;
        
        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;
        
        if (_owner == null) return;
        if (_state.TurnManager.CurrentPlayer != _owner) return;
        
        _owner.Health -= _owner.OriginalHealth * _damagePercentage / 100;
        _damageSound.Play();
    }

    public override bool BuffRemoved()
    {
        if (RoundsLeft > 0) return false;
        _manager.Debuffs.Remove(this);
        return true;
    }
}

public class HealingEffectBuff : TurnBuff
{
    private readonly int _healingPercentage;
    private readonly SoundEffect _healingSound;

    public HealingEffectBuff(GameState state, BuffManager manager, int rounds, int healingPercentage) :
        base(state, manager, rounds)
    {
        _healingPercentage = healingPercentage;

        // TODO: Add more fitting sound effect
        _healingSound = DataManager.GetInstance().FlaskCeruleanSound;
    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;

        if (_owner == null) return;
        if (_state.TurnManager.CurrentPlayer != _owner) return;

        _owner.Health = _owner.Health + _owner.OriginalHealth * _healingPercentage / 100 > 100 ?
            100 : _owner.Health + _owner.OriginalHealth * _healingPercentage / 100;
        _healingSound.Play();
    }
}

public class StaminaEffectDebuff : TurnBuff
{
    private readonly int _staminaAmount;
    private readonly SoundEffect _staminaSound;

    public StaminaEffectDebuff(GameState state, BuffManager manager, int rounds, int staminaAmount) :
        base(state, manager, rounds)
    {
        _staminaAmount = staminaAmount;

        // TODO: Add more fitting sound effect
        _staminaSound = DataManager.GetInstance().FlaskCeruleanSound;
    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;

        if (_owner == null) return;
        if (_state.TurnManager.CurrentPlayer != _owner) return;

        _owner.Stamina -= _staminaAmount;
        //_staminaSound.Play();
    }

    public override bool BuffRemoved()
    {
        if (RoundsLeft > 0) return false;
        _manager.Debuffs.Remove(this);
        return true;
    }
}

public class CardStaminaBuff : Buff
{
    protected readonly int _cardsPlayedOnActivation;
    private readonly int _staminaAmount;

    public CardStaminaBuff(GameState state, BuffManager manager, int staminaAmount) :
        base(state, manager)
    {
        _cardsPlayedOnActivation = state.PlayedCards.Count;
        _staminaAmount = staminaAmount;
    }

    public override void PerformEffect()
    {
        if (BuffRemoved())
            return;

        foreach (Card.Card card in _owner.HandStack.Cards)
        {
            if(card is MagicCard)
            {
                continue;
            }
            else if (card is AttackCard attackCard)
            {
                attackCard.StaminaCost = 
                    attackCard.OriginalStaminaCost - _staminaAmount > 0 ? 
                    attackCard.OriginalStaminaCost - _staminaAmount : 0;
            }
        }
    }

    public override bool BuffRemoved()
    {
        if (_state.PlayedCards.Count == _cardsPlayedOnActivation) return false;

        _manager.Buffs.Remove(this);
        foreach (Card.Card card in _owner.HandStack.Cards)
            if (card is AttackCard attackCard)
            {
                attackCard.StaminaCost = attackCard.OriginalStaminaCost;
            }

        return true;
    }
}

public class DamageReductionDebuff : TurnBuff
{
    protected readonly int _reductionPercentage;

    public DamageReductionDebuff(GameState state, BuffManager manager, int rounds, int reductionAmount) :
        base(state, manager, rounds)
    {
        _reductionPercentage = reductionAmount;
    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (CheckEvasionDebuff()) return;
        if (!RoundSwitched()) return;

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;

        foreach (Card.Card card in _owner.HandStack.Cards)
        {
            card.Buff = card switch
            {
                AttackCard attackCard => -attackCard.Damage * _reductionPercentage / 100,
                _ => card.Buff
            };
        }
    }

    public override bool BuffRemoved()
    {
        if (RoundsLeft > 0) return false;

        _manager.Debuffs.Remove(this);
        foreach (Card.Card card in _owner.HandStack.Cards)
            card.Buff = 0;

        return true;
    }

    public bool CheckEvasionDebuff()
    {
        return _owner.BuffManager.Debuffs.OfType<DamageEvasionDebuff>().Any();
    }
}

public class DamageEvasionDebuff : TurnBuff
{
    private int _evasionAmount;
    private int _limitEvasionAmount;

    public DamageEvasionDebuff(GameState state, BuffManager manager, int rounds, int evasionAmount) :
        base(state, manager, rounds)
    {
        _evasionAmount = evasionAmount;
        _limitEvasionAmount = _evasionAmount;
    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        _limitEvasionAmount = _evasionAmount;

        foreach (Card.Card card in _owner.HandStack.Cards)
        {
            card.Buff = card switch
            {
                MagicCard magicCard => -magicCard.Damage,
                AttackCard attackCard => -attackCard.Damage,
                _ => card.Buff
            };
        }
    }

    public override bool BuffRemoved()
    {
        int playedAttackCards = CountCardsByType<AttackCard>(_state.PlayedCards.Cards);
        _evasionAmount = Math.Min(_evasionAmount, _limitEvasionAmount - playedAttackCards);
        if (_evasionAmount > 0) 
            return false;

        _manager.Debuffs.Remove(this);
        foreach (Card.Card card in _owner.HandStack.Cards)
            card.Buff = 0;

        return true;
    }

    public static int CountCardsByType<T>(List<Card.Card> cards) where T : Card.Card
    {
        return cards.OfType<T>().Count();
    }
}

public class ThopsDebuff : DamageReductionDebuff
{
    public ThopsDebuff(GameState state, BuffManager manager, int rounds, int reductionAmount) :
        base(state, manager, rounds, reductionAmount)
    {

    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;

        foreach (Card.Card card in _owner.HandStack.Cards)
        {
            card.Buff = card switch
            {
               MagicCard magicCard => -magicCard.Damage * _reductionPercentage / 100,
                _ => card.Buff
            };
        }
    }
}

public class DamageIncreaseBuff : TurnBuff
{
    private readonly int _increaseAmount;

    public DamageIncreaseBuff(GameState state, BuffManager manager, int rounds, int increaseAmount) :
        base(state, manager, rounds)
    {
        _increaseAmount = increaseAmount;
    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;

        foreach (Card.Card card in _owner.HandStack.Cards)
        {
            card.Buff = card switch
            {
                MagicCard magicCard => _increaseAmount,
                AttackCard attackCard => _increaseAmount,
                _ => card.Buff
            };
        }
    }

    public override bool BuffRemoved()
    {
        if (RoundsLeft > 0) return false;

        _manager.Buffs.Remove(this);
        foreach (Card.Card card in _owner.HandStack.Cards)
            card.Buff = 0;

        return true;
    }
}

public class BloodboonDebuff : TurnBuff
{
    private readonly int _damagePercentage;
    private readonly SoundEffect _damageSound2;
    private readonly SoundEffect _damageSound3;

    public BloodboonDebuff(GameState state, BuffManager manager, int rounds, int damagePercentage) :
        base(state, manager, rounds)
    {
        _damagePercentage = damagePercentage;

        _damageSound2 = DataManager.GetInstance().BloodboonSound2;
        _damageSound3 = DataManager.GetInstance().BloodboonSound3;
    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;

        if (RoundsLeft == 2)
        {
            _damageSound2.Play();
        }
        else
        {
            _damageSound3.Play();
        }

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;

        if (_owner == null) return;
        if (_state.TurnManager.CurrentPlayer != _owner) return;

        _owner.Health -= _owner.OriginalHealth * _damagePercentage / 100;

        float enemyHealth = _owner.OpposingPlayer.Health;
        float orgEnemyHealth = _owner.OpposingPlayer.OriginalHealth;

        _owner.OpposingPlayer.Health = enemyHealth + orgEnemyHealth * _damagePercentage / 100 > 100 ?
            100 : enemyHealth + orgEnemyHealth * _damagePercentage / 100;
    }

    public override bool BuffRemoved()
    {
        if (RoundsLeft > 0) return false;
        _manager.Debuffs.Remove(this);
        return true;
    }
}

public class DestinedDeathDebuff : TurnBuff
{
    public DestinedDeathDebuff(GameState state, BuffManager manager, int rounds) :
        base(state, manager, rounds)
    {
    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;
    }

    public override bool BuffRemoved()
    {
        if (RoundsLeft > 0) return false;
        _owner.OriginalHealth = 100f;
        _manager.Debuffs.Remove(this);
        return true;
    }
}

public class MoonlightDebuff : TurnBuff
{
    public MoonlightDebuff(GameState state, BuffManager manager, int rounds) :
       base(state, manager, rounds)
    {
    }

    public override void PerformEffect()
    {
        if (BuffRemoved()) return;
        if (!RoundSwitched()) return;

        _currentRoundNumber = _state.TurnManager.RoundNumber;
        RoundsLeft--;
    }

    public override bool BuffRemoved()
    {
        if (RoundsLeft > 0) return false;
        _owner.OriginalHealth = 100f;
        _manager.Debuffs.Remove(this);
        return true;
    }
}