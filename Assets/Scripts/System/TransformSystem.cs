using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

//Systemִ��˳��Խ�����ȼ�Խ�ߣ�
[UnnamedFeature(100)]
public class TransformSystem : IExecuteSystem, IInitializeSystem
{

    private Array _AspectsEnum = Enum.GetValues(typeof(Aspects));

    //��ʼ�� ��ӻ�������
    public void Initialize()
    {
        for (int i = 0; i < Data.Width; i++)
        {
            for (int ii = 0; ii < Data.Height; ii++)
            {
                foreach (var idx in Data.GetMapNodeList(i, ii))
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
                    Data.AddEntityID(idx, tagComp.tag);
                }
            }
        }

        Data.AddRule(new Rule(Tag.BabaWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.FlagWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.IsWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.PushWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.RockWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.StopWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.WallWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.WinWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.YouWord, Aspects.Push));
        Data.AddRule(new Rule(Tag.Edge, Aspects.Stop));
        Data.RuleChanged = true;
    }
    public void Execute()
    {
        TransformTag();
        TransformAspect();
    }

    /// <summary>
    /// ����ת���� 
    /// ���Ż����ԡ�IsWord"���� �ڵ�֡�����ת�� 
    /// </summary>
    void TransformTag()
    {
        foreach (var rule in Data.GetRules())
        {
            if (rule.HasTagRule())
            {
                var tagRule = rule.GetTagRule();
                var tagList = Data.GetEntitiesByTag(tagRule);
                if (tagList.Count == 0)
                {
                    continue;
                }
                var ruleList = Data.GetEntitiesByTag(tagRule);
                foreach (var idx in tagList)
                {
                    var e = Contexts.Default.GetEntity(idx);
                    var tagComp = e.Get<TagComp>();
                    tagComp?.SetValue(tagRule);
                    Helper.SetEntityName(e);
                    Data.GameObjectChangedEvent(idx);
                }
                ruleList.AddRange(tagList);
                tagList.Clear();
            }
        }
    }
    /// <summary>
    /// ��������ת��
    /// ���Ż���ÿ�����Ե�������Ƿ�ı�
    /// </summary>
    void TransformAspect()
    {
        if (Data.RuleChanged)
        {
            foreach (Aspects asp in _AspectsEnum)
            {
                Data.GetEntitiesByAspect(asp).Clear();
                var tags = Helper.AspectRuleToTagList(asp);
                foreach (var tag in tags)
                {
                    Data.GetEntitiesByAspect(asp).AddRange(Helper.GetList(tag));
                }
            }
            Data.RuleChanged = false;
        }
    }
}