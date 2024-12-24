#nullable enable
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoZenith.Components;
using MonoZenith.Engine.Support;
using MonoZenith.Players;
using MonoZenith.Screen.RewardPanel;
using static MonoZenith.Game;

namespace MonoZenith.Support.Managers;

public class GameOverManager
{
    private Player? _currentWinner;
    private readonly TransitionComponent _gameOverTransitionComponent;
    private readonly SoundEffectInstance _playerDeathSound;
    private readonly SoundEffectInstance _enemyDeathSound;
    private readonly RewardPanel _rewardPanel;
    
    public bool TransitionComplete { get; set; }
    public Player? Winner => _currentWinner;

    public GameOverManager(Game game)
    {
        var dataManager = DataManager.GetInstance();
        TransitionComplete = false;
        _playerDeathSound = dataManager.PlayerDeathSound.CreateInstance();
        _enemyDeathSound = dataManager.EnemyDeathSound.CreateInstance();
        var newItemSound = dataManager.NewItemSound.CreateInstance();
        _gameOverTransitionComponent = new TransitionComponent(
            game, "YOU DIED", Color.Gold, dataManager.GameOverTransitionComponentFont,
            1f, 3f, 1f, () =>
            {
                if (_currentWinner is HumanPlayer) TryLoadSecondPhase();
                if (_currentWinner is NpcPlayer)
                {
                    BackToMainMenu();
                    return;
                }

                if (_rewardPanel?.Reward == null && 
                    LevelManager.CurrentLevel.SecondPhase == GetGameState().CurrentLevel)
                {
                    BackToMainMenu();
                    return;
                }
                
                TransitionComplete = true;
                
                if (_rewardPanel?.Reward != null)
                    newItemSound.Play();
            });
        _rewardPanel = new RewardPanel();
    }

    /// <summary>
    /// Reset the state of the GameOverManager.
    /// </summary>
    /// <param name="reward">Reward the player will receive if they win.</param>>
    public void InitializeState(Reward? reward)
    {
        _currentWinner = null;
        TransitionComplete = false;
        _gameOverTransitionComponent.Reset();
        _rewardPanel.Initialize(reward);
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

    public void UpdateRewardPanel(GameTime deltaTime) => _rewardPanel.Update(deltaTime);
    
    public void UpdateTransitionComponent(GameTime deltaTime) => _gameOverTransitionComponent.Update(deltaTime);
    
    public void DrawRewardPanel() => _rewardPanel.Draw();
    
    public void DrawTransitionComponent() => _gameOverTransitionComponent.Draw();

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