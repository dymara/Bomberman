using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour {

    public GameObject cellPrefab;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Generate(int width, int length)
    {

        Vector3 cubeSize = cellPrefab.transform.localScale;

        System.Console.WriteLine(cubeSize);

        int count_x = (int)((width - cubeSize.x) / (cubeSize.x * 2));

        int count_z = (int)((length - cubeSize.z) / (cubeSize.z * 2));


        for (int i = 0; i < count_x; i++)
        {
            for (int j = 0; j < count_z; j++)
            {
                CreateCell((i * 4 + cubeSize.x) * cubeSize.x/2, cubeSize.y/2, (j * 4 + cubeSize.z) * cubeSize.z/2);
            }
        }
    }

    private void CreateCell(float x, float y, float z)
    {
        GameObject newCell = Instantiate(cellPrefab);
            newCell.name = "Maze Cell " + x + ", " + z;
            newCell.transform.parent = transform;
            newCell.transform.localPosition =
                new Vector3(x, y, z);       
    }
}
