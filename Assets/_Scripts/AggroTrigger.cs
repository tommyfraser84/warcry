using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using RTS1.Units;
using RTS1.Units.Player;
using RTS1.Layers;

public class AggroTrigger : MonoBehaviour
{

    //private PlayerUnit playerUnit;

        private NavMeshAgent navMeshAgent;

    public BasicUnitProperties basicUnitProperties;

    private Collider[] rangeColliders;

    private Transform aggroTarget;

    private bool hasAggro = false;

   // public GameObject.Lay aggroLayer;

    private float distance;
    private float unitSpeed;

    private void Start()
    {
        //playerUnit = GetComponent<PlayerUnit>();
        // playerUnit.basicUnitProperties.aggroRange;
        navMeshAgent = GetComponent<NavMeshAgent>();
        unitSpeed = basicUnitProperties.speed / 10;
    }


    private void Update()
    {
        if (!hasAggro)
        {
            CheckForEnemyTargets();
        } else
        {
            MoveToAggroTarget();
        }
    }

    private void CheckForEnemyTargets()
    {
        Debug.Log("CheckForEnemyTargets()");
        rangeColliders = Physics.OverlapSphere(transform.position, basicUnitProperties.aggroRange);

        for (int i = 0; i < rangeColliders.Length; i++)
        {
            //Debug.Log(aggroLayer.value);
            //Debug.Log(rangeColliders[i].gameObject.layer);
            if (rangeColliders[i].gameObject.layer == (int) Layers.LayerName.PlayerUnit)
            {
                Debug.Log("Layer 8!");
                aggroTarget = rangeColliders[i].gameObject.transform;
                hasAggro = true;
                break;
            }
        }
    }

    private void MoveToAggroTarget()
    {
        Debug.Log("MoveToAggroTarget()");
        distance = Vector3.Distance(aggroTarget.position, transform.position);
        //playerUnit.navMeshAgent.stoppingDistance = (playerUnit.basicUnitProperties.attkRange + 1);
        navMeshAgent.stoppingDistance = basicUnitProperties.attkRange;
        if (distance <= basicUnitProperties.aggroRange)
        {
            Debug.Log("Within aggro range!");
            navMeshAgent.SetDestination(aggroTarget.position);
            //navMeshAgent.SetDestination(new Vector3(0,0,0));
            navMeshAgent.speed = unitSpeed;
            //Debug.Log("unitSpeed: " + unitSpeed);
            navMeshAgent.isStopped = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, basicUnitProperties.aggroRange);

        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, basicUnitProperties.attkRange);
    }
}


