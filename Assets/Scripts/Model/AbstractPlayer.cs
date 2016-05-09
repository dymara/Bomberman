using System;
using Assets.Scripts.Postion;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public abstract class AbstractPlayer : DynamicGameObject
    {
        public int remainingLives { set; get; }

        public PostionListener postionLisener { set; get; }

        public PlayerPositionManager positionManager { set; get; }

        public AbstractPlayer(int lives)
        {
            remainingLives = lives;
        }

        public override void OnExplode()
        {
            // TODO - change to proper implementation
            Debug.Log(DateTime.Now + " Player expoloded");
            remainingLives--;
            if (remainingLives == 0)
            {
                EndGame();
            }
            else
            {
                Kill();
            }
        }

        void Update()
        {
            if (postionLisener != null)
            {
                postionLisener.OnPostionChanged(transform.position);
            }
        }

        protected virtual void Kill()
        {

        }

        protected virtual void EndGame()
        {
            positionManager.RemovePlayer(this);
            Destroy(this.gameObject);
        }
    }
}
