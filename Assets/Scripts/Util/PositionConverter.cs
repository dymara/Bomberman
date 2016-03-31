using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class PositionConverter
    {

        private float cellSize;

        public PositionConverter(float cellSize)
        {
            this.cellSize = cellSize;
        }

        public Vector2 ConvertScenePositionToBoard(Vector3 position)
        {
            int x = (int)(position.x / cellSize);
            int y = (int)(position.z / cellSize);

            return new Vector2(x, y);
        }

        /**Return top left corner of cell.(minX i minZ)*/
        public Vector3 ConvertScenePositionToBoard(Vector2 position)
        {
            float x = position.x * cellSize;
            float z = position.y * cellSize;

            return new Vector3(x, 0.0f, z);
        }
    }
}
