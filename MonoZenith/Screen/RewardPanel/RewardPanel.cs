using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Components.RewardPanel;
using MonoZenith.Engine.Support;
using static MonoZenith.Game;

namespace MonoZenith.Screen.RewardPanel;

public class RewardPanel
{
    private readonly Vector2 _position;
    private readonly Texture2D _rewardContainerTexture;
    private readonly CollectRewardButton _collectRewardButton;
    private Reward _reward;
    private readonly float _scale;
    private bool _rewardCollected;
    
    public RewardPanel()
    {
        _rewardContainerTexture = DataManager.GetInstance().RewardContainer;
        _reward = null;
        _rewardCollected = false;
        _scale = 0.4f * AppSettings.Scaling.ScaleFactor;
        _position = new Vector2(
            ScreenWidth / 2f - _rewardContainerTexture.Width * _scale / 2,
            ScreenHeight / 2f - _rewardContainerTexture.Height * _scale / 2);
        _collectRewardButton = new CollectRewardButton(
            Instance, 
            _position + new Vector2(
                0, _rewardContainerTexture.Height * _scale + 25 * _scale), 
            _scale);
        _collectRewardButton.SetOnClickAction(() =>
        {
            // TODO: Assign reward to player when possible
            _rewardCollected = true;
            BackToMainMenu();
        });
    }

    public void Initialize(Reward reward)
    {
        _reward = reward;
        _rewardCollected = false;
    }

    public void Update(GameTime deltaTime)
    {
        _collectRewardButton.Update(deltaTime);
    }

    public void Draw()
    {
        if (_reward == null) return;
        float rewardScale = _scale * 0.5f;
        
        DrawImage(_rewardContainerTexture, _position, _scale);
        
        DrawText(_reward.RewardName, 
            _position + 
            new Vector2(
                _rewardContainerTexture.Width * _scale / 2f - 
                DataManager.GetInstance().RewardFont.MeasureString(_reward.RewardName).X / 2f, 
                40 * AppSettings.Scaling.ScaleFactor),
            DataManager.GetInstance().RewardFont, Color.White);
        
        DrawImage(_reward.RewardTexture, 
            _position + 
            new Vector2(
                _rewardContainerTexture.Width * _scale / 2f - _reward.RewardTexture.Width * rewardScale / 2f, 
                _rewardContainerTexture.Height * _scale / 2f - _reward.RewardTexture.Height * rewardScale / 2f 
                + 25 * AppSettings.Scaling.ScaleFactor),
            rewardScale);
        
        _collectRewardButton.Draw();
    }
}