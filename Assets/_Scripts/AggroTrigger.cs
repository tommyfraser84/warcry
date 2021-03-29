using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS1.Units.Player;

public class AggroTrigger : MonoBehaviour
{
    public PlayerUnit playerUnit;

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Collision!");
        //if there is no target currently set
        if (playerUnit.GetTarget() == null)
        {
            Debug.Log("no target at the moment");
            //if enemy is within sphere aggro range
            if (other.gameObject.layer == 13)
            {
                Debug.Log("enemy set");
                playerUnit.SetMove(other.transform.position);
                playerUnit.SetTarget(other.transform);
            }
        }
    }

}
