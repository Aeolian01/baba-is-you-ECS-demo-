using Entitas;
//相当于InputComp
public class InputComp : IComponent
{
    public int x;
    public int y;
    public void SetValue(int hor, int ver)
    {
        x = hor;
        y = ver;
    }
}
