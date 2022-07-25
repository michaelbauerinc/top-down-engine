using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using Core.Environment;
using UnityEngine;

namespace Core.Items
{
    public class Item : Interactable
    {
        public bool pickedUp = false;
        public bool destroyedOnUse = false;
        public string category;

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
        }

        public virtual void PickUpItem()
        {
            pickedUp = true;
            canPickUp = false;
            canInteract = false;
            itemRenderer.enabled = false;
            boxCollider.enabled = false;
        }
        public virtual void DropItem()
        {
            string playerDir = GameObject.Find("Player").GetComponent<PlayerController>().currentDirection;
            Vector3 dropLocation = GameObject.Find("Player").transform.position;
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
            canPickUp = true;
            canInteract = true;
            itemRenderer.enabled = true;
            boxCollider.enabled = true;
        }

        public virtual void UseItem()
        {
            if (destroyedOnUse)
            {
                Destroy(gameObject);

            }
        }
    }

}
