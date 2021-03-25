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

        private float unitSpeed;
        private string _animatorDefaultParam;

        public PlayerUnit(PlayerUnitState.UnitState unitState,BasicUnitProperties InputBasicUnitProperties)
        {
            playerUnitState = new PlayerUnitState(unitState);

        }

        public void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //animator = GetComponent<Animator>();
            unitSpeed = basicUnitProperties.Speed/10;
            _animatorDefaultParam = "speed";
        }

        public void Update()
        {
            float speedVal = scale(0f,unitSpeed,0f,1f,navMeshAgent.speed);
           // Debug.Log("speedVal: " + speedVal);
           // Debug.Log(_animatorDefaultParam);
            animator.SetFloat(_animatorDefaultParam, speedVal);
            Debug.Log("speed value: " + animator.GetFloat("speed"));

            //Distance between agent and current destination
            float dist = Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination);


            //Check if arrived at destination so it can stop
            if (dist < navMeshAgent.stoppingDistance)
                stop();

        }

        private void stop()
        {
            navMeshAgent.isStopped = true;
           navMeshAgent.speed = 0f;
        }

        public void move(Vector3 dest)
        {
            //Debug.Log("move attemped!");
            navMeshAgent.destination = dest;
            //Debug.Log("dest: " + dest);
            navMeshAgent.speed = unitSpeed;
            //Debug.Log("unitSpeed: " + unitSpeed);
            navMeshAgent.isStopped = false;
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
