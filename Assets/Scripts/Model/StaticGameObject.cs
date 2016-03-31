using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public abstract class StaticGameObject : MonoBehaviour, Explodable
    {
        public abstract void OnExplode();
    }
}
