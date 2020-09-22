using Entitas;
using Entitas.VisualDebugging.Unity;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Systems _gameSystem;
    public GameObject Player;

    //位置->实体 映射
    public static Dictionary<Vector2, List<Entity>> posToEntity = new Dictionary<Vector2, List<Entity>>();

    public void Start()
    {
        var contexts = Contexts.sharedInstance;
        LoadLevel(1);

#if UNITY_EDITOR
        ContextObserverHelper.ObserveAll(contexts);
#endif
        var Entity = Contexts.Default.CreateEntity();
        Entity.Add<PosComp>().SetValue(new Vector2(5, 5));
        Entity.Add<ObjectComp>().SetValue(Name.Objects.Baba);
        Entity.Add<PropertyComp>().SetValue(Name.Properties.You);

        for (int i = 0; i < 4; i++)
        {
            var t = Contexts.Default.CreateEntity();
            t.Add<PosComp>().SetValue(new Vector2(0, i));
            t.Add<ObjectComp>().SetValue(Name.Objects.Wall);
            t.Add<PropertyComp>().SetValue(Name.Properties.Stop);
        }
        #region 测试代码
        var t1 = Contexts.Default.CreateEntity();
        t1.Add<PosComp>().SetValue(new Vector2(5, 1));
        t1.Add<ObjectWordsComp>().SetValue(Name.ObjectWords.WallWord);
        var t2 = Contexts.Default.CreateEntity();
        t2.Add<PosComp>().SetValue(new Vector2(5, 2));
        t2.Add<IsWordComp>();
        var t3 = Contexts.Default.CreateEntity();
        t3.Add<PosComp>().SetValue(new Vector2(5, 3));
        t3.Add<ObjectWordsComp>().SetValue(Name.ObjectWords.RockWord);



        var t5 = Contexts.Default.CreateEntity();
        t5.Add<PosComp>().SetValue(new Vector2(5, 4));
        t5.Add<IsWordComp>();
        var t6 = Contexts.Default.CreateEntity();
        t6.Add<PosComp>().SetValue(new Vector2(5, 5));
        t6.Add<ProperWordsComp>().SetValue(Name.ProperWords.WinWord);

        posToEntity[t2.Get<PosComp>().value] = new List<Entity> { t2 };
        posToEntity[t3.Get<PosComp>().value] = new List<Entity> { t3 };

        posToEntity[t5.Get<PosComp>().value] = new List<Entity> { t5 };
        posToEntity[t6.Get<PosComp>().value] = new List<Entity> { t6 };
        #endregion

#if UNITY_EDITOR
        _gameSystem = FeatureObserverHelper.CreateFeature(null);
#else
		//init systems, auto collect matched systems, no manual Systems.Add(ISystem) required
		_gameSystem = new Feature(null);
#endif
        _gameSystem.Initialize();
    }

    public void Update()
    {
        _gameSystem.Execute();
        _gameSystem.Cleanup();
    }
    private void OnDestroy()
    {
        _gameSystem.TearDown();
    }

    public static void Win()
    {
        Debug.Log("win!");
        //下一关
    }
    //加载指定关卡
    public static void LoadLevel(int id)
    {
        MapReader.Instance.ReadFile(id);
    }
}
