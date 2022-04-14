using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Environment;

namespace Core.Items
{
    public class Item : Interactable
    {
        public bool pickedUp = false;
        public string category;
        public int inventoryIndex;

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
        }

        public virtual void UseItem()
        {
        }
    }

}
