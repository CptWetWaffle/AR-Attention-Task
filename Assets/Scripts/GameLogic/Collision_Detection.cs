using UnityEngine;
using System.Collections;
using Vuforia;

public class Collision_Detection : MonoBehaviour
{
    public static bool isColided = false;
    public Transform tile, imageDisplay;
    public GameObject frame, timer, Main_MenuUI, ikTarget;
    public Camera MainCamera;
    public Color lightGreen = new Color32(254, 254, 254, 255);

    public Texture2D frameCorrect, frameIncorrect;
    public static Texture selectedCube;
    bool isSelected = false;
    //new bool active = false;
    public static bool activateTimer = false;
    public static float timerPosition; //timer changes position wether the left or right hand is being used
    public static string selectedTexture = "NaN"; //actual texture being selected by the "hand"
    public static bool changeLevel = false, readyToActivateHints = false;

    AudioSource[] sounds = new AudioSource[4];
    AudioSource audio1;
    AudioSource audio2;
    AudioSource audio3;

    void Start()
    {
        //DontDestroyOnLoad(this);
        sounds = GetComponents<AudioSource>();
        audio1 = sounds[0];
        audio2 = sounds[1];
        audio3 = sounds[2];
    }

    void Update()
    {
        
        if (Main_MenuUI.activeSelf)
            this.gameObject.collider.enabled = false;
        else
            this.gameObject.collider.enabled = true;
    }

    //if "hand" collides with a cube
    void OnTriggerEnter(Collider cl)
    {
        /*TimerCount.timeout = true;
        TimerCount.myTimer = TimerCount.startTime;
        TimerCount.timeout = false; //reset timer
                                    //Debug.Log("colliding!");*/

        if (cl.tag == "hand")
        {
            readyToActivateHints = false;
            selectedTexture = imageDisplay.renderer.material.mainTexture.name;
            selectedCube = imageDisplay.renderer.material.mainTexture;


            activateTimer = true;

            if (Main_Menu.sound)
                audio1.Play();

            isColided = true;

            //plays animation and sets active the orange frame
            if (imageDisplay.renderer.material.color != lightGreen)
            {
                tile.animation.Play();
                frame.SetActive(true);
                timer.SetActive(true);
            }

            else
                tile.gameObject.collider.enabled = false;

            //TimerCount.myTimer = TimerCount.startTime;
        }
    }

    void OnTriggerExit(Collider cl)
    {
        if (cl.tag == "hand")
        {
            selectedTexture = "NaN";

            isColided = false;
            timer.SetActive(false);

            if (SpawnTiles.big)
                tile.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
            else if (SpawnTiles.medium)
                tile.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            else
                tile.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            tile.animation.Stop();

            if (imageDisplay.renderer.material.color == Color.white)
                frame.SetActive(false);

            if (imageDisplay.renderer.material.color == lightGreen)
                imageDisplay.renderer.material.color = lightGreen;

            else if (imageDisplay.renderer.material.color == Color.red)
                imageDisplay.renderer.material.color = Color.red;

            else
                imageDisplay.renderer.material.color = Color.white;

            isSelected = false;
        }
    }

    //"hand" keeps over the same cube
    void OnTriggerStay(Collider c)
    {
        if (c.tag == "hand")
        {
            selectedTexture = imageDisplay.renderer.material.mainTexture.name;
            selectedCube = imageDisplay.renderer.material.mainTexture;
            timerPosition = tile.transform.position.x;
            if (!isSelected)
            {
                if (true)
                //if (!active && TimerCount.timeout)
                {
                    //foreach (Texture target in SpawnTiles.selectionList)
                    //{

                    if (SpawnTiles.selectionList.Contains(imageDisplay.renderer.material.mainTexture))
                    {
                        Scoring.SetCorrect(1);
                        frame.renderer.material.mainTexture = frameCorrect;
                        //frame.SetActive(true);

                        if (Main_Menu.sound)
                            audio2.Play();

                        imageDisplay.renderer.material.color = lightGreen;



                        //removes the tile from the hints list
                        for (int i = 0; i < SpawnTiles.hints.Count; i++)
                        {
                            if (SpawnTiles.hints[i].gameObject == tile.gameObject)
                                SpawnTiles.hints.RemoveAt(i);
                        }

                        if (SpawnTiles.levelName != "L0")
                        {
                            Scoring.SetTotalPoints(Hints.hintActive ? 5 : 10);
                        }
                        //break;
                    }

                    else
                    {
                        Scoring.SetError(1);
                        frame.renderer.material.mainTexture = frameIncorrect;
                        //frame.SetActive(true);

                        if (Main_Menu.sound)
                            audio3.Play();

                        imageDisplay.renderer.material.color = lightGreen;
                        //break;
                    }
                    // }

                    if (SpawnTiles.big)
                    {
                        tile.transform.position = new Vector3(tile.transform.position.x, 0.4f,
                            tile.transform.position.z);
                        tile.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
                    }
                    else if (SpawnTiles.medium)
                    {
                        tile.transform.position = new Vector3(tile.transform.position.x, 0.45f,
                            tile.transform.position.z);
                        tile.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    }
                    else
                    {
                        tile.transform.position = new Vector3(tile.transform.position.x, 0.5f,
                            tile.transform.position.z);
                        tile.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }

                    //active = true;
                    isSelected = true;

                    if (Scoring.GetCorrect() == SpawnTiles.correctchoices)
                    {
                        ChangeLevel.myTimer = 1.0f;
                        ikTarget.collider.enabled = false;

                        GUIChangeLevel.setting = false;

                        if (Hints.hintActive)
                        {
                            GUIChangeLevel.levelFailed = true;
                            LevelChangeBehavior.LevelFailed();
                        }
                        else
                        {
                            GUIChangeLevel.levelFailed = false;
                            LevelChangeBehavior.logicApplied = true;
                            LevelChangeBehavior.LevelSucceeded();
                        }

                        GUIChangeLevel.changeLevelTimer = 6.0f;
                        changeLevel = true;
                    }
                    if (MainGUI.timeToDisplay <= 0)
                        readyToActivateHints = true;
                }
                /* else if (active)//if active
            {
                //send scoring
                //foreach (Texture target in SpawnTiles.selectionList)
                //{
                    if (SpawnTiles.selectionList.Contains(imageDisplay.renderer.material.mainTexture))
                    {
                        Scoring.DeleteCorrect(1);
                  //      break;
                    }
                    else
                    {
                        Scoring.DeleteError(1);
                    //    break;
                    }
                //}

                imageDisplay.renderer.material.color = Color.white;

                if (SpawnTiles.big)
                    tile.transform.position = new Vector3(tile.transform.position.x, 0.4f, tile.transform.position.z);
                else if (SpawnTiles.medium)
                    tile.transform.position = new Vector3(tile.transform.position.x, 0.45f, tile.transform.position.z);
                else
                    tile.transform.position = new Vector3(tile.transform.position.x, 0.5f, tile.transform.position.z);

                TimerCount.timeout = false;//reset timer
                active = false;
                isSelected = true;
            }*/
            }
        }
    }

    /*public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
{
    TimerCount.timeout = true;
    TimerCount.myTimer = TimerCount.startTime;
    TimerCount.timeout = false;//reset timer

    readyToActivateHints = false;
    selectedTexture = imageDisplay.renderer.material.mainTexture.name;
    selectedCube = imageDisplay.renderer.material.mainTexture;

    //displays timer
    if (!UDPReceive.calibrate)
        activateTimer = true;

    if (Main_Menu.sound)
        audio1.Play();

    isColided = true;

    //plays animation and sets active the orange frame
    if (imageDisplay.renderer.material.color != lightGreen)
    {
        tile.animation.Play();
        frame.SetActive(true);
        timer.SetActive(true);
    }

    else
        tile.gameObject.collider.enabled = false;

    TimerCount.myTimer = TimerCount.startTime;
}

public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
{
    selectedTexture = "NaN";

    isColided = false;
    timer.SetActive(false);

    if (SpawnTiles.big)
        tile.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
    else if (SpawnTiles.medium)
        tile.transform.localScale = new Vector3(0.27f, 0.27f, 0.27f);
    else
        tile.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);

    tile.animation.Stop();

    if (imageDisplay.renderer.material.color == Color.white)
        frame.SetActive(false);

    if (imageDisplay.renderer.material.color == lightGreen)
        imageDisplay.renderer.material.color = lightGreen;

    else if (imageDisplay.renderer.material.color == Color.red)
        imageDisplay.renderer.material.color = Color.red;

    else
        imageDisplay.renderer.material.color = Color.white;

    isSelected = false;
}*/
}
