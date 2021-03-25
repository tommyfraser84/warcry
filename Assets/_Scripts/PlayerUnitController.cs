using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS1.Units;
using RTS1.Units.Player;

public class PlayerUnitController : MonoBehaviour
{
    public BasicUnitProperties basicUnitProperties;
    public PlayerUnit playerUnit;
  

    // Start is called before the first frame update
    void Start()
    {
        //basicUnitProperties = GetComponent<BasicUnitProperties>();
        playerUnit = new PlayerUnit(PlayerUnitState.UnitState.Idle, basicUnitProperties);
        playerUnit.init(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        playerUnit.myUpdate();
    }
}
