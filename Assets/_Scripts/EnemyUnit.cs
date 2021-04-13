using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using RTS1.Units;
using RTS1.Units.Player;
using RTS1.Layers;

public class EnemyUnit : MonoBehaviour
{ 
    private NavMeshAgent navMeshAgent;

    private BasicUnitProperties basicUnitProperties;

    private Collider[] rangeColliders;

    private Transform aggroTarget;

    private Unit unit;

    private Unit aggroTargetUnit;

    private bool hasAggro = false;

    private float attkCooldown;

    private float distance;
    private float unitSpeed;

    private float aggroRange;


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        unit = GetComponent<Unit>();

        basicUnitProperties = unit.basicUnitProperties;

        Stop();

    }


    private void Update()
    {
        if (attkCooldown>0) attkCooldown -= Time.deltaTime;

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
        Debug.Log("aggroRange: " + aggroRange);

        for (int i = 0; i < rangeColliders.Length; i++)
        {

            Debug.Log(rangeColliders[i].gameObject.layer);
            if (rangeColliders[i].gameObject.layer == (int) Layers.LayerName.PlayerUnit)
            {
                Debug.Log("Layer 8!");
                aggroTarget = rangeColliders[i].gameObject.transform;
                aggroTargetUnit = aggroTarget.GetComponent<Unit>();
                hasAggro = true;
                break;
               
            }
        }
    }

    private void MoveToAggroTarget()
    {
        /* */
        if (aggroTarget == null || aggroTargetUnit.isDead)
        {
            navMeshAgent.SetDestination(transform.position);
            hasAggro = false;
        }
        else
        {
            Debug.Log("MoveToAggroTarget()");
            distance = Vector3.Distance(aggroTarget.position, transform.position);

            navMeshAgent.stoppingDistance = basicUnitProperties.attkRange;

            //check if within aggro range
            if (distance <= basicUnitProperties.attkRange)
            {
                Attack();
            }
            else if (distance <= basicUnitProperties.aggroRange)
            {
                //make the aggro target the target for the navmesh controller
                Debug.Log("Within aggro range!");
                navMeshAgent.SetDestination(aggroTarget.position);

                navMeshAgent.speed = unit.unitSpeed;
                Debug.Log("unitSpeed: " + unit.unitSpeed);
                navMeshAgent.isStopped = false;
            }
        }
    }

    private void Attack()
    {
        navMeshAgent.speed = 0f;
        Debug.Log("Attack()");
        if(attkCooldown <= 0)
        {
            aggroTargetUnit.TakeDamage(basicUnitProperties.damageBasic, basicUnitProperties.damagePiercing);
            attkCooldown = basicUnitProperties.attkSpeed;
            unit.animator.SetTrigger("Attack");
        }
        
    }

    private void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0f;
        //playerUnitState.ChangeState(PlayerUnitState.UnitState.Idle);
    }

}


