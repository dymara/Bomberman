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

        /** Returns top left corner of cell (minX and minZ). */
        public Vector3 ConvertBoardPositionToScene(Vector2 position, bool center=false)
        {
            float x = position.x * cellSize;
            float z = position.y * cellSize;

            return center ? new Vector3(x + cellSize / 2, 0.0f, z + cellSize / 2) : new Vector3(x, 0.0f, z);
        }
    }
}
