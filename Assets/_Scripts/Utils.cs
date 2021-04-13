using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS1.Utils
{
    public class Utils : MonoBehaviour
    {
        public static Utils utils;
        public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }

        private void Awake()
        {
            utils = this;
        }
    }
}
