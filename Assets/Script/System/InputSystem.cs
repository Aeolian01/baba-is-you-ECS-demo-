using Entitas;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(100)]
public class InputSystem : IExecuteSystem
{
    public void Execute()
    {
        var _group = Context<Default>.AllOf<YouComp>();
        foreach (var e in _group)
        {
            e.Get<YouComp>().SetValue((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        }
    }
}

