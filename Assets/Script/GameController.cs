﻿using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    private Systems _gameSystem;
    public Transform Map;
    public Dictionary<Name.SpriteName, Sprite> sprites = new Dictionary<Name.SpriteName, Sprite>();

    //位置->实体 映射
    public Dictionary<Vector2, List<Entity>> posToEntity = new Dictionary<Vector2, List<Entity>>();
    public Dictionary<Entity, GameObject> gos = new Dictionary<Entity, GameObject>();

    public void Start()
    {
        var contexts = Contexts.sharedInstance;
        LoadLevel(1);

#if UNITY_EDITOR
        ContextObserverHelper.ObserveAll(contexts);
#endif
        var Entity = Contexts.Default.CreateEntity();
        Entity.Add<PosComp>();
        Entity.Add<ObjectComp>().SetValue(Name.Objects.Baba);
        Entity.Add<PropertyComp>().SetValue(Name.Properties.You);

        for (int i = 0; i < 4; i++)
        {
            var t = Contexts.Default.CreateEntity();
            t.Add<PosComp>();
            Entity.Add<ObjectComp>().SetValue(Name.Objects.Wall);
            Entity.Add<PropertyComp>().SetValue(Name.Properties.Stop);
        }

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

    public void Win()
    {
        Debug.Log("win!");
        //下一关
    }
    //加载指定关卡
    public void LoadLevel(int id)
    {
        MapReader.Instance.ReadFile(id);
        //地板
        for (int i = MapReader.mapHeight - 1; i >=0; i--)
        {
            for (int j = 0; j <MapReader.mapWidth; j++)
            {
                var pos = new Vector2(j + 25f * (j + 1), i + 25f * (i + 1));
                var unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                unit.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
                var entity = Contexts.Default.CreateEntity();
                entity.Add<SpriteComp>().SetValue(Name.SpriteName.Empty);
                entity.Add<ObjectComp>().SetValue(Name.Objects.Empty);
                unit.Link(entity, Contexts.Default);
                Sprite sprite;
                if (sprites.TryGetValue(Name.SpriteName.Empty, out sprite))
                {
                    unit.GetComponent<Image>().sprite = sprite;
                }
                else
                {
                    sprites.Add(Name.SpriteName.Empty, Resources.Load<Sprite>("Sprites/Empty"));
                    unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.Empty];
                }
                posToEntity.Add(new Vector2(i, j), new List<Entity>() { entity });
                gos.Add(entity, unit);
            }
        }
        //非Empty对象
        for (int i = MapReader.mapHeight - 1; i >= 0; i--)
        {
            for (int j = 0; j < MapReader.mapWidth; j++)
            {
                var pos = new Vector2(j + 25f * (j + 1), i + 25f * (i + 1));
                switch (MapReader.map[i, j])
                {
                    case (int)Name.SpriteName.Wall:
                        var unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
                        var entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectComp>().SetValue(Name.Objects.Wall);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.Wall);
                        unit.Link(entity, Contexts.Default);
                        Sprite sprite;
                        if (sprites.TryGetValue(Name.SpriteName.Wall, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.Wall, Resources.Load<Sprite>("Sprites/Wall"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.Wall];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.Baba:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectComp>().SetValue(Name.Objects.Baba);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.Baba);
                        unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.Baba, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.Baba, Resources.Load<Sprite>("Sprites/BaBa"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.Baba];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.Flag:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectComp>().SetValue(Name.Objects.Flag);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.Flag);
                        unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.Flag, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.Flag, Resources.Load<Sprite>("Sprites/Flag"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.Flag];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.Rock:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectComp>().SetValue(Name.Objects.Rock);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.Rock);
                        unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.Rock, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.Rock, Resources.Load<Sprite>("Sprites/Rock"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.Rock];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
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
}
