#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using static MonoZenith.Game;

namespace MonoZenith.Support.Managers;

public class VoiceLineManager
{
    private Queue<SoundEffectInstance>? _voiceQueue = new();
    private SoundEffectInstance? _currentPlayingVoiceLine;

    /// <summary>
    /// Check if a voice line is currently playing.
    /// </summary>
    public bool IsPlaying => _currentPlayingVoiceLine?.State == SoundState.Playing;
    
    /// <summary>
    /// Check if the voice queue has voice lines.
    /// </summary>
    public bool HasVoiceLines => _voiceQueue?.Count > 0;

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
        if (_currentPlayingVoiceLine?.State == SoundState.Playing) return;

        if (_voiceQueue is { Count: > 0 })
        {
            PlayNextVoiceLine();
            return;
        }

        GetGameState().StateType = GameStateType.InGame;
    }

    /// <summary>
    /// Update the battle start voice lines.
    /// </summary>
    public void UpdateDeathVoiceLines()
    {
        if (_currentPlayingVoiceLine?.State == SoundState.Playing) return;

        if (_voiceQueue is { Count: > 0 })
        {
            PlayNextVoiceLine(); 
            return;
        }

        GetGameState().StateType = GameStateType.EndGame;
    }

    /// <summary>
    /// Update the battle end voice lines.
    /// </summary>
    public void UpdateVictoryVoiceLines()
    {
        if (_currentPlayingVoiceLine?.State == SoundState.Playing) return;

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
    /// Update the voice lines.
    /// </summary>
    public void UpdateVoiceLines()
    {
        if (_currentPlayingVoiceLine?.State == SoundState.Playing) return;

        if (_voiceQueue != null && _voiceQueue.Count > 0)
        {
            PlayNextVoiceLine();
        }
    }
}