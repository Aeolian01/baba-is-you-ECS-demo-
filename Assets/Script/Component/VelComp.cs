using Entitas;
using Entitas.VisualDebugging.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelComp : IComponent
{
    public Vector2 Value;
    public float Modifier;
    public void setValue(Vector2 vel,float m = 5 )
    {
        Modifier = m;
        Value = vel * Modifier;
    }
}

