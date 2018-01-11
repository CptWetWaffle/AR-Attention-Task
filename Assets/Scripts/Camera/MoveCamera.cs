using UnityEngine;
using System.Collections;
using Assets.Scripts.Vuforia;

//using vuforia;

public class MoveCamera : MonoBehaviour
{

    public static bool animStatus = false;
    public GameObject dana, pointer;

    void Start()
    {
        animStatus = false;
    }

    void Update()
    {
        /*if (pointer.activeSelf)
        {
            if (SpawnTiles.big)
                pointer.transform.localPosition = new Vector3(pointer.transform.localPosition.x, -0.592f, pointer.transform.localPosition.z);
            else if (SpawnTiles.medium)
                pointer.transform.localPosition = new Vector3(pointer.transform.localPosition.x, -0.757f, pointer.transform.localPosition.z);
            else
                pointer.transform.localPosition = new Vector3(pointer.transform.localPosition.x, -1.05f, pointer.transform.localPosition.z);
        }
        */
        if (animStatus)
        {
            SpawnTiles.readyToSpawn = false;
            dana.SetActive(false);
            pointer.SetActive(false);
            animation.wrapMode = WrapMode.Once;
            animation.Play();
            animStatus = false;
            Collision_Detection.activateTimer = false;
        }

        else
        {
            if (!animation.isPlaying)
            {
                dana.SetActive(true);
                //pointer.SetActive(true);
                TileEventHandler.activateTimer = true;
            }
            SpawnTiles.readyToSpawn = true;
        }
    }
}
