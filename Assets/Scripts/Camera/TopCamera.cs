using UnityEngine;
using System.Collections;

public class TopCamera : MonoBehaviour {
	
	//public Camera top;
    public GameObject gamePlayUI;

	void Awake () 
	{
		//top.enabled=false;
	}
	
	
	void Update () 
	{
        //shortcut key to enable/disable top camera
		/*if(Input.GetKeyDown(KeyCode.F1) && !top.enabled && gamePlayUI.activeSelf)
		{
			//top.enabled=true;
		}
        else if (Input.GetKeyDown(KeyCode.F1) && top.enabled && gamePlayUI.activeSelf)
		{
			//top.enabled=false;
		}*/
	}
}
