using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{

    public GameObject itemPrefab, levelsUI;
    public float height = 85;
    public Text levelTitle;

    int totalLevels;

    public String[] headerEN = new String[5];//english header
    public String[] headerPT = new String[5];//portuguese header

    int columns;
    int rows;
    string totalCubes, targets, correct, timer, category;

    string showTime;
    string memAtt;
    bool first = true;


    void Start()
    {
        headerEN = new String[] {"Total Cubes", "Correct Choices", "Targets", "Timer", "Category"};
        headerPT = new String[] {"Total de Cubos", "Escolhas Corretas", "Alvos", "Tempo", "Categoria"};

        //Main_MenuBtn.text = Language.main;
        
        SetHeaderLanguage();
        levelTitle.text = Language.levels;
        string filepath = Application.dataPath + @"/Settings/Levels.xml";
        XmlDocument xmlDoc = new XmlDocument();

        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);

            XmlNodeList levels = xmlDoc.GetElementsByTagName("Levels");
            
            foreach (XmlNode levelNumber in levels)
            {
                XmlNodeList levelcontent = levelNumber.ChildNodes;
                
                totalLevels = 0;
                

                foreach (XmlNode levelnum in levelcontent)
                {
                    string level = levelnum.Name;
                    
                   
                    //reads from the xml file and adds the values in the array values 
                    foreach (XmlNode xmlsettings in levelnum)
                    {
                        if (xmlsettings.Name == "columns")
                            columns = int.Parse(xmlsettings.InnerText);
                        if (xmlsettings.Name == "rows")
                            rows = int.Parse(xmlsettings.InnerText);

                        int tempTotal = columns * rows;
                        totalCubes = tempTotal.ToString();

                        if (xmlsettings.Name == "correctchoices")
                            correct = xmlsettings.InnerText;
                        if (xmlsettings.Name == "targets")
                            targets = xmlsettings.InnerText;
                        if (xmlsettings.Name == "showcorrect")
                            showTime = xmlsettings.InnerText;  

                        if (xmlsettings.Name == "timer")
                        {
                            if (showTime == "true")
                                timer = xmlsettings.InnerText;
                            else
                                timer = "∞";
                        }

                        if (xmlsettings.Name == "category")
                            category = xmlsettings.InnerText;  
                        if (xmlsettings.Name == "useMemory")
                        {
                            if (xmlsettings.InnerText == "true")
                                memAtt = "Mem";
                            else
                                memAtt = "Att";
                        }
                    }

                    totalLevels++;
                    GameObject newItem = Instantiate(itemPrefab) as GameObject;
                    newItem.name = gameObject.name + level;
                    newItem.transform.SetParent(gameObject.transform);

                    Button btn = newItem.GetComponentInChildren<Button>();
                    Text btnText = btn.GetComponentInChildren<Text>();

                    if (memAtt == "Mem")
                        btnText.text = level + "-" + memAtt;
                    else
                        btnText.text = level;

                    Text cubes = newItem.transform.Find("Cubes").GetComponent<Text>();
                    cubes.text = totalCubes;

                    Text correctCh = newItem.transform.Find("Correct").GetComponent<Text>();
                    correctCh.text = correct;

                    Text targetsN = newItem.transform.Find("Targets").GetComponent<Text>();
                    targetsN.text = targets;

                    Text timerT = newItem.transform.Find("Timer").GetComponent<Text>();
                    timerT.text = timer;

                    Text categoryT = newItem.transform.Find("Category").GetComponent<Text>();
                    categoryT.text = category;

                    btn.onClick.AddListener(() => { SpawnTiles.levelName = level; levelsUI.SetActive(false); /*Debug.Log(SpawnTiles.levelName); */GoToLevel();});  
                }

                float scrollHeight = height * totalLevels;
                RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();
                RectTransform scrollableRectTransform = gameObject.GetComponentInParent<RectTransform>();
                Vector2 width = scrollableRectTransform.sizeDelta;
                containerRectTransform.sizeDelta = new Vector2(width.x, scrollHeight);
            }
        }  
    }
    
    void SetHeaderLanguage()
    {
        if (first)
        {
            List<string> header = new List<string>();

            for (int i = 0; i < headerEN.Length; i++)
            {
                if(Language.lang == "PT")
                    header.Add(headerPT[i]);
                else if (Language.lang == "EN")
                    header.Add(headerEN[i]);
            }

            GameObject newItem = Instantiate(itemPrefab) as GameObject;
            newItem.name = gameObject.name + "header";
            newItem.transform.SetParent(gameObject.transform);

            Button btn = newItem.GetComponentInChildren<Button>();
            btn.gameObject.SetActive(false);
            Text cubes = newItem.transform.Find("Cubes").GetComponent<Text>();
            cubes.text = header[0];

            Text correctCh = newItem.transform.Find("Correct").GetComponent<Text>();
            correctCh.text = header[1];

            Text targetsN = newItem.transform.Find("Targets").GetComponent<Text>();
            targetsN.text = header[2];

            Text timerT = newItem.transform.Find("Timer").GetComponent<Text>();
            timerT.text = header[3];

            Text categoryT = newItem.transform.Find("Category").GetComponent<Text>();
            categoryT.text = header[4];
            first = false;
        }
    }

    void GoToLevel()
    {
        

        if (SpawnTiles.levelName != "L0")
            Main_Menu.gameStarted = true;

        HandControl.setHand = true;

        if (!LoadGame.gameLoaded)
            LoadGame.loadingGame = true;

        if (SpawnTiles.levelName != "L0")
        {
            for (int i = 0; i < SpawnTiles.cats.Count; i++)
            {
                if (Main_Menu.uid == SpawnTiles.cats[i])
                {
                    SpawnTiles.folder = Main_Menu.uid;
                    GUICategories.manualCategory = true;
                }
            }
        }

        if (!GUICategories.manualCategory)
            Main_Menu.categDistractors = true;

        if (!LoadMusics.musicsLoaded)
            LoadMusics.startLoadingMusics = true;

        SpawnTiles.SaveActualLevelNumber();

        if (LoadGame.gameLoaded)
        {
            ChangeLevel.levelSet = false;
            ChangeLevel.resetOnce = true;
            ChangeLevel.ResetLevel();
        }
    }
}