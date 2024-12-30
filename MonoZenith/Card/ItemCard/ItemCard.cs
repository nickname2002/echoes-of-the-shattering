using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith.Card;

public class ItemCard : Card
{   
    protected ItemCard(GameState state, Player owner) : 
        base(state, owner)
    {
        _owner = owner;
    }
    
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
    }

    public override bool IsAffordable() => true;
    public override void UpdateBuffsAndDebuffs()
    {
        UpdateDescription();
    }
    protected override void DrawMetaData()
    {
        return;
    }
}

public class LarvalTearCard : ItemCard
{
    public LarvalTearCard(GameState state, Player owner) :
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardLarvalTear;
        _soundOnPlay = DataManager.GetInstance().LarvalTearSound.CreateInstance();
        _description.Add("Discard all of your cards");
        _description.Add("and draw 5 new cards");
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        Owner.MoveCardsFromHandToReserve();
        Owner.CardsDrawn = true;
        Owner.DrawCardsFromDeck();
    }
}

public class BaldachinBlessingCard : ItemCard
{
    public BaldachinBlessingCard(GameState state, Player owner) :
        base(state, owner)
    {
        _frontTexture = DataManager.GetInstance().CardBaldachinBless;
        _soundOnPlay = DataManager.GetInstance().BaldachinBlessSound.CreateInstance();
        _description.Add("Reduce damage taken");
        _description.Add("by half next turn");
        _description.Add("but take 10 damage");
    }

    public override bool IsAffordable()
    {
        return _owner.Health > 10;
    }

    public override void PerformEffect()
    {
        base.PerformEffect();
        _owner.Health -= 10;
        _owner.BuffManager.Buffs.RemoveAll(x => x.GetType() == typeof(DamageReductionBuff));
        _owner.BuffManager.Buffs.Add(new DamageReductionBuff(
        _state,
        _owner.BuffManager,
        1,
        50));
    }
}