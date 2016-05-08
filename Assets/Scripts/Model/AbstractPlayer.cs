using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Postion;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public abstract class AbstractPlayer : DynamicGameObject
    {
        private int remainingLives;

        public PostionListener postionLisener { set; get; }

        public AbstractPlayer(int lives)
        {
            remainingLives = lives;
        }

        public void SetRemainingLives(int lives)
        {
            remainingLives = lives;
        }

        public int GetRemainingLives()
        {
            return remainingLives;
        }

        public override void OnExplode()
        {
            Debug.Log(DateTime.Now + " Player expoloded");
            remainingLives--;
            if(remainingLives == 0)
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
            Destroy(this.gameObject);
        }
    }
}
