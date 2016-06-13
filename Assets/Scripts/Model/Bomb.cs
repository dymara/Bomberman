using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Bomb : StaticGameObject
    {
        private const float EXPLOSION_SOUND_VOLUME = 0.4f;

        public int explosionRange { get; set; }

        public AudioClip explosionSound;

        public GameObject player { get; set; }

        private bool detonated = false;

        public override void OnExplode()
        {
            PlayExplosionSound();
            Destroy(this.gameObject);
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
