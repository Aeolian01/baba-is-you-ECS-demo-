using Entitas;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(200)]
public class ChangeRuleSystem : IExecuteSystem
{
    static Vector2Int down = new Vector2Int(1, 0);
    static Vector2Int up = new Vector2Int(-1, 0);
    static Vector2Int right = new Vector2Int(0, 1);
    static Vector2Int left = new Vector2Int(0, -1);
    public void Execute()
    {
        var isList = Data.GetEntitiesByTag(Tag.IsWord);
        bool ruleStatus = false;
        foreach (var idx in isList)
        {
            var e = Contexts.Default.GetEntity(idx);
            var isData = e.Get<IsComp>();
            var pos = e.Get<PosComp>().Pos;
            //left and right
            var leftWord = Data.GetWord(pos + left);
            var rightWord = Data.GetWord(pos + right);
            var rule = new Rule(Helper.WordToTag(leftWord), Helper.WordToTag(rightWord), Helper.WordToAsp(rightWord));

            // 规则不同 更新规则
            if (rule != isData.ruleH)
            {
                Data.RemoveRule(isData.ruleH);
                Data.AddRule(rule);

                //Aspect规则更新
                ruleStatus |= rule.HasAspectRule() || isData.ruleH.HasAspectRule();

                isData.ruleH = rule;
            }


            //up and down
            var upWord = Data.GetWord(pos + up);
            var downWord = Data.GetWord(pos + down);
            rule = new Rule(Helper.WordToTag(upWord), Helper.WordToTag(downWord), Helper.WordToAsp(downWord));

            // 规则不同 更新规则
            if (rule != isData.ruleV)
            {
                Data.RemoveRule(isData.ruleV);
                Data.AddRule(rule);

                //Aspect规则更新
                ruleStatus |= rule.HasAspectRule() || isData.ruleV.HasAspectRule();

                isData.ruleV = rule;
            }
        }
        Data.RuleChanged = ruleStatus;
    }
}