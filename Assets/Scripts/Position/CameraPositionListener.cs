using UnityEngine;

namespace Assets.Scripts.Position
{
    public class CameraPositionListener : IPositionListener
    {
        private static float X_OFFSET = 13.47f;

        private static float Z_OFFSET = 7.72f;

        private Camera minimapCamera;

        private float mazeWidth;

        private float mazeLength;

        public CameraPositionListener(Camera minimapCamera, float mazeWidth, float mazeLength)
        {
            this.minimapCamera = minimapCamera;
            this.mazeLength = mazeLength;
            this.mazeWidth = mazeWidth;
        }

        public void OnPostionChanged(Vector3 newPostion)
        {
            Vector3 cameraPostion = minimapCamera.transform.position;
            float newX = newPostion.x;
            float newZ = newPostion.z;
            if (newX < X_OFFSET)
            {
                newX = X_OFFSET;
            } else if(newX > mazeWidth - X_OFFSET)
            {
                newX = mazeWidth - X_OFFSET;
            }
            if (newZ < Z_OFFSET)
            {
                newZ = Z_OFFSET;
            } else if (newZ > mazeLength - Z_OFFSET)
            {
                newZ = mazeLength - Z_OFFSET;
            }
            cameraPostion.x = newX;
            cameraPostion.z = newZ;
            minimapCamera.transform.position = cameraPostion;
        }
    }
}
