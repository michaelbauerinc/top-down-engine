using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Items.Weapons.Ranged
{
    public class Bow : RangedWeapon
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
            if (isEquipped && playerController.isShooting())
            {
                itemRenderer.enabled = true;

                string currentDirection = playerController.currentDirection;
                Vector3 playerPos = playerController.gameObject.transform.position;
                animator.Play(animationString + currentDirection);

                // Position bow vertically
                float v = currentDirection == "down" ? .2f : .5f;
                // if playerController.gameObject is not facing the side, move item 0, if they are, move it .5f based on flipx
                float h = currentDirection == "side" ? playerController.gameObject.GetComponent<SpriteRenderer>().flipX ? .5f : -.5f : 0;

                gameObject.transform.position = new Vector3(playerPos.x + h, playerPos.y + v, currentDirection != "up" ? playerPos.z - 1 : playerPos.z + 1);
                itemRenderer.flipX = playerController.gameObject.GetComponent<SpriteRenderer>().flipX;
            }
            else if (pickedUp)
            {
                itemRenderer.enabled = false;

            }
        }

        void FixedUpdate()
        {
            if (playerController.shootFrames == 0)
            {
                string currentDirection = playerController.currentDirection;
                Instantiate(ammo, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity).gameObject.AddComponent<Ammo>().Shoot(currentDirection, gameObject.GetComponent<SpriteRenderer>().flipX);
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
