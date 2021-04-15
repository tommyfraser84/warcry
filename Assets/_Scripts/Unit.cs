using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using RTS1.Units;
using RTS1.Utils;
using RTS1.Input;
using RTS1.Layers;

public class Unit : MonoBehaviour
{
    public float currentHealth;

    public GameObject unitStatDisplay;

    public Image healthBarAmount;

    public BasicUnitProperties basicUnitProperties;

    public float unitSpeed;

    private NavMeshAgent navMeshAgent;

    public Animator animator;

    private string _animatorDefaultParam;

    public bool isDead;
    public float deathCount = 20f;

    private bool hasAggro = false;
    private Transform aggroTarget;

    public InputManager inputManager;

    private float attkCooldown;


    private Collider[] rangeColliders;

    private Unit aggroTargetUnit;

    private float distance;

    private bool selected;

    public GameObject selectedOutline;

    public int team;

    private void HandleHealth()
    {
        Camera camera = Camera.main;
        unitStatDisplay.transform.LookAt(camera.transform.position);

        healthBarAmount.fillAmount = currentHealth / basicUnitProperties.hp;

        if (isDead)
        {
            deathCount -= Time.deltaTime;
        }
        // if (deathCount <= 10) gameObject.GetComponent<MeshRenderer>().material.color.a = 1f;
        if (deathCount <= 0) Destroy(gameObject);

    }

    public void TakeDamage(float DamageBasic, float DamagePiercing)
    {
        // (Basic Damage - Target's Armor) + Piercing Damage = Maximum damage inflicted
        //The attacker does a random amount of damage from 50%-100% of this total each attack.

        float DamageTaken = ((DamageBasic - basicUnitProperties.armour) + DamagePiercing) * Random.Range(0.5f, 1);

        //Debug.Log("Damage Taken: " + DamageTaken);

        currentHealth -= DamageTaken;

        //Debug.Log("Current Health: " + currentHealth);



        if (currentHealth <= 0)
        {
            Die();
        } else
        {
            animator.SetTrigger("Take Damage");

        }
    }

    public void MoveToLoc(Vector3 dest)
    {

        Debug.Log("move attemped!");

        navMeshAgent.destination = dest;
        //Debug.Log("dest: " + dest);
        navMeshAgent.speed = unitSpeed;
        //Debug.Log("unitSpeed: " + unitSpeed);
        navMeshAgent.isStopped = false;
        //playerUnitState.ChangeState(PlayerUnitState.UnitState.Walk);

    }

    public void MoveToUnit(Transform targetUnit)
    {
        if (targetUnit.GetComponent<Unit>().isDead == false)
        {
            hasAggro = false;
        } else
        {
            hasAggro = true;
            aggroTarget = targetUnit;
        }
        /*
        if (aggroTarget == null || aggroTargetUnit.isDead)
        {
            navMeshAgent.SetDestination(transform.position);
            hasAggro = false;
            Stop();
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
        */
    }

    private void Die()
    {

        isDead = true;
        Debug.Log(isDead);
        animator.SetTrigger("Die");
        gameObject.layer = LayerMask.NameToLayer("Dead");
        //Destroy(gameObject);
        unitStatDisplay.SetActive(false);
    }

    private void Attack()
    {
        navMeshAgent.speed = 0f;
        Debug.Log("Attack()");
        if (attkCooldown <= 0)
        {
            aggroTargetUnit.TakeDamage(basicUnitProperties.damageBasic, basicUnitProperties.damagePiercing);
            attkCooldown = basicUnitProperties.attkSpeed;
            animator.SetTrigger("Attack");
        }

    }

    private void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0f;
        //playerUnitState.ChangeState(PlayerUnitState.UnitState.Idle);
    }


    private void Start()
    {
        currentHealth = basicUnitProperties.hp;
        unitSpeed = basicUnitProperties.speed / 10;
        navMeshAgent = GetComponent<NavMeshAgent>();
        _animatorDefaultParam = "speed";
        isDead = false;
        Stop();
        // deathCount = 20;
    }

    private void Update()
    {
        if (!isDead)
        {
            //Create animator speedvalue based on unit speed property
            float animatorSpeedVal = scale(0f, unitSpeed, 0f, 1f, navMeshAgent.speed);


            //Set animator with the value
            animator.SetFloat(_animatorDefaultParam, animatorSpeedVal);

            //TryDamage();
            HandleHealth();

            if (attkCooldown > 0) attkCooldown -= Time.deltaTime;

            if (!hasAggro)
            {
                CheckForEnemyTargets();
            }
            else
            {
                MoveToAggroTarget();
            }


            float dist = Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination);
            if (dist < navMeshAgent.stoppingDistance)
            {
                Stop();
                //is there an enemy within range at destination? If so attack, otherwise just stop

            }
            else
            {
                Selected(false);
                inputManager.selectedUnits.Remove(transform);
            }
        }

    }

    private void CheckForEnemyTargets()
    {
        Debug.Log("CheckForEnemyTargets()");
        rangeColliders = Physics.OverlapSphere(transform.position, basicUnitProperties.aggroRange);
        Debug.Log("aggroRange: " + basicUnitProperties.aggroRange);

        for (int i = 0; i < rangeColliders.Length; i++)
        {

            Debug.Log(rangeColliders[i].gameObject.layer);

            /*  int unitCheck;
              if (isEnemyUnit) {
                  unitCheck = (int)Layers.LayerName.EnemyUnit;
          }
          else {
                  unitCheck = (int)Layers.LayerName.PlayerUnit;
          }
          */
            int enemyLayer;
            if (gameObject.layer == (int)Layers.LayerName.PlayerUnit)
            {
                enemyLayer = (int)Layers.LayerName.EnemyUnit;
            }
            else
            {
                enemyLayer = (int)Layers.LayerName.PlayerUnit;
            }

            //check if on same layer (same team)
            if (rangeColliders[i].gameObject.layer == enemyLayer)
            {
                Debug.Log("Same layer");
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
            Stop();
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

                navMeshAgent.speed = unitSpeed;
                Debug.Log("unitSpeed: " + unitSpeed);
                navMeshAgent.isStopped = false;
            }
        }
    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
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



    public void Selected(bool isSelected)
    {
        Debug.Log("Selected function");

        selected = isSelected;
        selectedOutline.SetActive(selected);
    }

    public bool CheckSelected()
    {
        return selected;
    }


}
