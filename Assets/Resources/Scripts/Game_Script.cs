using System;
using System.Collections;
using UnityEngine;

public class Game_Script : MonoBehaviour
{

    [SerializeField]
    private GameState firstGameState;

    public enum GameState
    {
        PlayerTurn = 1,
        EnemyTurn = 2,
        Aftermath = 3
    }

    public GameState CurrentGameState { get; private set; }

    public bool InputLocked { get; set; }

    [SerializeField]
    private BottomButton_Script bottomButtonScript;

    public GameObject ActiveUnit { get; private set; }

    private void Start()
    {
        ChangeGameState(firstGameState);
        ActiveUnit = GameObject.Find("Archer");//REMOVE. PLACEHOLDER
        InputLocked = false;
    }

    public void ChangeGameState (GameState newState)
    {
        switch (newState)
        {
            case GameState.PlayerTurn:
                {
                    bottomButtonScript.ChangeButtonState(BottomButton_Script.bottomButtonState.playerMove);
                    break;
                }
            case GameState.EnemyTurn:
                {
                    bottomButtonScript.ChangeButtonState(BottomButton_Script.bottomButtonState.enemyMove);
                    StartCoroutine(PlaceholderEnemyMove());
                    break;
                }
            case GameState.Aftermath:
                {
                    throw new NotImplementedException();
                }
            default:
                throw new NotImplementedException();
        }
        CurrentGameState = newState;
        Debug.Log("New game state = " + CurrentGameState);
    }

    IEnumerator PlaceholderEnemyMove()
    {
        yield return new WaitForSeconds(3.0f);
        ChangeGameState(GameState.PlayerTurn);
    }
}
