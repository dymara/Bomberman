using UnityEngine;
using System;
using Assets.Scripts.Util;

namespace Assets.Scripts.Model
{
    public class Exit : Finding
    {
        public override void OnExplode()
        {
            Debug.Log(DateTime.Now + " EXIT DESTROYED!!!");
            AISpawner spawner = GameObject.FindGameObjectsWithTag("AISpawner")[0].GetComponent<AISpawner>();
            spawner.SpawnEnemiesAfterExitExploded(gameObject.transform.position);
        }
    }
}
