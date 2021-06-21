using Entitas;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(80)]
public class PushSystem : IExecuteSystem
{
    private HashSet<int> _moveTargetsList = new HashSet<int>();
    private HashSet<int> _tempTargetsList = new HashSet<int>();
    private Data data;
    public void Execute()
    {
        data = Contexts.Default.GetUnique<DataComp>().data;
        var inputComp = Contexts.Default.GetUnique<InputComp>();
        Vector2Int dir = inputComp.input;

        // 没有输入 方向为零
        if (dir == Vector2Int.zero)
            return;

        if (data.Timer < data.Interval && !inputComp.refresh)
            return;

        data.Timer = 0;

        _moveTargetsList.Clear();

        foreach (var idx in data.GetEntitiesByAspect(Aspects.You))
        {
            // 已经加入移到列表
            if (_moveTargetsList.Contains(idx))
                continue;

            var e = Contexts.Default.GetEntity(idx);

            var pos = e.Get<PosComp>().pos;
            _tempTargetsList.Clear();
            _tempTargetsList.Add(idx);
            var newPos = dir + pos;

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
            var mOldPos = mPos.pos;

            // 更新Map
            data.RemoveMapNode(mOldPos, midx);
            var mNewPos = mOldPos + dir;
            mPos.SetValue(mNewPos);
            data.AddMapNode(mNewPos, midx);

            // 显示更新
            Helper.SetEntityName(mEntity);
            data.GameObjectChangedEvent(midx);
        }

    }

    /// <summary>
    /// 收集下一个位置的可移动物体
    /// </summary>
    /// <param name="targets">可移动物体</param>
    /// <param name="pos">位置</param>
    /// <param name="dir">方向</param>
    /// <returns>可移动：true 不可移动：false</returns>
    private bool CollectMoveTargets(ref HashSet<int> targets, Vector2Int pos, Vector2Int dir)
    {
        //在新位置
        var nodeList = data.GetMapNodeList(pos.x, pos.y);

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
                if (data.CheckTagHasAspect(tag, Aspects.Push))
                {
                    targets.Add(idx);
                    var nextPos = e.Get<PosComp>().pos + dir;
                    if (!CollectMoveTargets(ref targets, nextPos, dir))
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
            if (data.CheckTagHasAspect(tag, Aspects.Stop))
            {
                return true;
            }
        }
        return false;
    }
}

