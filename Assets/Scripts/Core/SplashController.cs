using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour {

	void Start () {
        StartCoroutine(Timer());
	}

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(GameManager.instance.GetSplashDuration());
        GameManager.instance.SwitchGameState(GameState.MAIN_MENU);
    }
}
