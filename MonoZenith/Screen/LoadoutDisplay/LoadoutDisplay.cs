using MonoZenith.Components.LoadoutDisplay;

namespace MonoZenith.Screen.LoadoutDisplay;

public class LoadoutDisplay
{
    private readonly BackToOverworldButton _backToOverworldButton = new();
    
    public void Update()
    {
        _backToOverworldButton.Update();
    }

    public void Draw()
    {
        _backToOverworldButton.Draw();
    }
}