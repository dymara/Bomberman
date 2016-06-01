using UnityEngine;
using Assets.Scripts.Model;
using System.Collections.Generic;

namespace Assets.Scripts.Board
{
    public class GameCell
    {
        public AbstractCubeObject block { get; set; }

        public AbstractFinding finding { get; set; }

        public Bomb bomb { get; set; }

        public GameObject highlight { get; set; }

        private Vector2 coordinates;

        private HashSet<AbstractPlayer> players;

        private int monstersCount;

        public GameCell(Vector2 coordinates)
        {
            this.coordinates = coordinates;
            players = new HashSet<AbstractPlayer>();
            monstersCount = 0;
        }

        public Vector2 GetCoordinates()
        {
            return coordinates;
        }
        
        public float EuclideanDistanceTo(Vector2 position) 
        {
            return Mathf.Sqrt(Mathf.Pow(coordinates.x - position.x, 2) + Mathf.Pow(coordinates.y - position.y, 2));
        }
        
        public int ManhattanDistanceTo(Vector2 position) {
            return (int)(Mathf.Abs(coordinates.x - position.x) + Mathf.Abs(coordinates.y - position.y));
        }
        
        public bool IsEmpty() 
        {
            return this.block == null && this.finding == null && this.bomb == null;    
        }

        public void Explode()
        {
            ExplodeFinding();
            ExplodeBlock();
            ExplodeBomb();
            foreach (AbstractPlayer player in new HashSet<AbstractPlayer>(players))
            {
                player.OnExplode();
            }
        }

        private void ExplodeBlock()
        {
            if (block != null)
            {
                block.OnExplode();
                block = null;
            }
        }

        private void ExplodeFinding()
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

        private void ExplodeBomb()
        {
            if (bomb != null)
            {
                bomb.OnExplode();
                bomb = null;
            }
        }

        public bool AddPlayer(AbstractPlayer player)
        {
            if (player is Monster)
            {
                monstersCount++;
            }
            return players.Add(player);
        }

        public bool RemovePlayer(AbstractPlayer player)
        {
            if (player is Monster)
            {
                monstersCount--;
            }
            return players.Remove(player);
        }

        public bool isThereAMonster()
        {
            return monstersCount > 0;
        }
    }
}
