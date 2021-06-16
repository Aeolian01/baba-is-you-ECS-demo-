using Entitas;
using UnityEngine;

public class DataComp : IUnique, IComponent
{
    public Data data { private set; get; }

    public void SetValue(Data data)
    {
        this.data = data;
    }
}
