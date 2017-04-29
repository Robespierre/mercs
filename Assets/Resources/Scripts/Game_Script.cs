using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Script : MonoBehaviour {

    public enum GameState { playerTurn, enemyTurn, aftermath}
    public GameState currentGameState;
    public GameObject activeUnit;
    public bool inputLocked;

    public BottomButton_Script bottomButtonScript;

    private void Start()
    {
        ChangeGameState(GameState.playerTurn);
        activeUnit = GameObject.Find("Archer");//REMOVE. PLACEHOLDER
        inputLocked = false;
    }

    public void ChangeGameState (GameState newState)
    {
        switch (newState)
        {
            case GameState.playerTurn:
                {
                    bottomButtonScript.ChangeButtonState(BottomButton_Script.bottomButtonState.playerMove);
                    break;
                }
            case GameState.enemyTurn:
                {
                    bottomButtonScript.ChangeButtonState(BottomButton_Script.bottomButtonState.enemyMove);
                    StartCoroutine(PlaceholderEnemyMove());
                    break;
                }
            case GameState.aftermath:
                {
                    //shouldn't happen yet
                    break;
                }
        }
        currentGameState = newState;
        Debug.Log("New game state = " + currentGameState);
    }

    IEnumerator PlaceholderEnemyMove()
    {
        yield return new WaitForSeconds(3.0f);
        ChangeGameState(GameState.playerTurn);
    }
}
