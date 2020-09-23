using Entitas;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(0)]
public class TransSystem : ReactiveSystem
{
    public TransSystem()
    {
        monitors += Context<Default>.AllOf<PosComp>().OnAdded(TransHelper.Instance.Trans);
    }
    
}

