using UnityEngine;
using System.Collections;
using Assets.Scripts.Board;
using Assets.Scripts.Model;
using Assets.Scripts.Util;

public enum CellType { EMPTY, DESTRUCTIBLE, INDESTRUCTIBLE, PLAYER, BOMB };

public class Maze : MonoBehaviour
{

    //minimal distance from start postion to exit multiply width of borad
    private const float minExitDistance = 0.7f;

    public IndesctructibleCubeObject indestructibleCube;

    public DesctructibleCubeObject destructibleCube;

    public Exit mazeExit;

    public Component wall;

    public Component floor;

    private const float wallThickness = 0.5f;

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

        int[] startAreaX = new int[] { 0, 1, allXCell - 1, allXCell - 2};

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
        floor.transform.localScale = new Vector3(boardWidth / 10, 1f, boardLength / 10);
        CreateGameObject(boardWidth / 2, 0, boardLength / 2, floor, "Floor");
    }

    private void CreateWalls(float boardWidth, float boardLength, float wallHeight)
    {
        wall.transform.localScale = new Vector3(boardWidth + wallThickness, wallHeight, wallThickness);
        CreateGameObject(boardWidth / 2, wallHeight / 2, boardLength + wallThickness / 2, wall, "Wall1");
        CreateGameObject(boardWidth / 2, wallHeight / 2, -(wallThickness / 2), wall, "Wall2");
        wall.transform.localScale = new Vector3(wallThickness, wallHeight, boardLength + wallThickness);
        CreateGameObject(-(wallThickness / 2), wallHeight / 2, boardLength / 2, wall, "Wall3");
        CreateGameObject(boardWidth + wallThickness / 2, wallHeight / 2, boardLength / 2, wall, "Wall4");
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
        System.Random rnd = new System.Random();
        //create destructible cubes
        for (int x = 0; x < allXCell; x++)
        {
            for (int z = 0; z < allZCell; z++)
            {
                //if position is empty && random
                if (board[x, z] == CellType.EMPTY && rnd.Next(0, 2) % 2 == 0)
                {
                    float posX = (x * 2 + 1) * cubeSize.x / 2;
                    float posZ = (z * 2 + 1) * cubeSize.z / 2;
                    board[x, z] = CellType.DESTRUCTIBLE;
                    DesctructibleCubeObject cube = CreateGameObject(posX, cubeSize.y / 2, posZ, destructibleCube, "DestructibleCube");
                    cells[x, z].block = cube;

                    //to avoid place exit in rows near walls
                    if (posX < cubeSize.x * 2 || posZ < cubeSize.z * 2 || posX > boardWidth - cubeSize.x * 2 || posZ > boardLength - cubeSize.z * 2)
                    {
                        continue;
                    }
                    if (CountDistance(new Vector2(posX, posZ), new Vector2(startPositionX, startPositionZ)) > minExitDistance * boardWidth)
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
        Exit exit = CreateGameObject(exitPostion.x, 0.01f, exitPostion.y, mazeExit, "exit");
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
                //tmpBoard[x, z] = false;
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
}
