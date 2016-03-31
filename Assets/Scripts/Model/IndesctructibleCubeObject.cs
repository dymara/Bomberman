using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
