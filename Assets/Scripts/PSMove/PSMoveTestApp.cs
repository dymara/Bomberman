using UnityEngine;
using System.Collections;

public class PSMoveTestApp : MonoBehaviour 
{
    void Start () 
    {

    }

    void Update () 
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}