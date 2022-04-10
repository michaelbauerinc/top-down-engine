using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Libraries.AnimationMappings;

namespace Core.Items.Weapons.Melee
{
    public class Sword : MeleeWeapon
    {
        public override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {
            // AnimateWeapon();
            if (isEquipped && !playerController.isSliding() && !playerController.isJumping())
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

            if (playerController.isMeleeing())
            {
                gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                if (currentDirection != "side")
                {
                    itemRenderer.flipX = false;
                }
                gameObject.transform.position = new Vector3(playerPos.x, playerPos.y, currentDirection != "up" ? playerPos.z - 0.25f : playerPos.z + 0.25f);
                weaponAnimator.Play("weapon_sword_" + currentDirection);
            }
            else
            {
                weaponAnimator.Play("Empty");

                string playerAction = playerController.playerAction;
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
