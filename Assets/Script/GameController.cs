using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    private Systems _gameSystem;
    public Transform Map;
    public const int DetlaSize = 25;

    //spriteName -- Sprite
    public Dictionary<Name.SpriteName, Sprite> sprites = new Dictionary<Name.SpriteName, Sprite>();

    //位置->实体 映射
    public Dictionary<Vector2, List<Entity>> posToEntity = new Dictionary<Vector2, List<Entity>>();
    public Dictionary<Entity, GameObject> gos = new Dictionary<Entity, GameObject>();

    public void Start()
    {
        var contexts = Contexts.sharedInstance;
        Debug.Log(sprites[posToEntity[new Vector2(0, 0)][0].Get<SpriteComp>().name]);


#if UNITY_EDITOR
        ContextObserverHelper.ObserveAll(contexts);
#endif
        LoadLevel(1);

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
        for (int i = MapReader.mapHeight - 1; i >= 0; i--)
        {
            for (int j = 0; j < MapReader.mapWidth; j++)
            {
                var pos = new Vector2(DetlaSize * (j + 1), DetlaSize * (i + 1));
                var unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                var entity = Contexts.Default.CreateEntity();
                entity.Add<SpriteComp>().SetValue(Name.SpriteName.Empty);
                entity.Add<ObjectComp>().SetValue(Name.Objects.Empty);
                entity.Add<PosComp>().SetValue(new Vector2(i, j));
                if (j == 0 || j == MapReader.mapWidth - 1 || i == MapReader.mapHeight - 1 || i == 0)
                {
                    entity.Add<PropertyComp>().SetValue(Name.Properties.Stop);
                    entity.Add<SpriteComp>().SetValue(Name.SpriteName.Edge);
                }
                //unit.Link(entity, Contexts.Default);
                Sprite sprite;
                if (sprites.TryGetValue(entity.Get<SpriteComp>().name, out sprite))
                {
                    unit.GetComponent<Image>().sprite = sprite;
                }
                else
                {
                    sprites.Add(entity.Get<SpriteComp>().name, Resources.Load<Sprite>("Sprites/"+ entity.Get<SpriteComp>().name.ToString()));
                    unit.GetComponent<Image>().sprite = sprites[entity.Get<SpriteComp>().name];
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
                var pos = new Vector2(DetlaSize * (j + 1), DetlaSize * (i + 1));
                switch (MapReader.map[i, j])
                {
                    case (int)Name.SpriteName.Wall:
                        var unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        var entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectComp>().SetValue(Name.Objects.Wall);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.Wall);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        //unit.Link(entity, Contexts.Default);
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
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectComp>().SetValue(Name.Objects.Baba);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.Baba);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        //unit.Link(entity, Contexts.Default);
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
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectComp>().SetValue(Name.Objects.Flag);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.Flag);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        //unit.Link(entity, Contexts.Default);
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
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectComp>().SetValue(Name.Objects.Rock);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.Rock);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        //unit.Link(entity, Contexts.Default);
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
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectWordsComp>().SetValue(Name.ObjectWords.WallWord);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.WallWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.WallWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.WallWord, Resources.Load<Sprite>("Sprites/WallWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.WallWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.BabaWord:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectWordsComp>().SetValue(Name.ObjectWords.BabaWord);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.BabaWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.BabaWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.BabaWord, Resources.Load<Sprite>("Sprites/BaBaWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.BabaWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.FlagWord:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectWordsComp>().SetValue(Name.ObjectWords.FlagWord);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.FlagWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.FlagWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.FlagWord, Resources.Load<Sprite>("Sprites/FlagWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.FlagWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.RockWord:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ObjectWordsComp>().SetValue(Name.ObjectWords.RockWord);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.RockWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.RockWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.RockWord, Resources.Load<Sprite>("Sprites/RockWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.RockWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.IsWord:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<IsWordComp>();
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.IsWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.IsWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.IsWord, Resources.Load<Sprite>("Sprites/IsWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.IsWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.WinWord:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ProperWordsComp>().SetValue(Name.ProperWords.WinWord);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.WinWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.WinWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.WinWord, Resources.Load<Sprite>("Sprites/WinWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.WinWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.YouWord:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ProperWordsComp>().SetValue(Name.ProperWords.YouWord);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.YouWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.YouWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.YouWord, Resources.Load<Sprite>("Sprites/YouWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.YouWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.PushWord:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ProperWordsComp>().SetValue(Name.ProperWords.PushWord);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.PushWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.PushWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.PushWord, Resources.Load<Sprite>("Sprites/PushWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.PushWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                    case (int)Name.SpriteName.StopWord:
                        unit = GameObject.Instantiate(Resources.Load<GameObject>("Unit"), pos, Quaternion.identity, Map);
                        unit.GetComponent<RectTransform>().sizeDelta = new Vector2(DetlaSize, DetlaSize);
                        entity = Contexts.Default.CreateEntity();
                        entity.Add<ProperWordsComp>().SetValue(Name.ProperWords.StopWord);
                        entity.Add<SpriteComp>().SetValue(Name.SpriteName.StopWord);
                        entity.Add<PosComp>().SetValue(new Vector2(i, j));
                        entity.Add<PropertyComp>().SetValue(Name.Properties.Push);
                        //unit.Link(entity, Contexts.Default);
                        if (sprites.TryGetValue(Name.SpriteName.StopWord, out sprite))
                        {
                            unit.GetComponent<Image>().sprite = sprite;
                        }
                        else
                        {
                            sprites.Add(Name.SpriteName.StopWord, Resources.Load<Sprite>("Sprites/StopWord"));
                            unit.GetComponent<Image>().sprite = sprites[Name.SpriteName.StopWord];
                        }
                        posToEntity[new Vector2(i, j)].Add(entity);
                        gos.Add(entity, unit);
                        break;
                }
            }
        }

    }
    //刷新地图显示
    public void UpdateMap()
    {
        for (int i = 0; i < MapReader.mapWidth; i++)
        {
            for (int j = 0; j < MapReader.mapHeight; j++)
            {
                var entities = posToEntity[new Vector2(i, j)];
                foreach (var e in entities)
                {
                    GameObject go;
                    if (!gos.TryGetValue(e, out go)) {
                        Debug.LogError("entity can not find gameobject: ");
                    }
                    Sprite sprite;
                    if (sprites.TryGetValue(e.Get<SpriteComp>().name, out sprite))
                    {
                        go.GetComponent<Image>().sprite = sprite;
                    }
                    else
                    {
                        Debug.LogError("sprite not found: " + e.Get<SpriteComp>().name);
                        Debug.LogError($"pos:{i},{j}");
                    }
                }
            }
        }
    }
}
