using Microsoft.Xna.Framework;
using MonoZenith.Screen;
using MonoZenith.Support;

namespace MonoZenith;

public partial class Game
{
    public Screens ActiveScreen;
    private MainMenuScreen _mainMenuScreen;
    private GameScreen _gameScreen;
    private PauseScreen _pauseScreen; 

    /* Initialize game vars and load assets. */
    public void Init()
    {
        // Screen Setup
        SetScreenSize(1600, 900);
        SetWindowTitle("Echoes of the Shattering");

        ActiveScreen = Screens.MAIN_MENU;  
        _mainMenuScreen = new MainMenuScreen(this);
        _gameScreen = new GameScreen(this);
        _pauseScreen = new PauseScreen(this);
    }

    /* Update game logic. */
    public void Update(GameTime deltaTime)
    {
        switch (ActiveScreen)
        {
            case Screens.GAME:
                _gameScreen.Update(deltaTime);
                break;

            case Screens.MAIN_MENU:
                _mainMenuScreen.Update(deltaTime);
                break;
            
            case Screens.PAUSE:
                _pauseScreen.Update(deltaTime);
                break;

            default:
                _gameScreen.Update(deltaTime);
                break;
        }
    }
    
    /* Draw objects/backdrop. */
    public void Draw()
    {
        switch (ActiveScreen)
        {
            case Screens.GAME:
                _gameScreen.Draw();
                break;
            
            case Screens.MAIN_MENU:
                _mainMenuScreen.Draw();
                break;

            case Screens.PAUSE:
                _pauseScreen.Draw();
                break;

            default:
                _gameScreen.Draw();
                break;
        }
    }
}