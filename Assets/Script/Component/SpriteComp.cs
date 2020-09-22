using Entitas;

public class SpriteComp : IComponent
{
    public Name.SpriteName name
    {
        get;
        private set;
    }

    public void SetValue(Name.SpriteName n)
    {
        name = n;
    }
}
