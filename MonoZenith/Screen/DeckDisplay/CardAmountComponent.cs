using Microsoft.Xna.Framework;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen.DeckDisplay;

public class CardAmountComponent
{
    public readonly Card.Card Card;
    private readonly ImageButton _addButton;
    private readonly ImageButton _subtractButton;

    public float Scale = 0.15f * AppSettings.Scaling.ScaleFactor;
    public Vector2 Position { get; set; }
    public int Amount { get; set; }
    
    public CardAmountComponent(Card.Card card)
    {
        var pos = new Vector2(0, 0);
        Card = card;
        Amount = 0;
        Card.Position = pos;
        
        _addButton = new ImageButton(
            new Vector2(pos.X + MonoZenith.Card.Card.Width - 10 * AppSettings.Scaling.ScaleFactor, pos.Y + MonoZenith.Card.Card.Height),
            LoadImage("Images/Miscellaneous/Symbols/+.png"),
            scale: Scale * AppSettings.Scaling.ScaleFactor * 0.75f,
            onClickAction: () =>
            {
                if (DeckDisplay.GetAmountOfSelectedCards() < 30)
                {
                    Amount++;
                }
            }
        );
        
        _subtractButton = new ImageButton(
            new Vector2(pos.X + MonoZenith.Card.Card.Width - 10 * AppSettings.Scaling.ScaleFactor, pos.Y + MonoZenith.Card.Card.Height + 20 * 
                AppSettings.Scaling.ScaleFactor),
            LoadImage("Images/Miscellaneous/Symbols/-.png"),
            scale: Scale * AppSettings.Scaling.ScaleFactor * 0.75f,
            onClickAction: () =>
            {
                if (Amount > 0)
                {
                    Amount--;
                }
            }
        );
    }
    
    private void ChangePositions()
    {
        Card.Position = Position;
        _addButton.SetPosition(new Vector2(
            Position.X + MonoZenith.Card.Card.Width - (50 * AppSettings.Scaling.ScaleFactor), 
            Position.Y + MonoZenith.Card.Card.Height - 10 * AppSettings.Scaling.ScaleFactor));
        _subtractButton.SetPosition(new Vector2(
            Position.X - (10 * AppSettings.Scaling.ScaleFactor), 
            Position.Y + MonoZenith.Card.Card.Height - 10 * AppSettings.Scaling.ScaleFactor));
    }
    
    public void Update(GameTime deltaTime)
    {
        ChangePositions();
        Scale = 0.15f * AppSettings.Scaling.ScaleFactor;
        _addButton.Update(deltaTime);
        _subtractButton.Update(deltaTime);
    }

    public void Draw()
    {
        Card.Draw(active: true);
        _addButton.Draw();
        _subtractButton.Draw();
        
        // Draw amount of cards centered below the card
        int widthOfAmount = DataManager.GetInstance().CardAmountFont.MeasureString(Amount.ToString()).ToPoint().X;
        DrawText(
            Amount.ToString(), 
            new Vector2(Position.X + MonoZenith.Card.Card.Width / 2f - widthOfAmount / 2f, Position.Y + MonoZenith.Card.Card.Height), 
            DataManager.GetInstance().CardAmountFont, Color.Gold);
    }
}