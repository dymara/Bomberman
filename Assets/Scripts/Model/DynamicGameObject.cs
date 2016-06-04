using UnityEngine;

namespace Assets.Scripts.Model
{
    public abstract class DynamicGameObject : MonoBehaviour, Explodable
    {
        public abstract void OnExplode();
    }
}
