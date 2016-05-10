using System;
using Assets.Scripts.Postion;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public abstract class AbstractPlayer : DynamicGameObject
    {
        private int _remainingLives;
        public int remainingLives {
            get { return _remainingLives; }
            set { _remainingLives = value; OnLivesChanged(); }
        }

        public PostionListener postionLisener { set; get; }

        public PlayerPositionManager positionManager { set; get; }

        public AbstractPlayer(int lives)
        {
            _remainingLives = lives;
        }

        protected bool IsHumanPlayer()
        {
            return tag.Equals(Constants.HUMAN_PLAYER_TAG);
        }

        private void OnLivesChanged()
        {
            if (IsHumanPlayer())
            {
                GameManager.instance.OnPlayerLivesChanged(remainingLives);
            }
        }

        public override void OnExplode()
        {
            remainingLives--;
            if (IsHumanPlayer())
            {
                Debug.Log(DateTime.Now + " Player has blown himself up!");
            }
            else
            {
                Debug.Log(DateTime.Now + " Player killed an enemy!");
            }
            Kill();
        }

        protected virtual void Kill()
        {
            if (remainingLives == 0)
            {
                GameManager.instance.OnEnemyKilled();
                postionLisener.RemovePlayerFromCurrentCell();
                positionManager.RemovePlayer(this);
                Destroy(this.gameObject);
            }
        }

        void Update()
        {
            if (postionLisener != null)
            {
                postionLisener.OnPostionChanged(transform.position);
            }
        }

    }
}
