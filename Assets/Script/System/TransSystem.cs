using Entitas;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(0)]
public class TransSystem : IExecuteSystem
{
    List<Vector2> offsetPos1 = new List<Vector2> { new Vector2(1, 0), new Vector2(0, 1) };
    List<Vector2> offsetPos2 = new List<Vector2> { new Vector2(2, 0), new Vector2(0, 2) };
    public void Execute()
    {
        var _group = Context<Default>.AllOf<ObjectWordsComp, PosComp>();
        List<Entity> entities= new List<Entity>();
        entities.AddRange(_group);
        entities.Sort((x, y) =>
        {
            var posX = x.Get<PosComp>().value;
            var posY = y.Get<PosComp>().value;
            if (posX.x == posY.x)
                return (int)(posX.y - posY.y);
            return (int)(posX.x - posY.x);
        });
        foreach(var e in entities)
        {
            Trans(e);
        }
    }

    void Trans(Entity entity)
    {
        var pos = entity.Get<PosComp>();
        for (int i = 0; i < 2; i++)
        {
            var p1 = pos.value + offsetPos1[i];
            var p2 = pos.value + offsetPos2[i];
            if (GameController.posToEntity.TryGetValue(p1, out var es1) && GameController.posToEntity.TryGetValue(p2, out var es2))
            {
                if (IsTrans(es1, es2, out var e))
                {
                    var g = Context<Default>.AllOf<ObjectComp>().Where(t => t.Get<ObjectComp>().name ==
                                                                             Name.OwordToO(entity.Get<ObjectWordsComp>().name));

                    foreach (var t in g)
                    {
                        if (e == null)
                        {
                            t.Remove<PropertyComp>();
                            continue;
                        }
                        if (e.Has<ObjectWordsComp>())
                            Trans(t, e.Get<ObjectWordsComp>().name);
                        else
                            Trans(t, e.Get<ProperWordsComp>().name);
                    }
                }

            }
        }
    }
    bool IsTrans(List<Entity> es1, List<Entity> es2, out Entity entity)
    {
        bool f1 = false, f2 = false; entity = null;
        foreach (var e in es1)
        {
            if (e.Has<IsWordComp>())
            {
                f1 = true;
                break;
            }
        }
        foreach (var e in es2)
        {
            if (e.Has<ObjectWordsComp>() || e.Has<ProperWordsComp>())
            {
                f2 = true;
                entity = e;
                break;
            }
        }
        if (!(f1 && f2))
        {
            entity = null;
        }
        return true;
    }

    void Trans(Entity e, Name.ObjectWords objectWords)
    {
        e.Modify<ObjectComp>().SetValue(Name.OwordToO(objectWords));
    }
    void Trans(Entity e, Name.ProperWords properWords)
    {
        if (!e.Has<PropertyComp>())
            e.Add<PropertyComp>();
        e.Modify<PropertyComp>().SetValue(Name.PwordToP(properWords));
    }
}

