using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Monster : AbstractPlayer
    {
        public Monster() : base(1)
        {
        }

        protected override void Kill()
        {
            if (remainingLives == 0)
            {
                Debug.Log(DateTime.Now + " Player killed an enemy!");
                GameManager.instance.OnEnemyKilled();
                postionLisener.RemovePlayerFromCurrentCell();
                positionManager.RemovePlayer(this);
                Destroy(this.gameObject);
            }
        }

        protected override void OnLivesChanged(int newValue)
        {
            // Monster enemies do not have to request GameManager to update HUD
        }

        void OnTriggerEnter(Collider other)
        {
            String tag = other.gameObject.tag;
            if (tag.Equals(Constants.PLAYER_TAG))
            {
                other.gameObject.GetComponent<Player>().TriggerKill();
            }
            else if (tag.Equals(Constants.BOMB_TAG) || tag.Equals(Constants.MONSTER_TAG))
            {
                gameObject.GetComponent<AIBehavior>().MoveBack();
            }
        }
    }
}
