using Entitas;
using UnityEngine;

public class InputComp : IComponent
{
    public Vector2 Value;
    public Vector2 MousePos;
    public void setValue(Vector2 vel,Vector2 mousePos)
    {
        Value = vel;
        MousePos = mousePos;
    }
}
