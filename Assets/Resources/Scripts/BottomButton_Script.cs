using UnityEngine;
using UnityEngine.UI;

public class BottomButton_Script : MonoBehaviour
{
    public enum bottomButtonState { playerMove, enemyMove}

    bottomButtonState currentState;
    Text buttonText;
    Button thisButton;
    public delegate void ButtonAction();
    ButtonAction myAction;
    Game_Script gameScript;

    private void Awake()
    {
        buttonText = GetComponentInChildren<Text>();
        thisButton = GetComponent<Button>();
        gameScript = GameObject.Find("GameManager").GetComponent<Game_Script>();
        thisButton.onClick.AddListener(CostylClick);
    }

    public void ChangeButtonState(bottomButtonState newButtonState)
    {
        switch (newButtonState)
        {
            case bottomButtonState.enemyMove:
                {
                    buttonText.text = "Enemy move...";
                    thisButton.interactable = false;
                    break;
                }
            case bottomButtonState.playerMove:
                {
                    buttonText.text = "Continue";
                    thisButton.interactable = true;
                    myAction = StartEnemyMove;
                    break;
                }
        }
    }

    // I like this nazvanie :))
    void CostylClick()//I KNOW THIS IS UTTERLY WRONG, GONNA REWRITE IT
    {
        myAction();
    }
    void StartEnemyMove()
    {
        gameScript.ChangeGameState(Game_Script.GameState.EnemyTurn);
    }
}
