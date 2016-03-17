using UnityEngine;
using System.Collections;

public class BombManager : MonoBehaviour {

	public Bomb bombPrefab;

	void Update () {
		if (Input.GetKeyDown ("f")) {
			Vector3 bombPosition = this.transform.position + this.transform.forward * bombPrefab.transform.localScale.x;
			Bomb bomb = Instantiate (bombPrefab, bombPosition, Quaternion.identity) as Bomb;
			bomb.PlaceBomb ();
		}
	}
}
