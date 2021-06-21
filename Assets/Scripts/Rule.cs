using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Rule
{
    Tag _tag;
    Tag _tagRule;
    Aspects _aspectRule;
    Vector2Int _pos;

    public Rule(Tag tag, Tag tagRule, Aspects aspectRule,Vector2Int pos)
    {
        _tag = tag;
        _tagRule = tagRule;
        _aspectRule = aspectRule;
        _pos = pos;
    }
    public Rule(Tag tag, Tag tagRule, Vector2Int pos)
    {
        _tag = tag;
        _tagRule = tagRule;
        _aspectRule = 0;
        _pos = pos;
    }

    public Rule(Tag tag, Aspects aspectRule,Vector2Int pos)
    {
        _tag = tag;
        _tagRule = 0;
        _aspectRule = aspectRule;
        _pos = pos;
    }

    public Tag GetTag()
    {
        return _tag;
    }
    public Tag GetTagRule()
    {
        return _tagRule;
    }
    public Aspects GetAspectRule()
    {
        return _aspectRule;
    }
    public Vector2Int GetPos()
    {
        return _pos;
    }
    public bool HasTagRule()
    {
        return _tagRule != 0;
    }
    public bool HasAspectRule()
    {
        return _aspectRule != 0;
    }
    public bool HasRule()
    {
        return HasTagRule() || HasAspectRule();
    }
    public bool HasTag()
    {
        return _tag != 0;
    }

    public bool CheckAspectRule(Aspects aspects)
    {
        return _aspectRule == aspects;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public static bool operator ==(Rule left, Rule right)
    {
        return left._tag == right._tag && left._tagRule == right._tagRule && left._aspectRule == right._aspectRule;
    }

    public static bool operator !=(Rule left, Rule right)
    {
        return !(left == right);
    }
}