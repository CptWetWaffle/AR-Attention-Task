using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System;
using Assets.Scripts.GameLogic;
using GameLogic;
using UnityEngine.UI;
//using vuforia;

public class GUIChangeLevel : MonoBehaviour
{
    public GameObject mainGUI, valenceGroup, quitBtn, Main_MenuBtn, IKTarget;
    public Text completionText, totalScore, warning, congratsLabel, levelChangeLabel, levelChangeTimer, memLabel, attLabel, complLabel, memLevel, attLevel;

    public static GameObject[] iapsToggle;
    public static bool[] iapsChoice = new bool[9];//stores the boolean value of the valence toggles

    static string /*filepath = String.Empty,*/ filepath2 = String.Empty;
    XmlDocument xmlDoc;
    private static XmlWriter writer;
    string path;
    TextWriter file, file2;

    public static int iaps, temp;
    public static bool setting = false;

    public static bool valenceRated = false;
    public static bool levelFailed = false;
    public static bool stopLog = false;
    
    public static float changeLevelTimer = 6.0f;

    int input;
    //bool activateTimer = true;
    public static bool success = false;
    public static int levelStarted = 0;

    void Start()
    {
        IComparer myToggleComparer = new myTogglesSorter();//sorts the toggles by order
        iapsToggle = GameObject.FindGameObjectsWithTag("IAPSToggle");
        Array.Sort(iapsToggle, myToggleComparer);
        
        for (int i = 0; i < iapsToggle.Length; i++)
        {
            iapsToggle[i].GetComponent<Toggle>().isOn = false;//sets all toggles as false
        }

        levelChangeLabel.text = Language.getReady;
        levelChangeLabel.gameObject.SetActive(false);
        levelChangeTimer.gameObject.SetActive(false);
        this.gameObject.SetActive(false);//this is set to false and will be set to true in the end of each level
    }

    void Update()
    {
        quitBtn.gameObject.GetComponentInChildren<Text>().text = Language.quit;
        if(success)
            congratsLabel.text = Language.congrats;
        else
            congratsLabel.text = " ";

        memLabel.text = Language.memory + ":";
        attLabel.text = Language.attention + ":";
        complLabel.text = Language.levels + ":";
        memLevel.text = LevelChangeBehavior.memoryLevel.ToString();
        attLevel.text = LevelChangeBehavior.attentionLevel.ToString();

        if (!CountSessionTime.timeSessionExpired)//does this if session time has not ended yet
        {
            if (!ChangeLevel.allLevels)
            {
                if (SpawnTiles.valence && !valenceRated)
                    warning.text = Language.rate;//warns no rate targets if valence is activated

                else if ((SpawnTiles.valence && valenceRated) || !SpawnTiles.valence)
                {
                    valenceGroup.SetActive(false);
                    warning.text = " ";
                    //activateTimer = true;
                    levelChangeLabel.gameObject.SetActive(true);
                    levelChangeTimer.gameObject.SetActive(true);
                    levelChangeTimer.text = changeLevelTimer.ToString("0");

                    changeLevelTimer -= Time.deltaTime;
                    if (!setting)
                    {
                        ChangeLevel.levelSet = false;
                        ChangeLevel.setOnce = true;
                        ChangeLevel.SetLevel();
                        setting = true;
                    }

                    //next level will be set as soon as changeLevelTimer reaches zero
                    if (changeLevelTimer < 0)
                    {
                        changeLevelTimer = 0;
                        //activateTimer = false;
                        ContinueOrQuit(1);
                    }
                }
                else
                    warning.text = " ";   
            }
        }

        if (this.gameObject.activeSelf)
        {
            levelStarted = 0;
            SpawnTiles.practiceStarted = false;
            totalScore.text = Scoring.GetTotalPoints().ToString();//displays the score
            Main_MenuBtn.SetActive(false);

            if (SpawnTiles.valence)//displays valence group if valence is activated
                valenceGroup.SetActive(true);
            else
                valenceGroup.SetActive(false);
        }
        else
            levelStarted = 1;

        //IAPS toggle buttons can be chosen by pressing the numbers to the keyboard
        if (Input.anyKey)
        {
            if (int.TryParse(Input.inputString, out input))
            {
                iaps = input;
                input = input - 1;
                iapsChoice[input] = true;
                valenceRated = true;

                for (int a = 0; a < 9; a++)
                {
                    if (a != input)
                    {
                        iapsChoice[a] = false;
                    }
                }
            }
        }

        if(SpawnTiles.valence)
        {
            for (int i = 0; i < iapsToggle.Length; i++)
            {
                if (iapsChoice[i])
                    iapsToggle[i].GetComponent<Toggle>().isOn = true;
                else
                    iapsToggle[i].GetComponent<Toggle>().isOn = false;
            }

            for (int i = 0; i < iapsToggle.Length; i++)
            {
                if (iapsToggle[i].GetComponent<Toggle>().isOn)
                {
                    iapsChoice[i] = true;
                    valenceRated = true;
                }
                else
                    iapsChoice[i] = false;
            }  
        }

        if (!ChangeLevel.allLevels)
        {
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
                ContinueOrQuit(1);
             }
        }

        //shortcut key to exit to main menu or to quit application in case all levels are completed
        if (Input.GetKeyDown(KeyCode.Escape) && this.gameObject.activeSelf)
        {
            Collision_Detection.changeLevel = false;
            
            //SaveIAPSInfo();
            if (!ChangeLevel.allLevels)
            {
                Main_Menu.Main_MenuUI.SetActive(true);
                IKTarget.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                Application.Quit();
            }
        }

        if (CountSessionTime.timeSessionExpired)
        {
            warning.text = Language.session;
            stopLog = true;
        }

        if (ChangeLevel.allLevels)
        {
            warning.text = Language.allLevels;
            stopLog = true;
        }
        else
        {
            completionText.text = Language.level + SpawnTiles.actualLevel + Language.completed + Scoring.GetPoints().ToString() + Language.points;
        }
    }

    public void ContinueOrQuit(int cont)
    {
        if (cont == 1)
        {
            Hints.blinkTimer = 8.0f;
            SpawnTiles.SaveActualLevelNumber();
            levelChangeLabel.gameObject.SetActive(false);
            levelChangeTimer.gameObject.SetActive(false);

            if (SpawnTiles.valence && !valenceRated)
                warning.text = Language.rate;
            else
            {
                if (true)//if all is ready -> goes to next level
                {
                    Collision_Detection.changeLevel = false;
                    success = false;
                    //Debug.Log(ChangeLevel.levelSet);
                    SpawnTiles.practiceStarted = true;
                    ChangeLevel.resetOnce = true;
                    ChangeLevel.ResetLevel();
                    IKTarget.collider.enabled = true;
                    gameObject.SetActive(false);
                }
                /*else if (Main_Menu.LArm && !HandControl.leftCorrectPos)
                    warning.text = Language.left;
                else if (Main_Menu.RArm && !HandControl.rightCorrectPos)
                    warning.text = Language.right;*/
             }
            SaveLevelsInfo();//saves level info -> memory and attention current levels
            //SaveIAPSInfo();//saves level targets rate -> valence
        }
        else//if is not to continue it will return to main menu
        {
            Hints.blinkTimer = 8.0f;
            success = false;
            IKTarget.collider.enabled = true;
            Collision_Detection.changeLevel = false;
            SaveLevelsInfo();
            /*
            if (SpawnTiles.valence)
                SaveIAPSInfo();
*/
            if (!ChangeLevel.allLevels)
            {
                Main_Menu.Main_MenuUI.SetActive(true);
                this.gameObject.SetActive(false);   
            }
            else//if all levels are complete the application will close
                Application.Quit(); 
        }
    }

    //export valence rate to csv file
    void SaveIAPSInfo()
    {/*
        path = Application.dataPath + "/Attention_Task_Log/" + Main_Menu.uid + "/";

        if (!Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        filepath = path + Main_Menu.uid + DateTime.Now.ToString("yyyyMMdd") + SpawnTiles.actualLevel + "_Level_Details" + ".csv";
        file = new StreamWriter(filepath, true);
        */
        for (int i = 0; i < GUIChangeLevel.iapsChoice.Length; i++)
        {
            if (GUIChangeLevel.iapsChoice[i])
            {
                iaps = i + 1;
            }    
        }
        /*
        file.WriteLine("valence," + iaps);
        iaps = 0;
        file.WriteLine("");
        file.Close();
        /*
        for (int i = 0; i < iapsChoice.Length; i++)
        {
            iapsChoice[i] = false;
            iapsToggle[i].GetComponent<Toggle>().isOn = false;
        }*/
    }

    public class myTogglesSorter : IComparer
    {
        int IComparer.Compare(System.Object x, System.Object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
        }
    }

    //saves the current memory and attention levels
    void SaveLevelsInfo()
    {
        path = Application.dataPath + "/Attention_Task_Log/" + Main_Menu.uid + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";

        if (!Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        filepath2 = path + Main_Menu.uid + DateTime.Now.ToString("yyyyMMdd") + "_Levels_Completed" + ".xml";
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.NewLineOnAttributes = true;
        writer = XmlWriter.Create(filepath2, settings);
        writer.WriteStartDocument();

        writer.WriteStartElement("Config");

        writer.WriteElementString("memoryLevel", LevelChangeBehavior.memoryLevel.ToString());
        writer.WriteElementString("attentionLevel", LevelChangeBehavior.attentionLevel.ToString());

        //writer.WriteEndElement();//Calibration

        //writer.WriteStartElement("Config");

        writer.WriteEndElement();//Calibration

        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();  
    }
}



