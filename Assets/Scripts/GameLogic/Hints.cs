using UnityEngine;
using System.Collections;
//using vuforia;

public class Hints : MonoBehaviour {

    public static float timeToHint;
    public static float currentTime;
    GameObject hintTile;
    Transform imageDisplay;
    GameObject incorrectTile;
    public Transform tile;
    public static bool hintActive = false;
    public static float timer;
    public static bool timeCounter = false;
    public static bool blink = true;
    public static float blinkTimer = 8.0f;

    public Texture2D frameSelected;
    public GameObject frame;

    public Color32 orange;

	// detects key movement in order to offer an hint
	void Update () 
    {
        if (MainGUI.activateHint  && SpawnTiles.showcorrect)
        {
            ShowHint();
        }
	}

    //display correct answers
    void ShowHint()
    {
        if (SpawnTiles.hints.Count > 0)
        {
            for (int i = 0; i < SpawnTiles.incorrect.Count; i++)
            {
                incorrectTile = SpawnTiles.incorrect[i];
                if (SpawnTiles.big)
                {
                    incorrectTile.transform.position = new Vector3(incorrectTile.transform.position.x, 0.4f, incorrectTile.transform.position.z);
                }
                else if (SpawnTiles.medium)
                {
                    incorrectTile.transform.position = new Vector3(incorrectTile.transform.position.x, 0.45f, incorrectTile.transform.position.z);
                }
                else
                {
                    incorrectTile.transform.position = new Vector3(incorrectTile.transform.position.x, 0.5f, incorrectTile.transform.position.z);
                }

                incorrectTile.gameObject.collider.enabled = false;
            }

            for (int i = 0; i < SpawnTiles.hints.Count; i++)
            {
                hintTile = SpawnTiles.hints[i];
                imageDisplay = SpawnTiles.hints[i].transform.Find("ImageDisplay");
                
                foreach (Transform child in hintTile.transform)
                {
                    if (child.name == "frame")
                    {
                        child.gameObject.SetActive(true);
                    }
                }

                //cube color animation to point out where correct answers are
                blinkTimer -= Time.deltaTime;
                float lerp = Mathf.PingPong(Time.time, 0.6f) / 0.6f;
                imageDisplay.gameObject.renderer.material.color = Color.Lerp(orange, Color.white, lerp);
                
                if(blinkTimer<0)
                {
                    imageDisplay.gameObject.renderer.material.color = Color.white;
                    
                }
            }

            //foreach (Texture target in SpawnTiles.selectionList)
            //{
                if (SpawnTiles.selectionList.Contains(Collision_Detection.selectedCube))
                {
                    hintActive = true;
                    //Debug.Log(hintActive);
                }
            //}
        }
    }  
}
