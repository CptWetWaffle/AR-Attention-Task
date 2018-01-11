using Assets.Vuforia.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace GameLogic
{
    public class CountSessionTime : MonoBehaviour
    {
        public bool start { get; set; }
        public static float sessionTime;
        public static bool timeSessionExpired = false;

        private Text _time;
        private Text _points;
        private GameObject _imageTarget;
        private AudioSource _audio;
        private bool _playOnce;
        public GameObject ChangeLevelUi;
        private GameObject _play;
        private GameObject _reset;

        public GameObject EndScore;

        void Awake()
        {
            ChangeLevelUi = GameObject.Find("ChangeLevelUI");
            EndScore = GameObject.Find("TotalScore");
        }

        void Start()
        {
            start = false;
            sessionTime = 60;
            _time = GameObject.FindGameObjectWithTag("TIMER").GetComponentInChildren<Text>();
            _points = GameObject.Find("points").GetComponent<Text>();
            _imageTarget = GameObject.Find("ImageTarget");
            _audio = _imageTarget.GetComponent<AudioSource>();
            _playOnce = true;
            _play = GameObject.Find("Play");
            _reset = GameObject.Find("Reset");
        }

        void Update()
        {
            if (start)
                CountTime();
        }

        void CountTime()
        {
            /*if (sessionTime < 5 && _playOnce)
            {
                _audio.Play();
                _playOnce = false;
            }*/
            if (sessionTime >= 0)
            {
                sessionTime -= Time.deltaTime;
                _time.text = sessionTime.ToString("0");
                _points.text = Score.score.ToString();
                if (sessionTime <= 0)
                {

                    timeSessionExpired = true;
                    start = false;
                    foreach (var tile in Quickspawn.mTiles)
                    {
                        if (!tile.activeSelf) break;
                        //tile.GetComponentInChildren<VirtualButtonBehaviour>().enabled = false;
                        tile.SetActive(false);
                    }
                    ChangeLevelUi.SetActive(true);
                    ExportData.addLevelInfo(StartUp.LevelToLoad, Score.correct, Score.wrong, StartUp.LevelConfig().levelTime);
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
    }
}
