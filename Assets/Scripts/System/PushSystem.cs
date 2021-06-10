using Entitas;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(80)]
public class PushSystem : IExecuteSystem
{
    private HashSet<int> _moveTargetsList = new HashSet<int>();
    private HashSet<int> _tempTargetsList = new HashSet<int>();
    public void Execute()
    {
        if (Data.Timer < Data.Interval)
            return;
        Data.Timer = 0;
        _moveTargetsList.Clear();
        Vector2Int dir = Vector2Int.zero;
        foreach (var idx in Data.GetEntitiesByAspect(Aspects.You))
        {
            // 已经加入移到列表
            if (_moveTargetsList.Contains(idx))
                continue;

            var e = Contexts.Default.GetEntity(idx);
            var input = e.Get<InputComp>();
            dir = input.Input;

            // 没有输入 方向为零
            if (dir == Vector2Int.zero)
                return;

            var pos = e.Get<PosComp>();
            _tempTargetsList.Clear();
            _tempTargetsList.Add(idx);
            var newPos = input.Input + pos.Pos;

            // 能推动
            if (CollectMoveTargets(ref _tempTargetsList, newPos, dir))
            {
                // 加入移动列表
                foreach (var targetId in _tempTargetsList)
                {
                    _moveTargetsList.Add(targetId);
                }
            }
        }

        foreach (var midx in _moveTargetsList)
        {
            var mEntity = Contexts.Default.GetEntity(midx);
            var mPos = mEntity.Get<PosComp>();
            var mOldPos = mPos.Pos;

            // 更新Map
            Data.GetMapNodeList(mOldPos.x, mOldPos.y).Remove(midx);
            var mNewPos = mOldPos + dir;
            mPos.SetValue(mNewPos);
            Data.GetMapNodeList(mNewPos.x, mNewPos.y).Add(midx);

            // 显示更新
            Helper.SetEntityName(mEntity);
            Data.GameObjectChangedEvent(midx);
        }

    }

    /// <summary>
    /// 收集下一个位置的可移动物体
    /// </summary>
    /// <param name="list">可移动物体</param>
    /// <param name="Pos">位置</param>
    /// <param name="dir">方向</param>
    /// <returns>可移动：true 不可移动：false</returns>
    private bool CollectMoveTargets(ref HashSet<int> list, Vector2Int Pos, Vector2Int dir)
    {
        //在新位置
        HashSet<int> nodeList = Data.GetMapNodeList(Pos.x, Pos.y);

        //有实体
        if (nodeList.Count != 0)
        {
            //实体中有Stop 不可到达
            if (HasStop(nodeList)) return false;

            //递归收集物体
            foreach (var idx in nodeList)
            {
                var e = Contexts.Default.GetEntity(idx);
                var tag = e.Get<TagComp>().tag;
                if (Data.HasAspect(tag, Aspects.Push))
                {
                    list.Add(idx);
                    var nextPos = e.Get<PosComp>().Pos + dir;
                    if (!CollectMoveTargets(ref list, nextPos, dir))
                        return false;
                }
            }
        }
        return true;
    }

    private bool HasStop(IEnumerable<int> list)
    {
        foreach (var idx in list)
        {
            var e = Contexts.Default.GetEntity(idx);
            var tag = e.Get<TagComp>().tag;
            if (Data.HasAspect(tag, Aspects.Stop))
            {
                return true;
            }
        }
        return false;
    }
}

