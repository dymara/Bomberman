using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class MenuController : MonoBehaviour {

    private Button newGameButton;

    private bool newGameButtonPressed = false;

    private Button scoreBoardButton;

    private bool scoreBoardButtonPressed = false;

    private Button exitButton;

    private Canvas dialogCanvas;

    private Button exitYesButton;

    private Button exitNoButton;

	// Use this for initialization
	void Start () {
        GameManager.instance.SetCameraMenuMode(true);

        this.newGameButton = GameObject.Find("Play Button").GetComponent<Button>();
        this.scoreBoardButton = GameObject.Find("Score Board Button").GetComponent<Button>();
        this.exitButton = GameObject.Find("Exit Button").GetComponent<Button>();
        this.dialogCanvas = GameObject.Find("Dialog Canvas").GetComponent<Canvas>();
        this.exitYesButton = GameObject.Find("Yes Button").GetComponent<Button>();
        this.exitNoButton = GameObject.Find("No Button").GetComponent<Button>();

        this.dialogCanvas.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void NewGameButtonPressed()
    {
        if (!newGameButtonPressed && !scoreBoardButtonPressed)
        {
            newGameButtonPressed = true;
            GameManager.instance.ResetPlayerState();
            GameManager.instance.SwitchGameState(GameState.GAMEPLAY);
        }
    }

    public void ScoreBoardButtonPressed()
    {
        if (!newGameButtonPressed && !scoreBoardButtonPressed)
        {
            scoreBoardButtonPressed = true;
            GameManager.instance.SwitchGameState(GameState.MAIN_MENU);
            // GameManager.instance.SwitchGameState(GameState.SCORE_BOARD);
        }
    }

    public void ExitButtonPressed()
    {
        newGameButton.interactable = false;
        scoreBoardButton.interactable = false;
        exitButton.interactable = false;
        dialogCanvas.enabled = true;
    }

    public void ExitYesButtonPressed()
    {
        Application.Quit();
        ExitNoButtonPressed();
    }

    public void ExitNoButtonPressed()
    {
        dialogCanvas.enabled = false;
        newGameButton.interactable = true;
        scoreBoardButton.interactable = true;
        exitButton.interactable = true;
    }

}
