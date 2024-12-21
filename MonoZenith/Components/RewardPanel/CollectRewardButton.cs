using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoZenith.Engine.Support;

namespace MonoZenith.Components.RewardPanel;

public class CollectRewardButton : Button
{
    private readonly Texture2D _texture;
    private readonly Texture2D _hoverTexture;
    private Texture2D _currentTexture;
    private readonly SoundEffectInstance _rewardCollectedSound;
    private readonly float _textureScale;
    
    public CollectRewardButton(
        Game g, 
        Vector2 pos,
        float scale = 1f) 
        : base(g, pos, 0, 0, "", 0, Color.Black, Color.Black, 0, Color.Black)
    {
        _texture = DataManager.GetInstance().CollectRewardButton;
        _hoverTexture = DataManager.GetInstance().CollectRewardButtonHover;
        _rewardCollectedSound = DataManager.GetInstance().EndPlayerTurnSound.CreateInstance();
        _currentTexture = _texture;
        _textureScale = scale;
    }

    private void UpdateDimensions()
    {
        Width = (int)(_currentTexture.Width * _textureScale);
        Height = (int)(_currentTexture.Height * _textureScale);
    }
    
    private void DetermineCurrentTexture()
    {
        _currentTexture = IsHovered() ? _hoverTexture : _texture;
        UpdateDimensions();
    }

    public override void SetOnClickAction(Action a)
    {
        void RewardCollectedEvent()
        {
            _rewardCollectedSound.Play();
            a();
        }
        
        base.SetOnClickAction(RewardCollectedEvent);
    }

    public override void Update(GameTime deltaTime)
    {
        base.Update(deltaTime);
        DetermineCurrentTexture();
    }

    public override void Draw()
    {
        Game.DrawImage(_currentTexture, Position, _textureScale);
    }
}