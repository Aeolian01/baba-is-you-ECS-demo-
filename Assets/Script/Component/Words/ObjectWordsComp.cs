using Entitas;

public class ObjectWordsComp : IComponent
{
    public Name.ObjectWords name
    {
        get;
        private set;
    }

    public void SetValue(Name.ObjectWords n)
    {
        name = n;
    }
}
