namespace MonoZenith.Support
{
    public enum Screens
    {
        NONE,
        GAME,
        OVERWORLD,
        MAIN_MENU,
        CREDITS
    }

    public enum Region
    {
        Limgrave,
        Liurnia,
        AltusPlateau,
        None
    }
    
    public enum GameStateType
    {
        PlayingStartingVoiceLines,
        InGame,
        PlayingDeathVoiceLines,
        PlayingVictoryVoiceLines,
        EndGame,
        Paused
    }

    public enum Direction
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public enum LoadoutType
    {
        Deck = 0,
        Ashes = 1
    }

    public enum CardType
    {
        Melee = 0,
        Magic = 1,
        Item = 2
    }
}
