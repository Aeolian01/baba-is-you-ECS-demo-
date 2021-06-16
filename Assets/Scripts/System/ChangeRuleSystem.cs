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
    private Data data;
    public void Execute()
    {
        data = Contexts.Default.GetUnique<DataComp>().data;
        var isList = data.GetEntitiesByTag(Tag.IsWord);
        bool ruleStatus = false;
        foreach (var idx in isList)
        {
            var e = Contexts.Default.GetEntity(idx);
            var isData = e.Get<IsComp>();
            var pos = e.Get<PosComp>().pos;
            //left and right
            var leftWord = data.GetWord(pos + left);
            var rightWord = data.GetWord(pos + right);
            var rule = new Rule(Helper.WordToTag(leftWord), Helper.WordToTag(rightWord), Helper.WordToAsp(rightWord));

            // 规则不同 更新规则
            if (rule != isData.ruleH)
            {
                data.RemoveRule(isData.ruleH);
                data.AddRule(rule);

                //Aspect规则更新
                ruleStatus |= rule.HasRule() || isData.ruleH.HasRule();


                isData.ruleH = rule;
            }


            //up and down
            var upWord = data.GetWord(pos + up);
            var downWord = data.GetWord(pos + down);
            rule = new Rule(Helper.WordToTag(upWord), Helper.WordToTag(downWord), Helper.WordToAsp(downWord));

            // 规则不同 更新规则
            if (rule != isData.ruleV)
            {
                data.RemoveRule(isData.ruleV);
                data.AddRule(rule);

                //Aspect规则更新
                ruleStatus |= rule.HasRule() || isData.ruleV.HasRule();

                isData.ruleV = rule;
            }
        }
        data.RuleChanged = ruleStatus;
    }
}