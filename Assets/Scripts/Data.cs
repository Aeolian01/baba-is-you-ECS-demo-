using Entitas;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Tag
{
    None = 0,
    Wall,
    Baba,
    Flag,
    Rock,
    Edge,

    IsWord = 100,

    WallWord = 101,
    BabaWord,
    FlagWord,
    RockWord,

    WinWord = 201,
    YouWord,
    PushWord,
    StopWord,
}

[Flags]
public enum Aspects
{
    None = 0,
    Win = 1 << 0,
    You = 1 << 1,
    Push = 1 << 2,
    Stop = 1 << 3,
}
public class Data
{
    private Grid[,] _map;
    public int Width;
    public int Height;
    public float Timer = 0;
    public float Interval = 0.15f;

    private Dictionary<int, List<int>> _tagObjPool = new Dictionary<int, List<int>>();
    private Dictionary<int, List<int>> _aspObjPool = new Dictionary<int, List<int>>();
    private Dictionary<int, GameObject> _gameObjects = new Dictionary<int, GameObject>();
    private Dictionary<int, Sprite> _sprites = new Dictionary<int, Sprite>();
    private HashSet<Rule> _rule = new HashSet<Rule>();

    public bool RuleChanged;

    public Data()
    {
        foreach (var i in Enum.GetValues(typeof(Tag)))
        {
            _sprites.Add((int)i, Resources.Load<Sprite>("Sprites/" + i.ToString()));
        }
    }
    public HashSet<int> GetMapNodeList(int posX, int posY)
    {
        return _map[posX, posY].NodeList;
    }
    public Dictionary<int, List<int>> GetTagPool()
    {
        return _tagObjPool;
    }

    public Dictionary<int, List<int>> GetAspPool()
    {
        return _aspObjPool;
    }

    public HashSet<Rule> GetRules()
    {
        return _rule;
    }
    public void LoadLevel(int id)
    {
        var mapFile = MapReader.Instance.ReadFile(id);
        Width = MapReader.Instance.mapWidth;
        Height = MapReader.Instance.mapHeight;
        _map = new Grid[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            for (int ii = 0; ii < Width; ii++)
            {
                var e = Contexts.Default.CreateEntity();
                e.Add<PosComp>().SetValue(new Vector2Int(i, ii));
                e.Add<TagComp>().SetValue((Tag)mapFile[i, ii]);
                Helper.SetEntityName(e);
                if (_map[i, ii] == null)
                    _map[i, ii] = new Grid();
                _map[i, ii].Add(e.creationIndex);
                _gameObjects.Add(e.creationIndex, GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Sprite")));
                GameObjectChangedEvent(e.creationIndex);
            }
        }
        Contexts.Default.AddUnique<DataComp>().SetValue(this);
    }

    public Grid GetGrid(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x > Width || pos.y < 0 || pos.y > Height)
        {
            return null;
        }
        return _map[pos.x, pos.y];
    }

    public Entity GetEntity(int idx)
    {
        return Contexts.Default.GetEntity(idx);
    }

    public Tag GetWord(Vector2Int pos)
    {
        var gird = GetGrid(pos);
        if (gird != null)
        {
            foreach (var idx in gird.NodeList)
            {
                var e = GetEntity(idx).Get<TagComp>();
                if (e == null)
                    continue;
                if ((int)e.tag > 100)
                    return e.tag;
            }
        }
        return 0;
    }

    public IReadOnlyList<int> GetEntitiesByAspect(Aspects aspects)
    {
        //var tagList = Helper.RuleToTagList(aspects);
        //var result = new List<int>();
        //foreach (var tag in tagList)
        //{
        //    result.AddRange(Helper.GetList(tag));
        //}
        //return result;
        return Helper.GetList(aspects);
    }

    public IReadOnlyList<int> GetEntitiesByTag(Tag tag)
    {
        return Helper.GetList(tag);
    }

    public void AddEntityID(int id, Aspects asp)
    {
        Helper.GetList(asp).Add(id);
    }

    public void AddEntityID(int id, Tag tag)
    {
        Helper.GetList(tag).Add(id);
    }

    public void Clear(Tag tag)
    {
        Helper.GetList(tag).Clear();
    }

    public void Clear(Aspects asp)
    {
        Helper.GetList(asp).Clear();
    }

    public bool CheckTagHasAspect(Tag tag, Aspects aspects)
    {
        return _rule.Contains(new Rule(tag, aspects));
    }

    public void AddRule(Rule r)
    {
        if (!r.HasTag())
            return;
        if (!r.HasRule())
            return;
        _rule.Add(r);
    }

    public void RemoveRule(Rule r)
    {
        _rule.Remove(r);
    }

    public void GameObjectChangedEvent(int id)
    {
        var e = Contexts.Default.GetEntity(id);
        var pos = e.Get<PosComp>().pos;
        var tag = e.Get<TagComp>().tag;
        var gObj = _gameObjects[id];
        gObj.transform.position = new Vector3(pos.y, -pos.x, 0);
        gObj.GetComponent<SpriteRenderer>().sprite = _sprites[(int)tag];
    }
}
