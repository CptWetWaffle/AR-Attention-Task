using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System;

public class Language : MonoBehaviour {

    public static string lang = "PT";
    string tempLang;

    //main menu
    public new static string name;
    public static string hand, play, categories, instructions, levels, quit, welcome, handWarning, nameWarning;
    public static string practice, memory, calibration, emotional, task, attention, selection, info;

    //misc
    public static string main;

    //main GUI
    public static string timer, remaining, pause, unpause;
    
    //Level change GUI
    public static string level, completed, points, congrats, accumulated, allLevels;
    public static string cont, session, rate, left, right, getReady;

    //calibration GUI
    public static string network, filtering, devices, port, start, stop, cal, local, scale, filtered, copy, close, calibDone;

    void Start()
    {
        LoadFromXml();
        tempLang = lang;
    }

    void Update()
    {
        if (tempLang != lang)
        {
            LoadFromXml();
            tempLang = lang;
        }
    }

    public void ResetLanguage(int reset)
    {
        if (reset == 0)
            lang = "EN";
        else
            lang = "PT";

        LoadFromXml();
    }

    public void LoadFromXml()
    {
        string filepath = Application.dataPath + @"/Language_Files/" + lang + ".xml";
        XmlDocument xmlDoc = new XmlDocument();

        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);

            XmlNodeList menulist = xmlDoc.GetElementsByTagName("MainMenu");
            XmlNodeList misclist = xmlDoc.GetElementsByTagName("Misc");
            XmlNodeList mainGUI = xmlDoc.GetElementsByTagName("MainGUI");
            XmlNodeList levelChange = xmlDoc.GetElementsByTagName("LevelChange");
            XmlNodeList calibr = xmlDoc.GetElementsByTagName("Calibration");

            //main menu
            foreach (XmlNode wordsInfo in menulist)
            {
                XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                foreach (XmlNode words in xmlcontent)
                {
                    if (words.Name == "Name")
                        name = words.InnerText;
                    if (words.Name == "Hand")
                        hand = words.InnerText;
                    if (words.Name == "Play")
                        play = words.InnerText;
                    if (words.Name == "Categories")
                        categories = words.InnerText;
                    if (words.Name == "Instructions")
                        instructions = words.InnerText;
                    if (words.Name == "Levels")
                        levels = words.InnerText;
                    if (words.Name == "Quit")
                         quit = words.InnerText;
                    if (words.Name == "Welcome")
                         welcome = words.InnerText;
                    if (words.Name == "HandWarning")
                         handWarning = words.InnerText;
                    if (words.Name == "NameWarning")
                         nameWarning = words.InnerText;
                    if (words.Name == "Practice")
                        practice = words.InnerText;
                    if (words.Name == "Memory")
                        memory = words.InnerText;
                    if (words.Name == "Calibration")
                        calibration = words.InnerText;
                    if (words.Name == "Emotional")
                        emotional = words.InnerText;
                    if (words.Name == "Task")
                        task = words.InnerText;
                    if (words.Name == "Attention")
                        attention = words.InnerText;
                    if (words.Name == "Selection")
                        selection = words.InnerText;
                    if (words.Name == "Info")
                        info = words.InnerText;
                }
            }//main menu

            //misc
            foreach (XmlNode wordsInfo in misclist)
            {
                XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                foreach (XmlNode words in xmlcontent)
                {
                    if (words.Name == "MainMenu")
                        main = words.InnerText;
                }
            }//misc

            //main GUI
            foreach (XmlNode wordsInfo in mainGUI)
            {
                XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                foreach (XmlNode words in xmlcontent)
                {
                    if (words.Name == "Timer")
                        timer = words.InnerText;
                    if (words.Name == "Remaining")
                        remaining = words.InnerText;
                    if (words.Name == "Pause")
                        pause = words.InnerText;
                    if (words.Name == "Unpause")
                        unpause = words.InnerText;
                }
            }//main GUI

            //Level change GUI
            foreach (XmlNode wordsInfo in levelChange)
            {
                XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                foreach (XmlNode words in xmlcontent)
                {
                    if (words.Name == "Level")
                        level = words.InnerText;
                    if (words.Name == "Completed")
                        completed = words.InnerText;
                    if (words.Name == "Points")
                        points = words.InnerText;
                    if (words.Name == "Congrats")
                        congrats = words.InnerText;
                    if (words.Name == "Accumulated")
                        accumulated = words.InnerText;
                    if (words.Name == "AllLevels")
                        allLevels = words.InnerText;
                    if (words.Name == "Continue")
                        cont = words.InnerText;
                    if (words.Name == "Session")
                        session = words.InnerText;
                    if (words.Name == "Rate")
                        rate = words.InnerText;
                    if (words.Name == "Left")
                        left = words.InnerText;
                    if (words.Name == "Right")
                        right = words.InnerText;
                    if (words.Name == "GetReady")
                        getReady = words.InnerText;
                }
            }

            foreach (XmlNode wordsInfo in calibr)
            {
                XmlNodeList xmlcontent = wordsInfo.ChildNodes;

                foreach (XmlNode words in xmlcontent)
                {
                    if (words.Name == "Network")
                        network = words.InnerText;
                    if (words.Name == "Filtering")
                        filtering = words.InnerText;
                    if (words.Name == "Devices")
                        devices = words.InnerText;
                    if (words.Name == "Port")
                        port = words.InnerText;
                    if (words.Name == "Start")
                        start = words.InnerText;
                    if (words.Name == "Stop")
                        stop = words.InnerText;
                    if (words.Name == "Cal")
                        cal = words.InnerText;
                    if (words.Name == "Local")
                        local = words.InnerText;
                    if (words.Name == "Scale")
                        scale = words.InnerText;
                    if (words.Name == "Filtered")
                        filtered = words.InnerText;
                    if (words.Name == "Copy")
                        copy = words.InnerText;
                    if (words.Name == "Close")
                        close = words.InnerText;
                    if (words.Name == "CalibDone")
                        calibDone = words.InnerText;
                }
            }
        }
    }//load from xml
}

