using Entitas;
using UnityEngine;

public class ViewComp : IComponent
{
    public GameObject view;

    public void  setValue (GameObject go)
    {
        view = go;
    }
}
