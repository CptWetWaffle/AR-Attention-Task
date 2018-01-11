using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System;
using UnityEngine.UI;

public class Main_Menu : MonoBehaviour
{

    public static string uid = "";
    public bool handgui = true;

    XmlDocument xmlDoc;
    private static XmlWriter writer;

    static string initialText = "";

    /*public static bool LArm = false;
    public static bool RArm = false;*/

    public GameObject levelsGUI, mainPanel, /*Main_MenuBtn,*/ instructionsGUI, gamePlayUI, categoriesGUI, languageGroup, profilesGUI, plusBtn;

    string warningText = "";

    public static bool sound = true, music = true;
    public static bool textField = true;

    public static bool training = false;

    public Toggle soundToggle, pt, en, musicToggle;
    public Text idWelcome, warning, selectHand, instructions, levels, categories, quit, play, task, mainBtn, levelsSelText, infoLevelsText, memText, attText;
    public InputField idInput, memInput, attInput;
    public Button calibBtn, practice;

    public static GameObject Main_MenuUI;
    public static bool gameStarted = false;

    public static bool categDistractors = false;

    void Start()
    {
        /*Main_MenuUI = GameObject.Find("MainGuiPrefab");
        //Main_MenuBtn.SetActive(false);
        levelsGUI.SetActive(false);
        categoriesGUI.SetActive(false);
        instructionsGUI.SetActive(false);

        warningText = " ";

        if (Language.lang == "EN")
            en.isOn = true;
        else if (Language.lang == "PT")
            pt.isOn = true;

        /*if (LArm)
            leftHand.isOn = true;
        else if (RArm)
            rightHand.isOn = true;
            */
    }

    void Update()
    {
        /*if (GUIProfiles.hideUsersScroll)
            profilesGUI.SetActive(false);
        else
            profilesGUI.SetActive(true);

        calibBtn.gameObject.SetActive(false);

        if (mainPanel.activeSelf)
        {
            InitialText();
            task.GetComponentInChildren<Text>().text = Language.task;

            if (soundToggle.isOn)
                sound = true;
            else
                sound = false;

            if (musicToggle.isOn)
                music = true;
            else
                music = false;

            if (pt.isOn)
                Language.lang = "PT";
            else
                Language.lang = "EN";

            //Patient ID field
            if (textField)
                uid = idInput.text;
            else
                idInput.gameObject.SetActive(false);

            //Asks for the name or welcomes user
            idWelcome.text = initialText;

            //Arm choice text
            /*selectHand.text = Language.hand;*/
            /*RArm = rightHand.isOn;
            LArm = leftHand.isOn;*/
            /*play.text = Language.play;
            categories.text = Language.categories;
            levels.text = Language.levels;
            instructions.text = Language.instructions;
            quit.text = Language.quit;
            practice.GetComponentInChildren<Text>().text = Language.practice;
            levelsSelText.text = Language.selection;
            infoLevelsText.text = Language.info;
            memText.text = Language.memory;
            attText.text = Language.attention;

            if (calibBtn.gameObject.activeSelf)
                calibBtn.GetComponentInChildren<Text>().text = Language.calibration;

            //displaying warnings if is missing something
            warning.text = warningText;
        }*/
    }

    public void buttonEvent(int btnEvent)
    {
        if (btnEvent == 9)
        {
            if (GUIProfiles.totalProfiles > 0)
                GUIProfiles.hideUsersScroll = !GUIProfiles.hideUsersScroll;
        }

        else if (btnEvent == 7)//calibration button
        {
            /*if (LArm || RArm)
            {
                leftHand.gameObject.SetActive(false);
                rightHand.gameObject.SetActive(false);
                selectHand.gameObject.SetActive(false);
            }
            HandControl.setHand = true;*/

            if (uid == "")
            {
                warningText = Language.nameWarning;
            }

            /*else if (!LArm && !RArm)
            {
                warningText = Language.handWarning;
            }*/

            else
            {
                General_Settings.loadCalibValues = true;
                textField = false;
                initialText = Language.welcome + uid + "!";
                warningText = " ";
                //Main_MenuBtn.SetActive(true);
                languageGroup.SetActive(false);
                //UDPReceive.UDPConnected = false;
                ChangeLevel.resetOnce = true;
                ChangeLevel.ResetLevel();
            }
        }

        else if (btnEvent == 5 || btnEvent == 8 || btnEvent == 3)//Play, practice and Calibration buttons
        {
            /*if (LArm || RArm)
            {
                leftHand.gameObject.SetActive(false);
                rightHand.gameObject.SetActive(false);
                selectHand.gameObject.SetActive(false);
            }*/

            if (btnEvent == 5 && (memInput.text != "" || attInput.text != ""))
            {
                SetMemoryAndAttention();

                if (LevelChangeBehavior.memoryLevel < LevelChangeBehavior.attentionLevel)
                    SpawnTiles.levelName = "L" + LevelChangeBehavior.memoryLevel.ToString();
                else
                    SpawnTiles.levelName = "L" + LevelChangeBehavior.attentionLevel.ToString();

                LevelChangeBehavior.SetAsCompleted();
            }
            //Debug.Log(SpawnTiles.levelName);
            if (btnEvent == 5 && SpawnTiles.levelName == "L0")
            {
                SpawnTiles.levelName = "L1";
                LevelChangeBehavior.toggleMemory = false;
            }

            if (btnEvent == 8)
            {
                SpawnTiles.levelName = "L0";
                GUICategories.manualCategory = false;
            }

            HandControl.setHand = true;

            if (uid == "")
                warningText = Language.nameWarning;

            /*else if (!LArm && !RArm)
            {
                warningText = Language.handWarning;
                initialText = Language.welcome + uid + "!";
            }*/

            else
            {
                plusBtn.SetActive(false);
                if (SpawnTiles.levelName != "L0")
                {
                    for (int i = 0; i < SpawnTiles.cats.Count; i++)
                    {
                        if (uid == SpawnTiles.cats[i])
                        {
                            SpawnTiles.folder = uid;
                            GUICategories.manualCategory = true;
                            categDistractors = false;
                        }
                    }

                    if (!LoadMusics.musicsLoaded)
                        LoadMusics.startLoadingMusics = true;
                }

                if (!GUICategories.manualCategory)
                    categDistractors = true;

                if (!LoadGame.gameLoaded && btnEvent != 3)
                    LoadGame.loadingGame = true;

                textField = false;
                warningText = " ";
                //Main_MenuBtn.SetActive(true);
                languageGroup.SetActive(false);
                SpawnTiles.SaveActualLevelNumber();

                if (LoadGame.gameLoaded && btnEvent != 3)
                {
                    ChangeLevel.levelSet = false;
                    ChangeLevel.resetOnce = true;
                    ChangeLevel.ResetLevel();
                }
                else if (btnEvent == 3)
                {
                    languageGroup.SetActive(false);
                    HandControl.setHand = true;
                    levelsGUI.SetActive(true);
                    //Main_MenuBtn.SetActive(true);
                }
            }
        }

        else if (btnEvent == 4)//Categories button
        {
            languageGroup.SetActive(false);
            //Main_MenuBtn.SetActive(true);
            categoriesGUI.SetActive(true);
        }
        /*
        else if (btnEvent == 3)//levels button
        {
            languageGroup.SetActive(false);
            HandControl.setHand = true;
            levelsGUI.SetActive(true);
            Main_MenuBtn.SetActive(true); 
        }
        */
        else if (btnEvent == 2)//instructions button
        {
            //Main_MenuBtn.SetActive(true);
            languageGroup.SetActive(false);
            instructionsGUI.SetActive(true);
        }

        else if (btnEvent == 1)//quit button
        {
            Application.Quit();
        }

        else if (btnEvent == 0)//returns to main menu
        {
            Main_MenuUI.gameObject.SetActive(true);
            //Main_MenuBtn.SetActive(false);
            levelsGUI.SetActive(false);
            categoriesGUI.SetActive(false);
            ChangeLevel.levelSet = false;

            if (gamePlayUI.activeSelf)
                gamePlayUI.SetActive(false);

            instructionsGUI.SetActive(false);
        }
    }

    //sets the initial text according to if person already filled the name or not
    void InitialText()
    {
        if (uid == "")
            initialText = Language.name;

        else
            initialText = Language.welcome + uid + "!";
    }

    //reads memory and attention inputs and verifies if they match memory and attention levels, otherwise chosses aproximated levels
    void SetMemoryAndAttention()
    {
        /*int tempMem = LevelChangeBehavior.memoryLevel;
        int tempAtt = LevelChangeBehavior.attentionLevel;


        if (memInput.text != "")
        {
            LevelChangeBehavior.increaseMemoryTime = false;
            LevelChangeBehavior.memoryTrials = 0;
            tempMem = int.Parse(memInput.text);
        }

        if (attInput.text != "")
        {
            LevelChangeBehavior.increaseAttentionTime = false;
            LevelChangeBehavior.attentionTrials = 0;
            tempAtt = int.Parse(attInput.text);
        }

        //memory level
        if (tempMem >= LoadLevels.levels.Count)
        {
            if (LoadLevels.levels[LoadLevels.levels.Count - 1].useMemory)
                LevelChangeBehavior.memoryLevel = LoadLevels.levels.Count - 1;
            else
                LevelChangeBehavior.memoryLevel = LoadLevels.levels.Count - 2;
        }

        else if (tempMem <= 1)
        {
            if (LoadLevels.levels[1].useMemory)
                LevelChangeBehavior.memoryLevel = 1;
            else
                LevelChangeBehavior.memoryLevel = 2;
        }

        else if (LoadLevels.levels[tempMem].useMemory)
            LevelChangeBehavior.memoryLevel = tempMem;

        else if (tempMem > 1)
            LevelChangeBehavior.memoryLevel = tempMem - 1;

        //atention level
        if (tempAtt >= LoadLevels.levels.Count)
        {
            if (!LoadLevels.levels[LoadLevels.levels.Count - 1].useMemory)
                LevelChangeBehavior.attentionLevel = LoadLevels.levels.Count - 1;
            else
                LevelChangeBehavior.attentionLevel = LoadLevels.levels.Count - 2;
        }

        else if (tempAtt <= 1)
        {
            if (!LoadLevels.levels[1].useMemory)
                LevelChangeBehavior.attentionLevel = 1;
            else
                LevelChangeBehavior.attentionLevel = 2;
        }

        else if (!LoadLevels.levels[tempAtt].useMemory)
            LevelChangeBehavior.attentionLevel = tempAtt;

        else if (tempAtt > 1)
            LevelChangeBehavior.attentionLevel = tempAtt - 1;

        memInput.text = "";
        attInput.text = "";*/
    }
}
