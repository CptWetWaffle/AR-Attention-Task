using UnityEngine;
using System.Collections;

public class BlackScreenGUI : MonoBehaviour {

    public static bool coverActive;
    public GameObject targetImage, cover;

	void Start () 
    {
        //coverActive = true;
        //cover.SetActive(false);
        if (SpawnTiles.useMemory)
            coverActive = true;
        else
            coverActive = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (MainGUI.removeCover)
        {
            coverActive = false;
            cover.SetActive(false);
        }

        else
        {
            coverActive = true;
        }

        if (coverActive)
        {
            cover.SetActive(true);
        }
	}
}
