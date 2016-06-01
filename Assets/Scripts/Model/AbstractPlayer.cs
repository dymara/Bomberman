using Assets.Scripts.Postion;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public abstract class AbstractPlayer : DynamicGameObject
    {
        protected int _remainingLives;
        public int remainingLives {
            get { return _remainingLives; }
            set { 
                    _remainingLives = value; OnLivesChanged(value);
            }
        }

        public PostionListener postionLisener { set; get; }

        public PlayerPositionManager positionManager { set; get; }

        public AbstractPlayer(int lives)
        {
            _remainingLives = lives;
        }

        protected abstract void OnLivesChanged(int newValue);

        public override void OnExplode()
        {
            remainingLives--;
            Kill();
        }

        protected abstract void Kill();

        void Update()
        {
            if (postionLisener != null)
            {
                postionLisener.OnPostionChanged(transform.position);
            }
        }

    }
}
