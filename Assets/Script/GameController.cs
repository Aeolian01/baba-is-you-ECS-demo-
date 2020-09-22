using Entitas;
using Entitas.VisualDebugging.Unity;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
	private Systems _gameSystem;
	public Transform Map;

	//位置->实体 映射
	public Dictionary<Vector2, List<Entity>> posToEntity=new Dictionary<Vector2, List<Entity>>();

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

	public void Win() {
		Debug.Log("win!");
		//下一关
	}
	//加载指定关卡
	public void LoadLevel(int id) {
		MapReader.Instance.ReadFile(id);
		
		foreach (var i in MapReader.map) {
			var unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), Map);
			var entity = Contexts.Default.CreateEntity();
			switch (i) {
				case (int)Name.SpriteName.Empty:

					break;
				case (int)Name.SpriteName.Wall:
					break;
				case (int)Name.SpriteName.Baba:
					break;
				case (int)Name.SpriteName.Flag:
					break;
				case (int)Name.SpriteName.Rock:
					break;
				case (int)Name.SpriteName.WallWord:
					break;
				case (int)Name.SpriteName.BabaWord:
					break;
				case (int)Name.SpriteName.FlagWord:
					break;
				case (int)Name.SpriteName.RockWord:
					break;
				case (int)Name.SpriteName.IsWord:
					break;
				case (int)Name.SpriteName.WinWord:
					break;
				case (int)Name.SpriteName.YouWord:
					break;
				case (int)Name.SpriteName.PushWord:
					break;
				case (int)Name.SpriteName.StopWord:
					break;
			}
		}
	}
}
