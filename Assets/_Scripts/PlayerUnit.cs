using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



namespace RTS1.Units.Player
{
   
    public class PlayerUnit: MonoBehaviour
    {
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        public BasicUnitProperties basicUnitProperties;
        private PlayerUnitState playerUnitState;

        public PlayerUnit playerUnit;
        private Transform currentTarget;

        private float unitSpeed;
        private string _animatorDefaultParam;

        public GameObject unitStatDisplay;

        public Image healthBarAmount;

        private Unit unit;


        public void Start()
        {
            /*
            navMeshAgent = GetComponent<NavMeshAgent>();
            //animator = GetComponent<Animator>();
            unitSpeed = basicUnitProperties.speed/10;
            _animatorDefaultParam = "speed";
            playerUnitState = new PlayerUnitState(PlayerUnitState.UnitState.Idle);
            unit = GetComponent<Unit>();
            */
        }



    }
}
