using UnityEngine;
using Assets.Scripts.Model;
using System.Collections.Generic;

namespace Assets.Scripts.Board
{
    public class GameCell
    {
        private Vector2 coordinates;
        public AbstractCubeObject block { get; set; }
        public Finding finding { get; set; }
        public Bomb bomb { get; set; }
        private HashSet<AbstractPlayer> players;
        public GameObject highlight { get; set; }

        public GameCell(Vector2 coordinates)
        {
            this.coordinates = coordinates;
            players = new HashSet<AbstractPlayer>();
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
            foreach(AbstractPlayer player in players){
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
            return players.Add(player);
        }


        public bool RemovePlayer(AbstractPlayer player)
        {
            return players.Remove(player);
        }
    }
}
