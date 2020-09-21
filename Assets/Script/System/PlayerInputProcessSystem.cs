using Entitas;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(50)]
public class PlayerInputProcessSystem :ReactiveSystem
{
    public PlayerInputProcessSystem()
    {
        monitors += Context<Default>.AllOf<YouComp>().OnAdded(Process) ;

    }
	protected void Process(List<Entity> youEntities)
	{
        foreach(var you in youEntities)
        {
            int moveX,moveY;
            if (you.Get<YouComp>().horizontal == 0) moveX = 0;
            moveX = (you.Get<YouComp>().horizontal > 0) ? 1 : -1;
            if (you.Get<YouComp>().horizontal == 0) moveY = 0;
            moveY = (you.Get<YouComp>().vertical > 0) ? 1 : -1;
            you.Modify<YouComp>().SetValue(moveX, moveY);
        }
	}
}
