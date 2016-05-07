using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Bomb : StaticGameObject
    {

        private const float EXPLOSION_SOUND_VOLUME = 0.4f;

        [Range(1, 10)]
        public int detonateDelay;

        [Range(1, 10)]
        public int explosionRange;

        public AudioClip explosionSound;

        private TextMesh textMesh;

        private bool detonated = false;

        public GameObject player { get; set; }

        public override void OnExplode()
        {
            PlayExplosionSound();
            Destroy(this.gameObject);
        }

        void Awake()
        {
            textMesh = GetComponentInChildren<TextMesh>();
        }

        void Update()
        {
            this.textMesh.transform.LookAt(player.gameObject.transform);
            this.textMesh.transform.Rotate(new Vector3(0f, 180f, 0f));
        }

        public void SetCountValue(int value)
        {
            if (!hasBeenDetonated())
            {
                textMesh.text = value.ToString();
            }
        }

        private void PlayExplosionSound()
        {
            AudioSource.PlayClipAtPoint(explosionSound, this.transform.position, EXPLOSION_SOUND_VOLUME);
        }

        public bool hasBeenDetonated()
        {
            return detonated;
        }

        void OnDestroy()
        {
            detonated = true;
        }
    }
}
