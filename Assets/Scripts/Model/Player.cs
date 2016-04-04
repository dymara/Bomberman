using System;

namespace Assets.Scripts.Model
{
    public class Player : AbstractPlayer
    {
        private long score;

        public Player(String name, int lives) : base(lives)
        {
            this.name = name;
            this.score = 0;
        }

        public long GetScore()
        {
            return score;
        }

        public void AddToScoreLong(long bonus)
        {
            score += bonus;
        }
    }
}
