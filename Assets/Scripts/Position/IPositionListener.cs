using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Position
{
    interface IPositionListener
    {
        void OnPostionChanged(Vector3 newPostion);
    }
}
