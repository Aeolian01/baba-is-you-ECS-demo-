using Entitas;

public class ObjectComp : IComponent
{
    public Name.Objects name
    {
        get;
        private set;
    }

    public void SetValue(Name.Objects n)
    {
        name = n;
    }
}


