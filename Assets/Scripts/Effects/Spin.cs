using UnityEngine;

public class Spin : MonoBehaviour {

    private float speed;

    void Awake()
    {
        this.speed = GameManager.instance.GetExitSpinSpeed();
    }

    void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
