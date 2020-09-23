using Entitas;
using System;

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
        Edge,
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
        var w = word.ToString();
        foreach (int p in Enum.GetValues(typeof(Properties)))
        {
            string strName = Enum.GetName(typeof(Properties), p);
            if (w.Contains(strName))
                return (Properties)p;
        }
        return (Properties)word;
    }

    public static Objects OwordToO(ObjectWords word)
    {
        var w = word.ToString();
        foreach (int p in Enum.GetValues(typeof(Objects)))
        {
            string strName = Enum.GetName(typeof(Objects), p);
            if (w.Contains(strName))
                return (Objects)p;
        }
        return (Objects)word;
    }

    public static SpriteName OToSName(ObjectWords word)
    {
        var w = OwordToO(word).ToString();
        foreach (int p in Enum.GetValues(typeof(SpriteName)))
        {
            string strName = Enum.GetName(typeof(SpriteName), p);
            if (w.Equals(strName))
                return (SpriteName)p;
        }
        return (SpriteName)word;
    }

    public static SpriteName OToSName(Objects word)
    {
        var w = word.ToString();
        foreach (int p in Enum.GetValues(typeof(SpriteName)))
        {
            string strName = Enum.GetName(typeof(SpriteName), p);
            if (w.Equals(strName))
                return (SpriteName)p;
        }
        return (SpriteName)word;
    }
}


