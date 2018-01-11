using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;

public class EditImages : MonoBehaviour {

    public static void ResizeImages()
    {

    }
    /*
    public static IEnumerator LoadDistractors()
    {
        do
        {
            int rand = UnityEngine.Random.Range(0, SpawnTiles.files.Count);

            DirectoryInfo di2 = new DirectoryInfo(Application.dataPath + @"/Categories/Distractors/");

            FileInfo[] smFiles2 = di2.GetFiles();
            int j = 0;
            foreach (FileInfo fi2 in smFiles2)
            {
                //exclude .meta files
                if (fi2.Extension == ".jpg" || fi2.Extension == ".png" || fi2.Extension == ".gif" || fi2.Extension == ".jpeg")
                {
                    if (fi2.Name == SpawnTiles.files[rand])
                    {
                        WWW www = new WWW("file://" + Application.dataPath + @"/Categories/" + "Distractors/" + fi2.Name);
                        yield return www;
                        if (!SpawnTiles.distractors.Contains(www.texture))
                        {
                            SpawnTiles.distractors.Add(www.texture);
                            SpawnTiles.distractors[j].name = fi2.Name;
                            Debug.Log("Texture added to distractors" + fi2.Name + " Textures in distractors: " + SpawnTiles.distractors.Count);
                            j++;
                        }
                    }   
                }
            }
        } while (SpawnTiles.distractors.Count < SpawnTiles.totalToRender);
    }*/
}
