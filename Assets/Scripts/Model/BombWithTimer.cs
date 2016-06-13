using UnityEngine;

namespace Assets.Scripts.Model
{
    public class BombWithTimer : Bomb
    {

        public int detonateDelay { get; set; }

        public TextMesh textMesh;

        public TextMesh minimapTextMesh;
   

        void Update()
        {
            this.textMesh.transform.LookAt(player.gameObject.transform);
            this.textMesh.transform.Rotate(new Vector3(0f, 180f, 0f));
        }

        public void SetCountValue(int value)
        {
            if (!HasBeenDetonated())
            {
                textMesh.text = value.ToString();
                minimapTextMesh.text = value.ToString();
            }
        }
    }
}
