using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Controllers;
using Core.Items.Weapons.Ranged;
using Core.Environment;

namespace Core.Items
{
    public class Item : Interactable
    {
        public bool pickedUp = false;
        public bool isEquipped = false;
        public string category;
        Animator animator;
        PlayerController playerController;
        SpriteRenderer itemRenderer;
        public Ammo ammo;

        public override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            itemRenderer = GetComponent<SpriteRenderer>();
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {
            if (isEquipped && playerController.isShooting())
            {
                itemRenderer.enabled = true;

                string currentDirection = playerController.currentDirection;
                Vector3 playerPos = playerController.gameObject.transform.position;
                animator.Play("weapon_bow_shoot_" + currentDirection);

                // Position bow vertically
                float v = currentDirection == "down" ? .2f : .5f;
                // if playerController.gameObject is not facing the side, move item 0, if they are, move it .5f based on flipx
                float h = currentDirection == "side" ? playerController.gameObject.GetComponent<SpriteRenderer>().flipX ? .5f : -.5f : 0;

                gameObject.transform.position = new Vector3(playerPos.x + h, playerPos.y + v, currentDirection != "up" ? playerPos.z - 1 : playerPos.z + 1);
                itemRenderer.flipX = playerController.gameObject.GetComponent<SpriteRenderer>().flipX;
                if (playerController.shootFrames == 0)
                {
                    Instantiate(ammo, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity).gameObject.AddComponent<Ammo>().Shoot(currentDirection, gameObject.GetComponent<SpriteRenderer>().flipX);
                }
            }
            else if (pickedUp)
            {
                itemRenderer.enabled = false;

            }
        }

        public void PickUpItem()
        {
            pickedUp = true;
            canPickUp = false;
            canInteract = false;
            gameObject.SetActive(false);
        }

        public void UseItem()
        {
            isEquipped = !isEquipped;
            itemRenderer.enabled = !itemRenderer.enabled;
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }

}
