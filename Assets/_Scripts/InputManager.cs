﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS1.Units;
using RTS1.Units.Player;

namespace RTS1.Input
{

    public class InputManager : MonoBehaviour
    {

        public static InputManager im;

        [Header("Button Settings")]
        //set variables to 
        public OVRInput.Button selectDeselectButton;
        public OVRInput.Button actionButton;
        public OVRInput.Button multiSelectButton;

      
        //allow component to be attached to gameobject in editor
        LineRenderer rend;

        //setup v3 array to store initial points
        Vector3[] points;

        //List of units selected

        public List<Transform> selectedUnits = new List<Transform>();

        [Header("LineRender Settings")]
        //The transform of the hand/object used to draw a linerender from
        public Transform selectingHand;

        public Color lineColour1, lineColour2, lineColour3;

        void Start()
        {
            //set variable tolinerenderer component on the selectingHand object
            rend = selectingHand.GetComponent<LineRenderer>();

            //set linecolor to start
            rend.startColor = lineColour1;
            rend.endColor = lineColour1;

            //
            points = new Vector3[2];
        }

        void Update()
        {
           // Debug.Log(OVRInput.Get(multiSelectButton));

            MoveSelector(rend);
        }

        public void MoveSelector(LineRenderer rend)
        {

            //make a new Ray pointing forward from hand
            Ray ray;
            ray = new Ray(selectingHand.transform.position, selectingHand.transform.forward);
            RaycastHit hit;


            //set linerender point 1 to hand starting point
            points[0] = selectingHand.transform.position;


            //if something was hit
            if (Physics.Raycast(ray, out hit))
            {
                //set linerender point 2 to where raycast hit
                points[1] = hit.point;

                //give variable layer that was hit
                LayerMask layerHit = hit.transform.gameObject.layer;

                //which layer was hit?
                switch (layerHit.value)
                {
                    //8 - Units layer - Raycast hit a unit
                    case 8:
                        //Debug.Log("Unit layer hit");
                        // Make pointer linecolor2
                        rend.startColor = lineColour2;
                        rend.endColor = lineColour2;
                        //Debug.Log("case 8 - units layer");

                        // Select/deselect button was pressed
                        if (OVRInput.GetDown(selectDeselectButton))
                        {
                            //send unit hit transform to selectunit function, send whether multiselect is active
                            Debug.Log("8 - unit layer hit and selectbutton pressed");
                            SelectUnit(hit.transform, OVRInput.Get(multiSelectButton));
                        }
                        break;
                    //12 - floor layer hit
                    case 12:
                       //Debug.Log("Floor layer hit");
                        rend.startColor = lineColour1;
                        rend.endColor = lineColour1;
                        if (OVRInput.GetDown(selectDeselectButton))
                        {
                          Debug.Log("12 - Units deselected!");
                            DeselectUnits();

                        } else if (OVRInput.GetDown(actionButton) & HaveSelectedUnits())
                        {
                            Debug.Log("12 - Units selected and move button pressed");
                            foreach (Transform unit in selectedUnits)
                            {
                                //get playerunit script of selected unit in list
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                //trigger units moveunit function with vector3 of hit.point
                                pU.Move(hit.point);
                            }
                            //loop through units in selectedunits
                            //PlayerUnit pU = 
                        }
                        break;
                    //enemy unit layer hit
                    case 13:
                        // Debug.Log("Enemy layer hit");
                        rend.startColor = lineColour3;
                        rend.endColor = lineColour3;
                        if (OVRInput.GetDown(actionButton) & HaveSelectedUnits())
                        {
                            
                            foreach (Transform unit in selectedUnits)
                            {

                                //get playerunit script of selected unit in list
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                pU.Move(hit.transform.position);
                            }
                        }
                        break;
                    default:
                        //Debug.Log("Different layer hit");
                        //Make pointer linecolor1
                        rend.startColor = lineColour1;
                        rend.endColor = lineColour1;
                        if (OVRInput.GetDown(selectDeselectButton))
                        {
                            DeselectUnits();

                        }
                        break;
                }
                rend.material.color = rend.startColor;
                rend.SetPositions(points);
                rend.enabled = true;
            }
            //if something was not hit by raycast
            else
            {
                //disable the linerender
                rend.enabled = false;
            }

        }

        private void SelectUnit(Transform unit, bool canMultiselect)
        {
            Debug.Log("SelectUnit()");
            //Debug.Log(canMultiselect);
            //clear selection list if multiselect disabled
            if (!canMultiselect)
            {
                //deselect units
                DeselectUnits();
            }

                selectedUnits.Add(unit);
                //lets set an obj on the unit called Highlight
                Debug.Log("Selected: " + unit.gameObject.GetComponent<PlayerUnit>().CheckSelected());
                unit.gameObject.GetComponent<PlayerUnit>().Selected(true);
                //unit.
                Debug.Log("Selected: " + unit.gameObject.GetComponent<PlayerUnit>().CheckSelected());
                //unit.Find("Highlight").gameObject.SetActive(true);
        }

        private void DeselectUnits()
        {
            Debug.Log("DeselectUnits()");
            //if there are already selected units in the list selectedUnits
            if (selectedUnits != null)
            {
                //loop through list
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    //deselect units
                    selectedUnits[i].gameObject.GetComponent<PlayerUnit>().Selected(false);
                }
                selectedUnits.Clear();
            }
        }

        private bool HaveSelectedUnits()
        {
            if (selectedUnits.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
