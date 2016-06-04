using UnityEngine;

public class Configurator : MonoBehaviour {

    public GameState initialApplicationState;

    public bool displayWatermark;

    [Range(0, 10)]
    public float splashDuration;

    [Range(1, 999)]
    public int initialLevelNumber;

    [Range(1, 6)]
    public int initialPlayerLives;

    [Range(1, 5)]
    public int initialPlayerBombs;

    [Range(1, 16)]
    public int initialPlayerBombRange;

    [Range(1, 3)]
    public float initialPlayerSpeed;

    public bool initialPlayerRemoteDetonationBonus;

    /** Number of indestructible cubes in x axis */
    [Range(4, 128)]
    public int level1CubesXCount; //indestructibleCubesXNumber;

    /** Number of indestructible cubes in z axis */
    [Range(4, 128)]
    public int level1CubesZCount; //indestructibleCubesZNumber;

    [Range(1, 32)]
    public int exitExplosionEnemiesCount;

    public int enemiesMinDistance; 

    [Range(0, 100)]
    public float findingSpinSpeed;

    [Range(0, 5)]
    public float findingFloatSpeed;

    [Range(0, 1)]
    public float findingFloatDist;

    [Range(0, 100)]
    public float exitSpinSpeed;

    [Range(1, 10)]
    public int bombDetonateDelay;

    [Range(0, 10000)]
    public int blockDestructionPoints;

    [Range(0, 10000)]
    public int findingPickupPoints;

    [Range(0, 10000)]
    public int enemyKillingPoints;

    [Range(0, 10000)]
    public int levelClearedPoints;

    [Range(0, 10000)]
    public int timeMultiplierPoints;

    [Range(1, 2)]
    public float speedMultiplier;

    /** Cell size -> cube length and width */
    public float cellSize;

    /** Cube height. */
    public float cubeHeight;

    /** Wall height.*/
    public float wallHeight;

    /** Player start position in x axis - MIN -> 1, MAX -> width - 1 */
    public StartPosition startPositionX;

    /** Player start position in z axis - MIN -> 1, MAX -> length - 1 */
    public StartPosition startPositionZ;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
