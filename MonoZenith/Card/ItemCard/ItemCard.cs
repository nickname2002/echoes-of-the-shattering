using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Support.Managers;

namespace MonoZenith.Card;

public class ItemCard : Card
{   
    protected ItemCard(Game game, GameState state, Player owner) : 
        base(game, state, owner)
    {
        _owner = owner;
    }
    
    public override void PerformEffect()
    {
        _soundOnPlay.Play();
    }

    public override bool IsAffordable() => true;

    protected override void DrawMetaData()
    {
        return;
    }
}

public class LarvalTearCard : ItemCard
{
    public LarvalTearCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _frontTexture = DataManager.GetInstance(_game).CardLarvalTear;
        _soundOnPlay = DataManager.GetInstance(_game).LarvalTearSound.CreateInstance();
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
    public BaldachinBlessingCard(Game game, GameState state, Player owner) :
        base(game, state, owner)
    {
        _frontTexture = DataManager.GetInstance(_game).CardBaldachinBless;
        _soundOnPlay = DataManager.GetInstance(_game).BaldachinBlessSound.CreateInstance();
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
        _owner.OpposingPlayer.BuffManager.Debuffs.Add(new DamageReductionDebuff(
        _state,
        _owner.OpposingPlayer.BuffManager,
        2,
        50));
    }
}