using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameState initialApplicationState;

    private GameState state;

    // Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);  // Sets this to not be destroyed when reloading scene
        SwitchGameState(initialApplicationState);
    }

    public void SwitchGameState(GameState state)
    {
        this.state = state;

        switch (state)
        {
            case GameState.SPLASH:
                SceneFader.LoadScene("Splash", 0.5f, 1);
                // Hack - fadeOut time must be greater than zero. Otherwise animation framerate decreases significantly.
                break;
            case GameState.MAIN_MENU:
                SceneFader.LoadScene("Menu", 1, 0);
                break;
            case GameState.SCORE_BOARD:
                SceneFader.LoadScene("ScoreBoard", 1, 0);
                break;
            case GameState.GAMEPLAY:
                SceneFader.LoadScene("Gameplay", 0, 1);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
