using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Assets.Scripts.GameLogic;
using Assets.Vuforia.Scripts;

public class StartUp : MonoBehaviour
{
	public StartUp Instance;
	public static int LevelToLoad;

	private int _columns, _rows, _targets, _correctChoices;
	private bool _showCorrect, _useDistractors, _useMemory, _valence, _complete;
	private float _levelTime, _picTimer, _selectionTime;
	private string _folder, _imagesToDisplay;

	public static VirtualButtonBehaviour play, reset;

	private static readonly IList<Level> Levels = new List<Level>();

	void LoadLevelConfig()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("Levels", typeof(TextAsset));
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset.text);

		xmlDoc.LoadXml(textAsset.text);
		XmlNodeList levelsList = xmlDoc.GetElementsByTagName("Levels");

		foreach (XmlNode levelNumber in levelsList)
		{
			XmlNodeList levelContent = levelNumber.ChildNodes;

			foreach (XmlNode levelNum in levelContent)
			{
				string level = levelNum.Name;

				foreach (XmlNode settings in levelNum)
				{

					if (settings.Name == "columns")
						_columns = int.Parse(settings.InnerText);
					if (settings.Name == "rows")
						_rows = int.Parse(settings.InnerText);
					if (settings.Name == "targets")
						_targets = int.Parse(settings.InnerText);
					if (settings.Name == "correctchoices")
						_correctChoices = int.Parse(settings.InnerText);
					if (settings.Name == "showcorrect")
						_showCorrect = bool.Parse(settings.InnerText);
					if (settings.Name == "timer")
						_levelTime = float.Parse(settings.InnerText);
					if (settings.Name == "picTimer")
						_picTimer = float.Parse(settings.InnerText);
					if (settings.Name == "category")
						_folder = settings.InnerText;
					if (settings.Name == "imagesToDisplay")
						_imagesToDisplay = settings.InnerText;
					if (settings.Name == "useDistractors")
						_useDistractors = bool.Parse(settings.InnerText);
					if (settings.Name == "useMemory")
						_useMemory = bool.Parse(settings.InnerText);
					if (settings.Name == "valence")
						_valence = bool.Parse(settings.InnerText);
					if (settings.Name == "selectionTime")
						_selectionTime = float.Parse(settings.InnerText);

					_complete = false;
				}

				Levels.Add(new Level(level, _columns, _rows, _targets, _correctChoices, _levelTime, _picTimer, _folder, _imagesToDisplay, _useDistractors, _useMemory, _valence, _showCorrect, _complete, _selectionTime));

			}
		}
		//}	
	}

	void Awake()
	{
		DontDestroyOnLoad(this);
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
		LoadLevelConfig();
	}


	// Use this for initialization
	void Start()
	{
		LoadManager.Initialize();

		LevelToLoad = 1;
		//play = GameObject.Find("Play").GetComponentInChildren<VirtualButtonBehaviour>();
		//reset = GameObject.Find("Reset").GetComponentInChildren<VirtualButtonBehaviour>();


	}

	public static void LoadNext()
	{
		LevelToLoad++;
		/*if (LevelToLoad > 1)
		{
			play.enabled = false;
			reset.enabled = false;
			//Application.LoadLevel("StartScreen");
		}*/
	}

	public static Level LevelConfig()
	{
		/*if (LevelToLoad > 7)
		{
			play.enabled = false;
			reset.enabled = false;
			//Application.LoadLevel("StartScreen");
		}*/
		return Levels.Count > LevelToLoad ? Levels[LevelToLoad] : null;
	}
}
