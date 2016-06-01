using UnityEngine;
using Assets.Scripts.Util;
using System;

namespace Assets.Scripts.Model
{
    public abstract class AbstractFinding : StaticGameObject
    {
        private const float PICKUP_SOUND_VOLUME = 0.4f;

        public GameObject flash;

        public AudioClip pickUpSound;

        private PositionConverter positionConverter;

        private bool pickedUp = false;

        public override void OnExplode()
        {
            Destroy(this.gameObject);
        }

        public void PickUp(Player player)
        {
            if (this.gameObject != null && !pickedUp)
            {
                pickedUp = true;
                PlayFlashEffect();
                PlaySoundEffect();
                Destroy(this.gameObject);

                PowerUp(player);

                GameManager.instance.OnFindingPickedUp();
                Debug.Log(DateTime.Now + " Finding picked up!");
            }
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

        protected abstract void PowerUp(Player player);
    }

}
