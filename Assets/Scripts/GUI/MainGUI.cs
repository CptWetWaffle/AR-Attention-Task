using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using vuforia;

public class MainGUI : MonoBehaviour
{


    public GameObject IKTarget, gamePlayUI, imageDisplay, cover, targetBig, targetSmall, changeLevelUI, timer, Main_MenuUI, mainPanel, loadingText, Main_MenuBtn;
    public Transform layoutBig, layoutSmall;
    public Text time, cubes, points, pauseInfo;
    public Text timerLabel, cubesLabel;

    public static float timeToDisplay;

    static int pointsToDisplay;
    public static bool picture;

    public static float imgTimer;
    public static bool removeCover;//screen to avoid distractions and give some time before big picture be displayed
    public static bool pictureReady = false;
    public static bool allImages = false;
    public static bool clearTargets = false;
    public static bool activateHint = false, settingLevel = false;

    int cubesToFind;

    void Start()
    {
        /*imageDisplay.SetActive(false);
        gamePlayUI.SetActive(false);
        cover.SetActive(false);
        removeCover = false;
        mainPanel.SetActive(false);*/
    }

    void Update()
    {
        /*
        if (Main_MenuBtn.activeSelf)
            Main_MenuBtn.gameObject.GetComponentInChildren<Text>().text = Language.main;

        if (gamePlayUI.activeSelf)
            Main_MenuBtn.SetActive(true);

        timerLabel.text = Language.timer;
        cubesLabel.text = Language.remaining;

        if (Time.timeScale == 1)
            pauseInfo.text = Language.pause;
        else
            pauseInfo.text = Language.unpause;

        //pause/unpause -> activated by space bar
        if (Input.GetKeyDown(KeyCode.Space) && gamePlayUI.activeSelf)
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        if (SpawnTiles.categoriesLoaded && !LoadGame.loadingGame && SpawnTiles.totalLevels > 0)
        {
            loadingText.SetActive(false);
            mainPanel.SetActive(true);
        }

        cubesToFind = SpawnTiles.correctchoices - Scoring.GetCorrect();
        cubes.text = cubesToFind.ToString();

        if (SpawnTiles.levelName != "L0")
            points.text = Scoring.GetPoints().ToString();

        if (gamePlayUI.activeSelf && SpawnTiles.showcorrect)
        {
            timer.gameObject.SetActive(true);
            time.text = timeToDisplay.ToString("0");

            if (Collision_Detection.activateTimer)
            {
                timeToDisplay -= Time.deltaTime;
                if (timeToDisplay <= 0)
                {
                    if (Collision_Detection.selectedTexture == "NaN")
                    {
                        Collision_Detection.readyToActivateHints = true;
                        //TimerCount.timeout = true;
                    }

                    //if (TimerCount.timeout && SpawnTiles.hints.Count > 0 && Collision_Detection.readyToActivateHints)
                        //activateHint = true;

                    timeToDisplay = 0;
                }
            }
        }

        else
            timer.gameObject.SetActive(false);

        if (Collision_Detection.changeLevel || clearTargets)
        {
            GameObject[] tempTargets = GameObject.FindGameObjectsWithTag("DisplayTarget");

            for (int i = 0; i < tempTargets.Length; i++)
            {
                Destroy(tempTargets[i]);
            }
        }

        //displaying targets to memorize before level starts
        if (picture && pictureReady && !settingLevel)
        {
            Main_MenuBtn.SetActive(false);

            if (!allImages)
            {
                GameObject[] tempTargets = GameObject.FindGameObjectsWithTag("DisplayTarget");
                for (int i = 0; i < tempTargets.Length; i++)
                {
                    Destroy(tempTargets[i]);
                }

                for (int i = 0; i < SpawnTiles.selectionList.Count; i++)
                {
                    GameObject newImage = Instantiate(targetBig) as GameObject;
                    newImage.name = SpawnTiles.selectionList[i].name;
                    newImage.transform.SetParent(layoutBig, false);
                    Sprite sprite = new Sprite();
                    Texture2D tempText = SpawnTiles.selectionList[i] as Texture2D;
                    sprite = Sprite.Create(tempText, new Rect(0, 0, tempText.width, tempText.height), new Vector2(0, 0), 100.0f);
                    newImage.GetComponent<Image>().sprite = sprite;
                }

                IKTarget.SetActive(false);
                imageDisplay.SetActive(true);
                cover.SetActive(true);
                allImages = true;
            }

            imgTimer -= Time.deltaTime;
            if (imgTimer <= SpawnTiles.picTimer)
            {
                //removeCover = true;//image is displayed for 4 seconds and the blank screen removed 2 seconds before so the image is displayed for exactly 2 seconds (before this sometimes it took a bit to load)
                cover.SetActive(false);
            }

            if (imgTimer < 0)
            {
                GameObject[] displays = GameObject.FindGameObjectsWithTag("DisplayBig");
                if (displays.Length > 0)
                {
                    for (int i = 0; i < displays.Length; i++)
                        Destroy(displays[i]);
                }

                imageDisplay.SetActive(false);
                gamePlayUI.SetActive(true);
                IKTarget.SetActive(true);
                imgTimer = SpawnTiles.picTimer;
                MoveCamera.animStatus = true;
                SpawnTiles.spawnOnce = true;
                imgTimer = SpawnTiles.picTimer;
                picture = false;
                pictureReady = false;
            }
        }

        //displaying the targets on the top left side of the screen
        else if (!SpawnTiles.useMemory && pictureReady && !settingLevel)
        {
            clearTargets = false;

            if (!allImages)
            {
                for (int i = 0; i < SpawnTiles.selectionList.Count; i++)
                {
                    GameObject newTarget = Instantiate(targetSmall) as GameObject;
                    newTarget.transform.SetParent(layoutSmall, false);
                    Sprite sprite = new Sprite();
                    Texture2D tempText = SpawnTiles.selectionList[i] as Texture2D;
                    sprite = Sprite.Create(tempText, new Rect(0, 0, tempText.width, tempText.height), new Vector2(0, 0), 100.0f);

                    Image[] uiSprites = newTarget.GetComponentsInChildren<Image>();

                    foreach (Image uiSprite in uiSprites)
                    {
                        if (uiSprite.gameObject.transform.parent.name == newTarget.name)
                            uiSprite.sprite = sprite; //this gameObject is a child, because its transform.parent is not null  
                    }
                }
                allImages = true;
            }
        }*/
    }

    public void SetButtonOption(int option)
    {
        if (option == 0)
        {
            clearTargets = true;
            gamePlayUI.SetActive(false);
            Main_MenuUI.SetActive(true);
        }
    }
}
