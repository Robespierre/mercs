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
    }

    private void OnMouseUpAsButton()
    {
        if (gameScript.CurrentGameState == Game_Script.GameState.PlayerTurn && gameScript.InputLocked == false)
        {
            TileOccupant_Script occScript = gameScript.ActiveUnit.GetComponent<TileOccupant_Script>();
            curField = GameObject.Find("GameManager").GetComponent<Field_Script>().currentField;
            //ADD CHECK IF IT'S MOVEMENT STATE FOR THE MERC
            if (curField == null)
            {
                Debug.Log("cf");
            }
            // possible NullReferenceException. This situation should be handled
            StartCoroutine(curField.WalkToTile(curField.IntsToTile(occScript.tileI, occScript.tileJ), curField.IntsToTile(tileI, tileJ)));//DEBUG NEEDED
        }
    }
}
