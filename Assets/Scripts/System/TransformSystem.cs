using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(100)]
public class TransformSystem : IExecuteSystem, IInitializeSystem
{
    private Data data;

    //初始化 添加基础规则
    public void Initialize()
    {
        data = Contexts.Default.GetUnique<DataComp>().data;
        for (int i = 0; i < data.Width; i++)
        {
            for (int ii = 0; ii < data.Height; ii++)
            {
                foreach (var idx in data.GetMapNodeList(i, ii))
                {
                    var e = Contexts.Default.GetEntity(idx);
                    var tagComp = e.Get<TagComp>();
                    if (tagComp == null)
                    {
                        tagComp = e.Add<TagComp>();
                    }
                    if (tagComp.tag == Tag.IsWord)
                    {
                        e.Add<IsComp>();
                    }
                    data.AddEntityID(idx, tagComp.tag);
                }
            }
        }

        data.AddRule(new Rule(Tag.BabaWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.FlagWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.IsWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.PushWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.RockWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.StopWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.WallWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.WinWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.YouWord, Aspects.Push, Vector2Int.zero));
        data.AddRule(new Rule(Tag.Edge, Aspects.Stop, Vector2Int.zero));
    }
    public void Execute()
    {
        data = Contexts.Default.GetUnique<DataComp>().data;
        if (data.RuleChanged)
        {
            TransformTag();
            TransformAspect();
        }
        data.ResetRuleChanged();
    }

    void TransformTag()
    {
        foreach (var rule in data.GetRules())
        {
            if (rule.HasTagRule())
            {
                var tag = rule.GetTag();
                var tagRule = rule.GetTagRule();
                var tagList = data.GetEntitiesByTag(tag);
                if (tagList.Count == 0)
                {
                    continue;
                }
                foreach (var idx in tagList)
                {
                    var e = Contexts.Default.GetEntity(idx);
                    var tagComp = e.Get<TagComp>();
                    tagComp?.SetValue(tagRule);
                    Helper.SetEntityName(e);
                    data.GameObjectChangedEvent(idx);
                    data.AddEntityID(idx, tagRule);
                }
                data.Clear(tag);
            }
        }
    }

    void TransformAspect()
    {
        foreach (var i in data.GetChangedAspRules())
        {
            var asp = (Aspects)i;
            data.Clear(asp); ;
            var tags = Helper.AspectRuleToTagList(asp);
            foreach (var tag in tags)
            {
                foreach (var idx in data.GetEntitiesByTag(tag))
                {
                    data.AddEntityID(idx, asp);
                }
            }
        }
    }
}