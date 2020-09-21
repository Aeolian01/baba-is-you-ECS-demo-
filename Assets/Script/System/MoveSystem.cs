using Entitas;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(0)]

public class MoveSystem : IExecuteSystem
{
    public void Execute()
    {
        //移动You
        var playerGroup = Context<Default>.AllOf<YouComp, PosComp>();
        foreach (var e in playerGroup)
        {
            var newPos = new Vector2(e.Get<YouComp>().horizontal + e.Get<PosComp>().value.x, e.Get<YouComp>().vertical + e.Get<PosComp>().value.y);
            move(e,newPos);
        }
    }
    private void move(Entity e,Vector2 newPos)
    {
        //在新的位置查看是否与stop push win重合
        List<Entity> overlapEntities;
        //有重叠
        if (GameController.posToEntity.TryGetValue(newPos, out overlapEntities))
        {
            //先检测能不能到达
            //重叠元素中有Stop 不可到达
            if (hasStop(overlapEntities)) return;
            //重叠元素有Edge 不可到达
            if (hasEdge(overlapEntities)) return;
            //然后检测赢了吗
            //重叠元素中有Win 则胜利
            if (hasWin(overlapEntities))
            {
                GameController.Win();
                return;
            }
            //尝试推 
            //递归
            movePush(e,new Vector2(e.Get<YouComp>().horizontal,e.Get<YouComp>().vertical), overlapEntities);
            //移动到新的位置
            //修改两个格子的映射信息
            GameController.posToEntity[e.Get<PosComp>().value].Remove(e);
            e.Modify<PosComp>().SetValue(newPos);
            GameController.posToEntity[e.Get<PosComp>().value].Add(e);
        }
        //没有重叠
        else
        {
            //移动到新位置
            //修改格子映射信息
            GameController.posToEntity[e.Get<PosComp>().value].Remove(e);
            e.Modify<PosComp>().SetValue(newPos);
            GameController.posToEntity[e.Get<PosComp>().value].Add(e);
        }
    }
    private bool hasStop(List<Entity> list)
    {
        foreach (var entity in list)
        {
            if (entity.Has<StopComp>())
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
            if (entity.Has<EdgeComp>())
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
            if (entity.Has<WinComp>())
            {
                return true;
            }
        }
        return false;
    }
    private void movePush(Entity e,Vector2 dir, List<Entity> list)
    {
        var nextPos = e.Get<PosComp>().value+dir;

        foreach (var entity in list)
        {
            if (entity.Has<PushComp>())
            {
                move(e, nextPos);
            }
        }
    }
}

