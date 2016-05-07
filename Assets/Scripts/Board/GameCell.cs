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

        public GameCell(Vector2 coordinates)
        {
            this.coordinates = coordinates;
            players = new HashSet<AbstractPlayer>();
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
            foreach(Player player in players){
                player.OnExplode();
            }
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
