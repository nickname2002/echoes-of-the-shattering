#nullable enable
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using static MonoZenith.Game;

namespace MonoZenith.Support.Managers;

public class VoiceLineManager
{
    private Queue<SoundEffectInstance>? _voiceQueue = new();
    private SoundEffectInstance? _currentPlayingVoiceLine;

    /// <summary>
    /// Initialize the voice queue.
    /// </summary>
    /// <param name="voiceLines">The voice lines to initialize the queue with.</param>
    public void InitializeVoiceQueue(List<SoundEffectInstance>? voiceLines)
    {
        if (voiceLines == null || voiceLines.Count == 0)
        {
            _voiceQueue = null;
            return;
        }

        _voiceQueue = new Queue<SoundEffectInstance>(voiceLines);
        _currentPlayingVoiceLine = null;
    }

    /// <summary>
    /// Update the starting voice lines.
    /// </summary>
    public void UpdateStartingVoiceLines()
    {
        if (GetGameState().StateType == GameStateType.Paused 
            || _currentPlayingVoiceLine?.State == SoundState.Playing) return;

        if (_voiceQueue is { Count: > 0 })
        {
            PlayNextVoiceLine();
            return;
        }

        GetGameState().StateType = GameStateType.InGame;
    }

    /// <summary>
    /// Update the death voice lines.
    /// </summary>
    public void UpdateDeathVoiceLines()
    {
        if (GetGameState().StateType == GameStateType.Paused 
            || _currentPlayingVoiceLine?.State == SoundState.Playing) return;

        if (_voiceQueue is { Count: > 0 })
        {
            PlayNextVoiceLine(); 
            return;
        }

        GetGameState().StateType = GameStateType.EndGame;
    }

    /// <summary>
    /// Update the victory voice lines.
    /// </summary>
    public void UpdateVictoryVoiceLines()
    {
        if (GetGameState().StateType == GameStateType.Paused 
            || _currentPlayingVoiceLine?.State == SoundState.Playing) return;

        if (_voiceQueue is { Count: > 0 })
        {
            PlayNextVoiceLine();
            return;
        }
        
        GetGameState().StateType = GameStateType.EndGame;
    }
    
    /// <summary>
    /// Play the next voice line in the queue.
    /// </summary>
    public void PlayNextVoiceLine()
    {
        if (_voiceQueue == null || _voiceQueue.Count == 0)
        {
            _currentPlayingVoiceLine = null;
            return;
        }

        _currentPlayingVoiceLine = _voiceQueue.Dequeue();
        _currentPlayingVoiceLine.Play();
    }

    /// <summary>
    /// Pause the currently playing voice line and stop playback.
    /// </summary>
    public void PauseVoiceLines()
    {
        _currentPlayingVoiceLine?.Pause();
    }

    /// <summary>
    /// Resume playback of the current voice line.
    /// </summary>
    public void ResumeVoiceLines()
    {
        _currentPlayingVoiceLine?.Resume();
    }
}