using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public abstract class DynamicGameObject : MonoBehaviour, Explodable
    {
        public abstract void OnExplode();
    }
}
