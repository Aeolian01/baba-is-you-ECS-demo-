using Entitas;

public static class Name
{
    public enum SpriteName
    {
        Empty,
        Wall,
        Baba,
        Flag,
        Rock,
        WallWord,
        BabaWord,
        FlagWord,
        RockWord,
        IsWord,
        WinWord,
        YouWord,
        PushWord,
        StopWord,
    }

    public enum Objects
    {
        Empty,
        Wall,
        Baba,
        Flag,
        Rock,
    }

    public enum Properties
    {
        None,
        Stop,
        Push,
        Win,
        You,
    }

    public enum ObjectWords
    {
        WallWord,
        BabaWord,
        FlagWord,
        RockWord,
    }

    public enum IsWord
    {
        IsWord
    }

    public enum ProperWords
    {
        WinWord,
        YouWord,
        PushWord,
        StopWord,
    }
}


