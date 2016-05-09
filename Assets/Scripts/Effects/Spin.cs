using UnityEngine;

public class Spin : MonoBehaviour {

    private float speed;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
