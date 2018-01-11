using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class LoadGame : MonoBehaviour
{

    public GameObject loadingPanel, Main_MenuBtn;
    // Use this for initialization
    public static bool loadingGame = false;
    public static bool gameLoaded = false, startLoadingCatImages = false, distractorsSet = false;
    public static bool readyToLoadImages = false, readyToSetImages = false, categoryImagesLoaded = false, distractorsLoaded = false;
    bool loadStarted = false, checkForDistractors = true, gameWithDistractors = false;

    public static List<Texture> levelCategory = new List<Texture>();
    public static List<Texture> distractors = new List<Texture>();//list of the distractors images
    public static List<string> distractorsNames = new List<string>();

    List<String> catFiles = new List<String>();

    int catArray;

    void Update()
    {
        if (checkForDistractors && SpawnTiles.totalLevels > 0)
        {
            for (int i = 0; i < LoadLevels.levels.Count; i++)
            {
                if (LoadLevels.levels[i].useDistractors)
                {
                    gameWithDistractors = true;
                }
            }
            checkForDistractors = false;
        }

        if (!checkForDistractors && loadingGame && !loadStarted)
        {
            loadingPanel.SetActive(true);
            Main_MenuBtn.SetActive(false);
            LoadImages();
            loadStarted = true;
        }

        if (startLoadingCatImages)
            LoadCategImages();

        if (SpawnTiles.startLoadingDistrators)
            LoadDistractors();

        if (categoryImagesLoaded && distractorsSet)
        {
            SpawnTiles.initTexture();
            gameLoaded = true;
        }
    }

    void LoadImages()
    {
        if (readyToLoadImages)
        {
            for (int i = 0; i < SpawnTiles.totCategories; i++)
            {
                List<string> tempList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(Application.dataPath + @"/Categories/" + SpawnTiles.cats[i] + "/");

                FileInfo[] smFiles = di.GetFiles();
                int a = 0;

                foreach (FileInfo fi in smFiles)
                {
                    //exclude .meta files
                    if (fi.Extension == ".jpg" || fi.Extension == ".png" || fi.Extension == ".JPG" || fi.Extension == ".PNG")
                    {
                        tempList.Add(fi.Name);
                        a++;
                    }
                }
                SpawnTiles.imagesNames.Add(tempList);
            }

            if (GUICategories.manualCategory)
            {
                for (int i = 0; i < SpawnTiles.cats.Count; i++)
                {
                    if (SpawnTiles.cats[i] == SpawnTiles.folder)
                    {
                        for (int a = 0; a < SpawnTiles.imagesNames[i].Count; a++)
                        {
                            catFiles.Add(SpawnTiles.imagesNames[i][a]);
                        }
                    }
                }
            }

            //loads distractors -> not considered as a category
            if (!SpawnTiles.distractorsLoaded && gameWithDistractors)
            {
                DirectoryInfo di2 = new DirectoryInfo(Application.dataPath + @"/Categories/Distractors/");

                FileInfo[] smFiles2 = di2.GetFiles();
                int j = 0;
                foreach (FileInfo fi2 in smFiles2)
                {
                    //exclude .meta files
                    if (fi2.Extension == ".jpg" || fi2.Extension == ".png" || fi2.Extension == ".JPG" || fi2.Extension == ".PNG")
                    {
                        SpawnTiles.files.Add(fi2.Name);
                        j++;
                    }
                }
                SpawnTiles.distractorsLoaded = true;
            }
            else if (!gameWithDistractors)
            {
                //Debug.Log("this game won't need distractors");
                readyToSetImages = true;
            }
            ChangeLevel.setOnce = true;
            ChangeLevel.SetLevel();

            readyToLoadImages = false;
        }
    }

    void LoadDistractors()
    {
        for (int i = 0; i < distractors.Count; i++)
        {
            Destroy(distractors[i]);
        }

        distractors.Clear();

        do
        {
            int rand = UnityEngine.Random.Range(0, SpawnTiles.files.Count);

            DirectoryInfo di2 = new DirectoryInfo(Application.dataPath + @"/Categories/Distractors/");

            FileInfo[] smFiles2 = di2.GetFiles();
            int j = 0;
            foreach (FileInfo fi2 in smFiles2)
            {
                //exclude .meta files
                if (fi2.Extension == ".jpg" || fi2.Extension == ".png" || fi2.Extension == ".JPG" || fi2.Extension == ".PNG")
                {
                    if (fi2.Name == SpawnTiles.files[rand])
                    {
                        WWW www = new WWW("file://" + Application.dataPath + @"/Categories/" + "Distractors/" + SpawnTiles.files[rand]);
                        if (!distractorsNames.Contains(fi2.Name))
                        {
                            distractors.Add(www.texture);
                            distractors[j].name = SpawnTiles.files[rand];
                            distractorsNames.Add(fi2.Name);
                            j++;
                        }
                    }
                }
            }
        } while (distractors.Count < SpawnTiles.totalToRender - SpawnTiles.correctchoices);
        distractorsSet = true;
        SpawnTiles.startLoadingDistrators = false;

    }

    void LoadCategImages()
    {
        for (int i = 0; i < levelCategory.Count; i++)
        {
            Destroy(levelCategory[i]);
        }
        levelCategory.Clear();

        DirectoryInfo di3 = new DirectoryInfo(Application.dataPath + @"/Categories/" + SpawnTiles.folder + "/");
        FileInfo[] smFiles3 = di3.GetFiles();

        for (int i = 0; i < SpawnTiles.cats.Count; i++)
        {
            if (SpawnTiles.cats[i] == SpawnTiles.folder)
            {
                int imagesNeeded;

                if (SpawnTiles.useDistractors)
                    imagesNeeded = SpawnTiles.targets;
                else
                    imagesNeeded = SpawnTiles.totalToRender;

                //Debug.Log("imagesNeeded: " + imagesNeeded);
                //Debug.Log("SpawnTiles.imagesNames[i].Count: " + SpawnTiles.imagesNames[i].Count);


                if (SpawnTiles.imagesNames[i].Count < imagesNeeded && GUICategories.manualCategory)
                    SpawnTiles.imagesNames[i] = catFiles;

                int b = 0;

                foreach (FileInfo fi3 in smFiles3)
                {
                    if (fi3.Extension == ".jpg" || fi3.Extension == ".png" || fi3.Extension == ".JPG" || fi3.Extension == ".PNG")
                    {
                        WWW www = new WWW("file://" + Application.dataPath + @"/Categories/" + SpawnTiles.folder + "/" + fi3.Name);

                        int catImagesToLoad;

                        catArray = i;

                        ShuffleCatImages();

                        if (SpawnTiles.imagesNames[i].Count >= 50)
                            catImagesToLoad = 50;
                        else
                            catImagesToLoad = SpawnTiles.imagesNames[i].Count;


                        for (int a = 0; a < catImagesToLoad; a++)
                        {
                            if (fi3.Name == SpawnTiles.imagesNames[i][a])
                            {
                                levelCategory.Add(www.texture);
                                levelCategory[b].name = fi3.Name;
                                //Debug.Log("levelCategory[b].name: " + levelCategory[b].name);
                                b++;
                            }
                        }
                    }
                }

            }
        }
        categoryImagesLoaded = true;
        startLoadingCatImages = false;
    }

    void ShuffleCatImages()
    {
        for (int i = 0; i < SpawnTiles.imagesNames[catArray].Count; i++)
        {
            string temp = SpawnTiles.imagesNames[catArray][i];
            int randomIndex = UnityEngine.Random.Range(i, SpawnTiles.imagesNames[catArray].Count);
            SpawnTiles.imagesNames[catArray][i] = SpawnTiles.imagesNames[catArray][randomIndex];
            SpawnTiles.imagesNames[catArray][randomIndex] = temp;
        }
    }
}

