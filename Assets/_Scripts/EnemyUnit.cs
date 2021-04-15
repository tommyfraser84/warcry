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


    //private Transform aggroTarget;



    //private bool hasAggro = false;




    private float unitSpeed;

    private float aggroRange;


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        //unit = GetComponent<Unit>();

       // basicUnitProperties = unit.basicUnitProperties;


    }



}


