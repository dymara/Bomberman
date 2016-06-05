using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Bomb : StaticGameObject
    {
        private const float EXPLOSION_SOUND_VOLUME = 0.4f;

        public int detonateDelay { get; set; }

        public int explosionRange { get; set; }

        public AudioClip explosionSound;

        public GameObject player { get; set; }

        public TextMesh textMesh;

        public TextMesh minimapTextMesh;

        private bool detonated = false;        

        public override void OnExplode()
        {
            PlayExplosionSound();
            Destroy(this.gameObject);
        }

        void Update()
        {
            this.textMesh.transform.LookAt(player.gameObject.transform);
            this.textMesh.transform.Rotate(new Vector3(0f, 180f, 0f));
        }

        public void SetCountValue(int value)
        {
            if (!HasBeenDetonated())
            {
                textMesh.text = value.ToString();
                minimapTextMesh.text = value.ToString();
            }
        }

        private void PlayExplosionSound()
        {
            AudioSource.PlayClipAtPoint(explosionSound, this.transform.position, EXPLOSION_SOUND_VOLUME);
        }

        public bool HasBeenDetonated()
        {
            return detonated;
        }

        void OnDestroy()
        {
            detonated = true;
        }
    }
}
