using Entitas;
using Entitas.VisualDebugging.Unity;
using System;
using UnityEngine;



public class Main : MonoBehaviour
{
    private static Systems _gameSystem;


    void Start()
    {
        var contexts = Contexts.sharedInstance;
#if UNITY_EDITOR
        ContextObserverHelper.ObserveAll(contexts);
#endif
        Data.LoadLevel(1);



#if UNITY_EDITOR
        _gameSystem = FeatureObserverHelper.CreateFeature(null);
#else
        //init systems, auto collect matched systems, no manual Systems.Add(ISystem) required
        _gameSystem = new Feature(null);
#endif
        _gameSystem.Initialize();
    }

    void Update()
    {
        Data.Timer += Time.deltaTime;
        _gameSystem.Execute();
        _gameSystem.Cleanup();
    }
    private void OnDestroy()
    {
        _gameSystem.TearDown();
    }

}
