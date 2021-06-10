using Entitas;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public static class Helper
{

    public static IGroup GetGroupByType<T>(T t) where T : IComponent
    {
        return Context<Default>.AllOf<T>();
    }

    public static bool CheckWin()
    {
        //var win = Data.WinList;
        //var you = Data.YouList;
        //foreach (var idx1 in win)
        //{
        //    var Pos = Contexts.Default.GetEntity(idx1).Get<Pos>();
        //    foreach (var idx2 in you)
        //    {
        //        if (Contexts.Default.GetEntity(idx2).Get<Pos>().Equals(Pos))
        //            return true;
        //    }
        //}
        return false;
    }

    public static Aspects WordToAsp(Tag mat)
    {
        if ((int)mat <= 200)
            return (Aspects)0;
        return (Aspects)(1 << ((int)mat - 201));
    }

    public static Tag WordToTag(Tag mat)
    {
        if ((int)mat <= 100 || (int)mat > 200)
            return (Tag)0;
        return (Tag)((int)mat - 100);
    }
    public static List<int> GetList(Tag tag)
    {
        if (!Data.GetTagPool().TryGetValue((int)tag, out var list))
        {
            list = new List<int>();
            Data.GetTagPool().Add((int)tag, list);
        }
        return list;
    }

    public static List<int> GetList(Aspects asp)
    {
        if (!Data.GetAspPool().TryGetValue((int)asp, out var list))
        {
            list = new List<int>();
            Data.GetAspPool().Add((int)asp, list);
        }
        return list;
    }
    /// <summary>
    /// 所有包括 xx（Aspect)规则的 Tag
    /// </summary>
    /// <param name="aspects"></param>
    /// <returns></returns>
    public static IEnumerable<Tag> AspectRuleToTagList(Aspects aspects)
    {
        return Data.GetRules().Where(x => x.CheckAspectRule(aspects)).Select(x => x.GetTag());
    }

    public static void SetEntityName(Entity e)
    {
#if UNITY_EDITOR
        var posComp = e.Get<PosComp>();
        var tagComp = e.Get<TagComp>();

        e.name = $"{posComp.Pos} {tagComp.tag}";
#endif
    }
}
