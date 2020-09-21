using Entitas;
//相当于InputComp
public class YouComp : IComponent
{
    public int horizontal;
    public int vertical;
    public void SetValue(int hor, int ver) {
        horizontal = hor;
        vertical = ver;
    }
}
