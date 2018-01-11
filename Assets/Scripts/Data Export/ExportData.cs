using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class ExportData: MonoBehaviour
{

    public class LevelInfo
    {
        public int Wrong { get; private set; }
        public int Right { get; private set; }
        public float Time { get; private set; }
        public int Level { get; private set; }
        public int Score { get; set; }


        public LevelInfo(int level, int right, int wrong, float time)
        {
            Level = level;
            Right = right;
            Wrong = wrong;
            Time = time;
            Score = (Right * 10) - (wrong * 5);
        }

    }

    public static int TotalWrong { get; set; }
    public static float TotalTime { get; set; }
    public static int TotalRight { get; set; }

    public static int TotalScore
    {
        get
        {
            var score = 0;
            foreach (var level in Levels)
            {
                score += (level.Right * 10 - level.Wrong * 5);
            }
            return score;
        }
    }

    public static IList<LevelInfo> Levels { get; set; }

    public ExportData()
    {
        Levels = new List<LevelInfo>();
        TotalWrong = 0;
        TotalRight = 0;
        TotalTime = 0.0f;
    }

    public static void addLevelInfo(int level, int right, int wrong, float time)
    {
        foreach (var l in Levels)
        {
            if(l.Level == level)
                return;
        }
        Levels.Add(new LevelInfo(level,right,wrong,time));
    }

    public static string[] LevelsToCSV()
    {
        string[] CSV = new string[Levels.Count];
        var i = 0;
        foreach (var level in Levels)
        {
            CSV[i] = level.Level + "," + level.Right + "," + level.Wrong + "," + level.Time + "," + level.Score;
            i++;
        }
        return CSV;
    }

}
