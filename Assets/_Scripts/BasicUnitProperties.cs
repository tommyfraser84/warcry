using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS1.Units
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Basic")]
    public class BasicUnitProperties:ScriptableObject
    {
        public string Name;

        public int HP;
        public int DamageBasic;
        public int DamagePiercing;
        public int Range;
   
        public int Armour;
        public float Speed;

        public bool CanAttack;

        }
    }
