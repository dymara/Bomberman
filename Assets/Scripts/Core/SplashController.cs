using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour {

    [Range(0, 120)]
    public float duration;

	void Start () {
        StartCoroutine(Timer());
	}

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        GameManager.instance.SwitchGameState(GameState.MAIN_MENU);
    }
}
