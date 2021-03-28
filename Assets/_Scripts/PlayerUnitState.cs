using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS1.Units.Player
{
    public class PlayerUnitState
    {

        public PlayerUnitState(UnitState startState)
        {
            state = startState;
        }

        public enum UnitState
        {
            Idle,
            Walk,
            Attack,
            Dead
        }

        private UnitState state;
        private Transform target;

        public UnitState GetCurrentState()
        {
            return state;
        }

        public void ChangeState(UnitState changeState)
        {
            state = changeState;
        }

        public void SetTarget(Transform enemyTransform)
        {
            target = enemyTransform;
        }

    }
}
