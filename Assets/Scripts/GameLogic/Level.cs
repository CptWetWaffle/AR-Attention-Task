using UnityEngine;
using System.Collections;

public class Level {

    public string levelName;
    public int columns;
    public int rows;
    public int targets;
    public int correctChoices;
    public float levelTime;
    public float picTimer;
    public string category;
    public string imagesToDisplay;
    public bool useDistractors;
    public bool useMemory;
    public bool valence;
    public bool showCorrect;
    public bool completed;
    public float selectionTime;

    public Level(string ln, int col, int row, int tar, int cc, float lt, float pt, string cat, string itd, bool ud, bool um, bool val, bool sc, bool comp, float selT)
    {
        levelName = ln;
        columns = col;
        rows = row;
        targets = tar;
        correctChoices = cc;
        levelTime = lt;
        picTimer = pt;
        category = cat;
        imagesToDisplay = itd;
        useDistractors = ud;
        useMemory = um;
        valence = val;
        showCorrect = sc;
        completed = comp;
        selectionTime = selT;
    }
}
