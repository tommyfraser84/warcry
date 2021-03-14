using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS1.Units;

public class BasicUnitActions : MonoBehaviour
{
    public BasicUnitProperties basicUnit;
    public GameObject selectedOutline;

    public void Walk()
    {
        //change animation
        basicUnit.status = BasicUnitProperties.UnitStatus.Walk;
    }

    public void Idle()
    {
        //change animation
        basicUnit.status = BasicUnitProperties.UnitStatus.Idle;
    }

    public void Attack()
    {
        //change animation
        basicUnit.status = BasicUnitProperties.UnitStatus.Attack;
    }
    public void Dead()
    {
        //change animation
        basicUnit.status = BasicUnitProperties.UnitStatus.Dead;
    }

    public void Selected(bool selected)
    {
        selectedOutline.SetActive(selected);
        basicUnit.Selected = selected;
    }

    public void TakeDamage(int DamageBasic, int DamagePiercing)
    {
        // (Basic Damage - Target's Armor) + Piercing Damage = Maximum damage inflicted
        //The attacker does a random amount of damage from 50%-100% of this total each attack.

        int DamageTaken = Mathf.RoundToInt(((DamageBasic - basicUnit.Armour) + DamagePiercing) * Random.Range(0.5f, 1));

        Debug.Log(DamageTaken);

        basicUnit.HP -= DamageTaken;

        if (basicUnit.HP <= 0)
        {
            Dead();
        } else
        {
            //damage anim
        }
    }

}
