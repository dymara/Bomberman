using UnityEngine;
using Assets.Scripts.Util;
using System;

namespace Assets.Scripts.Model
{
    public class Finding : StaticGameObject
    {
        public GameObject flash;

        private PositionConverter positionConverter;

        public override void OnExplode()
        {
            Destroy(this.gameObject);
        }

        public void pickUp(Player player)
        {
            PlayFlashEffect();
            Destroy(this.gameObject);
            player.bombs++;
            Debug.Log(DateTime.Now + " Finding picked up");
        }

        private void PlayFlashEffect()
        {
            Vector3 explosionPosition = gameObject.transform.position;
            GameObject explosionObject = Instantiate(flash, explosionPosition, Quaternion.identity) as GameObject;
            ParticleSystem explosion = explosionObject.GetComponent<ParticleSystem>();
            explosion.Play();
            Destroy(explosionObject, explosion.duration);
        }
    }
}
