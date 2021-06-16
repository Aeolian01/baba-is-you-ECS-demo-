using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(100)]
public class TransformSystem : IExecuteSystem, IInitializeSystem
{

    private Array _AspectsEnum = Enum.GetValues(typeof(Aspects));
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

        data.AddRule(new Rule(Tag.BabaWord, Aspects.Push));
        data.AddRule(new Rule(Tag.FlagWord, Aspects.Push));
        data.AddRule(new Rule(Tag.IsWord, Aspects.Push));
        data.AddRule(new Rule(Tag.PushWord, Aspects.Push));
        data.AddRule(new Rule(Tag.RockWord, Aspects.Push));
        data.AddRule(new Rule(Tag.StopWord, Aspects.Push));
        data.AddRule(new Rule(Tag.WallWord, Aspects.Push));
        data.AddRule(new Rule(Tag.WinWord, Aspects.Push));
        data.AddRule(new Rule(Tag.YouWord, Aspects.Push));
        data.AddRule(new Rule(Tag.Edge, Aspects.Stop));
        data.RuleChanged = true;
    }
    public void Execute()
    {
        data = Contexts.Default.GetUnique<DataComp>().data;
        TransformTag();
        TransformAspect();
    }

    /// <summary>
    /// 物体转物体 
    /// 可优化：对”IsWord"排序 在单帧内完成转变 
    /// </summary>
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
    /// <summary>
    /// 物体属性转变
    /// 可优化：每个属性单独标记是否改变
    /// </summary>
    void TransformAspect()
    {
        if (data.RuleChanged)
        {
            foreach (Aspects asp in _AspectsEnum)
            {
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
            data.RuleChanged = false;
        }
    }
}