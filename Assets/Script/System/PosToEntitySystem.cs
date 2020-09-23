using Entitas;
using Entitas.VisualDebugging.Unity;
using System;
using System.Collections.Generic;

[UnnamedFeature(1)]
public class PosToEntitySystem : ReactiveSystem
{
    public PosToEntitySystem()
    {
        monitors += Context<Default>.AllOf<PosComp>().OnAdded(GameData.Instance.ReFreshDic);
    }
}