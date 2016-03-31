using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class DesctructibleCubeObject : AbstractCubeObject
    {
        public override void OnExplode()
        {
        }

        public static explicit operator DesctructibleCubeObject(GameObject v)
        {
            throw new NotImplementedException();
        }
    }
}
