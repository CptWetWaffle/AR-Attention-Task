using UnityEngine;
using System.Collections;

public class LevelChangeBehavior : MonoBehaviour {

    public static int attentionLevel = 0;
    public static int memoryLevel = 0;
    public static float initialMemoryTimer;
    public static float initialAttentionTimer;
    public static float tempMemoryTimer;
    public static float tempPicTimer;
    public static float tempAttentionTimer;
    public static int memoryTrials = 0;
    public static int attentionTrials = 0;
    public static bool increaseMemoryTime = false;
    public static bool increaseAttentionTime = false;
    public static bool logicApplied = false, toggleMemory = false;

    public static void LevelFailed()//game logic in case level was failed
    {
        GUIChangeLevel.success = false;
        if (SpawnTiles.levelName != "L0")
        {
            toggleMemory = false;
            if (SpawnTiles.useMemory)
            {
                memoryTrials++;//increase this variable so the program can know when reaches 3 level failures

                if (memoryTrials < 3)//if is still not the 3rd failure sets this variable to true so when level is being loaded it will be increased 30%
                    increaseMemoryTime = true;

                else if (memoryTrials >= 3 && memoryLevel - 2 > 0)//if it is already the 3rd failure, decreases 1 level and resets the memoryTrials variable so it starts counting from zero again
                {
                    memoryTrials = 0;
                    increaseMemoryTime = false;
                    memoryLevel = memoryLevel - 2;
                }

                //sets the next Level
                if (ChangeLevel.attentionAllLevels)
                    SpawnTiles.levelName = "L" + memoryLevel.ToString();
                else
                    SpawnTiles.levelName = "L" + attentionLevel.ToString();
            }

            else//does exactly the same but for attention levels
            {
                attentionTrials++;
                if (attentionTrials < 3)
                    increaseAttentionTime = true;

                else if (attentionTrials >= 3 && attentionLevel - 2 > 0)
                {
                    attentionTrials = 0;
                    increaseAttentionTime = false;
                    attentionLevel = attentionLevel - 2;
                }
                if (ChangeLevel.memoryAllLevels)
                    SpawnTiles.levelName = "L" + attentionLevel.ToString();
                else
                    SpawnTiles.levelName = "L" + memoryLevel.ToString();
            }
        }
        else
        {
            toggleMemory = true;
        }
    }

    //if level was completed within the time limit
    public static void LevelSucceeded()
    {
        GUIChangeLevel.success = true;
        if (SpawnTiles.levelName != "L0")
        {
            toggleMemory = false;
            for (int i = 0; i < LoadLevels.levels.Count; i++)
            {
                if (SpawnTiles.levelName == LoadLevels.levels[i].levelName)
                {
                    if (logicApplied)
                    {
                        if (SpawnTiles.useMemory)
                        {
                            if (initialMemoryTimer == SpawnTiles.timer)//checks if was competed within the original level time, this means there was not any previous failure, if level was completed but there were previous failures, next level will be the same but now with the original time limit again
                            {
                                LoadLevels.levels[i].completed = true;//is set as completed
                                if (!(memoryLevel + 2 > LoadLevels.levels.Count - 1))//and next level will be increased
                                    memoryLevel = memoryLevel + 2;
                            }

                            memoryTrials = 0;
                            increaseMemoryTime = false;

                            if (ChangeLevel.attentionAllLevels)
                                SpawnTiles.levelName = "L" + memoryLevel.ToString();
                            else
                                SpawnTiles.levelName = "L" + attentionLevel.ToString();
                        }

                        else//does the same for attention levels
                        {
                            if (initialAttentionTimer == SpawnTiles.timer)
                            {
                                LoadLevels.levels[i].completed = true;
                                if (!(attentionLevel + 2 > LoadLevels.levels.Count - 1))
                                    attentionLevel = attentionLevel + 2;
                            }

                            attentionTrials = 0;
                            increaseAttentionTime = false;

                            if (ChangeLevel.memoryAllLevels)
                                SpawnTiles.levelName = "L" + attentionLevel.ToString();
                            else
                                SpawnTiles.levelName = "L" + memoryLevel.ToString();
                        }

                        if (LoadLevels.levels[LoadLevels.levels.Count - 1].completed && LoadLevels.levels[LoadLevels.levels.Count - 2].completed)
                            ChangeLevel.allLevels = true;

                        logicApplied = false;
                    }
                }
            }
        }
        else
        {
            toggleMemory = true;
        }
    }

    //in case session is started from indicated memory and attention levels on the main menu ->this method sets all the previous levels as completed
    public static void SetAsCompleted()
    {
        for (int i = 0; i < attentionLevel; i++)
        {
            if (!LoadLevels.levels[i].useMemory)
                LoadLevels.levels[i].completed = true;
        }

        for (int i = 0; i < memoryLevel; i++)
        {
            if (LoadLevels.levels[i].useMemory)
                LoadLevels.levels[i].completed = true;
        }
    }
}
