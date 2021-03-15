using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //allow component to be attached to gameobject in editor
    LineRenderer rend;

    //public LayerMask layerMask;

    //setup v3 array to store initial points
    Vector3[] points;

    Transform selectedUnit;

    public Transform selectingHand;

    // Start is called before the first frame update
    void Start()
    {

        rend = selectingHand.GetComponent<LineRenderer>();
        rend.startColor = Color.red;
        rend.endColor = Color.red;
        points = new Vector3[2];

    }

    // Update is called once per frame
    void Update()
    {
        AlignLineRenderer(rend);
       // Debug.Log(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger));
    }

    public void AlignLineRenderer(LineRenderer rend)
    {


        Ray ray;
        ray = new Ray(selectingHand.transform.position, selectingHand.transform.forward);
        RaycastHit hit;

        points[0] = selectingHand.transform.position;

   

        if (Physics.Raycast(ray,out hit))
        {
           // Debug.Log("hit!");
            points[1] = hit.point;

            LayerMask layerHit = hit.transform.gameObject.layer;

          

            switch (layerHit.value) { 
                //units layer
                case 8:
                    //Debug.Log("unit layer hit!");
                    rend.startColor = Color.green;
                    rend.endColor = Color.green;
                    if (OVRInput.GetDown(OVRInput.Button.One)){
                        Debug.Log("Trigger down!");
                        if (selectedUnit != null)
                        {
                            selectedUnit.gameObject.GetComponent<BasicUnitActions>().Selected(false);
                        }
                        selectedUnit = hit.transform;
                        hit.transform.gameObject.GetComponent<BasicUnitActions>().Selected(true);
                    } else
                    {
                        Debug.Log("Not triggering!");
                    }
                    break;
                default:
                   // Debug.Log("unknown layer hit!");
                    rend.startColor = Color.red;
                    rend.endColor = Color.red;
                   if (OVRInput.GetDown(OVRInput.Button.One)){
                        Debug.Log("Trigger down!");
                        if (selectedUnit != null)
                        {
                            selectedUnit.gameObject.GetComponent<BasicUnitActions>().Selected(false);
                        }
                        //hit.transform.gameObject.GetComponent<BasicUnitActions>().Selected(false);
                    }
                    break;
            }
            rend.material.color = rend.startColor;
            rend.SetPositions(points);
            rend.enabled = true;
        }
        else
        {
            //Debug.Log("nothing hit!");
            rend.enabled = false;
        }

       

    }

}
