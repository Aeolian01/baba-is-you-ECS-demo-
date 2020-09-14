using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour,IView
{
    public void Link(Context context, IEntity entity)
    {
        gameObject.Link(entity,context);
        //gameObject.GetEntityLink
    }

}
