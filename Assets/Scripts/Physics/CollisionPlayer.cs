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
    public class CollisionPlayer : MonoBehaviour
    {
        PlayerController playerController;
        UIController uiController;

        // Start is called before the first frame update
        void Start()
        {
            playerController = gameObject.GetComponent<PlayerController>();
            uiController = GameObject.Find("UI").GetComponent<UIController>();
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
            if (other.GetType() == typeof(CircleCollider2D) && !other.gameObject.transform.IsChildOf(playerController.gameObject.transform))
            {
                if (playerController.health > 0)
                {
                    try
                    {
                        MultiTag tags = other.gameObject.GetComponent<MultiTag>();
                        if (tags.HasTag("hurtbox"))
                        {
                            if (tags.HasTag("jumpable-hurtbox"))
                            {
                                if (playerController.isJumping())
                                {
                                    return;
                                }
                            }
                            StatMap statMap = other.gameObject.GetComponent<StatMap>();
                            int damageToDo = statMap.allStats["power"];
                            if (damageToDo > 0 && !playerController.hasInvincibilityFrames)
                            {
                                playerController.hitStun = 0;
                                playerController.health -= damageToDo;
                                uiController.UpdatePlayerHealthBar();
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

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!playerController.IsInHitstun() && !playerController.hasInvincibilityFrames)
            {
                OnTriggerEnter2D(other);
            }
        }
    }
}
