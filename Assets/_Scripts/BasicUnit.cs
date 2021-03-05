using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS1
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Basic")]
    public class BasicUnit:ScriptableObject
    {
        public string Name;

        //Audio??

        public int HP;
        public int DamageMin;
        public int DamageNormal;
        public int Range;
   
        public int Armour;
        public int MoveSpeed;

        public bool Selected;
        public bool CanAttack;

        public enum UnitStatus
        {
            Idle,
            Walk,
            Attack,
            Dead
        }

        public UnitStatus status;


 

        public void TakeDamage(int DamageTakenMin, int DamageTakenRandom)
        {

            int DamageTakenRandomAfterArmour = DamageTakenRandom - Armour;
            if (DamageTakenRandomAfterArmour < 0) DamageTakenRandomAfterArmour = 0;
            int TotalDamage = DamageTakenMin + DamageTakenRandomAfterArmour;

            HP -= TotalDamage;

            if (HP <= 0)
            {
                Die();
            }
            //record damage stats?
        }

        public void Die()
        {
            status = UnitStatus.Dead;    
            //death animation / set dying status
                //remove unit from field
                //record stats?
        }

    }
}
