using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Script : MonoBehaviour {

    enum turnState { movement, action, done }

    private int health;

    public void ChangeHealth(int changeValue)
    {
        health -= changeValue;
        if (health < 0)
        {
            health = 0;
        }
    }
}
