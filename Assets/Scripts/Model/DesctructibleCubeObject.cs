using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class DesctructibleCubeObject : AbstractCubeObject
    {
        public override void OnExplode()
        {
            GameManager.instance.OnBlockDestroyed();
            Destroy(this.gameObject);
        }

        public static explicit operator DesctructibleCubeObject(GameObject v)
        {
            throw new NotImplementedException();
        }
    }
}
