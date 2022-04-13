using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Core.Utils;
using Core.Controllers;

namespace Core.Physics
{
    public class CollisionBasic : MonoBehaviour
    {
        EnemyController enemyController;

        // Start is called before the first frame update
        void Start()
        {
            enemyController = gameObject.GetComponent<EnemyController>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void FixedUpdate()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (enemyController.health > 0)
            {
                try
                {
                    MultiTag tags = other.gameObject.GetComponent<MultiTag>();
                    int damage = 4;
                    if (tags.HasTag("hurtbox"))
                    {
                        enemyController.hitStun = 0;
                        enemyController.health -= damage;
                    }
                }
                catch (NullReferenceException)
                {
                    // object is dead/has been destroyed already prob
                }
            }
        }
    }
}
