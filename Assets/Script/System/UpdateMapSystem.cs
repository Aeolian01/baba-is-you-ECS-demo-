using Entitas;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(-1)]
public class UpdateMapSystem : IExecuteSystem
{
    public void Execute()
    {
        GameController.Instance.UpdateMap();
    }
}
