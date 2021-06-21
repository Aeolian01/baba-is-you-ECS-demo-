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
        int y = Input.GetKeyDown(KeyCode.A) ? -1 : 0;
        y = Input.GetKeyDown(KeyCode.D) ? 1 : y;
        y = Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.D) ? 0 : y;
        int x = Input.GetKeyDown(KeyCode.S) ? -1 : 0;
        x = Input.GetKeyDown(KeyCode.W) ? 1 : x;
        x = Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.W) ? 0 : x;


        int y1 = Input.GetKey(KeyCode.A) ? -1 : 0;
        y1 = Input.GetKey(KeyCode.D) ? 1 : y1;
        y1 = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) ? 0 : y1;
        int x1 = Input.GetKey(KeyCode.S) ? -1 : 0;
        x1 = Input.GetKey(KeyCode.W) ? 1 : x1;
        x1 = Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W) ? 0 : x1;

        var input = Contexts.Default.AddUnique<InputComp>();

        if (x != 0)
        {
            input.SetValue(new Vector2Int(-x, 0));
            input.SetRefresh(true);
        }
        else if (y != 0)
        {
            input.SetValue(new Vector2Int(0, y));
            input.SetRefresh(true);
        }
        else if (x1 != 0)
        {
            input.SetValue(new Vector2Int(-x1, 0));
            input.SetRefresh(false);
        }
        else if (y1 != 0)
        {
            input.SetValue(new Vector2Int(0, y1));
            input.SetRefresh(false);
        }
        else
        {
            input.SetValue(Vector2Int.zero);
            input.SetRefresh(false);
        }
    }
}