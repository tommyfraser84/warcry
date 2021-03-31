using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS1.Units
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Basic")]
    public class BasicUnitProperties:ScriptableObject
    {
        public string Name;

        public float hp;
        public float damageBasic;
        public float damagePiercing;
        public float aggroRange;
        public float attkRange;

        public float armour;
        public float speed;


        }
    }
