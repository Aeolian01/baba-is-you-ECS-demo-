using Entitas;

using UnityEngine;
using UnityEngine.UI;

//System执行顺序（越大优先级越高）
[UnnamedFeature(-1)]
public class UpdateMapSystem : IExecuteSystem
{
    public void Execute()
    {
        GameData.Instance.UpdateMap();       
    }
}
