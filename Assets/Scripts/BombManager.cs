using UnityEngine;
using Assets.Scripts.Model;

public class BombManager : MonoBehaviour {

    private const float MESSAGE_DISPLAY_TIME = 1.5f;

    private const string WALL_TAG = "Wall";

    private const string DESTRUCTIBLE_TAG = "Destructible";

    private const string INDESTRUCTIBLE_TAG = "Indestructible";

    private const string BOMB_TAG = "Bomb";

    public GameObject sampleCube;

    public Bomb bombPrefab;

    private bool displayMessage = false;

    private GUILabelFade message;

    void Update() {
		if (Input.GetKeyDown("f")) {
            Vector3 bombPosition = this.transform.position + this.transform.forward;
            Vector3 correctedBombPosition = CorrectBombPosition(bombPosition);
            if (correctedBombPosition != Vector3.zero) {
                Bomb bomb = Instantiate(bombPrefab, CorrectBombPosition(bombPosition), Quaternion.identity) as Bomb;
                bomb.PlaceBomb();
            } else  {
                Rect rect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 200f, 200f);
                message = new GUILabelFade(3f, "You can't place a bomb here!", rect, 40, false);
                displayMessage = true;
            }
		}
    }

    Vector3 CorrectBombPosition(Vector3 initialPosition)
    {
        if (initialPosition.y < 0)
        {
            return Vector3.zero;
        }

        int blockSize = (int)(sampleCube.transform.localScale.x);
        int xPosition = (int)(initialPosition.x / blockSize) * blockSize;
        int zPosition = (int)(initialPosition.z / blockSize) * blockSize;
        Vector3 correctedPositon = new Vector3(xPosition + blockSize / 2.0f, 1.0f, zPosition + blockSize / 2.0f);

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, blockSize / 2 - 0.1f);
        foreach (Collider collider in colliders)
        {
            GameObject gameObject = collider.gameObject;
            if (gameObject.tag.Equals(WALL_TAG) || gameObject.tag.Equals(DESTRUCTIBLE_TAG) || gameObject.tag.Equals(INDESTRUCTIBLE_TAG) || gameObject.tag.Equals(BOMB_TAG))
            {
                return Vector3.zero;
            }
        }
        
        return correctedPositon;
    }

    void OnGUI()
    {
        if (displayMessage)
        {
            message.Render(Color.white);
        }
    }
}
