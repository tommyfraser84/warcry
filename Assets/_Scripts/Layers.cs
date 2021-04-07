using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS1.Layers {


    public class Layers : ScriptableObject
    {
        public enum LayerName : int
        {
            PlayerUnit = 8,
            EnemyUnit = 13,
            Floor = 12
        }
    }

}
