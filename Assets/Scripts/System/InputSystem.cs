using Entitas;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(90)]
public class InputSystem : IExecuteSystem
{
    private Data data;

    public void Execute()
    {
        data = Contexts.Default.GetUnique<DataComp>().data;
        int y = (int)Input.GetAxisRaw("Horizontal");
        int x = (int)Input.GetAxisRaw("Vertical");
        var input = Contexts.Default.AddUnique<InputComp>();
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