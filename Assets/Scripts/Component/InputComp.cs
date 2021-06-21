using Entitas;
using UnityEngine;

public class InputComp : IUnique, IComponent
{
    public Vector2Int input { private set; get; }
    public bool refresh { private set; get; }

    public void SetValue(Vector2Int input)
    {
        this.input = input;
    }

    public void SetRefresh(bool refresh)
    {
        this.refresh = refresh;
    }
}