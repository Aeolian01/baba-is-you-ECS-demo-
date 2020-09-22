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
        Wall,
        Baba,
        Flag,
        Rock,
        Empty,
    }

    public enum Properties
    {
        Win,
        You,
        Push,
        Stop,
        None,
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
    public static Properties PwordToP(ProperWords word)
    {
        return (Properties)word;
    }
    public static Objects OwordToO(ObjectWords word)
    {
        return (Objects)word;
    }
    public static SpriteName OwordToSName(ObjectWords word)
    {
        return (SpriteName)word;
    }
}


