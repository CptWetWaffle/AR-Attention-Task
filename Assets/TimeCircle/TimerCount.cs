using UnityEngine;
using System.Collections;
using Assets.Scripts.Vuforia;

public class TimerCount : MonoBehaviour
{

    public float myTimer = 0.0f;
    public float startTime;//number of seconds timer will be displayed
    public bool pressed { get; set; }

    public bool timeout = false;

    public GameObject PressAction;
    float timerPos = 0.25f;
    private GameObject _imageDisplay;
    private TilePress _tile;
    public bool done = false;

    /*void Start()
    {
        myTimer = startTime;
    }*/

    private void Start()
    {
        PressAction = this.transform.parent.gameObject;
        _tile = this.GetComponentInParent<TilePress>();
    }

    void Update()
    {
        if (pressed && !timeout)
        {
            myTimer -= Time.deltaTime;

            if (myTimer <= 0 && myTimer > -1)
            {
                timeout = true;

                myTimer = -1;
                IVirtualPress press = PressAction.GetComponentInChildren(typeof(IVirtualPress)) as IVirtualPress;
                if(press!=null) press.OnPressed();
            }

            this.renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, startTime, myTimer));//set float to "cut" texture - from grayscale channel

        }
        /*if (timeout && !done && _tile != null)
        {
            done = true;
            if(_tile.row == TilePress.ActiveRow)
                TilePress.ActiveRow = 0;
        }*/
    }
}
