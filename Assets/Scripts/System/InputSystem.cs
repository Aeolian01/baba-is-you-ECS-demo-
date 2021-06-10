using Entitas;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(90)]
public class InputSystem : IExecuteSystem, ICleanupSystem
{
    public void Cleanup()
    {
        foreach (var idx in Data.GetEntitiesByAspect(Aspects.You))
        {
            var e = Contexts.Default.GetEntity(idx);
            e.Remove<InputComp>();
        }
    }

    public void Execute()
    {
        int y = (int)Input.GetAxisRaw("Horizontal");
        int x = (int)Input.GetAxisRaw("Vertical");
        foreach (var idx in Data.GetEntitiesByAspect(Aspects.You))
        {
            var e = Contexts.Default.GetEntity(idx);
            var input = e.Add<InputComp>();
            if (x != 0)
            {
                input.SetValue(new Vector2Int(-x, 0));
            }
            else
            {
                input.SetValue(new Vector2Int(0, y));
            }
        }
    }
}