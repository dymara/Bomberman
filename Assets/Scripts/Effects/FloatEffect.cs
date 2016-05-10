using UnityEngine;
using System;

namespace Assets.Scripts.Effects
{
    public class FloatEffect : MonoBehaviour
    {
        private float speed;

        private float floatingDistance;

        private float diff = 0f;

        private bool up = true;

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void SetDistance(float distance)
        {
            this.floatingDistance = distance;
        }

        void Update()
        {
            float oldPosition = transform.position.y;
            if (up)
            {
                transform.Translate(Vector3.up  * speed * Time.deltaTime);
                diff += transform.position.y - oldPosition;
                if (Math.Abs(diff) > floatingDistance)
                {
                    up = false;
                    diff = 0;
                }
            }
            else
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                diff += oldPosition - transform.position.y;
                if (Math.Abs(diff) > floatingDistance)
                {
                    up = true;
                    diff = 0;
                }
            }
            
        }
    }
}
