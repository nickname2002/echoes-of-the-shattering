using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Screen;
using MonoZenith.Support;

namespace MonoZenith;

public partial class Game
{
    private Screens _activeScreen;
    private GameScreen _gameScreen;
    private PauseScreen _pauseScreen; 

    /* Initialize game vars and load assets. */
    public void Init()
    {
        // Screen Setup
        SetScreenSize(1600, 900);
        SetBackgroundColor(Color.Gray);
        SetWindowTitle("Echoes of the Shattering");

        _activeScreen = Screens.GAME;  
        _gameScreen = new GameScreen(this);
        _pauseScreen = new PauseScreen(this);
    }

    /* Update game logic. */
    public void Update(GameTime deltaTime)
    {
        switch (_activeScreen)
        {
            case Screens.GAME:
                _gameScreen.Update(deltaTime);
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
        switch (_activeScreen)
        {
            case Screens.GAME:
                _gameScreen.Draw();
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