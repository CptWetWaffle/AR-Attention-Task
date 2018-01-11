using UnityEngine;
using System.Collections;

public class RenderHands : MonoBehaviour {
	
	public GameObject dana;
	
	bool activeHands =false;
	
	// Use this for initialization
	void Start () 
	{
		dana.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(MoveCamera.animStatus)
		{
			dana.SetActive(false);
		}
		else if(!MoveCamera.animStatus && !activeHands)
		{
			dana.SetActive(true); //Debug.Log("activehands");
			activeHands =true;
		}
	}
	
//	// Turn on the bit using an OR operation:
//	private void Show() 
//	{
//    	camera.cullingMask |= 1 << LayerMask.NameToLayer("Dana");
//	}
// 
//	// Turn off the bit using an AND operation with the complement of the shifted int:
//	private void Hide() 
//	{
//    	camera.cullingMask &=  ~(1 << LayerMask.NameToLayer("Dana"));
//	}
	
	
}
