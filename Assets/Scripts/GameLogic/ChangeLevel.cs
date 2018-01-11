using UnityEngine;
using System.Collections.Generic;
using System;
//using vuforia;


public class ChangeLevel : MonoBehaviour
{
    public GameObject GUIchangelevel, IKTarget;
    public static GameObject gamePlayUI;
    public static int temp, levelNumber;
    public static bool allLevels = false, memoryAllLevels, attentionAllLevels, levelSet = false;
    public static float myTimer = 1.0f;
    public static List<String> completedCats = new List<String>();

    string path;

    public static bool resetOnce = false;
    public static bool setOnce = false, stopLog = false;

    public static bool changeSet = false;

    void Awake()
    {
        gamePlayUI = GameObject.Find("GamePlayUI");
    }

    void Update()
    {
        if (Collision_Detection.changeLevel)//if level has been completed checks if is the last one and enables the change level GUI
        {
            ReadLevel();

            myTimer -= Time.deltaTime;

            if (SpawnTiles.actualLevel >= LoadLevels.levels.Count - 2)
            {
                if (LoadLevels.levels[SpawnTiles.actualLevel].useMemory)
                    memoryAllLevels = true;
                else
                    attentionAllLevels = true;
            }

            if (myTimer <= 0 && myTimer > -1)
            {
                myTimer = -1;
                GUIchangelevel.SetActive(true);

                gamePlayUI.SetActive(false);
            }
        }
    }

    void ReadLevel()
    {
        //reads the actual level and transform it into an int
        string actualLevel = SpawnTiles.levelName;
        string numberL = actualLevel[1].ToString();//level in SpawnTiles script is a string so needs to be "split" to parse it into an int, the first number corresponds to the position 1 of the array of characters
        Debug.Log("my level is" + actualLevel);
        if (actualLevel.Length > 2)//if current level is higher than 9, or higher than 99 it adds the next characters posiotened on the array of characters
        {
            for (int i = 2; i < actualLevel.Length; i++)
            {
                numberL = numberL + actualLevel[i].ToString();
            }
        }

        //parses the obtained string number into an int
        temp = int.Parse(numberL);
    }

    //cleans variables and prepares application to receive a new level
    public static void ResetLevel()
    {
        if (resetOnce)
        {
            Resources.UnloadUnusedAssets();
            GUIChangeLevel.iaps = 0;
            Scoring.halfScore = 0;
            Scoring.fullScore = 0;
            stopLog = true;
            setOnce = true;

            if (!levelSet)
                SetLevel();

            Scoring.audio1.Stop();
            Scoring.hasPlayed = false;
            GUIChangeLevel.valenceRated = false;
            MainGUI.activateHint = false;
            MainGUI.clearTargets = true;
            Collision_Detection.activateTimer = false;

            //deletes all cubes from previous level
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
            if (tiles.Length > 0)
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    if(tiles[i].name != "Tile" && tiles[i].name != "TileBig" && tiles[i].name != "TileMedium")
                    Destroy(tiles[i]);
                }
            }

            MainGUI.allImages = false;
            General_Settings.enable = true;

            if (SpawnTiles.useMemory)
            {
                MainGUI.imgTimer = SpawnTiles.picTimer;
                MainGUI.picture = true;
                MainGUI.pictureReady = true;
            }
            else
            {

                gamePlayUI.SetActive(true);
                MainGUI.pictureReady = true;
                MoveCamera.animStatus = true;
                SpawnTiles.spawnOnce = true;
            }

            if (Main_Menu.Main_MenuUI.activeSelf)
                Main_Menu.Main_MenuUI.SetActive(false);

            if (Main_Menu.uid != "")
                CSVDataSave.startLog = true;

            resetOnce = false;
        }
    }

    //sets the new level according to level number set on LevelChangeBehavior
    public static void SetLevel()
    {
        if (setOnce)
        {
            MainGUI.settingLevel = true;
            Scoring.SetCorrect(0);
            Scoring.SetError(0);
            SpawnTiles.selectionList.Clear();
            SpawnTiles.texturesToRender.Clear();
            SpawnTiles.hints.Clear();
            SpawnTiles.incorrect.Clear();
            SpawnTiles.texturesToExport.Clear();
            Collision_Detection.selectedTexture = "NaN";

            Hints.hintActive = false;

            for (int i = 0; i < LoadLevels.levels.Count; i++)
            {
                if (SpawnTiles.levelName == LoadLevels.levels[i].levelName)
                {

                    SpawnTiles.columns = LoadLevels.levels[i].columns;
                    SpawnTiles.rows = LoadLevels.levels[i].rows;
                    SpawnTiles.totalToRender = SpawnTiles.columns * SpawnTiles.rows;
                    SpawnTiles.targets = LoadLevels.levels[i].targets;
                    SpawnTiles.correctchoices = LoadLevels.levels[i].correctChoices;

                    if (!GUICategories.manualCategory)
                        SpawnTiles.folder = LoadLevels.levels[i].category;

                    SpawnTiles.imagesToDisplay = LoadLevels.levels[i].imagesToDisplay;

                    if (Main_Menu.categDistractors)
                        SpawnTiles.useDistractors = false;
                    else
                        SpawnTiles.useDistractors = LoadLevels.levels[i].useDistractors;

                    if (LevelChangeBehavior.toggleMemory)
                        SpawnTiles.useMemory = !SpawnTiles.useMemory;
                    else
                        SpawnTiles.useMemory = LoadLevels.levels[i].useMemory;

                    SpawnTiles.valence = LoadLevels.levels[i].valence;
                    SpawnTiles.showcorrect = LoadLevels.levels[i].showCorrect;
                    SpawnTiles.completed = LoadLevels.levels[i].completed;
                    //TimerCount.startTime = LoadLevels.levels[i].selectionTime;

                    //increases time in case level was failed previously
                    if (SpawnTiles.useMemory)
                    {
                        if (!LevelChangeBehavior.increaseMemoryTime)
                        {
                            LevelChangeBehavior.initialMemoryTimer = LoadLevels.levels[i].levelTime;
                            LevelChangeBehavior.tempMemoryTimer = LoadLevels.levels[i].levelTime;
                            LevelChangeBehavior.tempPicTimer = LoadLevels.levels[i].picTimer;
                            SpawnTiles.timer = LoadLevels.levels[i].levelTime; ;
                            SpawnTiles.picTimer = LoadLevels.levels[i].picTimer;
                        }
                        else
                        {
                            LevelChangeBehavior.tempMemoryTimer = LevelChangeBehavior.tempMemoryTimer + (LevelChangeBehavior.tempMemoryTimer * 0.3f);
                            SpawnTiles.timer = LevelChangeBehavior.tempMemoryTimer;
                            LevelChangeBehavior.tempPicTimer = LevelChangeBehavior.tempPicTimer + (LevelChangeBehavior.tempPicTimer * 0.3f);
                            SpawnTiles.picTimer = LevelChangeBehavior.tempPicTimer;
                            //Debug.Log(LevelChangeBehavior.tempPicTimer);
                        }
                    }
                    else
                    {
                        if (!LevelChangeBehavior.increaseAttentionTime)
                        {
                            LevelChangeBehavior.initialAttentionTimer = LoadLevels.levels[i].levelTime;
                            LevelChangeBehavior.tempAttentionTimer = LoadLevels.levels[i].levelTime;
                            SpawnTiles.timer = LoadLevels.levels[i].levelTime;
                        }
                        else
                        {
                            LevelChangeBehavior.tempAttentionTimer = LevelChangeBehavior.tempAttentionTimer + (LevelChangeBehavior.tempAttentionTimer * 0.3f);
                            SpawnTiles.timer = LevelChangeBehavior.tempAttentionTimer;
                        }
                    }
                }
            }
            MainGUI.timeToDisplay = SpawnTiles.timer;
            //Debug.Log(SpawnTiles.timer);

            LoadGame.startLoadingCatImages = true;

            if (SpawnTiles.useDistractors)
                SpawnTiles.startLoadingDistrators = true;
            else
                LoadGame.distractorsSet = true;

            levelSet = true;
            MainGUI.settingLevel = false;
            setOnce = false;
        }
    }
}
