
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapReader:Singleton<MapReader>
{
    public const int mapWidth = 20;
    public const int mapHeight = 20;

    public static int[,] map;
    public void ReadFile(int id) {
        map = new int[mapWidth, mapHeight];
        string path = Application.dataPath + "/Resources/"+"level_" + id +".csv";
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
        string str = "";
        int row = 0;
        while ((str=sr.ReadLine())!=null)
        {
            string[] s = str.Split(',');
            for (int col = 0; col < s.Length; col++) { 
                map[row,col] = int.Parse(s[col]);
            }
            row++;
        }
        sr.Close();
        foreach (var i in map) {
            //Debug.Log(i);
        }
    }

}

