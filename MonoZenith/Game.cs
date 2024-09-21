using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZenith.Classes.Screen;
using MonoZenith.Components;

namespace MonoZenith;

public partial class Game
{
    private ActiveScreen _activeScreen;
    private GameScreen _gameScreen;
    private PauseScreen _pauseScreen; 
    //Suggestion for later to add pausescreen to GameScreen.cs instead in Game.cs

    /* Initialize game vars and load assets. */
    public void Init()
    {
        
    }

    /* Update game logic. */
    public void Update(GameTime deltaTime)
    {

    }
    
    /* Draw objects/backdrop. */
    public void Draw()
    {
    
    }
}