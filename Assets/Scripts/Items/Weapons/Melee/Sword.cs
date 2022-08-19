using System;
using System.Collections;
using System.Collections.Generic;
using Core.Libraries.AnimationMappings;
using UnityEngine;

namespace Core.Items.Weapons.Melee
{
    public class Sword : MeleeWeapon
    {
        CircleCollider2D hurtBox;
        private string playerDirection;
        private Vector3 playerPos;

        public override void Awake()
        {
            base.Awake();
            hurtBox = GetComponent<CircleCollider2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {
            playerDirection = playerController.currentDirection;
            playerPos = playerController.gameObject.transform.position;
        }

        void FixedUpdate()
        {
            if (isEquipped && !playerController.isSliding() && !playerController.isJumping() && !playerController.isShooting())
            {
                itemRenderer.enabled = true;
                HandleHurtBox();
                AnimateWeapon();
            }
            else if (pickedUp)
            {
                itemRenderer.enabled = false;
            }
        }

        private void HandleHurtBox()
        {
            if (playerDirection == "side")
            {
                hurtBox.radius = 0.8f;

                if (!playerController.IsFacingLeft())
                {
                    hurtBox.offset = new Vector2(0.4f, 0.8f);
                }
                else
                {
                    hurtBox.offset = new Vector2(-0.4f, 0.8f);

                }
                hurtBox.transform.position = new Vector2(playerPos.x, playerPos.y + 0.9f);

            }
            else if (playerDirection == "up")
            {
                hurtBox.transform.position = new Vector2(playerPos.x, playerPos.y);
                hurtBox.offset = new Vector2(-0.5f, 0.8f);
                hurtBox.radius = 0.6f;

            }
            else
            {
                hurtBox.transform.position = new Vector2(playerPos.x, playerPos.y);
                hurtBox.offset = new Vector2(-0.5f, 0f);
                hurtBox.radius = 0.6f;
            }
        }
        public void AnimateWeapon()
        {
            // handle melee attack
            if (playerController.isMeleeing())
            {
                hurtBox.enabled = true;
                if (playerDirection != "side")
                {
                    itemRenderer.flipX = false;
                }
                gameObject.transform.position = new Vector3(playerDirection != "down" ? playerPos.x : playerPos.x + 0.5f, playerPos.y, playerDirection != "up" ? playerPos.z - 0.25f : playerPos.z + 0.25f);
                weaponAnimator.Play("weapon_sword_" + playerDirection);
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
                    h = SwordMappings.animationMappings[playerAction][playerDirection][currentAnim][0];
                    v = SwordMappings.animationMappings[playerAction][playerDirection][currentAnim][1];
                }
                catch (KeyNotFoundException)
                {
                    // Debug.Log(playerAction);
                    // Debug.Log(playerDirection);
                    // Debug.Log(currentAnim);
                    // Debug.Log(playerController.gameObject.GetComponent<SpriteRenderer>().sprite.name);
                }

                // if we're running, flip the horizontal offset of the sword to match the player
                if (playerController.IsFacingRight() && playerDirection == "side")
                {
                    h = h * -1;
                }

                //handles weapon rotation
                if (playerDirection == "up")
                {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, -30);
                    itemRenderer.flipX = false;
                }
                else if (playerDirection == "down")
                {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, 30);
                    itemRenderer.flipX = true;
                }
                else
                {
                    itemRenderer.flipX = playerController.gameObject.GetComponent<SpriteRenderer>().flipX;
                    gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                gameObject.transform.position = new Vector3(playerPos.x + h, playerPos.y + v, playerDirection != "up" ? playerPos.z - 0.25f : playerPos.z + 0.5f);
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
