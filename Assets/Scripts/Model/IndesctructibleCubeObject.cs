using UnityEngine;
using System;

namespace Assets.Scripts.Model
{
    public class IndesctructibleCubeObject : AbstractCubeObject
    {
        public override void OnExplode()
        {
        }

        public static explicit operator IndesctructibleCubeObject(GameObject v)
        {
            throw new NotImplementedException();
        }
    }
}
