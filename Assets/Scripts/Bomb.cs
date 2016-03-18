using UnityEngine;
using System.Collections;
public class Bomb : MonoBehaviour {

    [Range(1, 10)]
	public int detonateDelay;
	public GameObject[] explosions;
    public AudioClip explosionSound;

    private TextMesh textMesh;

	public void PlaceBomb() {
        textMesh = GetComponentInChildren<TextMesh>();
        StartCoroutine(Detonate());
	}

	IEnumerator Detonate() {
        int timeLeft = detonateDelay;
        while (timeLeft > 0)
        {
            textMesh.text = timeLeft.ToString();
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

		int explosionIndex = Random.Range (0, explosions.Length);
		GameObject explosionObject = Instantiate(explosions[explosionIndex], this.transform.position, Quaternion.identity) as GameObject;

		ParticleSystem explosion = explosionObject.GetComponent<ParticleSystem> ();
        AudioSource.PlayClipAtPoint(explosionSound, this.transform.position, 0.4f);
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
