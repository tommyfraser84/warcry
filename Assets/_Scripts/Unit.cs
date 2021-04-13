using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using RTS1.Units;
using RTS1.Utils;

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

    public void TakeDamage(float DamageBasic, float DamagePiercing)
    {
        // (Basic Damage - Target's Armor) + Piercing Damage = Maximum damage inflicted
        //The attacker does a random amount of damage from 50%-100% of this total each attack.

        float DamageTaken = ((DamageBasic - basicUnitProperties.armour) + DamagePiercing) * Random.Range(0.5f, 1);

        Debug.Log("Damage Taken: " + DamageTaken);

        currentHealth -= DamageTaken;

        Debug.Log("Current Health: " + currentHealth);

        animator.SetTrigger("Take Damage");
    }

    private void Die()
    {
        Destroy(gameObject);
    }


    private void Start()
    {
        currentHealth = basicUnitProperties.hp;
        unitSpeed = basicUnitProperties.speed / 10;
        navMeshAgent = GetComponent<NavMeshAgent>();
        _animatorDefaultParam = "speed";
    }

    private void Update()
    {
        //Create animator speedvalue based on unit speed property
        float animatorSpeedVal = scale(0f, unitSpeed, 0f, 1f, navMeshAgent.speed);


        //Set animator with the value
        animator.SetFloat(_animatorDefaultParam, animatorSpeedVal);

        //TryDamage();
        HandleHealth();
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
}
