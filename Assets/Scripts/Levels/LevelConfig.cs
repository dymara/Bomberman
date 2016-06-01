using UnityEngine;
using System.Collections;

public class LevelConfig
{
    public float levelDuration;

    public int monstersCount;

    public int extraBombsCount;
	
	public int extraLivesCount;
	
    public Vector2 boardSize;

    public override string ToString()
    {
		string SEPARATOR = ", ";
		return "LevelConfig {" +
			"levelDuration: " + levelDuration + SEPARATOR +
			"monstersCount: " + monstersCount + SEPARATOR +
			"extraLivesCount: " + extraLivesCount + SEPARATOR +
			"extraBombsCount: " + extraBombsCount + SEPARATOR + 
			"boardSize: " + boardSize + "}";
    }
}
