using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Bomb : StaticGameObject
    {
        private int range;
        private const float EXPLOSION_SOUND_VOLUME = 0.4f;
        private const string DESTRUCTIBLE_TAG = "Destructible";

        [Range(1, 10)]
        public int detonateDelay;
        [Range(1, 100)]
        public int explosionRadius;
        public GameObject[] explosions;
        public AudioClip explosionSound;

        private TextMesh textMesh;


        public Bomb(int range)
        {
            this.range = range;
        }

        public int GetRange()
        {
            return range;
        }

        public override void OnExplode()
        {
        }

        public void PlaceBomb()
        {
            textMesh = GetComponentInChildren<TextMesh>();
            StartCoroutine(Detonate());
        }

        IEnumerator Detonate()
        {
            int timeLeft = detonateDelay;
            while (timeLeft > 0)
            {
                textMesh.text = timeLeft.ToString();
                yield return new WaitForSeconds(1);
                timeLeft--;
            }

            PlayExplosionEffect();
            PlayExplosionSound();
            DestroyBoxes();
            Destroy(this.gameObject);
        }

        private void PlayExplosionEffect()
        {
            int explosionIndex = UnityEngine.Random.Range(0, explosions.Length);
            GameObject explosionObject = Instantiate(explosions[explosionIndex], this.transform.position, Quaternion.identity) as GameObject;

            ParticleSystem explosion = explosionObject.GetComponent<ParticleSystem>();
            explosion.Play();
            Destroy(explosionObject, explosion.duration);
        }

        private void PlayExplosionSound()
        {
            AudioSource.PlayClipAtPoint(explosionSound, this.transform.position, EXPLOSION_SOUND_VOLUME);
        }

        private void DestroyBoxes()
        {
            RaycastHit[] hitBoxes = Physics.SphereCastAll(this.transform.position, explosionRadius, Vector3.down);
            if (hitBoxes != null)
            {
                foreach (RaycastHit hit in hitBoxes)
                {
                    GameObject hitGameObject = hit.collider.gameObject;
                    if (hitGameObject.tag != null && hitGameObject.tag.Equals(DESTRUCTIBLE_TAG))
                    {
                        Destroy(hitGameObject);
                    }
                }
            }
        }

    }
}
