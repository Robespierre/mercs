using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary_Script : TileObject_Script
{
    private int health;//MOVE TO CREATURE_SCRIPT

    public void ChangeHealth(int changeValue)//MOVE TO CREATURE_SCRIPT
    {
        health -= changeValue;
        if (health < 0)
        {
            health = 0;
        }
    }
}

