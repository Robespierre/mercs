using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field_Script : MonoBehaviour
{
    public Field currentField;

    private void Awake()
    {
        currentField = new Field(7);
        //prototype shitcode area starting, don't be frightened
        currentField.PrototypeAssignTiles();
        currentField.IntsToTile(5, 1).SetOccupant(GameObject.Find("Warrior"));
        currentField.IntsToTile(1, 0).SetOccupant(GameObject.Find("Archer"));
        currentField.IntsToTile(2, 4).SetOccupant(GameObject.Find("Rock (0)"));
        currentField.IntsToTile(5, 3).SetOccupant(GameObject.Find("Rock (1)"));
        currentField.IntsToTile(5, 5).SetOccupant(GameObject.Find("Wolf (0)"));
        currentField.IntsToTile(1, 5).SetOccupant(GameObject.Find("Wolf (1)"));
        currentField.IntsToTile(3, 6).SetOccupant(GameObject.Find("Wolf (2)"));
    }
}


public class Field
{
    const float TILESTEP = 1.0f;
    public float tileStep
    {
        get { return TILESTEP; }
    }
    public enum direction { up, down, left, right}

    public int size { get; set; }
    Tile[,] tileSet;

    public Field(int inputSize)
    {
        size = inputSize;
        tileSet = new Tile[inputSize, inputSize];
    }

    public void SpawnField()
    {
        float startPoint = ((size / 2.0f) - 0.5f);
        Vector2 tilePos = new Vector2(startPoint, startPoint);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Tile newTile = new Tile();
                tileSet[i, j] = newTile;
                GameObject newTileObject = GameObject.Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
                newTile.AssignGameObject(newTileObject);
                newTileObject.transform.position = tilePos;
                tilePos = new Vector2(tilePos.x - 1, tilePos.y);
            }
            tilePos = new Vector2(startPoint, tilePos.y - 1);
        }
    }
    public Tile GetEmptyLeft() //returns random tile in a left column
    {
        List<Tile> emptyLeftTiles = new List<Tile>();
        for (int row = 0; row < size; row++)
        {
            if (tileSet[row, size] == null)
            {
                emptyLeftTiles.Add(tileSet[row, size]);
            }
        }
        if (emptyLeftTiles.Count == 0)
        {
            return null;
        } else
        {
            int randy = Random.Range(0, emptyLeftTiles.Count);
            return emptyLeftTiles[randy];
        }
    }
    public void PrototypeAssignTiles()
    {
        int count = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Tile newTile = new Tile();
                GameObject newTileObject = GameObject.Find("Tile(Clone) (" + count + ")");
                count++;
                newTile.AssignGameObject(newTileObject);
                tileSet[i, j] = newTile;
            }
        }
    }
    public void OccupantStep(Tile startTile, Tile finishTile)
    {
        if (finishTile.GetOccupant() != null)
        {
            Debug.Log("Tile is not empty!");
            return;
        }
        if (Vector2.Distance(startTile.GetGameObject().transform.position, finishTile.GetGameObject().transform.position) != 1.0f)
        {
            Debug.Log("Tiles aren't adjacent!");
            return;
        }
        finishTile.SetOccupant(startTile.GetOccupant());
        startTile.SetOccupant(null);
        finishTile.GetOccupant().transform.position = new Vector3(finishTile.GetGameObject().transform.position.x, finishTile.GetGameObject().transform.position.y, finishTile.GetOccupant().transform.position.z);
    }
    public Tile IntsToTile(int i, int j)
    {
        return tileSet[i, j];
    }
}

public class Tile
{
    GameObject tileGameobject;
    private GameObject occupant;

    public Tile()
    {
        occupant = null;
    }

    public void AssignGameObject(GameObject tileObj)
    {
        tileGameobject = tileObj;
    }
    public GameObject GetGameObject()
    {
        return tileGameobject;
    }
    
    public GameObject GetOccupant()
    {
        return occupant;
    }
    public void SetOccupant(GameObject newOccupant)
    {
        occupant = newOccupant;
    }
}
