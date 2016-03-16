using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour
{

    public GameObject indestructibleCube;

    public GameObject destructibleCube;

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

        bool[,] board = new bool[allXCell, allZCell];
        for (int x = 0; x < allXCell; x++)
        {
            for (int z = 0; z < allZCell; z++)
            {
                board[x, z] = false;
            }
        }

        for (int i = 0; i < count_x; i++)
        {
            for (int j = 0; j < count_z; j++)
            {
                board[2 * i + 1, 2 * j + 1] = true;
                CreateIndestructibleCube((i * 4 + cubeSize.x) * cubeSize.x / 2, cubeSize.y / 2, (j * 4 + cubeSize.z) * cubeSize.z / 2);
            }
        }

        int[] startArea = new int[] { 0, 1, allXCell - 1, allZCell - 1, allXCell - 2, allZCell - 2 };

        foreach (int x in startArea)
        {
            foreach (int z in startArea)
            {
                board[x, z] = true;
            }
        }

        System.Random rnd = new System.Random();

        for (int x = 0; x < allXCell; x++)
        {
            for (int z = 0; z < allZCell; z++)
            {
                if (!board[x, z] && rnd.Next(0, 2) % 2 == 0)
                {
                    CreateDestructibleCube((x * 2 + 1) * cubeSize.x / 2, cubeSize.y / 2, (z * 2 + 1) * cubeSize.z / 2);
                }
            }
        }
    }

    private void CreateIndestructibleCube(float x, float y, float z)
    {
        GameObject newCell = Instantiate(indestructibleCube);
        newCell.name = "Maze Cell " + x + ", " + z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =
            new Vector3(x, y, z);
    }

    private void CreateDestructibleCube(float x, float y, float z)
    {
        GameObject newCell = Instantiate(destructibleCube);
        newCell.name = "Maze Cell " + x + ", " + z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =
            new Vector3(x, y, z);
    }
}
