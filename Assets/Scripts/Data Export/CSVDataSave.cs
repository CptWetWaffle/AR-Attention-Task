using UnityEngine;
using System.IO;
using System;
//using //vuforia;

public class CSVDataSave : MonoBehaviour
{
    //this array will be the header for the csv file - change this list according to the ones needed for each game
    public String[] header = new String[21];
    //public static List<String> header = new List<String>();

    //this list will store the values for each variable in the previous list, number of items = amount of "titles" in the arrary header --> fill in this array in the SetValues() function
    public String[] variables = new String[21];

    TextWriter file;
    string filepath = String.Empty;
    string path;
    public static bool islogging = false;
    public static bool startLog = false;

    //Game object to track position (x, y and z)
    public GameObject IKTarget;

    //public static int mouseClicks = 0;

    string pictureVisible;
    string targetName;

    void Start()
    {
        header = new String[21];
        variables = new String[21];
        header = new String[] { "Time", "LevelStarted", "IKTargetX", "IKTargetZ", "Valence", "LevelNumber", "Memory", "Category", "LevelFailures", "LevelTime", "PicTime", "SelectedTexture", "FullScore", "HalfScore", "Wrong", "TargetsToFind", "PictureVisible", "HintsActive", "Score", "Columns", "Rows" };
    }

    void Update()
    {
        SetValues();//continuously updates values

        if (startLog && !islogging)
        {
            logInit();
        }

        if (GUIChangeLevel.stopLog)
        {
            CancelInvoke("CSVWrite");
            file.Close();
        }
    }

    //sets the values for each variable
    void SetValues()
    {
        int hours = int.Parse(DateTime.Now.ToString("HH")) * 60 * 60;
        int minutes = int.Parse(DateTime.Now.ToString("mm")) * 60;
        int seconds = int.Parse(DateTime.Now.ToString("ss"));
        int totalSeconds = hours + minutes + seconds;
        string temp = totalSeconds + "." + DateTime.Now.Millisecond.ToString("000");

        if (MainGUI.picture)
            pictureVisible = "1";
        else
            pictureVisible = "0";

        string levelFail;

        if (SpawnTiles.useMemory)
            levelFail = LevelChangeBehavior.memoryTrials.ToString();
        else
            levelFail = LevelChangeBehavior.attentionTrials.ToString();

        int toFind = SpawnTiles.correctchoices - Scoring.GetCorrect();

        int mem;
        int actHints;

        if (SpawnTiles.useMemory)
            mem = 1;
        else
            mem = 0;

        if (Hints.hintActive)
            actHints = 1;
        else
            actHints = 0;

        //float posX, posZ;

/*        posX = IKTarget.transform.position.x;
        posZ = IKTarget.transform.position.z;*/


        string actualLevelNumber = SpawnTiles.levelName;
        string numberL = actualLevelNumber[1].ToString();//level in SpawnTiles scripe is a string so needs to be "split" to parse it into an int, the first number corresponds to the position 1 of the array of characters

        if (actualLevelNumber.Length > 2)//if current level is higher than 9, or higher than 99 it adds the next characters posiotened on the array of characters
        {
            for (int i = 2; i < actualLevelNumber.Length; i++)
            {
                numberL = numberL + actualLevelNumber[i].ToString();
            }
        }

        //Debug.Log("dataExport: " + SpawnTiles.timer.ToString());
        //Debug.Log(Collision_Detection.selectedTexture);
        variables[0] = temp;//time in seconds
        variables[1] = GUIChangeLevel.levelStarted.ToString();//SessionStarted to count
        variables[2] = "";//IKTargetX or hand->if calibrated
        variables[3] = "";//IKTargetZ or hand->if calibrated
        variables[4] = GUIChangeLevel.iaps.ToString();//Valence
        variables[5] = numberL;//LevelNumber
        variables[6] = mem.ToString();//Memory
        variables[7] = SpawnTiles.folder;//Category
        variables[8] = levelFail;//LevelFailures
        variables[9] = SpawnTiles.timer.ToString();//LevelTime
        variables[10] = SpawnTiles.picTimer.ToString();//PicTimer
        variables[11] = Collision_Detection.selectedTexture;//the actual texture being selected
        variables[12] = Scoring.fullScore.ToString();//FullScore - correct choice
        variables[13] = Scoring.halfScore.ToString();//corect choice after hints displayed
        variables[14] = Scoring.GetError().ToString();//Wrong
        variables[15] = toFind.ToString();//Targets to find
        variables[16] = pictureVisible;//PictureVisible
        variables[17] = actHints.ToString();//HintsActive
        variables[18] = Scoring.GetTotalPoints().ToString();//TotalScore  
        variables[19] = SpawnTiles.columns.ToString();//Columns
        variables[20] = SpawnTiles.rows.ToString();//Rows 
    }

    //initiates the csv file
    void logInit()
    {
        islogging = true;
        path = Application.dataPath + "/Attention_Task_Log/" + Main_Menu.uid + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
        if (!Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        filepath = path + Main_Menu.uid + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        file = new StreamWriter(filepath, true);

        //builds the string that will be the header of the csv file
        string fillHeader = header[0];

        for (int i = 1; i < header.Length; i++)
        {
            fillHeader = fillHeader + "," + header[i];
        }

        //writes the first line of the file (header)
        file.WriteLine(fillHeader);

        InvokeRepeating("CSVWrite", 1F, 0.031F);//invokes the function that adds new lines to the files in time seconds, then repeatedly every repeatRate seconds
        startLog = false;
    }

    //writes new line in the csv file
    void CSVWrite()
    {
        string newLine = variables[0];

        for (int i = 1; i < variables.Length; i++)
        {
            newLine = newLine + "," + variables[i];
        }

        file.Write(newLine);
        file.WriteLine("");
    }
}