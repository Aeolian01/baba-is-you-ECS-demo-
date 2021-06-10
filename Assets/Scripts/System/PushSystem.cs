using Entitas;
using System.Collections.Generic;
using UnityEngine;

//Systemִ��˳��Խ�����ȼ�Խ�ߣ�
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
            // �Ѿ������Ƶ��б�
            if (_moveTargetsList.Contains(idx))
                continue;

            var e = Contexts.Default.GetEntity(idx);
            var input = e.Get<InputComp>();
            dir = input.Input;

            // û������ ����Ϊ��
            if (dir == Vector2Int.zero)
                return;

            var pos = e.Get<PosComp>();
            _tempTargetsList.Clear();
            _tempTargetsList.Add(idx);
            var newPos = input.Input + pos.Pos;

            // ���ƶ�
            if (CollectMoveTargets(ref _tempTargetsList, newPos, dir))
            {
                // �����ƶ��б�
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

            // ����Map
            Data.GetMapNodeList(mOldPos.x, mOldPos.y).Remove(midx);
            var mNewPos = mOldPos + dir;
            mPos.SetValue(mNewPos);
            Data.GetMapNodeList(mNewPos.x, mNewPos.y).Add(midx);

            // ��ʾ����
            Helper.SetEntityName(mEntity);
            Data.GameObjectChangedEvent(midx);
        }

    }

    /// <summary>
    /// �ռ���һ��λ�õĿ��ƶ�����
    /// </summary>
    /// <param name="list">���ƶ�����</param>
    /// <param name="Pos">λ��</param>
    /// <param name="dir">����</param>
    /// <returns>���ƶ���true �����ƶ���false</returns>
    private bool CollectMoveTargets(ref HashSet<int> list, Vector2Int Pos, Vector2Int dir)
    {
        //����λ��
        HashSet<int> nodeList = Data.GetMapNodeList(Pos.x, Pos.y);

        //��ʵ��
        if (nodeList.Count != 0)
        {
            //ʵ������Stop ���ɵ���
            if (HasStop(nodeList)) return false;

            //�ݹ��ռ�����
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

