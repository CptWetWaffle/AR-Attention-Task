using UnityEngine;
using System.Collections;
using Assets.Vuforia;
using Vuforia;
using Assets.Vuforia.Scripts;


public class IMEventHandler : MonoBehaviour, IVirtualButtonEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES

    private GameObject tile1;

    #endregion // PRIVATE_MEMBER_VARIABLES



    #region PUBLIC_METHODS

    /// <summary>
    /// Called when the virtual button has just been pressed:
    /// </summary>
    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log("OnButtonPressed");
    }


    /// <summary>
    /// Called when the virtual button has just been released:
    /// </summary>
    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log("OnButtonReleased");
    }

    #endregion // PUBLIC_METHODS


    void Start()
    {
        // Get handle to the object
        //tile1 = GameObject.FindGameObjectsWithTag("Tile")[4];

        // Register with the virtual buttons TrackableBehaviour
        VirtualButtonBehaviour vb =
                            GetComponentInChildren<VirtualButtonBehaviour>();
        if (vb)
        {
            vb.RegisterEventHandler(this);
        }

    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }


    void Update()
    {

    }

}

