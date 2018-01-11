using UnityEngine;
using System.Collections;
using Assets.Scripts.GameLogic;
using Assets.Vuforia.Scripts;
using GameLogic;
using UnityEngine.UI;
using Vuforia;

public class Score : MonoBehaviour
{
    private CountSessionTime _start;
    private GameObject _play;
    private GameObject _reset;
    private Text _points;

    public GameObject EndScore;
    public GameObject ChangeLevelUi;

    public static int score { get { return correct * 10 - wrong * 5; } }
    public static int correct { get; set; }
    public static int wrong { get; set; }

    void Awake()
    {
        ChangeLevelUi = GameObject.Find("ChangeLevelUI");
        EndScore = GameObject.Find("TotalScore");
    }

    void Start()
    {
        _start = GameObject.Find("Start").GetComponent<CountSessionTime>() as CountSessionTime;
        _play = GameObject.Find("Play");
        _reset = GameObject.Find("Reset");
        _points = GameObject.Find("points").GetComponent<Text>();

    }

    void Update()
    {
        if (StartUp.LevelConfig() != null)
            if (correct == StartUp.LevelConfig().correctChoices && _start.start)
            {
                _start.start = false;
                foreach (var tile in Quickspawn.mTiles)
                {
                    if (!tile.activeSelf) break;
                    //tile.GetComponentInChildren<VirtualButtonBehaviour>().enabled = false;
                    tile.SetActive(false);
                }
                _play.SetActive(true);
                _points.text = score.ToString();
                ChangeLevelUi.SetActive(true);
                ExportData.addLevelInfo(StartUp.LevelToLoad, correct, wrong,
                    StartUp.LevelConfig().levelTime - CountSessionTime.sessionTime);
                EndScore.GetComponent<Text>().text = ExportData.TotalScore.ToString();
                if (StartUp.LevelConfig().levelName != "L8")
                {
                    _play.GetComponent<VirtualButtonBehaviour>().enabled = true;
                    _play.GetComponent<PlayPress>().ActivateGlow();
                    _play.GetComponent<PlayPress>().spawnNext = true;
                }
                else
                {
                    _play.GetComponent<VirtualButtonBehaviour>().enabled = false;
                    _reset.GetComponent<VirtualButtonBehaviour>().enabled = false;
                    SaveExport.Instance.Save();
                }

            }
    }


}
