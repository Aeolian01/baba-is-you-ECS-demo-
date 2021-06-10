using Entitas;
using UnityEngine;

public class PosComp : IComponent
{
    public Vector2Int Pos { private set; get; }

    public void SetValue(Vector2Int pos)
    {
        Pos = pos;
    }
}