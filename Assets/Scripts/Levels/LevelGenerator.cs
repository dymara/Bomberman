using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{
	private const int BASE_BOARD_SIZE = 5;
	
    public LevelConfig GenerateLevelConfig(int level)
    {
        LevelConfig levelConfig = new LevelConfig();
		levelConfig.boardSize = GetBoardSize(level);
        return levelConfig;
    }

    private Vector2 GetBoardSize(int level)
    {
		int sizeX = BASE_BOARD_SIZE + level / 2;
		int sizeY = BASE_BOARD_SIZE + level / 2;
		
		return new Vector2(sizeX, sizeY);
    }
}
