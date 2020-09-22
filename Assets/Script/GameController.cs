using Entitas;
using Entitas.VisualDebugging.Unity;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private Systems _gameSystem;
	public GameObject Player;

	//位置->实体 映射
	public static Dictionary<Vector2, List<Entity>> posToEntity=new Dictionary<Vector2, List<Entity>>();

    public void Start()
	{
		var contexts = Contexts.sharedInstance;
		LoadLevel(1);

#if UNITY_EDITOR
		ContextObserverHelper.ObserveAll(contexts);
#endif
		var Entity = Contexts.Default.CreateEntity();
		Entity.Add<PosComp>();
		Entity.Add<YouComp>();



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

	public static void Win() {
		Debug.Log("win!");
		//下一关
	}
	//加载指定关卡
	public static void LoadLevel(int id) {
		MapReader.Instance.ReadFile(id);
	}
}
