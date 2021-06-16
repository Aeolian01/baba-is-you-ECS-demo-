using Entitas;
using UnityEngine;

public class InputComp : IUnique, IComponent
{
    public Vector2Int input { private set; get; }

    public void SetValue(Vector2Int input)
    {
        this.input = input;
    }
}