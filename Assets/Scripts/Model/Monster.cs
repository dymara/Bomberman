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
    }
}
