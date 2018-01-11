using UnityEngine;
using System.Collections;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Vuforia;
using Assets.Vuforia.Scripts;
using GameLogic;
using Vuforia;

public class ClosePress : MonoBehaviour, IVirtualPress
{

    private GameObject[] _tiles;
    private CountSessionTime _start;
    private GameObject _targetsDisplay;
    private GameObject _timer;
    private Renderer[] _timerComp;
    private TimerCount _timerCount;
    private VirtualButtonBehaviour _play, _reset;

    void Start () {
        _start = GameObject.Find("Start").GetComponent<CountSessionTime>() as CountSessionTime;
        _targetsDisplay = GameObject.Find("TargetsDisplay");
        _timer = this.transform.Find("TimeCircle").gameObject;
        _timerComp = _timer.GetComponentsInChildren<Renderer>();
        _timerCount = _timer.GetComponent<TimerCount>();
        _timerCount.startTime = 2.0f;
        /*_play = GameObject.Find("Play").GetComponent<PlayPress>();
        _reset = GameObject.Find("Reset").GetComponent<ResetPress>();*/
        _play = GameObject.Find("Play").GetComponent<VirtualButtonBehaviour>();
        _reset = GameObject.Find("Reset").GetComponent<VirtualButtonBehaviour>();

    }

    public void OnPress()
    {
        /*_play.TimeOut();
        _reset.TimeOut();*/
        _play.enabled = false;
        _reset.enabled = false;
        _timer.GetComponent<Renderer>().enabled = true;
        _timerCount.myTimer = _timerCount.startTime;
        _timerCount.timeout = false;
        _timerCount.pressed = true;
        foreach (var comp in _timerComp)
        {
            comp.enabled = true;
        }
    }

    public void OnRelease()
    {
        _timerCount.timeout = true;
        _timer.GetComponent<Renderer>().enabled = false;
        foreach (var comp in _timerComp)
        {
            comp.enabled = false;
        }
        _reset.enabled = true;
        _play.enabled = true;
    }

    public void OnPressed()
    {
        Application.LoadLevel("StartScreen");
        TimeOut();
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
