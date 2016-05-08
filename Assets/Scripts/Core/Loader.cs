using UnityEngine;

public class Loader : MonoBehaviour {

    public GameManager gameManager;

    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
    }

}
