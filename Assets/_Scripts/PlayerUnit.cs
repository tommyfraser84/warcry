using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using RTS1.Input;


namespace RTS1.Units.Player
{
   
    public class PlayerUnit: MonoBehaviour
    {
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        public BasicUnitProperties basicUnitProperties;
        private PlayerUnitState playerUnitState;
        public GameObject selectedOutline;
        public PlayerUnit playerUnit;
        private Transform currentTarget;

        public InputManager inputManager;


        private bool selected;

        private float unitSpeed;
        private string _animatorDefaultParam;

        public GameObject unitStatDisplay;

        public Image healthBarAmount;

        public float currentHealth;

        private void HandleHealth()
        {
            Camera camera = Camera.main;
            unitStatDisplay.transform.LookAt(camera.transform.position);

            healthBarAmount.fillAmount = currentHealth / basicUnitProperties.hp;

            if (currentHealth<= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            inputManager.selectedUnits.Remove(gameObject.transform);
            Destroy(gameObject);
        }

        public void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //animator = GetComponent<Animator>();
            unitSpeed = basicUnitProperties.speed/10;
            _animatorDefaultParam = "speed";
            playerUnitState = new PlayerUnitState(PlayerUnitState.UnitState.Idle);

            currentHealth = basicUnitProperties.hp;

        }

        public void Update()
        {

            //Create animator speedvalue based on unit speed property
            float animatorSpeedVal = scale(0f, unitSpeed, 0f, 1f, navMeshAgent.speed);

            //Set animator with the value
            animator.SetFloat(_animatorDefaultParam, animatorSpeedVal);
            //Move();
            float dist = Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination);
            if (dist < navMeshAgent.stoppingDistance)
            {
                Stop();
                //is there an enemy within range at destination? If so attack, otherwise just stop

            }

            HandleHealth();
        }

        private void Stop()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0f;
            playerUnitState.ChangeState(PlayerUnitState.UnitState.Idle);
        }

        public void Move(Vector3 dest)
        {

            Debug.Log("move attemped!");

            navMeshAgent.destination = dest;
            //Debug.Log("dest: " + dest);
            navMeshAgent.speed = unitSpeed;
            //Debug.Log("unitSpeed: " + unitSpeed);
            navMeshAgent.isStopped = false;
            playerUnitState.ChangeState(PlayerUnitState.UnitState.Walk);


        }


        public void TakeDamage(int DamageBasic, int DamagePiercing)
        {
            // (Basic Damage - Target's Armor) + Piercing Damage = Maximum damage inflicted
            //The attacker does a random amount of damage from 50%-100% of this total each attack.

            int DamageTaken = Mathf.RoundToInt(((DamageBasic - basicUnitProperties.armour) + DamagePiercing) * Random.Range(0.5f, 1));

            Debug.Log(DamageTaken);

            currentHealth -= DamageTaken;

            animator.SetTrigger("Take Damage");
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

        public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }

    }
}
