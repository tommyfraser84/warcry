using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace RTS1.Units.Player
{
   
    public class PlayerUnit: MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        public Animator animator;
        public BasicUnitProperties basicUnitProperties;
        private PlayerUnitState playerUnitState;
        public GameObject selectedOutline;
        public PlayerUnit playerUnit;
        private Transform currentTarget;

        private float unitSpeed;
        private string _animatorDefaultParam;

        public void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //animator = GetComponent<Animator>();
            unitSpeed = basicUnitProperties.Speed/10;
            _animatorDefaultParam = "speed";
            playerUnitState = new PlayerUnitState(PlayerUnitState.UnitState.Idle);

        }

        public void Update()
        {
            Move();

        }

        private void Stop()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0f;
            playerUnitState.ChangeState(PlayerUnitState.UnitState.Idle);
        }

        public void SetMove(Vector3 dest)
        {
            Debug.Log("move attemped!");
            navMeshAgent.destination = dest;
            //Debug.Log("dest: " + dest);
            navMeshAgent.speed = unitSpeed;
            //Debug.Log("unitSpeed: " + unitSpeed);
            navMeshAgent.isStopped = false;
            playerUnitState.ChangeState(PlayerUnitState.UnitState.Walk);
        }

        public void SetTarget(Transform target)
        {
            currentTarget = target;
        }

        public Transform GetTarget()
        {
            return currentTarget;
        }


        private void Move()
        {            
            //Create animator speedvalue based on unit speed property
            float animatorSpeedVal = scale(0f, unitSpeed, 0f, 1f, navMeshAgent.speed);

            //Set animator with the value
            animator.SetFloat(_animatorDefaultParam, animatorSpeedVal);


            //Distance between agent and current destination
            float dist = Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination);

            //if a target is set, make navmesh destination same as targets position
            //if (currentTarget != null) navMeshAgent.destination = currentTarget.position;

            //Check if arrived at destination
            if (dist < navMeshAgent.stoppingDistance)
            {
                Stop();
                //is there an enemy within range at destination? If so attack, otherwise just stop
                if (GetTarget() != null && playerUnitState.GetCurrentState() != PlayerUnitState.UnitState.Attack)
                {
                    Attack(currentTarget);
                }    
            }
        }

        private void Attack(Transform target)
        {
            playerUnitState.ChangeState(PlayerUnitState.UnitState.Attack);
            //set animator trigger for attack
            animator.SetBool("Attack",true);
            //perform enemy takedamage on enemy unit
            //target.getComponent<PlayerUnit>().TakeDamage();
        } 

        public void TakeDamage(int DamageBasic, int DamagePiercing)
        {
            // (Basic Damage - Target's Armor) + Piercing Damage = Maximum damage inflicted
            //The attacker does a random amount of damage from 50%-100% of this total each attack.

            int DamageTaken = Mathf.RoundToInt(((DamageBasic - basicUnitProperties.Armour) + DamagePiercing) * Random.Range(0.5f, 1));

            Debug.Log(DamageTaken);

            basicUnitProperties.HP -= DamageTaken;

            animator.SetTrigger("Take Damage");

            if (basicUnitProperties.HP <= 0)
            {
                // Dead();
            }
            else
            {
                //damage anim
            }
        }

        public void Selected(bool selected)
        {
            selectedOutline.SetActive(selected);
            basicUnitProperties.Selected = selected;
        }

        public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }

    }
}
