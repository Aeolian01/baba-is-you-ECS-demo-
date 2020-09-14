using Entitas;
using Entitas.VisualDebugging.Unity;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private Systems _gameSystem;
	public GameObject Player;
    public void Start()
	{
		var contexts = Contexts.sharedInstance;

#if UNITY_EDITOR
		ContextObserverHelper.ObserveAll(contexts);
#endif
		var playerEntity = Contexts.Default.CreateEntity();
		playerEntity.Add<PosComp>();
		playerEntity.Add<VelComp>();
        playerEntity.Add<InputComp>();
		playerEntity.Add<RotComp>();
		playerEntity.Add<PlayerTag>();
		playerEntity.Add<ViewComp>().setValue(Player);
		var view = Player.GetComponent<View>();
		view.Link(Contexts.Default, playerEntity);
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
}
