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

    public Rule(Tag tag, Tag tagRule, Aspects aspectRule)
    {
        _tag = tag;
        _tagRule = tagRule;
        _aspectRule = aspectRule;
    }
    public Rule(Tag tag, Tag tagRule)
    {
        _tag = tag;
        _tagRule = tagRule;
        _aspectRule = 0;
    }

    public Rule(Tag tag, Aspects aspectRule)
    {
        _tag = tag;
        _tagRule = 0;
        _aspectRule = aspectRule;
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
    public bool HasTagRule()
    {
        return _tagRule != 0;
    }
    public bool HasAspectRule()
    {
        return _aspectRule != 0;
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