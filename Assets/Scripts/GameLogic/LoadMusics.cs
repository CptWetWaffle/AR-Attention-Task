using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LoadMusics : MonoBehaviour
{
    int totalMusics;
    List<AudioClip> musicsList = new List<AudioClip>();
    AudioSource audioSource;
    List<int> tempMusics = new List<int>();
    public static bool musicsLoaded = false, startLoadingMusics = false;
    public float myTimer = 4.0f;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.15f;
    }

    //Load all musics inside Musics folder
    void LoadMusicFiles()
    {
        if (!musicsLoaded)
        {
            DirectoryInfo di;
            string path;

            if (GUICategories.manualCategory)
            {
                path = Application.dataPath + @"/Musics/" + Main_Menu.uid + "/";
                di = new DirectoryInfo(path);
            }
            else
            {
                path = Application.dataPath + @"/Musics/";
                di = new DirectoryInfo(path);
            }
            //Debug.Log(path);
            FileInfo[] smFiles = di.GetFiles();
            int totalMusics = 0;
            foreach (FileInfo fi in smFiles)
            {
                if (fi.Extension == ".ogg")//excluding .meta files
                {
                    WWW www = new WWW("file://" + path + fi.Name);
                    AudioClip clip = www.GetAudioClip(true, true, AudioType.OGGVORBIS);
                    musicsList.Add(clip);
                    musicsList[totalMusics].name = fi.Name;
                    totalMusics++;
                }
            }
            musicsLoaded = true;
        }
    }

    void Update()
    {
        if (audioSource.isPlaying && !Main_Menu.music)
            audioSource.Stop();

        if (startLoadingMusics)
        {
            LoadMusicFiles();
            startLoadingMusics = false;
        }
        //Play a random music along all session -> starts after choose Play on the main menu
        if (musicsList.Count > 0 && !audioSource.isPlaying && Main_Menu.gameStarted && Main_Menu.music)
        {
            int temp = UnityEngine.Random.Range(0, musicsList.Count);
            if (tempMusics.Count < musicsList.Count && !tempMusics.Contains(temp))
            {
                
                audioSource.clip = musicsList[temp];
                audioSource.Play();
                tempMusics.Add(temp);
                Debug.Log("current music playing: "  + temp + " - " + musicsList[temp]);
                //Debug.Log(tempMusics.Count);
            }
            else if (tempMusics.Count == musicsList.Count)
            {
                tempMusics.Clear();
            }
        }
    }
}