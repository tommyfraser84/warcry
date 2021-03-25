using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS1.Units.Player
{
    public class PlayerUnitState: ScriptableObject
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

        public UnitState GetCurrentState()
        {
            return state;
        }

        public void ChangeState(UnitState changeState)
        {
            state = changeState;
        }

    }
}
