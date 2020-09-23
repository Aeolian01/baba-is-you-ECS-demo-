using Entitas;
using Entitas.VisualDebugging.Unity;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameController: MonoBehaviour
{
   
    private Systems _gameSystem;

    public void Start()
    {
        var gameData = GameData.Instance;
        var contexts = Contexts.sharedInstance;
       


#if UNITY_EDITOR
        ContextObserverHelper.ObserveAll(contexts);
#endif
        gameData.LoadLevel(1);
        //Debug.Log(gameData.sprites[gameData.posToEntity[new Vector2(0, 0)][0].Get<SpriteComp>().name]);
        #region 测试代码
#if false
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
#endif
        #endregion



#if UNITY_EDITOR
        _gameSystem = FeatureObserverHelper.CreateFeature(null);
#else
		//init systems, auto collect matched systems, no manual Systems.Add(ISystem) required
		_gameSystem = new Feature(null);
#endif
        _gameSystem.Initialize();
        TransHelper.Instance.Trans();
    }

    public void Update()
    {
        GameData.Timer += Time.deltaTime;
        _gameSystem.Execute();
        _gameSystem.Cleanup();
    }
    private void OnDestroy()
    {
        _gameSystem.TearDown();
    }

  
}
