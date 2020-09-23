using Entitas;
using System;
using System.Collections.Generic;

[UnnamedFeature(1)]
public class PosToEntitySystem : IExecuteSystem
{
    public void Execute()
    {
        var _group = Context<Default>.AllOf<PosComp>();
        var dic = GameData.Instance.posToEntity;
        dic.Clear();
        foreach(var e in _group)
        {
            var pos = e.Get<PosComp>().value;
#if UNITY_EDITOR
            var s = e.name.Split(new char[] { '(' });
            e.name = s[0] + $"({(int)pos.x},{(int)pos.y})";
#endif
            List<Entity> entitys;
            if (dic.TryGetValue(pos,out entitys)&&entitys!=null)
            {
                entitys.Add(e);
            }
            else
            {
                entitys = new List<Entity>();
                entitys.Add(e);
                dic[pos] = entitys;
            }
        }
    }
}
