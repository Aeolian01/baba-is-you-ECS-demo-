using Entitas;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(0)]
public class RotationSystem : IExecuteSystem
{
    public void Execute()
    {
        var Group = Context<Default>.AllOf<RotComp,ViewComp,PlayerTag>();
        foreach(var e in Group)
        {
            var ang = e.Get<RotComp>().Angle;
            e.Get<ViewComp>().view.transform.rotation = Quaternion.Euler(new Vector3(0, 0, ang));
        }
    }
}