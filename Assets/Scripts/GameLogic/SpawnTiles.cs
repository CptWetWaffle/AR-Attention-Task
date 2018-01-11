using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System;

public class SpawnTiles : MonoBehaviour
{
    public static int columns, rows, targets, correctchoices, totalToRender;
    public static bool showcorrect, useDistractors, useMemory, valence, completed;
    public static float timer, picTimer;
    public static string imagesToDisplay;

    public Transform tile, mediumTile, table, bigTile;
    public GameObject calibPrefab;

    public static float spacing = 0.35f;//space between the tiles
    public static float xoffset = -1.9f;
    public static float zoffset = -6;
    public static bool spawn = false;

    public static List<Texture> selectionList = new List<Texture>();//list with targets
    public static List<GameObject> hints = new List<GameObject>();//list that stores the cubes with the correct answers
    public static List<GameObject> incorrect = new List<GameObject>();//list that stores the distractors cubes
    public static List<Texture> texturesToRender = new List<Texture>();//list that stores all the images that will be displayed on the current level
    public static List<string> texturesToExport = new List<string>();//list with names of textures being displayed on the current level

    public static List<String> cats = new List<String>();//list of categories
    public static List<String> files = new List<String>();//list of distractors files

    //public static List<List<Texture>> images = new List<List<Texture>>();//list of list of all images split by categories
    public static List<List<String>> imagesNames = new List<List<String>>();

    public static string levelName = "L1";
    public static int totalLevels = 0;

    public static bool big = false;
    public static bool medium = false;

    public static string folder = "";
    public static int catNumber = 0;
    float height = 60f;

    static string filepath = String.Empty;
    XmlDocument xmlDoc;
    private static XmlWriter writer;
    public static string path;
    public static TextWriter file;
    public static bool distractorsLoaded = false;
    public static bool readyToSpawn = false;

    public static Texture randtxt;
    public static bool spawnOnce = false;

    public static int totCategories;

    public static bool imagesLoaded = false;


    public static bool categoriesLoaded = false;
    public static bool startLoadingDistrators = false, practiceStarted = false;

    public static int actualLevel = 1;

    //load the categories by reading folders
    IEnumerator Start()
    {
        //spawnOnce = true;
        if (!categoriesLoaded)
        {
            if (Directory.Exists(Application.dataPath + @"/Categories/"))
            {
                DirectoryInfo di = new DirectoryInfo(Application.dataPath + @"/Categories/");
                DirectoryInfo[] folders = di.GetDirectories();

                foreach (DirectoryInfo fi in folders)
                {
                    yield return fi.Name;

                    if (fi.Name != "Distractors")
                    {
                        cats.Add(fi.Name);
                        totCategories++;
                    }
                }
            }
            categoriesLoaded = true;
            LoadGame.readyToLoadImages = true;
        }
    }

    void Update()
    {
        if (readyToSpawn && spawn)//allows level to be set on the table
        {
            spawn = false;
            Spawn();
            readyToSpawn = false;
        }

        if (spawnOnce)
        {
            spawn = true;
            spawnOnce = false;
        }
    }

    //returns a random texture from the selected category folder
    public static Texture randTexture()
    {
        Texture tp = null;

        int rand = UnityEngine.Random.Range(0, LoadGame.levelCategory.Count);
        tp = LoadGame.levelCategory[rand];
        tp.name = LoadGame.levelCategory[rand].name;
        return tp;
    }

    //populate the lists with target textures and textures to render on cubes
    public static void initTexture()
    {
        startLoadingDistrators = false;
        if (folder != "")//first select the targets which will be stored inside the selectionList
        {
            if (imagesToDisplay == "")//if imagesToDisplay is empty targets will be chosen randomly
            {
                do
                {
                    randtxt = randTexture();

                    if (!selectionList.Contains(randtxt))
                    {
                        selectionList.Add(randtxt);
                        //Debug.Log("random target: " + randtxt.name);

                        if (GUICategories.manualCategory)
                        {
                            for (int i = 0; i < cats.Count; i++)
                            {
                                if (cats[i] == folder)
                                {
                                    for (int a = 0; a < imagesNames[i].Count; a++)
                                    {
                                        if (imagesNames[i][a] == randtxt.name)
                                        {
                                            imagesNames[i].RemoveAt(a);
                                        }
                                    }
                                }
                            }
                        }
                    }
                } while (selectionList.Count < targets);
            }
            else//else if imagesToDisplay are not empty targets are the ones specified on the field split by commas
            {
                string[] words = imagesToDisplay.Split(new char[] { ',' });

                for (int i = 0; i < words.Length; i++)
                {
                    int tempImage = int.Parse(words[i]);
                    selectionList.Add(LoadGame.levelCategory[tempImage]);
                }
            }

            //starts populating the array texturesToRender that will store all the images will be rendered on that level
            for (int i = 0; i < targets; i++)
            {
                Texture tp = selectionList[i];
                texturesToRender.Add(tp);

            }

            //chooses amongst targets the enough to match the number of correct choices
            if (correctchoices > targets)
            {
                for (int j = targets; j < correctchoices; j++)
                {
                    int rand = UnityEngine.Random.Range(0, selectionList.Count);
                    Texture tp = selectionList[rand];
                    texturesToRender.Add(tp);
                }
            }

            //populates the array testuresToRender with the images that are not targets - > considered as distractors
            if (totalToRender > correctchoices)
            {
                do
                {
                    Texture randTxt2;
                    //Debug.Log("distractors[0]: " + LoadGame.distractors[0].name);
                    if (useDistractors)//if useDistractors is activated then distractors will be loaded from distractors folder otherwise will be from the same category folder
                    {
                        randTxt2 = LoadGame.distractors[0];
                        randTxt2.name = LoadGame.distractorsNames[0];
                        //Debug.Log("LoadGame.distractorsNames[0]: " + LoadGame.distractorsNames[0]); 
                    }
                    else
                        randTxt2 = randTexture();

                    if (!texturesToRender.Contains(randTxt2) && !selectionList.Contains(randTxt2))
                    {
                        texturesToRender.Add(randTxt2);
                        if (useDistractors)
                        {
                            LoadGame.distractors.RemoveAt(0);
                            LoadGame.distractorsNames.RemoveAt(0);
                        }
                    }

                    else if (texturesToRender.Contains(randTxt2) && !selectionList.Contains(randTxt2))
                    {
                        for (int i = 0; i < cats.Count; i++)
                        {
                            if (cats[i] == folder)
                            {
                                if (texturesToRender.Count >= imagesNames[i].Count)
                                {
                                    //Debug.Log("texturesToRender.Count >= imagesNames[i].Count: " + texturesToRender.Count + ">" + imagesNames[i].Count);
                                    texturesToRender.Add(randTxt2);

                                }
                            }
                        }
                    }

                } while (texturesToRender.Count < totalToRender);
            }
            //shuffles testuresToRender to offer a more random display on the table
            shuffleList();

            //saves the level info
            SaveLevelInfo();

            if (texturesToRender.Count == totalToRender && !MainGUI.settingLevel && !imagesLoaded)
            {
                imagesLoaded = true;
                LoadGame.loadingGame = false;
                GUIChangeLevel.levelStarted = 1;
                if (SpawnTiles.levelName != "L0")
                {
                    Main_Menu.gameStarted = true;
                }
                else
                {
                    practiceStarted = true;
                }
                ChangeLevel.resetOnce = true;
                ChangeLevel.ResetLevel();
            }

            if (!Main_Menu.gameStarted && levelName != "L0")
            {
                Main_Menu.gameStarted = true;
                ChangeLevel.resetOnce = true;
                ChangeLevel.ResetLevel();
            }
        }
        LoadGame.distractorsSet = false;
        LoadGame.categoryImagesLoaded = false;
    }

    //Shuffles the texturesToRender List
    public static void shuffleList()
    {
        LoadGame.levelCategory.Clear();

        for (int i = 0; i < texturesToRender.Count; i++)
        {
            Texture temp = texturesToRender[i];
            int randomIndex = UnityEngine.Random.Range(i, texturesToRender.Count);
            texturesToRender[i] = texturesToRender[randomIndex];
            texturesToRender[randomIndex] = temp;
        }

        for (int i = 0; i < texturesToRender.Count; i++)
        {
            string temp = texturesToRender[i].name;
            texturesToExport.Add(temp);
        }
    }

    //Create Grid
    void Spawn()
    {
        if (Main_Menu.gameStarted || practiceStarted)
        {
            Debug.Log("Started to spawn");
            int i = 1;//increment for name
            int count = 0;
            int j = 0;//increment for texturesToRender
            for (int z = 0; z < rows; z++)
            { //rows
                for (int x = 0; x < columns; x++)
                { //columns	
                    spacing = 0.35f;
                    xoffset = 0;
                    zoffset = -6;

                    SpacingAndOffSet();

                    height = 61f;

                    Transform tileToUse;

                    if (big)
                        tileToUse = bigTile;
                    else if (medium)
                        tileToUse = mediumTile;
                    else
                        tileToUse = tile;

                    Transform t = Instantiate(tileToUse, new Vector3((xoffset + x) * spacing * 350, height, (zoffset + z) * spacing), Quaternion.identity) as Transform;//objects to transform
                    t.gameObject.name = "Tile" + i;//set the name of the object
                    t.gameObject.tag = "Tile";
                    t.parent = GameObject.FindGameObjectWithTag("Target").transform;

                    GameObject go = t.Find("ImageDisplay").gameObject; //convert transforms into gameobjects

                    i++;

                    Texture temp = texturesToRender[j];

                    if (selectionList.Contains(temp))
                    {
                        go.renderer.material.mainTexture = temp; //assign random texture -----> it has to change into a texture from a list (size = correctchoices)
                        go.renderer.material.mainTextureScale = new Vector2(-1, -1);//flip the texture
                        go.renderer.material.color = Color.white;
                        hints.Add(t.gameObject);
                        count++;
                    }
                    else
                    {
                        go.renderer.material.mainTexture = temp;
                        go.renderer.material.color = Color.white;
                        go.renderer.material.mainTextureScale = new Vector2(-1, -1);//flip the texture
                        incorrect.Add(t.gameObject);
                    }

                    j++;
                }
            }

            tile.transform.position = new Vector3(0, 20000, 0);
            mediumTile.transform.position = new Vector3(0, 20000, 0);
            bigTile.transform.position = new Vector3(0, 20000, 0);
            spawn = false;
        }
    }//end of Spawn

    //calculates the offsets and spacing according to number of columns and rows
    void SpacingAndOffSet()
    {
        for (int i = 2; i <= rows; i++)
        {
            zoffset = zoffset - 0.87f;
        }
        /*
        for (int i = 6; i <= columns; i++)
        {
            xoffset = xoffset - 0.5f;
        }*/

        for (int i = 2; i <= columns; i++)
        {
            xoffset = xoffset - 0.5f;
        }

        if (columns >= 6 || rows >= 3)
            spacing = 0.33f;

        if (columns >= 8 || rows >= 4)
            spacing = 0.31f;

        if (columns == 9 || rows == 5)
            spacing = 0.29f;

        //if (columns > 4  && columns < 6 && rows <= 3)
        if (columns < 6 && rows <= 3)
        {
            big = true;
            table.position = new Vector3(table.position.x, -0.34f, table.position.z);
            spacing = 0.45f;
            zoffset = -4.9f;

            if (columns > 4)
                xoffset = -2f;

            if (rows == 2)
                zoffset = -5.3f;

            if (rows == 3)
                zoffset = -5.8f;
        }
        else if (columns >= 6 && columns < 8 && rows <= 3)
        {
            big = false;
            medium = true;
            table.position = new Vector3(table.position.x, -0.32f, table.position.z);
            spacing = 0.39f;

            if (rows == 1)
                zoffset = -5.5f;

            if (rows == 2)
                zoffset = -5.9f;

            if (rows == 3)
                zoffset = -6.6f;
        }
        else
        {
            table.position = new Vector3(table.position.x, -0.27f, table.position.z);
            medium = false;
            big = false;
        }

        if (columns == 7 && rows <= 3)
        {
            spacing = 0.35f;
            xoffset = -3f;

            if (rows == 1)
                zoffset = -6.4f;

            if (rows == 2)
                zoffset = -6.7f;

            if (rows == 3)
                zoffset = -7.2f;
        }

        if (columns == 8)
            xoffset = -3.5f;

        if (columns == 8 && rows <= 3)
        {
            if (rows == 1)
                zoffset = -7.2f;
            if (rows == 2)
                zoffset = -7.7f;
            if (rows == 3)
                zoffset = -8.2f;
        }

        if (columns == 9)
            xoffset = -4f;

        if (columns == 9)
        {
            if (rows == 1)
                zoffset = -7.7f;
            if (rows == 2)
                zoffset = -8.2f;
            if (rows == 3)
                zoffset = -8.7f;
            if (rows == 4)
                zoffset = -9.1f;
            if (rows == 5)
                zoffset = -9.4f;
        }


    }

    //export cubes textures and calibration values to csv file
    public static void SaveLevelInfo()
    {
        if (Main_Menu.uid != "")
        {
            path = Application.dataPath + "/Attention_Task_Log/" + Main_Menu.uid + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";

            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            filepath = path + Main_Menu.uid + DateTime.Now.ToString("yyyyMMddHHmmss") + levelName + "_Level_Details" + ".csv";
            file = new StreamWriter(filepath, true);

            string newLine = texturesToExport[0];

            string targetsLine = selectionList[0].name;

            for (int i = 1; i < texturesToExport.Count; i++)
            {
                newLine = newLine + "," + texturesToExport[i];
            }

            for (int i = 1; i < selectionList.Count; i++)
            {
                targetsLine = targetsLine + "," + selectionList[i].name;
            }

            file.WriteLine(levelName + "," + newLine);
            file.WriteLine("Targets," + targetsLine);
            file.WriteLine("");
            file.Close();
        }
    }

    public static void SaveActualLevelNumber()
    {
        string actualLevelNumber = levelName;
        string numberL = actualLevelNumber[1].ToString();//level in SpawnTiles scripe is a string so needs to be "split" to parse it into an int, the first number corresponds to the position 1 of the array of characters

        if (actualLevelNumber.Length > 2)//if current level is higher than 9, or higher than 99 it adds the next characters posiotened on the array of characters
        {
            for (int i = 2; i < actualLevelNumber.Length; i++)
            {
                numberL = numberL + actualLevelNumber[i].ToString();
            }
        }

        //parses the obtained string number into an int
        actualLevel = int.Parse(numberL);
    }

}