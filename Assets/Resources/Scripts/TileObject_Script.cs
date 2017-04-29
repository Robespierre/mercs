using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject_Script : MonoBehaviour
{
    Game_Script gameScript;
    Field curField;

    public int tileI;
    public int tileJ;

    private void Awake()
    {
        gameScript = GameObject.Find("GameManager").GetComponent<Game_Script>();
        curField = GameObject.Find("GameManager").GetComponent<Field_Script>().currentField;
    }

    private void OnMouseUpAsButton()
    {
        if (gameScript.currentGameState == Game_Script.GameState.playerTurn && !gameScript.inputLocked)
        {
            TileOccupant_Script occScript = gameScript.activeUnit.GetComponent<TileOccupant_Script>(); 
            //ADD CHECK IF IT'S MOVEMENT STATE FOR THE MERC
            if (curField == null)
            {
                Debug.Log("cf");
            }
            if (curField == null)
            {
                Debug.Log("os");
            }
            StartCoroutine(curField.WalkThePath(curField.IntsToTile(occScript.tileI, occScript.tileJ), curField.IntsToTile(tileI, tileJ)));//DEBUG NEEDED
        }
    }
}
