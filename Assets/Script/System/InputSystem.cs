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
            int h = (int)Input.GetAxisRaw("Horizontal");
            int v = (int)Input.GetAxisRaw("Vertical");
            var you = e.Get<YouComp>();
            if (you.horizontal == h && you.vertical == v)
                return;
            if (h != 0)
                e.Modify<YouComp>().SetValue(h, 0);
            else
                e.Modify<YouComp>().SetValue(0, v);
        }
    }
}

