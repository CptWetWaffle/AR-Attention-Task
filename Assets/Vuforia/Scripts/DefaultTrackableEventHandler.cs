/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using System.Collections.Generic;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Vuforia;
using GameLogic;
using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {

        public GameObject ChangeLevelUi;
        public static bool Played = false;

        #region PRIVATE_MEMBER_VARIABLES

        private TrackableBehaviour mTrackableBehaviour;
        private GameObject _mScoreBoard;
        private GameObject _mPrism;
        private CountSessionTime _start;
        private GameObject[] _tiles;
        private GameObject[] _buttons;

        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            _mScoreBoard = GameObject.Find("Scoreboard");
            _mPrism = GameObject.Find("Prism");
            _start = GameObject.Find("Start").GetComponent<CountSessionTime>() as CountSessionTime;
            _tiles = GameObject.FindGameObjectsWithTag("Tile");
            _buttons = GameObject.FindGameObjectsWithTag("Button");
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }

        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                if (!_start.start && component.tag == "Tile")
                    break;
                component.enabled = true;
            }

            foreach (var button in _buttons)
            {
                var renderers = button.GetComponentsInChildren<Renderer>();
                foreach (var rend in renderers)
                {
                    if (rend.name == "frame" || rend.name == "Plane" || rend.name == "TimeCircle")
                        rend.enabled = false;
                }
            }

            if (_start.start)
                foreach (var tile in _tiles)
                {
                    var renderers = tile.GetComponentsInChildren<Renderer>();
                    foreach (var rend in renderers)
                    {
                        if (rend.name == "frame" || rend.name == "Plane" || rend.name == "TimeCircle" || rend.name == "Tile")
                            rend.enabled = false;
                        if (rend.name == "frame" && tile.GetComponentInChildren<TilePress>().Pressed)
                            rend.enabled = true;
                    }
                }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                if (!_start.start && component.tag == "Tile")
                    continue;
                component.enabled = true;
            }
            _mScoreBoard.GetComponentInChildren<Canvas>().enabled = true;
            _mPrism.GetComponentInChildren<Renderer>().enabled = true;
            //mScoreBoard.SetActive(true);

            if(!_start.start && Played)
                ChangeLevelUi.SetActive(true);
            //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            //mScoreBoard.SetActive(false);
            _mScoreBoard.GetComponentInChildren<Canvas>().enabled = false;
            _mPrism.GetComponentInChildren<Renderer>().enabled = false;
            ChangeLevelUi.SetActive(false);

            //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
