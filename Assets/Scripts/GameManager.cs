using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;

    private Maze mazeInstance;

    public int width;

    public int length;

    // Use this for initialization
    void Start () {
        BeginGame();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void BeginGame() {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate(width, length);
    }

    private void RestartGame() { }
}
