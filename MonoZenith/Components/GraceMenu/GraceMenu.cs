using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;

namespace MonoZenith.Components;

public class GraceMenu : Component
{
    private readonly GraceMenuButton _limgraveButton;
    private readonly GraceMenuButton _caelidButton;
    private readonly GraceMenuButton _liurniaButton;
    private readonly GraceMenuButton _leyndellButton;
    private readonly Texture2D _graceMenuBackdrop;
    public bool Hidden;
    
    public GraceMenu(Game g) : base(g, Vector2.Zero, 275, 400)
    {
        Position = new Vector2(50, Game.ScreenHeight / 2 - Height / 2);
        Hidden = true;
        
        _graceMenuBackdrop = DataManager.GetInstance(g).GraceMenuBackdrop;
        
        _limgraveButton = new GraceMenuButton(g, new Vector2(Position.X + 30.7f, Position.Y + 100.4f), 
            DataManager.GetInstance(g).LimgraveButtonTexture, 
            DataManager.GetInstance(g).LimgraveButtonHoverTexture);
        _limgraveButton.SetOnClickAction(() => Game.DebugLog("Limgrave selected"));

        _caelidButton = new GraceMenuButton(g, new Vector2(Position.X + 141.6f, Position.Y + 100.4f), 
            DataManager.GetInstance(g).CaelidButtonTexture, 
            DataManager.GetInstance(g).CaelidButtonHoverTexture);
        _caelidButton.SetOnClickAction(() => Game.DebugLog("Caelid selected"));

        _liurniaButton = new GraceMenuButton(g, new Vector2(Position.X + 30.7f, Position.Y + 230.8f), 
            DataManager.GetInstance(g).LiurniaButtonTexture, 
            DataManager.GetInstance(g).LiurniaButtonHoverTexture);
        _liurniaButton.SetOnClickAction(() => Game.DebugLog("Liurnia selected"));
        
        _leyndellButton = new GraceMenuButton(g, new Vector2(Position.X + 141, Position.Y + 230.8f), 
            DataManager.GetInstance(g).LeyndellTexture2D, 
            DataManager.GetInstance(g).LeyndellButtonHoverTexture);
        _leyndellButton.SetOnClickAction(() => Game.DebugLog("Leyndell selected"));
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