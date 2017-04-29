using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateField_Script : MonoBehaviour
{

    Field myField = new Field(7);//prototype magic number

    private void Awake()
    {
        //spawning field
        myField.SpawnField();

        //spawning mercs:
        //...
    }

    public Field GetField()
    {
        return myField;
    }
}
