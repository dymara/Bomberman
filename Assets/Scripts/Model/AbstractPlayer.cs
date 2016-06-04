using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Position;

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

        public BoardPostionListener postionLisener { set; get; }

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

        protected void Update()
        {
            if (postionLisener != null)
            {
                postionLisener.OnPostionChanged(transform.position);
            }
        }
    }
}
