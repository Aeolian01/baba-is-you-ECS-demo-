using Entitas;
using UnityEngine;

public class RotComp : IComponent
{
    public float Angle;

    public void setValue(float angle)
    {
        Angle = angle;
    }
}