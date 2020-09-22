using Entitas;

public class PropertyComp : IComponent
{
    public Name.Properties name
    {
        get;
        private set;
    }

    public void SetValue(Name.Properties n)
    {
        name = n;
    }
}

