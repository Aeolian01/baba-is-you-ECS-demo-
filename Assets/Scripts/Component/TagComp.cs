﻿using Entitas;
using System;

public class TagComp : IComponent
{
    public Tag tag { private set; get; }
    public void SetValue(Tag t)
    {
        tag = t;
    }

}