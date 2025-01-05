using Microsoft.Xna.Framework;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen.LoadoutDisplay;

public class LoadoutDisplay
{
    private readonly ImageButton _backToOverworldButton;

    // ReSharper disable once ConvertConstructorToMemberInitializers
    public LoadoutDisplay()
    {
        _backToOverworldButton = new ImageButton(
            new Vector2(30 * AppSettings.Scaling.ScaleFactor, 30 * AppSettings.Scaling.ScaleFactor),
            LoadImage("Images/LoadoutDisplay/Buttons/to-overworld.png"),
            scale: 0.18f,
            onClickAction: () =>
            {
                DataManager.GetInstance().EndPlayerTurnSound.CreateInstance().Play();
                ShowLoadoutDisplay(false);
            }
        );
    }
        
    public void Update()
    {
        _backToOverworldButton.Update(DeltaTime);
    }

    public void Draw()
    {
        _backToOverworldButton.Draw();
    }
}