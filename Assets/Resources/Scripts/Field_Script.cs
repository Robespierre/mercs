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
        var startPoint = size / 2.0f - 0.5f;
        var tilePos = new Vector2(startPoint, startPoint);
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                var newTile = new Tile();
                tileSet[i, j] = newTile;
                var newTileObject = GameObject.Instantiate(Resources.Load("Prefabs/Tile")) as GameObject;
                newTile.AssignGameObject(newTileObject);
                // ToDo: possible NullRef here. Need to handle this cases
                newTileObject.transform.position = tilePos;
                tilePos = new Vector2(tilePos.x - 1, tilePos.y);
            }
            tilePos = new Vector2(startPoint, tilePos.y - 1);
        }
    }
    public Tile GetEmptyLeft() //returns random tile in a left column
    {
        var emptyLeftTiles = new List<Tile>();
        for (var row = 0; row < size; row++)
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
                newTile.SetI(i);
                newTile.SetJ(j);
                newTileObject.GetComponent<TileObject_Script>().tileI = i;
                newTileObject.GetComponent<TileObject_Script>().tileJ = j;
                tileSet[i, j] = newTile;
            }
        }
    }
    private void OccupantStep(Tile startTile, Tile finishTile)
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
        finishTile.GetOccupant().GetComponent<TileOccupant_Script>().tileI = finishTile.GetI();
        finishTile.GetOccupant().GetComponent<TileOccupant_Script>().tileJ = finishTile.GetJ();
        finishTile.GetOccupant().transform.position = new Vector3(finishTile.GetGameObject().transform.position.x,
                                                                  finishTile.GetGameObject().transform.position.y,
                                                                  finishTile.GetOccupant().transform.position.z);
    }

    // ToDo: refactor this method cause it's very big with too many if conditions
    public Tile[] FindPath(Tile startTile, Tile finishTile)
    {
        if (startTile == finishTile)
        {
            Debug.Log("Already on this tile!");
            return null;
        }
        if (finishTile.GetOccupant() != null)
        {
            Debug.Log("Tile is occupied!");
            return null;
        }

        Tile[,] previous = new Tile[size, size];
        bool[,] active = new bool[size, size];
        bool[,] newActive = new bool[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                previous[i, j] = null;
                active[i,j] = false;
                newActive[i, j] = false;
            }
        }
        active[startTile.GetI(), startTile.GetJ()] = true;
        int iteration = 0;
        bool blocked = false;
        while ((!active[finishTile.GetI(), finishTile.GetJ()]) && !blocked)
        {
            blocked = true;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (active[i,j])
                    {
                        if ((i+1<size) && (tileSet[i+1,j].GetOccupant() == null) && (previous[i+1,j] == null))
                        {
                            newActive[i+1, j] = true;
                            previous[i + 1, j] = tileSet[i, j];
                            blocked = false;
                        }
                        if ((j + 1 < size) && (tileSet[i, j+1].GetOccupant() == null) && (previous[i, j+1] == null))
                        {
                            newActive[i, j+1] = true;
                            previous[i, j+1] = tileSet[i, j];
                            blocked = false;
                        }
                        if ((i - 1 >= 0) && (tileSet[i - 1, j].GetOccupant() == null) && (previous[i - 1, j] == null))
                        {
                            newActive[i - 1, j] = true;
                            previous[i - 1, j] = tileSet[i, j];
                            blocked = false;
                        }
                        if ((j - 1 >= 0) && (tileSet[i, j - 1].GetOccupant() == null) && (previous[i, j - 1] == null))
                        {
                            newActive[i, j - 1] = true;
                            previous[i, j - 1] = tileSet[i, j];
                            blocked = false;
                        }
                        active[i, j] = false;
                    }
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (newActive[i, j])
                    {
                        active[i, j] = true;
                        newActive[i, j] = false;
                    }
                }
            }
            iteration++;
        }
        if (blocked)
        {
            Debug.Log("Path is blocked!");
            return null;
        }
        Tile[] path = new Tile[iteration+1];
        Tile toAdd = finishTile;
        for (int stepNum = iteration; stepNum >= 0; stepNum--)
        {
            path[stepNum] = toAdd;
            toAdd = previous[toAdd.GetI(), toAdd.GetJ()];
        }
        if (path[0] != startTile)
        {
            Debug.Log("Path is not fulfilled");
            return null;
        }
        return path;
    }

    public Tile IntsToTile(int i, int j)
    {
        return tileSet[i, j];
    }
    public Tile UpperTile(Tile sourceTile)
    {
        return sourceTile.GetI() + 1 < size ? IntsToTile(sourceTile.GetI() + 1, sourceTile.GetJ()) : null;
    }
    public Tile LowerTile(Tile sourceTile)
    {
        if (sourceTile.GetI() - 1 >= 0)
        {
            return IntsToTile(sourceTile.GetI() - 1, sourceTile.GetJ());
        }
        else
        {
            return null;
        }
    }
    public Tile RightTile(Tile sourceTile)
    {
        if (sourceTile.GetJ() + 1 < size)
        {
            return IntsToTile(sourceTile.GetI(), sourceTile.GetJ() + 1);
        }
        else
        {
            return null;
        }
    }
    public Tile LeftTile(Tile sourceTile)
    {
        if (sourceTile.GetJ() - 1 >= 0)
        {
            return IntsToTile(sourceTile.GetI(), sourceTile.GetJ() - 1);
        }
        else
        {
            return null;
        }
    }
    
    public IEnumerator WalkToTile(Tile startTile, Tile finishTile)
    {
        var gameScript = GameObject.Find("GameManager").GetComponent<Game_Script>();
        gameScript.InputLocked = true;
        Tile[] walkway = FindPath(startTile, finishTile);
        if (walkway != null)
        {
            for (var step = 0; step < walkway.Length - 1; step++)
            {
                yield return new WaitForSeconds(0.2f);
                OccupantStep(IntsToTile(walkway[step].GetI(), walkway[step].GetJ()), IntsToTile(walkway[step+1].GetI(), walkway[step+1].GetJ()));
                yield return new WaitForSeconds(0.2f);
            }
        }

        gameScript.InputLocked = false;
    }
    public IEnumerator WalkToAdjacent(Tile startTile, Tile finishTile)
    {
        var gameScript = GameObject.Find("GameManager").GetComponent<Game_Script>();
        var fieldScript = GameObject.Find("GameManager").GetComponent<Field_Script>();
        gameScript.InputLocked = true;
        Tile[] walkway = null;
        Tile[] adjTiles = {
                            fieldScript.currentField.UpperTile(finishTile),
                            fieldScript.currentField.LowerTile(finishTile),
                            fieldScript.currentField.LeftTile(finishTile),
                            fieldScript.currentField.RightTile(finishTile)
                          };
        foreach (Tile adjTile in adjTiles)
        {
            if ((adjTile != null) && (adjTile.GetOccupant() == null))//if the checked tile is okay
            {
                if ((walkway == null) || (walkway.Length > FindPath(startTile, adjTile).Length))//and the path is better than the current one
                {
                    walkway = FindPath(startTile, adjTile);
                }
            }
        }
        if (walkway == null)
        {
            Debug.Log("Can't get to adjacent tile!");
            yield return null;
        }
        for (int step = 0; step < walkway.Length - 1; step++)
        {
            yield return new WaitForSeconds(0.2f);
            OccupantStep(IntsToTile(walkway[step].GetI(), walkway[step].GetJ()), IntsToTile(walkway[step + 1].GetI(), walkway[step + 1].GetJ()));
            yield return new WaitForSeconds(0.2f);
        }
        gameScript.InputLocked = false;
    }

}

public class Tile
{
    GameObject tileGameobject;
    private GameObject occupant;

    int tileI;
    int tileJ;

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
    public int GetI()
    {
        return tileI;
    }
    public int GetJ()
    {
        return tileJ;
    }
    public void SetI(int newI)
    {
        tileI = newI;
    }
    public void SetJ(int newJ)
    {
        tileJ = newJ;
    }
}
