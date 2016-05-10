using UnityEngine;
using Assets.Scripts.Util;
using System;

namespace Assets.Scripts.Model
{
    public class Finding : StaticGameObject
    {
        private const float PICKUP_SOUND_VOLUME = 0.4f;

        public GameObject flash;

        public AudioClip pickUpSound;

        private PositionConverter positionConverter;

        public override void OnExplode()
        {
            Destroy(this.gameObject);
        }

        public void pickUp(Player player)
        {
            PlayFlashEffect();
            PlaySoundEffect();
            Destroy(this.gameObject);
            // TODO - Handle different finding types
            player.bombs++;
            player.maximumBombsCount++;
            Debug.Log(DateTime.Now + " Finding picked up!");
        }

        private void PlayFlashEffect()
        {
            Vector3 explosionPosition = gameObject.transform.position;
            GameObject explosionObject = Instantiate(flash, explosionPosition, Quaternion.identity) as GameObject;
            ParticleSystem explosion = explosionObject.GetComponent<ParticleSystem>();
            explosion.Play();
            Destroy(explosionObject, explosion.duration);
        }

        private void PlaySoundEffect()
        {
            AudioSource.PlayClipAtPoint(pickUpSound, this.transform.position, PICKUP_SOUND_VOLUME);
        }
    }
}
