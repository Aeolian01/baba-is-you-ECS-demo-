using Entitas;
using UnityEngine;

//每个对象都必须有
public class ViewComp : IComponent
{
    public GameObject view;

    public void  setValue (GameObject go)
    {
        view = go;
    }
}
