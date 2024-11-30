#nullable enable
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Players;

namespace MonoZenith.Support.Managers;

public class GameOverManager
{
    private Player? _currentWinner;
    private readonly TransitionComponent _gameOverTransitionComponent;
    private readonly SoundEffectInstance _playerDeathSound;
    private readonly SoundEffectInstance _enemyDeathSound;

    public GameOverManager(Game game)
    {
        var dataManager = DataManager.GetInstance(game);
        _gameOverTransitionComponent = new TransitionComponent(
            game, "YOU DIED", Color.Gold, dataManager.GameOverTransitionComponentFont,
            1f, 3f, 1f, game.BackToMainMenu);

        _playerDeathSound = dataManager.PlayerDeathSound.CreateInstance();
        _enemyDeathSound = dataManager.EnemyDeathSound.CreateInstance();
    }

    /// <summary>
    /// Reset the state of the GameOverManager.
    /// </summary>
    public void InitializeState()
    {
        _currentWinner = null;
        _gameOverTransitionComponent.Reset();
    }

    /// <summary>
    /// Determines the winner and updates the game over state.
    /// </summary>
    /// <param name="player">The player character.</param>
    /// <param name="npc">The non-player character.</param>
    /// <returns>The winning player, or null if there is no winner.</returns>
    public Player? HasWinner(Player player, Player npc)
    {
        if (npc.Health < 1)
            return HandleWin(player, "ENEMY FELLED", Color.Gold, _enemyDeathSound);

        if (player.Health < 1)
            return HandleWin(npc, "YOU DIED", new Color(180, 30, 30), _playerDeathSound);

        return null;
    }

    /// <summary>
    /// Displays the game over message.
    /// </summary>
    public void DisplayGameOverMessage() => _gameOverTransitionComponent.Draw();

    /// <summary>
    /// Updates the transition component.
    /// </summary>
    /// <param name="deltaTime">The time since the last update.</param>
    public void UpdateGameOverTransition(GameTime deltaTime) => _gameOverTransitionComponent.Update(deltaTime);

    private Player HandleWin(Player winner, string message, Color color, SoundEffectInstance soundEffect)
    {
        if (_currentWinner == null)
            soundEffect.Play();

        _currentWinner = winner;
        _gameOverTransitionComponent.Content = message;
        _gameOverTransitionComponent.Color = color;

        return winner;
    }
}