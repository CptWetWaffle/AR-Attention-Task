using UnityEngine;
using System.Collections;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Vuforia;
using Assets.Vuforia.Scripts;
using GameLogic;
using UnityEngine.UI;
using Vuforia;

public class ResetPress : MonoBehaviour, IVirtualPress
{

    private GameObject[] _tiles;
    private CountSessionTime _start;
    private GameObject _targetsDisplay;
    private GameObject _timer, _play;
    private Renderer[] _timerComp;
    private TimerCount _timerCount;
    public GameObject _changeLevelUI;


    void Start()
    {
        _start = GameObject.Find("Start").GetComponent<CountSessionTime>() as CountSessionTime;
        _targetsDisplay = GameObject.Find("TargetsDisplay");
        _timer = this.transform.Find("TimeCircle").gameObject;
        _timerComp = _timer.GetComponentsInChildren<Renderer>();
        _timerCount = _timer.GetComponent<TimerCount>();
        _timerCount.startTime = 2.0f;
        _play = GameObject.Find("Play");
        //_changeLevelUI = GameObject.Find("ChangeLevelUI");
    }

    public void OnPress()
    {
        /*if (OptionManager.ActiveOption == 'c')
        {
            _timerCount.timeout = true;
            return;
        }*/
        _play.GetComponent<VirtualButtonBehaviour>().enabled = false;
        OptionManager.ActiveOption = 'r';
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
        //if(OptionManager.ActiveOption =='c')
        _play.GetComponent<VirtualButtonBehaviour>().enabled = true;
    }

    public void OnPressed()
    {
        _changeLevelUI.SetActive(false);
        _start.start = false;
        Quickspawn.respawn = true;
        _play.GetComponent<VirtualButtonBehaviour>().enabled = true;
        _play.GetComponent<PlayPress>().respawn = true;
        _play.GetComponent<PlayPress>().ActivateGlow();
        _targetsDisplay.GetComponent<HorizontalLayoutGroup>().enabled = false;
        OptionManager.ActiveOption = 'p';
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
