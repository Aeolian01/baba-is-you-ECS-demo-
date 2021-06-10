using Entitas;
using UnityEngine;

public class InputComp : IComponent
{
    public Vector2Int Input { private set; get; }

    public void SetValue(Vector2Int input)
    {
        Input = input;
    }
}