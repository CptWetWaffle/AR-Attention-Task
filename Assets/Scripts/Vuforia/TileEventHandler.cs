using Assets.Vuforia.Scripts;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts.Vuforia
{
    /// <summary>
    /// A behaviour that implements the IVirtualButtonEventHandler interface and
    /// </summary>
    public class TileEventHandler : MonoBehaviour, IVirtualButtonEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES

        /*private GameObject _mTile;
        private GameObject _mTime;*/
        //private bool _start;

        public static IVirtualButtonEventHandler EventHandler;

        private static bool isColided = false;
        private Transform _tile, _imageDisplay;
        private GameObject _frame, _timer;
        public Color lightGreen = new Color32(254, 254, 254, 255);

        public Texture2D frameCorrect, frameIncorrect;
        public static Texture selectedCube;
//    bool isSelected = false;
        //new bool active = false;
        public static bool activateTimer = false;
        public static float timerPosition; //timer changes position wether the left or right hand is being used
        public static string selectedTexture = "NaN"; //actual texture being selected by the "hand"
        public static bool changeLevel = false, readyToActivateHints = false;

        /*AudioSource[] sounds = new AudioSource[4];
    AudioSource audio1;
    AudioSource audio2;
    AudioSource audio3;*/

        #endregion // PRIVATE_MEMBER_VARIABLES



        #region PUBLIC_METHODS

        public TileEventHandler()
        {
            if (EventHandler == null)
                EventHandler = this;
        }

        /// <summary>
        /// Called when the virtual button has just been pressed:
        /// </summary>
        public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
        {
            var virtualButtonPress = vb.transform.GetComponent(typeof(IVirtualPress)) as IVirtualPress;
            if (virtualButtonPress == null) return;
            virtualButtonPress.OnPress();
        }


        /// <summary>
        /// Called when the virtual button has just been released:
        /// </summary>
        public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
        {
            var virtualButtonPress = vb.transform.GetComponent(typeof(IVirtualPress)) as IVirtualPress;
            if (virtualButtonPress == null) return;
            virtualButtonPress.OnRelease();
            
        }

        #endregion // PUBLIC_METHODS



        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            // Register with the virtual buttons TrackableBehaviour
            VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
            for (int i = 0; i < vbs.Length; ++i)
            {
                vbs[i].RegisterEventHandler(this);
                Debug.Log(vbs[i].VirtualButtonName);
            }

        }


        void Update()
        {
            /*if (isColided)
        {
            if (SpawnTiles.selectionList.Contains(_imageDisplay.renderer.material.mainTexture))
            {
                Scoring.SetCorrect(1);
                frame.renderer.material.mainTexture = frameCorrect;

                if (Main_Menu.sound)
                    audio2.Play();

                _imageDisplay.renderer.material.color = lightGreen;

                //removes the tile from the hints list
                for (int i = 0; i < SpawnTiles.hints.Count; i++)
                {
                    if (SpawnTiles.hints[i].gameObject == _tile.gameObject)
                        SpawnTiles.hints.RemoveAt(i);
                }

                if (SpawnTiles.levelName != "L0")
                {
                    Scoring.SetTotalPoints(Hints.hintActive ? 5 : 10);
                }
            }
            else {
                Scoring.SetError(1);
                frame.renderer.material.mainTexture = frameIncorrect;

                if (Main_Menu.sound)
                    audio3.Play();

                _imageDisplay.renderer.material.color = lightGreen;
            }

            if (SpawnTiles.big)
            {
                _tile.transform.position = new Vector3(_tile.transform.position.x, 0.4f,
                    _tile.transform.position.z);
                _tile.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
            }
            else if (SpawnTiles.medium)
            {
                _tile.transform.position = new Vector3(_tile.transform.position.x, 0.45f,
                    _tile.transform.position.z);
                _tile.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            }
            else
            {
                _tile.transform.position = new Vector3(_tile.transform.position.x, 0.5f,
                    _tile.transform.position.z);
                _tile.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            isSelected = true;

            if (Scoring.GetCorrect() == SpawnTiles.correctchoices)
            {
                ChangeLevel.myTimer = 1.0f;
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
        }*/
            /*if (_start)
        {
            var aux = float.Parse(_mTime.GetComponent<Text>().text);
            aux -= Time.deltaTime;
            if (aux >= 0)
            {
                _mTime.GetComponent<Text>().text = aux.ToString();
                _start = false;
            }
            _mTime.GetComponent<Text>().text = "0";
        }*/
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PRIVATE_METHODS

        #endregion // PRIVATE_METHODS
    }
}
