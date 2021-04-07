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

    //private PlayerUnit playerUnit;

        private NavMeshAgent navMeshAgent;

    public BasicUnitProperties basicUnitProperties;

    private Collider[] rangeColliders;

    private Transform aggroTarget;

    private bool hasAggro = false;

    private string _animatorDefaultParam;

    public Animator animator;

    // public GameObject.Lay aggroLayer;

    private float distance;
    private float unitSpeed;

    public GameObject unitStatDisplay;

    public Image healthBarAmount;

    public float currentHealth;

    private void HandleHealth()
    {
        Camera camera = Camera.main;
        unitStatDisplay.transform.LookAt(camera.transform.position);

        healthBarAmount.fillAmount = currentHealth / basicUnitProperties.hp;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        //playerUnit = GetComponent<PlayerUnit>();
        // playerUnit.basicUnitProperties.aggroRange;
        navMeshAgent = GetComponent<NavMeshAgent>();
        unitSpeed = basicUnitProperties.speed / 10;

        currentHealth = basicUnitProperties.hp;

        _animatorDefaultParam = "speed";
    }


    private void Update()
    {

        //Create animator speedvalue based on unit speed property
        float animatorSpeedVal = scale(0f, unitSpeed, 0f, 1f, navMeshAgent.speed);

        //Set animator with the value
        animator.SetFloat(_animatorDefaultParam, animatorSpeedVal);

        if (!hasAggro)
        {
            CheckForEnemyTargets();
        } else
        {
            MoveToAggroTarget();
        }

        HandleHealth();
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
    /* */
        if (aggroTarget == null)
        {
            navMeshAgent.SetDestination(transform.position);
            hasAggro = false;
        } else
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
    }

    private void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0f;
        //playerUnitState.ChangeState(PlayerUnitState.UnitState.Idle);
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


    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}


