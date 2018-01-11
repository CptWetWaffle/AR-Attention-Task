using System;
using Assets.Scripts.GameLogic;
using Assets.Vuforia.Scripts;
using GameLogic;
using UnityEngine;

namespace Assets.Scripts.Vuforia
{
    public class TilePress : MonoBehaviour, IVirtualPress
    {
        public static int ActiveRow = 0;
        private GameObject _frame;
        private CountSessionTime _start;
        private GameObject _timer;
        private Renderer[] _timerComp;
        private TimerCount _timerCount;
        private GameObject _imageDisplay;
        //private Color lightGreen = new Color32(254, 254, 254, 255);
        private VirtualButtonBehaviour _button;
        private AudioSource _audio1, _audio2, _audio3;
        private VirtualButtonBehaviour _play, _reset, _close;
        public bool Pressed { get; set; }
        public Texture2D FrameCorrect, FrameIncorrect, FramePress;
        public int row { get; set; }

        void Update()
        {
            if (!_timerCount.timeout && _timerCount.pressed)
            {
                _play.enabled = false;
                _reset.enabled = false;
                _close.enabled = false;
                if (ActiveRow < row)
                {
                    ActiveRow = row;
                    foreach (var tile in Quickspawn.mTiles)
                    {
                        if (!tile.activeSelf) break;
                        if (tile.GetComponentInChildren<TilePress>().row < row)
                            if (!tile.GetComponentInChildren<TilePress>().Pressed)
                                tile.GetComponentInChildren<VirtualButtonBehaviour>().enabled = false;
                    }
                }
            }
            /*else
            {
                if (ActiveRow == 0)
                {
                    foreach (var tile in Quickspawn.mTiles)
                    {
                        if (!tile.activeSelf) break;
                        tile.GetComponentInChildren<VirtualButtonBehaviour>().enabled = true;
                    }
                    ActiveRow = -1;
                }
            }*/
            //Debug.Log("Active row: " + ActiveRow);
        }

        public void Start()
        {
            _frame = this.transform.parent.Find("frame").gameObject;
            _timer = this.transform.parent.Find("TimeCircle").gameObject;
            _timerComp = _timer.GetComponentsInChildren<Renderer>();
            _timerCount = _timer.GetComponent<TimerCount>();
            if(StartUp.LevelConfig()!=null)
                _timerCount.startTime = StartUp.LevelConfig().selectionTime;
            _start = GameObject.Find("Start").GetComponent<CountSessionTime>() as CountSessionTime;
            _imageDisplay = this.transform.parent.Find("ImageDisplay").gameObject;
            var sounds = this.transform.parent.GetComponents<AudioSource>();
            _audio1 = sounds[0];
            _audio2 = sounds[1];
            _audio3 = sounds[2];
            _play = GameObject.Find("Play").GetComponentInChildren<VirtualButtonBehaviour>();
            _reset = GameObject.Find("Reset").GetComponentInChildren<VirtualButtonBehaviour>();
            _close = GameObject.Find("Close").GetComponentInChildren<VirtualButtonBehaviour>();
            Pressed = false;
        }

        public void OnPress()
        {
            //if (ActiveRow > row) return;
            /*foreach (var tile in Quickspawn.mTiles)
            {
                if(!tile.activeSelf) break;
                if (tile.GetComponentInChildren<TilePress>().row < row)
                    tile.GetComponentInChildren<VirtualButtonBehaviour>().enabled = false;
            }*/
            if (!_start.start) return;
            /*if (!this.transform.parent.animation.isPlaying)
                this.transform.parent.animation.Play();*/
            _frame.GetComponent<Renderer>().material.mainTexture = FramePress;
            _frame.GetComponent<Renderer>().enabled = true;
            _timer.GetComponent<Renderer>().enabled = true;

            _timerCount.myTimer = StartUp.LevelConfig().selectionTime;
            _timerCount.timeout = false;
            _timerCount.pressed = true;
            foreach (var comp in _timerComp)
            {
                comp.enabled = true;
            }

            _audio1.Play();
        }

        public void OnRelease()
        {
            if (row == ActiveRow)
            {
                foreach (var tile in Quickspawn.mTiles)
                {
                    if (!tile.activeSelf) break;
                    if(!tile.GetComponentInChildren<TilePress>().Pressed)
                        tile.GetComponentInChildren<VirtualButtonBehaviour>().enabled = true;
                }
                ActiveRow = 0;
            }
            _play.enabled = true;
            _reset.enabled = true;
            _close.enabled = true;
            _timerCount.timeout = true;
            _frame.GetComponent<Renderer>().enabled = false;
            _timer.GetComponent<Renderer>().enabled = false;
            foreach (var comp in _timerComp)
            {
                comp.enabled = false;
            }
        }

        public void OnPressed()
        {
            Pressed = true;
            this.GetComponent<VirtualButtonBehaviour>().enabled = false;
            if (Quickspawn.TargetTextures.Contains(_imageDisplay.renderer.material.mainTexture))
            {
                _frame.GetComponent<Renderer>().material.mainTexture = FrameCorrect;
                _frame.GetComponent<Renderer>().enabled = true;
                _audio2.Play();
                Score.correct++;
            }
            else
            {
                _frame.GetComponent<Renderer>().material.mainTexture = FrameIncorrect;
                _frame.GetComponent<Renderer>().enabled = true;
                _audio3.Play();
                Score.wrong++;
            }
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
