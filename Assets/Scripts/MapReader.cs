
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapReader : Singleton<MapReader>
{
    public int mapWidth = 20;
    public int mapHeight = 20;
    public int[,] ReadFile(int id)
    {
        int[,] map;
        string path = Application.dataPath + "/Resources/" + "Level_" + id + ".csv";
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
        string str = "";
        int row = 0;

        str = sr.ReadLine();
        string[] s = str.Split(',');
        if (s.Length != 2)
        {
            Debug.LogError("格式错误：第一行应为 高 宽");
            return null;
        }
        mapHeight = int.Parse(s[0]);
        mapWidth = int.Parse(s[1]);
        map = new int[mapHeight, mapWidth];

        while ((str = sr.ReadLine()) != null)
        {
            s = str.Split(',');
            for (int col = 0; col < s.Length; col++)
            {
                map[row, col] = int.Parse(s[col]);
            }
            row++;
        }
        sr.Close();
        return map;
    }

}

