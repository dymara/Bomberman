using UnityEngine;
using System.Collections;
using Assets.Scripts.Board;
using Assets.Scripts.Model;
using Assets.Scripts.Util;

public enum CellType { EMPTY, DESTRUCTIBLE, INDESTRUCTIBLE, PLAYER, BOMB };

public class Maze : MonoBehaviour
{
    // Minimal distance from start to exit postion (from 0.0 to 1.0)
    private const float MIN_EXIT_DISTANCE = 0.7f;

    private const float WALL_THICKNESS = 0.5f;

    /* TEXTURE TAILING CONSTANTS */

    private const float WALL_TEXTURE_WIDTH = 8.5f;

    private const float FLOOR_TEXTURE_SIZE = 3.75f;

    public IndesctructibleCubeObject indestructibleCube;

    public DesctructibleCubeObject destructibleCube;

    public Exit mazeExit;

    public Component wallA;

    public Component wallB;

    public Component floor;

    private System.Random rnd = new System.Random();

    private readonly float[] ROTATIONS = { 0, 90 };

    public Board Generate(float boardWidth, float boardLength, float cubeWidth, float cubeHeight, float wallHeight, float startPositionX, float startPositionZ, PositionConverter positionConverter)
    {
        Vector3 cubeSize = new Vector3(cubeWidth, cubeHeight, cubeWidth);

        indestructibleCube.transform.localScale = cubeSize;

        destructibleCube.transform.localScale = cubeSize;

        int allXCell = (int)(boardWidth / cubeSize.x);
        int allZCell = (int)(boardLength / cubeSize.z);

        int count_x = (allXCell - 1) / 2;
        int count_z = (allZCell - 1) / 2;

        //arraylist for places with destructible cubes for exit
        ArrayList availableExits;

        //array for minimap
        CellType[,] tmpBoard = new CellType[allXCell, allZCell];

        GameCell[,] cells = new GameCell[allXCell, allZCell];

        PrepareBoard(allXCell, allZCell, tmpBoard, cells);

        CreateFloor(boardWidth, boardLength);

        CreateWalls(boardWidth, boardLength, wallHeight);

        //create indestructible cubes
        CreateIndestructibleCubes(cubeSize, count_x, count_z, tmpBoard, cells);

        int[] startAreaX = new int[] { 0, 1, allXCell - 1, allXCell - 2 };
        int[] startAreaZ = new int[] { 0, 1, allZCell - 1, allZCell - 2 };

        //reserve start area
        ReserveStartArea(tmpBoard, startAreaX, startAreaZ);

        availableExits = CreateDestructibleCubes(cubeSize, allXCell, allZCell, tmpBoard, boardWidth, boardLength, startPositionX, startPositionZ, cells);

        //create exit position
        CreateExit(availableExits, cubeWidth, cells, positionConverter);

        return new Board(cells, new Vector2(allXCell, allZCell));
    }

    private void CreateFloor(float boardWidth, float boardLength)
    {
        TailTexture(floor, boardWidth / FLOOR_TEXTURE_SIZE, boardLength / FLOOR_TEXTURE_SIZE);
        floor.transform.localScale = new Vector3(boardWidth / 10, 1f, boardLength / 10);
        CreateGameObject(boardWidth / 2, 0, boardLength / 2, floor, "Floor");
    }

    private void CreateWalls(float boardWidth, float boardLength, float wallHeight)
    {
        TailTexture(wallA, boardWidth / WALL_TEXTURE_WIDTH, 1.0f);
        wallA.transform.localScale = new Vector3(boardWidth + WALL_THICKNESS, wallHeight, WALL_THICKNESS);
        CreateGameObject(boardWidth / 2, wallHeight / 2, boardLength + WALL_THICKNESS / 2, wallA, "Wall1");
        CreateGameObject(boardWidth / 2, wallHeight / 2, -(WALL_THICKNESS / 2), wallA, "Wall2");

        TailTexture(wallB, boardLength / WALL_TEXTURE_WIDTH, 1.0f);
        wallB.transform.localScale = new Vector3(WALL_THICKNESS, wallHeight, boardLength + WALL_THICKNESS);
        CreateGameObject(-(WALL_THICKNESS / 2), wallHeight / 2, boardLength / 2, wallB, "Wall3");
        CreateGameObject(boardWidth + WALL_THICKNESS / 2, wallHeight / 2, boardLength / 2, wallB, "Wall4");
    }

    private void CreateIndestructibleCubes(Vector3 cubeSize, int count_x, int count_z, CellType[,] board, GameCell[,] cells)
    {
        const int offset = 3;
        for (int i = 0; i < count_x; i++)
        {
            for (int j = 0; j < count_z; j++)
            {
                int x = 2 * i + 1;
                int z = 2 * j + 1;
                board[x, z] = CellType.INDESTRUCTIBLE;
                IndesctructibleCubeObject cube = CreateGameObject((i * 4 + offset) * cubeSize.x / 2, cubeSize.y / 2, (j * 4 + offset) * cubeSize.z / 2, indestructibleCube, "IndestructibleCube");
                cells[x, z].block = cube;
            }
        }
    }

    private ArrayList CreateDestructibleCubes(Vector3 cubeSize, int allXCell, int allZCell, CellType[,] board, float boardWidth, float boardLength, float startPositionX, float startPositionZ, GameCell[,] cells)
    {
        ArrayList availableExits = new ArrayList();

        // create destructible cubes
        for (int x = 0; x < allXCell; x++)
        {
            for (int z = 0; z < allZCell; z++)
            {
                // if position is empty && random
                if (board[x, z] == CellType.EMPTY && rnd.Next(0, 2) % 2 == 0)
                {
                    float posX = (x * 2 + 1) * cubeSize.x / 2;
                    float posZ = (z * 2 + 1) * cubeSize.z / 2;
                    board[x, z] = CellType.DESTRUCTIBLE;
                    DesctructibleCubeObject cube = CreateGameObject(posX, cubeSize.y / 2, posZ, destructibleCube, "DestructibleCube");
                    ApplyRandomRotation(cube.gameObject);
                    cells[x, z].block = cube;

                    // avoid placing exit in rows near walls
                    if (posX < cubeSize.x * 2 || posZ < cubeSize.z * 2 || posX > boardWidth - cubeSize.x * 2 || posZ > boardLength - cubeSize.z * 2)
                    {
                        continue;
                    }
                    if (CountDistance(new Vector2(posX, posZ), new Vector2(startPositionX, startPositionZ)) > MIN_EXIT_DISTANCE * boardWidth)
                    {
                        availableExits.Add(new Vector2(posX, posZ));
                    }
                }
            }
        }

        return availableExits;
    }

    private void CreateExit(ArrayList availableExits, float cubeWidth, GameCell[,] cells, PositionConverter positionConverter)
    {
        int index = new System.Random().Next(0, availableExits.Count);
        Vector2 exitPostion = (Vector2)availableExits[index];
        mazeExit.transform.localScale = new Vector3(cubeWidth / 10, 1f, cubeWidth / 10);
        Exit exit = CreateGameObject(exitPostion.x, 0.01f, exitPostion.y, mazeExit, "Exit");
        Vector2 position = positionConverter.ConvertScenePositionToBoard(exit.transform.localPosition);
        cells[(int)position.x, (int)position.y].finding = exit;
    }

    private T CreateGameObject<T>(float x, float y, float z, T prefab, string name) where T : Component
    {
        T newGameObject = Instantiate(prefab) as T;
        newGameObject.name = name + " " + x + ", " + z;
        newGameObject.transform.parent = transform;
        newGameObject.transform.localPosition = new Vector3(x, y, z);
        return newGameObject;
    }

    private float CountDistance(Vector2 p1, Vector2 p2)
    {
        return Mathf.Sqrt(Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.y - p2.y, 2));
    }

    private static void PrepareBoard(int allXCell, int allZCell, CellType[,] board, GameCell[,] cells)
    {
        for (int x = 0; x < allXCell; x++)
        {
            for (int z = 0; z < allZCell; z++)
            {
                board[x, z] = CellType.EMPTY;
                cells[x, z] = new GameCell(new Vector2(x, z));
            }
        }
    }

    private static void ReserveStartArea(CellType[,] board, int[] startAreaX, int[] startAreaZ)
    {
        foreach (int x in startAreaX)
        {
            foreach (int z in startAreaZ)
            {
                board[x, z] = CellType.BOMB;
            }
        }
    }

    /* TEXTURING METHODS */

    private void TailTexture(Component component, float tailingXValue, float tailingYValue)
    {
        GameObject gObject = component.gameObject;
        MeshRenderer[] renderers = gObject.GetComponentsInChildren<MeshRenderer>(true);

        foreach (var renderer in renderers)
        {
            renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(tailingXValue, tailingYValue));
            renderer.sharedMaterial.SetTextureScale("_BumpMap", new Vector2(tailingXValue, tailingYValue));
        }
    }

    private void ApplyRandomRotation(GameObject gObject)
    {
        gObject.transform.Rotate(Vector3.up, ROTATIONS[rnd.Next(ROTATIONS.Length)]);
    }
}
