using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;
using MonoZenith.Support;

namespace MonoZenith.Components;

public class GraceMenu : Component
{
    private GameState _state;
    private readonly GraceMenuButton _limgraveButton;
    private readonly GraceMenuButton _caelidButton;
    private readonly GraceMenuButton _liurniaButton;
    private readonly GraceMenuButton _leyndellButton;
    private readonly Texture2D _graceMenuBackdrop;
    public bool Hidden;
    
    public GraceMenu(Game g, GameState state) : base(g, Vector2.Zero, 275, 400)
    {
        _state = state;
        Position = new Vector2(50, Game.ScreenHeight / 2 - Height / 2);
        Hidden = true;
        
        _graceMenuBackdrop = DataManager.GetInstance(g).GraceMenuBackdrop;
        
        _limgraveButton = new GraceMenuButton(g, new Vector2(Position.X + 30.7f, Position.Y + 100.4f), 
            DataManager.GetInstance(g).LimgraveButtonTexture, 
            DataManager.GetInstance(g).LimgraveButtonHoverTexture);
        _limgraveButton.SetOnClickAction(() => SetRegionActive(Region.LIMGRAVE));

        _caelidButton = new GraceMenuButton(g, new Vector2(Position.X + 141.6f, Position.Y + 100.4f), 
            DataManager.GetInstance(g).CaelidButtonTexture, 
            DataManager.GetInstance(g).CaelidButtonHoverTexture);
        _caelidButton.SetOnClickAction(() => SetRegionActive(Region.CAELID));

        _liurniaButton = new GraceMenuButton(g, new Vector2(Position.X + 30.7f, Position.Y + 230.8f), 
            DataManager.GetInstance(g).LiurniaButtonTexture, 
            DataManager.GetInstance(g).LiurniaButtonHoverTexture);
        _liurniaButton.SetOnClickAction(() => SetRegionActive(Region.LIURNIA));
        
        _leyndellButton = new GraceMenuButton(g, new Vector2(Position.X + 141, Position.Y + 230.8f), 
            DataManager.GetInstance(g).LeyndellTexture2D, 
            DataManager.GetInstance(g).LeyndellButtonHoverTexture);
        _leyndellButton.SetOnClickAction(() => SetRegionActive(Region.LEYNDELL));
    }

    /// <summary>
    /// Sets new region active upon the click of a grace menu button.
    /// </summary>
    /// <param name="activeRegion">New active region.</param>
    private void SetRegionActive(Region activeRegion)
    {
        _state.CurrentRegion = activeRegion;
        Console.WriteLine($"Player selected region: {activeRegion}");
        Hidden = true;
        _state.SwitchTurn();
    }
    
    public override void Update(GameTime deltaTime)
    {
        if (Hidden)
            return;
        
        _limgraveButton.Update(deltaTime);
        _caelidButton.Update(deltaTime);
        _liurniaButton.Update(deltaTime);
        _leyndellButton.Update(deltaTime);
    }

    public override void Draw()
    {
        if (Hidden)
            return;
        
        Game.DrawImage(_graceMenuBackdrop, Position);
        _limgraveButton.Draw();
        _caelidButton.Draw();
        _liurniaButton.Draw();
        _leyndellButton.Draw();
    }
}