using Entitas;
using System.Collections.Generic;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(50)]
public class PlayerInputProcessSystem :ReactiveSystem
{
    public PlayerInputProcessSystem()
    {
        monitors += Context<Default>.AllOf<InputComp>().OnAdded(Process) ;

    }
	protected void Process(List<Entity> inputs)
	{
        var playerEntity = Context<Default>.AllOf<PlayerTag>().GetSingleEntity();
        if (playerEntity == null)
            return;
        foreach(var input in inputs)
        {
            //速度
            var vel = playerEntity.Modify<VelComp>();
            var dir = input.Get<InputComp>().Value;
            vel.setValue(dir);

            //朝向
            var targetDir = input.Get<InputComp>().MousePos - playerEntity.Get<PosComp>().Value;
            var angle = Vector2.SignedAngle(Vector2.up, targetDir);
            playerEntity.Modify<RotComp>().setValue(angle);
        }
	}
}
