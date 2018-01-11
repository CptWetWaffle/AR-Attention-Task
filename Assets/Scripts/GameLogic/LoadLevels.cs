using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;

public class LoadLevels : MonoBehaviour
{

    int columns, rows, targets, correctchoices;
    bool showcorrect, useDistractors, useMemory, valence, complete;
    float levelTime, picTimer, selectionTime;
    string folder, imagesToDisplay;

    public static List<Level> levels = new List<Level>();

    void Awake()
    {
        LoadLevelConfig();
    }
    //reads all the levels from xml file and stores info in levels list
    void LoadLevelConfig()
    {
        string filepath = Application.dataPath + @"/Settings/Levels.xml";
        XmlDocument xmlDoc = new XmlDocument();

        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);
            XmlNodeList levelsList = xmlDoc.GetElementsByTagName("Levels");

            foreach (XmlNode levelNumber in levelsList)
            {
                XmlNodeList levelcontent = levelNumber.ChildNodes;

                foreach (XmlNode levelnum in levelcontent)
                {
                    string level = levelnum.Name;

                    foreach (XmlNode xmlsettings in levelnum)
                    {

                        if (xmlsettings.Name == "columns")
                            columns = int.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "rows")
                            rows = int.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "targets")
                            targets = int.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "correctchoices")
                            correctchoices = int.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "showcorrect")
                            showcorrect = bool.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "timer")
                            levelTime = float.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "picTimer")
                            picTimer = float.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "category")
                            folder = xmlsettings.InnerText;
                        if (xmlsettings.Name == "imagesToDisplay")
                            imagesToDisplay = xmlsettings.InnerText;
                        if (xmlsettings.Name == "useDistractors")
                            useDistractors = bool.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "useMemory")
                            useMemory = bool.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "valence")
                            valence = bool.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "selectionTime")
                            selectionTime = float.Parse(xmlsettings.InnerText);

                        complete = false;
                    }//foreach xmlcontent

                    levels.Add(new Level(level, columns, rows, targets, correctchoices, levelTime, picTimer, folder, imagesToDisplay, useDistractors, useMemory, valence, showcorrect, complete, selectionTime));

                }//foreach level
            }//foreach levelnum
        }//foreach configList	

        SpawnTiles.totalLevels = LoadLevels.levels.Count;

        if (LevelChangeBehavior.memoryLevel == 0 && LevelChangeBehavior.attentionLevel == 0)
        {
            if (levels[1].useMemory)
            {
                LevelChangeBehavior.memoryLevel = 1;
                LevelChangeBehavior.attentionLevel = 2;
            }
            else
            {
                LevelChangeBehavior.memoryLevel = 2;
                LevelChangeBehavior.attentionLevel = 1;
            }
        }
    }//loadconfig
}
