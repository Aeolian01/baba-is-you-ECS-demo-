
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapReader:Singleton<MapReader>
{
    public int[,] map= new int[5,5];
    public void ReadFile(int id) {
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
            Debug.Log(i);
        }
    }

}

