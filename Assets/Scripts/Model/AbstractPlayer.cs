using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model
{
    public class AbstractPlayer : DynamicGameObject
    {
        private int remainingLives;

        public AbstractPlayer(int lives)
        {
            remainingLives = lives;
        }

        public int GetRemainingLives()
        {
            return remainingLives;
        }

        public override void OnExplode()
        {
            remainingLives--;
        }
    }
}
