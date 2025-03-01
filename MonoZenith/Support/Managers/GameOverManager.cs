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
    private Player? _secretWinner;
    private readonly TransitionComponent _gameOverTransitionComponent;
    private readonly SoundEffectInstance _playerDeathSound;
    private readonly SoundEffectInstance _enemyDeathSound;
    private readonly RewardPanel _rewardPanel;
    
    public bool TransitionComplete { get; set; }
    public Player? Winner => _currentWinner;

    public GameOverManager()
    {
        var dataManager = DataManager.GetInstance();
        TransitionComplete = false;
        _playerDeathSound = dataManager.PlayerDeathSound.CreateInstance();
        _enemyDeathSound = dataManager.EnemyDeathSound.CreateInstance();
        var newItemSound = dataManager.NewItemSound.CreateInstance();
        _gameOverTransitionComponent = new TransitionComponent(
            "YOU DIED", Color.Gold, dataManager.GameOverTransitionComponentFont,
            1f, 3f, 1f, () =>
            {
                if (_secretWinner is HumanPlayer) TryLoadSecondPhase();
                if (_currentWinner is NpcPlayer)
                {
                    BackToOverworld();
                    return;
                }
                
                if ((RewardCollected() || LevelHasNoReward())
                    && LevelHasNoSecondPhase())
                {
                    BackToOverworld();
                    return;
                }
                
                TransitionComplete = true;
                
                if (_rewardPanel?.Reward != null)
                    newItemSound.Play();
            });
        _rewardPanel = new RewardPanel();
    }
    
    private bool LevelHasNoSecondPhase() => LevelManager.CurrentLevel.SecondPhase == null;

    private bool LevelHasNoReward() => GetGameState().CurrentLevel?.LevelReward == null;
    
    private bool RewardCollected()
    {
        bool inSecondPhase = 
            LevelManager.CurrentLevel.SecondPhase != null 
            && GetGameState().CurrentLevel == LevelManager.CurrentLevel.SecondPhase;
        bool inSecondPhaseAndCollected = 
            LevelManager.CurrentLevel.SecondPhase != null
            && inSecondPhase
            && LevelManager.CurrentLevel.SecondPhase.RewardCollected;
        bool hasNoSecondPhaseAndCollected = 
            LevelManager.CurrentLevel.SecondPhase == null
            && LevelManager.CurrentLevel.RewardCollected;
        
        return inSecondPhaseAndCollected || hasNoSecondPhaseAndCollected;
    }
    
    /// <summary>
    /// Reset the state of the GameOverManager.
    /// </summary>
    /// <param name="reward">Reward the player will receive if they win.</param>>
    public void InitializeState(Reward? reward)
    {
        _currentWinner = null;
        _secretWinner = null;
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
    
    /// <summary>
    /// Configure transition properties for transitioning from the level to the reward/main menu.
    /// </summary>
    /// <param name="message">Message to display.</param>
    /// <param name="color">Color of the message.</param>
    private void ConfigureTransitionDefault(string message, Color color)
    {
        _gameOverTransitionComponent.Content = message;
        _gameOverTransitionComponent.Color = color;
    }
    
    /// <summary>
    /// Configure transition properties for transitioning from the first to second phase.
    /// </summary>
    private void ConfigureTransitionForSecondPhase()
    {
        _gameOverTransitionComponent.Content = "";
        _gameOverTransitionComponent.Color = Color.White;
        _gameOverTransitionComponent.SetTempTransitionTimers(
            fadeInDuration: 0f, displayDuration: 0f, fadeOutDuration: 0f);
    }
    
    private Player HandleWin(Player winner, string message, Color color, SoundEffectInstance soundEffect)
    {
        bool inFirstPhase = 
            LevelManager.CurrentLevel.SecondPhase != null
            && GetGameState().CurrentLevel != LevelManager.CurrentLevel.SecondPhase;

        if (inFirstPhase && winner is HumanPlayer)
        {
            ConfigureTransitionForSecondPhase();
            _secretWinner = winner;
            return winner;
        }

        if (winner is HumanPlayer)
        {
            LevelManager.SetNextLevelUnlocked(LevelManager.CurrentLevel);
            SaveGame();
        }

        if (GetGameState().StateType != GameStateType.EndGame)
            return winner;
        
        if (_currentWinner == null)
            soundEffect.Play();
        
        ConfigureTransitionDefault(message, color);
        return _currentWinner = winner;
    }
}