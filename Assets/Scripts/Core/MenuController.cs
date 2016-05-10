using UnityEngine;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // TODO - THIS IS A TEMPORARY SOLUTION BEFORE A MAIN MENU SCENE GETS IMPLEMENTED
        GameManager.instance.ResetPlayerState();
        GameManager.instance.SwitchGameState(GameState.GAMEPLAY);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
