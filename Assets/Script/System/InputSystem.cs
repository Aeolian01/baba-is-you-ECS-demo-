using Entitas;
using UnityEngine;

//System执行顺序（越大优先级越高）
[UnnamedFeature(100)]
public class InputSystem : IExecuteSystem
{
    public void Execute()
    {
        var _group = Context<Default>.AllOf<InputComp>();
        foreach (var e in _group)
        {
            //水平数值输入
            var input = e.Modify<InputComp>();
            var In= new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //鼠标位置
            var mousePos = Input.mousePosition;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));
            Debug.Log(mouseWorldPos);
            input.setValue(In, mouseWorldPos);

        }
    }
}

