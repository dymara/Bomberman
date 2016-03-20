using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour
{
    //minimal distance from start postion to exit multiply width of borad
    private const float minExitDistance = 0.7f;

    public GameObject indestructibleCube;

    public GameObject destructibleCube;

    public GameObject mazeExit;

    public float startPositionX;

    public float startPositionZ;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Generate(int width, int length)
    {
        Vector3 cubeSize = indestructibleCube.transform.localScale;

        int allXCell = (int)(width / cubeSize.x);

        int allZCell = (int)(length / cubeSize.z);

        int count_x = (allXCell - 1) / 2;

        int count_z = (allZCell - 1) / 2;

        //arraylist for places with destructible cubes for exit
        ArrayList availableExits = new ArrayList();

        //array used to create destructible cubes
        bool[,] board = new bool[allXCell, allZCell];
        for (int x = 0; x < allXCell; x++)
        {
            for (int z = 0; z < allZCell; z++)
            {
                board[x, z] = false;
            }
        }

        //create indestructible cubes
        for (int i = 0; i < count_x; i++)
        {
            for (int j = 0; j < count_z; j++)
            {
                //reserve position
                board[2 * i + 1, 2 * j + 1] = true;
                CreateGameObject((i * 4 + cubeSize.x) * cubeSize.x / 2, cubeSize.y / 2, (j * 4 + cubeSize.z) * cubeSize.z / 2, indestructibleCube, "indestructibleCube");
            }
        }

        int[] startArea = new int[] { 0, 1, allXCell - 1, allZCell - 1, allXCell - 2, allZCell - 2 };

        //reserve start area
        foreach (int x in startArea)
        {
            foreach (int z in startArea)
            {
                board[x, z] = true;
            }
        }

        System.Random rnd = new System.Random();
        //create destructible cubes
        for (int x = 0; x < allXCell; x++)
        {
            for (int z = 0; z < allZCell; z++)
            {
                //if position is empty && random
                if (!board[x, z] && rnd.Next(0, 2) % 2 == 0)
                {
                    float posX = (x * 2 + 1) * cubeSize.x / 2;
                    float posZ = (z * 2 + 1) * cubeSize.z / 2;
                    CreateGameObject(posX, cubeSize.y / 2, posZ, destructibleCube, "destructibleCube");
                    //to avoid place exit in rows near walls
                    if (posX < cubeSize.x * 2 || posZ < cubeSize.z * 2 || posX > width - cubeSize.x * 2 || posZ > length - cubeSize.z * 2)
                    {
                        continue;
                    }
                    if (countDistance(new Vector2(posX, posZ), new Vector2(startPositionX, startPositionZ)) > minExitDistance * width)
                    {
                        availableExits.Add(new Vector2(posX, posZ));
                    }
                }
            }
        }

        //create exit position
        int index = rnd.Next(0, availableExits.Count);
        Vector2 exitPostion = (Vector2)availableExits[index];
        CreateGameObject(exitPostion.x, 0.01f, exitPostion.y, mazeExit, "exit");

    }

    private void CreateGameObject(float x, float y, float z, GameObject cubePrefab, string name)
    {
        GameObject newCell = Instantiate(cubePrefab);
        newCell.name = name + " " + x + ", " + z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(x, y, z);
    }

    private float countDistance(Vector2 p1, Vector2 p2)
    {
        return Mathf.Sqrt(Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.y - p2.y, 2));
    }
}
