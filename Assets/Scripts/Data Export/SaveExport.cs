using UnityEngine;
using System.Collections;
using System.IO;

public class SaveExport : MonoBehaviour
{
    public static SaveExport Instance;
    public string Filename { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);
        Filename = "default.csv";

    }

    public void Save()
    {
        var directoryPath = Application.persistentDataPath + "/Logs";

        if (!Directory.Exists(directoryPath))
            System.IO.Directory.CreateDirectory(directoryPath);
        var completePath = directoryPath + "/" + Filename;
        Debug.Log(completePath);
        if (!File.Exists(completePath))
        {
            File.WriteAllLines(completePath, ExportData.LevelsToCSV());
        }
        else
        {
            foreach (var level in ExportData.LevelsToCSV())
            {
                File.AppendAllText(completePath, level);
            }
        }


    }
}
