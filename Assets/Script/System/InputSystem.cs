using Entitas;
using System.Linq;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(100)]
public class InputSystem : IExecuteSystem
{
    public void Execute()
    {
        var _group = Context<Default>.AllOf<PropertyComp>().Where(e=>e.Get<PropertyComp>().name==Name.Properties.You);
        foreach (var e in _group)
        {
            if (!e.Has<InputComp>())
                e.Add<InputComp>();
            int h = (int)Input.GetAxisRaw("Horizontal");
            int v = (int)Input.GetAxisRaw("Vertical");
            var input = e.Get<InputComp>();
            if (input.horizontal == h && input.vertical == v)
                return;
            if (h != 0)
                e.Modify<InputComp>().SetValue(h, 0);
            else
                e.Modify<InputComp>().SetValue(0, v);
        }
    }
}

