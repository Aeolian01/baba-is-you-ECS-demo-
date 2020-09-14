using Entitas;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(0)]

public class MoveSystem : IExecuteSystem
{
    public void Execute()
    {
        var _group = Context<Default>.AllOf<PosComp, VelComp, ViewComp>();
        foreach(var e in _group)
        {
            var pos = e.Modify<PosComp>();
            var vel = e.Get<VelComp>();

            pos.Value.x += vel.Value.x * Time.deltaTime;
            pos.Value.y += vel.Value.y * Time.deltaTime;

            e.Get<ViewComp>().view.transform.position = pos.Value;
        }
    }
}

