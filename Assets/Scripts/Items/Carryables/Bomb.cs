using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using Core.Items;
using UnityEngine;

namespace Core.Items.Carryables
{
    public class Bomb : Carryable
    {
        public bool isTriggered;
        public int detonationTimer = 300;
        private string deathSprite = "bomb_explosion_10";

        private CircleCollider2D hurtbox;

        public override void Awake()
        {
            base.Awake();

            hurtbox = transform.GetChild(0).GetComponentInChildren<CircleCollider2D>();
            animator.Play("empty");
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        public void FixedUpdate()
        {
            if (itemRenderer.sprite.name == deathSprite)
            {
                Destroy(gameObject);
            }
            else if (itemRenderer.sprite.name.Contains("explosion") && !hurtbox.enabled)
            {
                hurtbox.enabled = true;
            }
            else if (isTriggered && detonationTimer <= 0)
            {
                animator.Play("item_bomb_ground");
            }
            else if (isTriggered)
            {
                animator.Play("bomb_triggered");
                detonationTimer--;
            }
        }
        // Update is called once per frame
        void Update()
        {

            if (!isTriggered)
            {
                string playerDir = playerController.currentDirection;
                Vector3 carryLocation = GameObject.FindGameObjectsWithTag("Player1")[0].transform.position;
                switch (playerDir)
                {
                    case "up":
                        carryLocation.y += 1;
                        carryLocation.z++;
                        break;
                    case "down":
                        carryLocation.y -= 0.2f;
                        carryLocation.z--;
                        break;
                    case "side":
                        carryLocation.x = playerController.IsFacingLeft() ? carryLocation.x -= 0f : carryLocation.x += 0f;
                        // carryLocation.y += 0;
                        carryLocation.z--;
                        break;
                    default:
                        break;
                }
                if (beingCarried)
                {
                    itemRenderer.flipX = playerController.playerRenderer.flipX;
                    gameObject.transform.position = carryLocation;
                    itemRenderer.enabled = true;
                    animator.Play("bomb_hold_" + playerDir);

                }
                else if (pickedUp)
                {
                    itemRenderer.enabled = false;
                }
            }

        }

        public void Throw()
        {
            string playerDir = GameObject.FindGameObjectsWithTag("Player1")[0].GetComponent<PlayerController>().currentDirection;
            Vector3 dropLocation = GameObject.FindGameObjectsWithTag("Player1")[0].transform.position;
            dropLocation.z = 99;
            switch (playerDir)
            {
                case "up":
                    dropLocation.y += 1;
                    break;
                case "down":
                    dropLocation.y -= 0.5f;
                    break;
                case "side":
                    dropLocation.x = playerController.IsFacingLeft() ? dropLocation.x -= 1 : dropLocation.x += 1;
                    dropLocation.y += 0.5f;
                    break;
                default:
                    break;
            }
            transform.position = dropLocation;
            pickedUp = false;
            isTriggered = true;
            canPickUp = false;
            canInteract = false;
            itemRenderer.enabled = true;
            boxCollider.enabled = true;
        }

        public override void PickUpItem()
        {
            base.PickUpItem();
        }

        public override void UseItem()
        {
            base.UseItem();
            Throw();
        }
    }
}
