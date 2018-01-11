using UnityEngine;
using System.Collections;

public class HandControl : MonoBehaviour {
	
	public GameObject BorderU, BorderD, BorderL, BorderR;
	float xmin,xmax,zmin,zmax;//x,y borders
	
	float Speed;//hand speed
	float horizontalAxis = 0.0f;
	float verticalAxis = 0.0f;

    public static bool leftCorrectPos = false;
    public static bool rightCorrectPos = false;
    public static bool setHand = false;
	// Use this for initialization
	void Start () 
	{
		//hand motion range
		//xmin = BorderL.transform.position.x; 
        xmin = -1.50f; 
		//xmax = BorderR.transform.position.x;	
        xmax = 1.50f;
		zmin=BorderD.transform.position.z;
		zmax=BorderU.transform.position.z;   
	}
    

    void SetHandPosition()
    {
        //initial position of the hand
       /* if (Main_Menu.LArm == true)
        {
            transform.position = new Vector3(xmin, transform.position.y, transform.position.z);
        }
        else if (Main_Menu.RArm == true)
        {
            transform.position = new Vector3(xmax, transform.position.y, transform.position.x);
        }*/
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (setHand)
        {
            SetHandPosition();
            setHand = false;
        }

        CheckTargetPosition();
		horizontalAxis = Input.GetAxis("Horizontal");
       	verticalAxis = Input.GetAxis("Vertical");

        Speed = Time.deltaTime * 1f;
        

        float h = Speed * horizontalAxis;
        float v = Speed * verticalAxis;

		transform.Translate(h, 0, v);
		
		//X Y Borders
		//horizontal
		if(transform.position.x > xmax)
		{
			transform.position = new Vector3(xmax, transform.position.y, transform.position.z);
		}
		if (transform.position.x < xmin)
		{
			transform.position = new Vector3(xmin, transform.position.y, transform.position.z);
		}

		//Apply vertical movement based on the tiles range
		if (transform.position.z >  zmax)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, zmax);
		}
		if (transform.position.z < zmin)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, zmin);
		}
	}

    void CheckTargetPosition()
    {
       /* if (Main_Menu.LArm && transform.position.x <= -1.45f)
            leftCorrectPos = true;
        else if (Main_Menu.LArm && transform.position.x > -1.45f)
            leftCorrectPos = false;

        if (Main_Menu.RArm && transform.position.x >= 1.45f)
            rightCorrectPos = true;
        else if (Main_Menu.RArm && transform.position.x < 1.45f)
            rightCorrectPos = false;*/
    }
}
