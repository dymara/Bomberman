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

        protected bool isHumanPlayer()
        {
            return tag.Equals(Constants.HUMAN_PLAYER_TAG);
        }

        private void OnLivesChanged()
        {
            if (isHumanPlayer())
            {
                GameManager.instance.OnPlayerLivesChanged(remainingLives);
            }
        }

        public override void OnExplode()
        {
            // TODO - change to proper implementation
            Debug.Log(DateTime.Now + " Player expoloded!");
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
            postionLisener.RemovePlayerFromCurrentCell();
            positionManager.RemovePlayer(this);
            Destroy(this.gameObject);
        }
    }
}
