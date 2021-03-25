using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace RTS1.Units.Player
{
   
    public class PlayerUnit
    {
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private BasicUnitProperties basicUnitProperties;
        private PlayerUnitState playerUnitState;
        private string _animatorDefaultParam = "speed";

        private float unitSpeed;

        public PlayerUnit(PlayerUnitState.UnitState unitState,BasicUnitProperties InputBasicUnitProperties)
        {
            playerUnitState = new PlayerUnitState(unitState);
            basicUnitProperties = InputBasicUnitProperties;
        }

        public void init(GameObject self)
        {
            navMeshAgent = self.GetComponent<NavMeshAgent>();
            animator = self.GetComponent<Animator>();

            //basicUnitProperties = self.GetComponent<BasicUnitProperties>();

            unitSpeed = basicUnitProperties.Speed;

            //_animatorDefaultParam = "speed";
        }

        public void myUpdate()
        {
            float speedVal = Mathf.Clamp(navMeshAgent.speed, 0f, 1f);
            Debug.Log(_animatorDefaultParam);
            animator.SetFloat(_animatorDefaultParam, speedVal);

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
            navMeshAgent.destination = dest;
            navMeshAgent.speed = unitSpeed;
            navMeshAgent.isStopped = false;
        }

    }
}
