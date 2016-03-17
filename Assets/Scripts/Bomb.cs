using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public float detonateDelay;
	public GameObject[] explosions;

	public void PlaceBomb() {
		StartCoroutine (Detonate());
	}

	IEnumerator Detonate() {
		yield return new WaitForSeconds(detonateDelay);

		int explosionIndex = Random.Range (0, explosions.Length);
		GameObject explosionObject = Instantiate(explosions[explosionIndex], this.transform.position, Quaternion.identity) as GameObject;

		ParticleSystem explosion = explosionObject.GetComponent<ParticleSystem> ();
		explosion.Play ();
		DestroyBoxes ();
		Destroy (this.gameObject);
		Destroy (explosionObject, explosion.duration);
	}

	private void DestroyBoxes() {
		RaycastHit[] hitBoxes = Physics.SphereCastAll (this.transform.position, 4f, Vector3.down);
		if (hitBoxes != null) {
			foreach (RaycastHit hit in hitBoxes) {
				GameObject hitGameObject = hit.collider.gameObject;
				if (hitGameObject.tag.Equals("Destructible")) {
					Destroy (hitGameObject);
				}
			}
		}
	}
}
