using Entitas;
using Entitas.VisualDebugging.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Default]
public class PosComp : IComponent
{
    public Vector2 value;

    public void SetValue(Vector2 pos)
    {
        value = pos;
    }
}