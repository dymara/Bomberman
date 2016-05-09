using UnityEngine;
using System;

namespace Assets.Scripts.Model
{
    public class Exit : Finding
    {
        public override void OnExplode()
        {
            // TODO - change to proper implementation
            Debug.Log(DateTime.Now + " EXIT DESTROYED!!!");
            AISpawner spawner = GameObject.FindGameObjectsWithTag(Constants.AI_SPAWNER_NAME)[0].GetComponent<AISpawner>();
            spawner.SpawnEnemiesAfterExitExploded(gameObject.transform.position);
        }
    }
}
