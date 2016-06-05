using UnityEngine;

public class ScoreBoardController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // TODO - THIS IS A TEMPORARY SOLUTION BEFORE A SCORE BOARD SCENE GETS IMPLEMENTED
        GameManager.instance.SwitchGameState(GameState.MAIN_MENU);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
