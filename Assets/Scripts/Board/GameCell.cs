using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Board
{
    public class GameCell
    {
        private Vector2 coordinates;
        public AbstractCubeObject block {get; set; }
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

        }
    }
}
