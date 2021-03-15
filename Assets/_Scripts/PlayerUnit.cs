using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS1.Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerUnit : MonoBehaviour
    {
        private NavMeshAgent navAgent;

        // Start is called before the first frame update
        void OnEnable()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveUnit(Vector3 _destination)
        {
            navAgent.SetDestination(_destination);
        }

    }
}
