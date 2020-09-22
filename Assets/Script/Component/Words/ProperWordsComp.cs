using Entitas;

public class ProperWordsComp : IComponent
{
    public Name.ProperWords name
    {
        get;
        private set;
    }

    public void SetValue(Name.ProperWords n)
    {
        name = n;
    }
}
