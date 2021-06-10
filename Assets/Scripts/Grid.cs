using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public HashSet<int> NodeList = new HashSet<int>();
    public void Add(int idx)
    {
        NodeList.Add(idx);
    }
    public void Remove(int idx)
    {
        NodeList.Remove(idx);
    }

}
