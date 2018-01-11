using Assets.Scripts.Vuforia;
using Assets.Vuforia.Scripts;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Vuforia
{
    public class PlayPress : MonoBehaviour, IVirtualPress
    {
        private GameObject[] _tiles;
        private CountSessionTime _start;
        private GameObject _targetsDisplay;
        private GameObject _timer;
        private Renderer[] _timerComp;
        private TimerCount _timerCount;
        private GameObject _glow;
        public GameObject ChangeLevelUi;

        public bool spawnNext { get; set; }
        public bool respawn { get; set; }

        void Start()
        {
            respawn = false;
            _start = GameObject.Find("Start").GetComponent<CountSessionTime>() as CountSessionTime;
            _targetsDisplay = GameObject.Find("TargetsDisplay");
            _timer = this.transform.Find("TimeCircle").gameObject;
            _timerComp = _timer.GetComponentsInChildren<Renderer>();
            _timerCount = _timer.GetComponent<TimerCount>();
            _timerCount.startTime = 2.0f;
            _glow = this.transform.Find("Glow").gameObject;
            //OptionManager.ActiveOption = 'p';
            //_changeLevelUI = GameObject.Find("ChangeLevelUI");

        }

        public void OnPress()
        {
            /*if (OptionManager.ActiveOption != 'p')
            {
                _timerCount.timeout = true;
                return;
            }*/
            _timer.GetComponent<Renderer>().enabled = true;
            _timerCount.myTimer = _timerCount.startTime;
            _timerCount.timeout = false;
            _timerCount.pressed = true;
            foreach (var comp in _timerComp)
            {
                comp.enabled = true;
            }
        }

        public void ActivateGlow()
        {
            _glow.renderer.enabled = true;
        }

        public void DeactivateGlow()
        {
            _glow.renderer.enabled = false;
        }

        public void OnRelease()
        {
            _timerCount.timeout = true;
            _timer.GetComponent<Renderer>().enabled = false;
            foreach (var comp in _timerComp)
            {
                comp.enabled = false;
            }
        }

        public void OnPressed()
        {
            ChangeLevelUi.SetActive(false);
            Score.correct = 0;
            Score.wrong = 0;
            this.GetComponent<VirtualButtonBehaviour>().enabled = false;
            if (spawnNext && !respawn)
            {
                StartUp.LoadNext();
                Quickspawn.spawnNext = true;
            }
            respawn = false;
            if(!spawnNext)
                foreach (var tile in Quickspawn.mTiles)
                {
                    if (!tile.activeSelf) break;
                    tile.GetComponent<Renderer>().enabled = true;
                    tile.transform.Find("ImageDisplay").gameObject.GetComponent<Renderer>().enabled = true;
                    //tile.GetComponentInChildren<VirtualButtonBehaviour>().enabled = true;
                    //tile.GetComponentInChildren<TilePress>().Pressed = false;
                    foreach (var rend in tile.GetComponentsInChildren<Renderer>())
                    {
                        if (rend.name == "frame" || rend.name == "Plane" || rend.name == "TimeCircle")
                            rend.enabled = false;
                    }
                }
            DeactivateGlow();
            TimeOut();
            _start.start = true;
            _targetsDisplay.GetComponent<HorizontalLayoutGroup>().enabled = true;
            DefaultTrackableEventHandler.Played = true;
        }


        public void TimeOut()
        {
            _timerCount.timeout = true;
            foreach (var comp in _timerComp)
            {
                comp.enabled = false;
            }
        }
    }
}
