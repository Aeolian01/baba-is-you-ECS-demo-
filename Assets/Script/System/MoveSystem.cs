using Entitas;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(50)]
public class MoveSystem : ReactiveSystem
{
    public MoveSystem()
    {
        monitors += Context<Default>.AllOf<InputComp>().OnAdded(move).Where(e => !(e.Get<InputComp>().x == 0 
                                                                                       && e.Get<InputComp>().y == 0));
    }

    public void move(List<Entity> entities)
    {
        if (GameData.Timer < 0.12f)
            return;
        GameData.Timer = 0;
        //移动You
        foreach (var e in entities)
        {
            var input = e.Get<InputComp>();
            Debug.Log($"dir:({input.x},{input.y})");
            //Debug.Log($"pos:({e.Get<PosComp>().value.x},{e.Get<PosComp>().value.y})");
            var newPos = new Vector2(input.x + e.Get<PosComp>().value.x, 
                                     input.y + e.Get<PosComp>().value.y);
            move(e, newPos,new Vector2(input.x,input.y));
        }
    }
    private bool move(Entity e, Vector2 newPos,Vector2 dir)
    {
        //在新的位置查看是否与stop push win重合
        List<Entity> overlapEntities;

        if (GameData.Instance.posToEntity == null || GameData.Instance.posToEntity.Count == 0)
            return false;

        //有重叠
        if (GameData.Instance.posToEntity.TryGetValue(newPos, out overlapEntities))
        {
            //先检测能不能到达
            //重叠元素中有Stop 不可到达
            if (hasStop(overlapEntities)) return false;
            //重叠元素有Edge 不可到达
            if (hasEdge(overlapEntities)) return false;

            //然后检测赢了吗   移动之后再检测

            //尝试推走重叠实体
            //递归
            if (movePush(dir, overlapEntities))
            {
                //可以推走
                //或者不用推

                //移动到新的位置
                e.Modify<PosComp>().SetValue(newPos);
                return true;
            }
            else {
                //推不走 那当前实体就过不去
                return false;
            }
        }
        //没有重叠
        else
        {
            e.Modify<PosComp>().SetValue(newPos);
            return true;
        }
    }
    private bool hasStop(List<Entity> list)
    {
        foreach (var entity in list)
        {
            if (entity.Has<PropertyComp>()&&entity.Get<PropertyComp>().name==Name.Properties.Stop)
            {
                return true;
            }
        }
        return false;
    }
    private bool hasEdge(List<Entity> list)
    {
        foreach (var entity in list)
        {
            if (entity.Has<SpriteComp>()&&entity.Get<SpriteComp>().name==Name.SpriteName.Edge)
            {
                return true;
            }
        }
        return false;
    }
    private bool hasWin(List<Entity> list)
    {
        foreach (var entity in list)
        {
            if (entity.Has<PropertyComp>() && entity.Get<PropertyComp>().name == Name.Properties.Win)
            {
                return true;
            }
        }
        return false;
    }
    //尝试移动重叠的所有实体
    private bool movePush(Vector2 dir, List<Entity> list)
    {
        foreach (var entity in list)
        {
            if (entity.Has<PropertyComp>() && entity.Get<PropertyComp>().name == Name.Properties.Push)
            {
                var nextPos = entity.Get<PosComp>().value + dir;
                //这个需要push的实体能推走
                if (move(entity, nextPos,dir))
                {
                    return true;
                }
                //这个实体推不走
                else {
                    return false;
                }
            }
        }

        //没有要push的实体
        return true;
    }
}

