using UnityEngine;
using System.Collections;
using Assets.Vuforia.Scripts;
using Assets.Scripts.GameLogic;

public class Tile : MonoBehaviour {

    private GameObject _frame,_timer;
    private VirtualButtonBehaviour _button;
    private Color lightGreen = new Color32(254, 254, 254, 255);
    public Texture2D frameCorrect, frameIncorrect;


    // Use this for initialization
    void Start () {
        _frame = this.transform.FindChild("frame").gameObject;
        _timer = this.transform.FindChild("TimeCircle").gameObject;
        _button = this.GetComponentInChildren<VirtualButtonBehaviour>();
    }

    public void OnCorrect()
    {
        _button.enabled = false;
        _frame.renderer.material.mainTexture = frameCorrect;
        _frame.renderer.enabled = true;
        Score.correct++;
    }

    public void OnIncorrect()
    {
        _button.enabled = false;
        _frame.renderer.material.mainTexture = frameIncorrect;
        _frame.renderer.enabled = true;
        Score.wrong++;
    }
}
