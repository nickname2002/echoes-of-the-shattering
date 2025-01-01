﻿namespace MonoZenith.Support
{
    public enum Screens
    {
        NONE,
        GAME,
        OVERWORLD,
        PAUSE,
        MAIN_MENU
    }

    public enum Region
    {
        Limgrave,
        Liurnia,
        AltusPlateau,
        None
    }

    public enum CardLabel
    {
        A,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        JOKER,
        GRACE,
        POWER
    }
    
    public enum GameStateType
    {
        PlayingStartingVoiceLines,
        InGame,
        PlayingDeathVoiceLines,
        PlayingVictoryVoiceLines,
        EndGame
    }

    public enum Direction
    {
        Top,
        Right,
        Bottom,
        Left
    }
}
