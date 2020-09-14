using Entitas;
using Entitas.VisualDebugging.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Default]
public class PosComp : IComponent
{
    public Vector2 Value;

    public void setValue(Vector2 pos)
    {
        Value = pos;
    }
}