﻿using UnityEngine;
using System;

namespace Assets.Scripts.Model
{
    public class Exit : AbstractFinding
    {
        public override void OnExplode()
        {
            Debug.Log(DateTime.Now + " EXIT DESTROYED!!!");
            AISpawner spawner = GameObject.Find(Constants.AI_SPAWNER_NAME).GetComponent<AISpawner>();
            spawner.SpawnEnemiesAfterExitExploded(gameObject.transform.position);
        }

        protected override void PowerUp(Player player)
        {
            // Nothing to do here
        }
    }
}
