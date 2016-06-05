using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // TODO - THIS IS A TEMPORARY SOLUTION BEFORE A MAIN MENU SCENE GETS IMPLEMENTED
        // GameManager.instance.ResetPlayerState();
        // GameManager.instance.SwitchGameState(GameState.GAMEPLAY);

        GameManager.instance.SetCameraMenuMode(true);
        StartCoroutine(StartGame());
    }
	
	// Update is called once per frame
	void Update () {

    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(15);
        GameManager.instance.ResetPlayerState();
        GameManager.instance.SwitchGameState(GameState.GAMEPLAY);
    }
}
