using Entitas;
using UnityEngine;

public class PosComp : IComponent
{
    public Vector2Int pos { private set; get; }

    public void SetValue(Vector2Int pos)
    {
        this.pos = pos;
    }
}