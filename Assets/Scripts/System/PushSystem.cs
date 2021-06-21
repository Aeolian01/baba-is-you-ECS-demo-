using Entitas;
using System.Collections.Generic;
using UnityEngine;

//Systemִ��˳��Խ�����ȼ�Խ�ߣ�
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

        // û������ ����Ϊ��
        if (dir == Vector2Int.zero)
            return;

        if (data.Timer < data.Interval && !inputComp.refresh)
            return;

        data.Timer = 0;

        _moveTargetsList.Clear();

        foreach (var idx in data.GetEntitiesByAspect(Aspects.You))
        {
            // �Ѿ������Ƶ��б�
            if (_moveTargetsList.Contains(idx))
                continue;

            var e = Contexts.Default.GetEntity(idx);

            var pos = e.Get<PosComp>().pos;
            _tempTargetsList.Clear();
            _tempTargetsList.Add(idx);
            var newPos = dir + pos;

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
            var mOldPos = mPos.pos;

            // ����Map
            data.RemoveMapNode(mOldPos, midx);
            var mNewPos = mOldPos + dir;
            mPos.SetValue(mNewPos);
            data.AddMapNode(mNewPos, midx);

            // ��ʾ����
            Helper.SetEntityName(mEntity);
            data.GameObjectChangedEvent(midx);
        }

    }

    /// <summary>
    /// �ռ���һ��λ�õĿ��ƶ�����
    /// </summary>
    /// <param name="targets">���ƶ�����</param>
    /// <param name="pos">λ��</param>
    /// <param name="dir">����</param>
    /// <returns>���ƶ���true �����ƶ���false</returns>
    private bool CollectMoveTargets(ref HashSet<int> targets, Vector2Int pos, Vector2Int dir)
    {
        //����λ��
        var nodeList = data.GetMapNodeList(pos.x, pos.y);

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

