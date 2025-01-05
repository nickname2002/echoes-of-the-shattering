using MonoZenith.Engine.Support;
using MonoZenith.Support.Managers;
using static MonoZenith.Game;

namespace MonoZenith.Card;

public class ItemCard : Card
{   
    protected ItemCard()
    {
        _owner = null;
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
    }
}

public class LarvalTearCard : ItemCard
{
    public LarvalTearCard()
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
    public BaldachinBlessingCard()
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
        GetGameState(),
        _owner.BuffManager,
        1,
        50));
    }
}