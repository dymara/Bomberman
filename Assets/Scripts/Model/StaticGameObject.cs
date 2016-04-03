using UnityEngine;

namespace Assets.Scripts.Model
{
    public abstract class StaticGameObject : MonoBehaviour, Explodable
    {
        public abstract void OnExplode();
    }
}
