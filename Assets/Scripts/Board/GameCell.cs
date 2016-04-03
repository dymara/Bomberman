using UnityEngine;
using Assets.Scripts.Model;

namespace Assets.Scripts.Board
{
    public class GameCell
    {
        private Vector2 coordinates;
        public AbstractCubeObject block { get; set; }
        public Finding finding { get; set; }
        public Bomb bomb { get; set; }

        public GameCell(Vector2 coordinates)
        {
            this.coordinates = coordinates;
        }

        public Vector2 GetCoordinates()
        {
            return coordinates;
        }

        public void Explode()
        {
            explodeFinding();
            explodeBlock();
            explodeBomb();
        }

        private void explodeBlock()
        {
            if (block != null)
            {
                block.OnExplode();
                block = null;
            }
        }

        private void explodeFinding()
        {
            // Findings should be blown up only if no block covers them at the moment of explosion
            if (finding != null && block == null)
            {
                finding.OnExplode();
                if (finding.GetType() != typeof(Exit))
                {
                    // exit should not be destroyed
                    finding = null;
                }
            }
        }

        private void explodeBomb()
        {
            if (bomb != null)
            {
                bomb.OnExplode();
                bomb = null;
            }
        }
    }
}
