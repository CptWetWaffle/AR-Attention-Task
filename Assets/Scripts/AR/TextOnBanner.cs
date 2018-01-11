using UnityEngine;
using System.Collections;

public class TextOnBanner : MonoBehaviour
{

    public bool ShowGUI = false;
    public string TagToUse = "Banner";
    public float radius = 5.0f;
    float boxW = 1500f;
    float boxH = 250f;

    private GameObject targetObject;

    private string objName;


    void Update()
    {

        if (!targetObject) //Let's check if we don't have target object.
        {
            targetObject = null; //Set it to null to avoid errors
        }




        //Check if get objects with tag within our set radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            if (hit.tag == TagToUse && !targetObject)
            {
                targetObject = hit.gameObject;
                objName = targetObject.name;
                ShowGUI = true;
            }
        }
    }

    void OnGUI()
    {
        if (ShowGUI)
        {
            //Show the gui pls.
            ShowObjectName(targetObject);

        }
    }

    void ShowObjectName(GameObject targetObject)
    {
        //Thanks.
        if (targetObject)
        {
            Vector2 TextLocation = Camera.main.WorldToScreenPoint(targetObject.transform.position);

            TextLocation.y = Screen.height - TextLocation.y;

            TextLocation.x -= boxW * 0.5f;
            TextLocation.y -= boxH * 0.5f;

            GUI.Box(new Rect(TextLocation.x, TextLocation.y, boxW, boxH), objName);
        }
    }

}