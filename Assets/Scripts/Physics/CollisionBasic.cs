using System;
using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using Core.Items.Weapons;
using Core.Utils;
using Core.Utils.Stats;
using UnityEngine;

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
            if (other.GetType() != typeof(BoxCollider2D))
            {
                if (enemyController.health > 0)
                {
                    try
                    {
                        MultiTag tags = other.gameObject.GetComponent<MultiTag>();
                        if (tags.HasTag("hurtbox"))
                        {
                            StatMap statMap = other.gameObject.GetComponent<StatMap>();
                            int damageToDo = statMap.allStats["power"];
                            if (damageToDo > 0)
                            {
                                enemyController.hitStun = 0;
                                enemyController.health -= damageToDo;
                            }
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
}
