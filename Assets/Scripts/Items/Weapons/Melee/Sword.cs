using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Libraries.AnimationMappings;

namespace Core.Items.Weapons.Melee
{
    public class Sword : MeleeWeapon
    {
        CircleCollider2D hurtBox;

        public override void Awake()
        {
            base.Awake();
            hurtBox = GameObject.Find("Hurtbox").GetComponent<CircleCollider2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {

            if (isEquipped && !playerController.isSliding() && !playerController.isJumping() && !playerController.isShooting())
            {
                itemRenderer.enabled = true;
                AnimateWeapon();
            }
            else if (pickedUp)
            {
                itemRenderer.enabled = false;
            }
        }

        void FixedUpdate()
        {

        }

        public void AnimateWeapon()
        {
            string currentDirection = playerController.currentDirection;
            Vector3 playerPos = playerController.gameObject.transform.position;

            // handle hurtbox
            if (currentDirection == "side")
            {
                hurtBox.radius = 0.8f;

                if (playerController.gameObject.GetComponent<SpriteRenderer>().flipX == true)
                {
                    hurtBox.offset = new Vector2(0.4f, -0.2f);

                }
                else
                {
                    hurtBox.offset = new Vector2(-0.4f, -0.2f);

                }
                hurtBox.transform.position = new Vector2(playerPos.x, playerPos.y + 0.9f);

            }
            else if (currentDirection == "up")
            {
                hurtBox.transform.position = new Vector2(playerPos.x, playerPos.y);
                hurtBox.offset = new Vector2(0, 0.8f);
                hurtBox.radius = 0.6f;

            }
            else
            {
                hurtBox.transform.position = new Vector2(playerPos.x, playerPos.y);
                hurtBox.offset = new Vector2(0, 0f);
                hurtBox.radius = 0.6f;
            }

            // handle melee attack
            if (playerController.isMeleeing())
            {
                hurtBox.enabled = true;
                if (currentDirection != "side")
                {
                    itemRenderer.flipX = false;
                }
                gameObject.transform.position = new Vector3(playerPos.x, playerPos.y, currentDirection != "up" ? playerPos.z - 0.25f : playerPos.z + 0.25f);
                weaponAnimator.Play("weapon_sword_" + currentDirection);
            }
            else
            {
                hurtBox.enabled = false;
                weaponAnimator.Play("Empty");

                string playerAction = playerController.playerAction;
                playerAction = playerAction == "interacting" ? "idle" : playerAction;

                string currentAnim = playerController.gameObject.GetComponent<SpriteRenderer>().sprite.name.Substring(
                playerController.gameObject.GetComponent<SpriteRenderer>().sprite.name.Length - 2);

                try
                {
                    h = SwordMappings.animationMappings[playerAction][currentDirection][currentAnim][0];
                    v = SwordMappings.animationMappings[playerAction][currentDirection][currentAnim][1];
                }
                catch (KeyNotFoundException)
                {
                    // Debug.Log(playerAction);
                    // Debug.Log(currentDirection);
                    // Debug.Log(currentAnim);
                    // Debug.Log(playerController.gameObject.GetComponent<SpriteRenderer>().sprite.name);
                }

                if (playerController.gameObject.GetComponent<SpriteRenderer>().flipX && currentDirection == "side")
                {
                    h = h * -1;
                }

                if (currentDirection == "up")
                {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, -30);

                }
                else if (currentDirection == "down")
                {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, 30);
                }
                else
                {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, 0);

                }

                gameObject.transform.position = new Vector3(playerPos.x + h, playerPos.y + v, currentDirection != "up" ? playerPos.z - 0.25f : playerPos.z + 0.25f);

                if (currentDirection == "down")
                {
                    itemRenderer.flipX = true;
                }
                else if (currentDirection == "up")
                {
                    itemRenderer.flipX = false;
                }
                else
                {
                    itemRenderer.flipX = playerController.gameObject.GetComponent<SpriteRenderer>().flipX;

                }
            }
        }

        public override void PickUpItem()
        {
            base.PickUpItem();
        }

        public override void UseItem()
        {
            base.UseItem();
        }
    }
}
